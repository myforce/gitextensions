using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management.Automation.Runspaces;
using System.Management.Automation;
using System.Text.RegularExpressions;
using System.IO;

namespace MyForceReleaser
{
    public partial class MyForceReleaserGUI : Form
    {
        MyForceReleaser _Model;
        public MyForceReleaserGUI(MyForceReleaser Model)
        {
            InitializeComponent();
            _Model = Model;

            progressBar.Visible = false;
            progressBar.Minimum = 0;

            LoadProducts();
        }

        public void LoadProducts()
        {
            //Clear the current products
            dataGridViewProducts.Rows.Clear();
            dataGridViewReleases.Rows.Clear();

            //Determine the repo name from the remote URL (Eg: Bison, ContactCentre, ...
            string originURL = _Model.Git.RunGitCmd("remote get-url origin");
            Regex regex = new Regex(@"^.*?([^/]+)$");
            Match match = regex.Match(originURL);
            if (!match.Success || match.Groups.Count < 2)
            {
                MessageBox.Show("Unable to parse repo name from: " + originURL);
                return;
            }
            string repoName = "";
            try
            {
                repoName = System.IO.Path.GetFileNameWithoutExtension(match.Groups[1].Value.Trim());
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            //Load the valid products from the internal repo
            var ValidProducts = MyForceReleaser.LoadValidProducts(_Model);
            if (ValidProducts.Count <= 0)
            {
                MessageBox.Show("Unable to load ValidProducts from the internal repo! Please verify the file (internal repo)" + MyForceReleaser.FILELIST_INTERNAL[MyForceReleaser.FILE_INTERNAL_VALIDPRODUCTS]);
                return;
            }
            if (!ValidProducts.ContainsKey(repoName))
            {
                MessageBox.Show("Repository: " + repoName + " not found in the list of valid products!");
                return;
            }

            //Load the resource files for each valid product
            Dictionary<string, List<string>> dicResourceFiles = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> dicSqlFiles = new Dictionary<string, List<string>>();
            MyForceReleaser.LoadResourceMapsForValidProducts(_Model, ref dicResourceFiles, ref dicSqlFiles);
            if (dicResourceFiles.Count <= 0)
                MessageBox.Show("Unable to load resource files for this repo! Please verify the file (current repo)" + MyForceReleaser.FILELIST_REPO[MyForceReleaser.FILE_REPO_MAPPRODUCTNAMESTORESOURCEFILES]);

            //Load all available tags
            string strAllTags = MyForceReleaser.GetTags(_Model);

            //Fill the grids
            foreach (var ProductName in ValidProducts[repoName])
            {
                bool bRetrievedFromTag = false;
                string strCurrentVersion = "not defined";
                string strNextVersion = "not defined";

                //Check if there are any regular resource files where we can get versions from
                if (dicResourceFiles.ContainsKey(ProductName) && dicResourceFiles[ProductName].Count > 0)
                {
                    string FilePath = System.IO.Path.Combine(_Model.Git.GetWorkingDir(), dicResourceFiles[ProductName][0]);
                    if (FilePath.ToLower().EndsWith(".rc"))
                    {
                        //Parse from cpp file
                        strCurrentVersion = StaticTools.FindInFile(FilePath, @"FILEVERSION\s+((?:\d+,)+\d+)", 1).Replace(',','.');
                    } else {
                        //Parse from assembly info
                        strCurrentVersion = StaticTools.FindInFile(FilePath, "\\[assembly:\\s+AssemblyVersion\\(\"([^\"\\*]*)\"\\)\\]", 1);
                    }                    
                }
                else
                {
                    //No regular resource files... Maybe sql files with versions in it ?
                    if (dicSqlFiles.ContainsKey(ProductName))
                    {
                        string FilePath = System.IO.Path.Combine(_Model.Git.GetWorkingDir(), dicSqlFiles[ProductName][0]);
                        strCurrentVersion = StaticTools.FindInFile(FilePath, @"((?:\d+\.){3}\d+)", 1).Replace(',', '.');
                    }
                    else
                    {
                        //No resource or sql => We use the version from the previous tag
                        bRetrievedFromTag = true;
                        string strRemoteBranch = MyForceReleaser.GetRemoteBranchName(_Model);
                        if (!string.IsNullOrWhiteSpace(strRemoteBranch))
                        {
                            //Parse version from the current branch
                            string strRemoteVersion = strRemoteBranch.Replace("Version-", "");
                            var matches = Regex.Matches(strAllTags, "refs/tags/" + ProductName + "-v?(" + strRemoteVersion + @".\d+)\^{}");
                            foreach (Match tagmatch in matches)
                            {
                                string strCurrentTagMatch = "";
                                if (tagmatch.Groups.Count >= 2)
                                    strCurrentTagMatch = tagmatch.Groups[1].Value.Trim();

                                if (StaticTools.IsValidVersionNumber(strCurrentTagMatch))
                                {
                                    if (strCurrentVersion == "not defined" || StaticTools.CompareFileVersions(strCurrentVersion, strCurrentTagMatch) < 0)
                                        strCurrentVersion = strCurrentTagMatch;
                                }
                            }
                        }
                    }
                }

                //Increment the next version number
                if (StaticTools.IsValidVersionNumber(strCurrentVersion))
                {
                    strNextVersion = StaticTools.IncrementVersionNumber(strCurrentVersion);

                    //Verify if the tag already exists => if not this product is ready to release
                    if (bRetrievedFromTag || !Regex.Match(strAllTags, "refs/tags/" + ProductName + "-v?" + strCurrentVersion + @"\^{}").Success)
                        dataGridViewReleases.Rows.Add(ProductName, bRetrievedFromTag ? strNextVersion : strCurrentVersion, !bRetrievedFromTag, bRetrievedFromTag);
                    else
                        dataGridViewProducts.Rows.Add(ProductName, strCurrentVersion, strNextVersion, false);
                }
                else
                    MessageBox.Show("Couldn't determine any version for " + ProductName + ". Releasing will work but not for this product!");
            }
        }

        private void dataGridViewProducts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3 && e.RowIndex >= 0)
            {
                var dataGridView = (DataGridView)sender;
                var cell = dataGridViewProducts[e.ColumnIndex, e.RowIndex];
                if (cell.Value == null) 
                    cell.Value = false;
                cell.Value = !(bool)cell.Value;
            }
        }

        private void dataGridViewReleases_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2 && e.RowIndex >= 0)
            {
                var dataGridView = (DataGridView)sender;
                var cell = dataGridViewReleases[e.ColumnIndex, e.RowIndex];
                if (cell.Value == null)
                    cell.Value = false;
                cell.Value = !(bool)cell.Value;
            }
        }

        private void UpdateVersionNumbers_Click(object sender, EventArgs e)
        {
            bool ProductsFoundToRelease = false;

            //Load the resource files for each valid product
            Dictionary<string, List<string>> ResourceFilesForProducts = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> SqlFilesForProducs = new Dictionary<string, List<string>>();
                
            MyForceReleaser.LoadResourceMapsForValidProducts(_Model, ref ResourceFilesForProducts, ref SqlFilesForProducs);
            if (ResourceFilesForProducts.Count <= 0)
                return; //Already notified so just abort once here                  

            //Change all version numbers
            List<Command> lstToExecute = new List<Command>();
            for (int RowIndex = 0; RowIndex < dataGridViewProducts.Rows.Count; RowIndex++)
            {
                string strProduct = (string)dataGridViewProducts[0, RowIndex].Value;
                if ((bool)dataGridViewProducts[3, RowIndex].Value)
                {
                    ProductsFoundToRelease = true;
                    Command cmd = new Command(System.IO.Path.Combine(_Model.InteralRepositoryPath, MyForceReleaser.FILELIST_INTERNAL[MyForceReleaser.FILE_INTERNAL_CHANGEVERSIONS]));
                    cmd.Parameters.Add("Directory", _Model.Git.GetWorkingDir().TrimEnd('\\'));
                    cmd.Parameters.Add("Version", (string)dataGridViewProducts[2, RowIndex].Value);
                    cmd.Parameters.Add("DisableChecks", true);
                    if (ResourceFilesForProducts.ContainsKey(strProduct))
                        cmd.Parameters.Add("IncludeList", ResourceFilesForProducts[strProduct]);

                    if (SqlFilesForProducs.ContainsKey(strProduct))
                        cmd.Parameters.Add("SqlList", SqlFilesForProducs[strProduct]);

                    lstToExecute.Add(cmd);
                }
            }
            
            if (!ProductsFoundToRelease)
            {
                MessageBox.Show("No products checked to update the version numbers!");
                return;
            }

            // Create a runspace to host the PowerScript environment:
            Runspace runspace = System.Management.Automation.Runspaces.RunspaceFactory.CreateRunspace();
            runspace.Open(); 
            try
            {
                progressBar.Visible = true;
                progressBar.Value = 0;
                progressBar.Maximum = lstToExecute.Count;
                foreach (var cmd in lstToExecute)
                {
                    Pipeline pipeline = runspace.CreatePipeline();
                    pipeline.Commands.Add(cmd);
                    progressBar.Increment(1);
                    pipeline.Invoke();
                }
                progressBar.Visible = false;
            }
            catch (Exception ex)
            {
                progressBar.Visible = false;
                MessageBox.Show(ex.Message);
                return;
            }
            finally { runspace.Close(); }

            //Commit the changes
            _Model.Git.RunGitCmd("commit -am \"GitExtensions MyForce Releaser Plugin: Updated version numbers for release\"");

            MessageBox.Show("Version are updated. Please verify them and then re-open this plugin to actually release the programs!");

            //Close the GUI so user can verify his actions
            Close();
        }

        private void ReleasePrograms_Click(object sender, EventArgs e)
        {
            bool ProductsFoundToRelease = false;           

            //Change all version numbers
            //bool DoDummyCommitSinceOnlyTagBasedRelease = true;
            List<Command> lstToExecute = new List<Command>();
            for (int RowIndex = 0; RowIndex < dataGridViewReleases.Rows.Count; RowIndex++)
            {
                string strProduct = (string)dataGridViewReleases[0, RowIndex].Value;
                if ((bool)dataGridViewReleases[2, RowIndex].Value)
                {
                    //if (!((bool)dataGridViewReleases[3, RowIndex].Value))
                    //    DoDummyCommitSinceOnlyTagBasedRelease = false; //We have program that actually has changed resources
                    ProductsFoundToRelease = true;

                    Command cmdTag = new Command(System.IO.Path.Combine(_Model.InteralRepositoryPath, MyForceReleaser.FILELIST_INTERNAL[MyForceReleaser.FILE_INTERNAL_CREATETAG]));
                    cmdTag.Parameters.Add("ProductNames", strProduct);
                    cmdTag.Parameters.Add("Version", (string)dataGridViewReleases[1, RowIndex].Value);
                    lstToExecute.Add(cmdTag);

                    Command cmdPush = new Command(System.IO.Path.Combine(_Model.InteralRepositoryPath, MyForceReleaser.FILELIST_INTERNAL[MyForceReleaser.FILE_INTERNAL_CREATETAG]));
                    cmdPush.Parameters.Add("ProductNames", strProduct);
                    cmdPush.Parameters.Add("Version", (string)dataGridViewReleases[1, RowIndex].Value);
                    cmdPush.Parameters.Add("Push", true);
                    lstToExecute.Add(cmdPush);
                }
            }

            if (!ProductsFoundToRelease)
            {
                MessageBox.Show("No products checked to release!");
                return;
            }

            // Create a runspace to host the PowerScript environment:
            Runspace runspace = System.Management.Automation.Runspaces.RunspaceFactory.CreateRunspace();
            runspace.Open();
            runspace.SessionStateProxy.Path.SetLocation(_Model.Git.GetWorkingDir().TrimEnd('\\'));

            try
            {
                #region commented
                /*if (DoDummyCommitSinceOnlyTagBasedRelease)
                {
                    string strDummyVersionFile = System.IO.Path.Combine(_Model.Git.GetWorkingDir(), "_myforcereleaser.dummycommit");
                    if (System.IO.File.Exists(strDummyVersionFile))
                    {
                        System.IO.File.Delete(strDummyVersionFile);
                    }
                    else
                    {
                        System.IO.File.WriteAllText(strDummyVersionFile, strDummyVersionFile);
                        MessageBox.Show(_Model.Git.RunGitCmd("add _myforcereleaser.dummycommit"));
                    }

                    MessageBox.Show(_Model.Git.RunGitCmd("commit -am \"GitExtensions MyForce Releaser Plugin: Manipulated dummy version file for release due to only tag based releases!\""));
                }*/
                #endregion               

                progressBar.Value = 0;
                progressBar.Visible = true;
                progressBar.Maximum = lstToExecute.Count;                
                foreach (var cmd in lstToExecute)
                {
                    Pipeline pipeline = runspace.CreatePipeline();
                    pipeline.Commands.Add(cmd);
                    progressBar.Increment(1);
                    pipeline.Invoke();
                }
                progressBar.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                progressBar.Visible = false;
                return;
            }
            finally { runspace.Close(); }

            MessageBox.Show("Programs are released!");

            //Close the GUI
            Close();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            MyForceReleaserGUISettings settings = new MyForceReleaserGUISettings();
            settings.InternalRepoPath = _Model.InteralRepositoryPath;

            settings.ShowDialog();
            if (settings.InternalRepoPath.Equals(_Model.InteralRepositoryPath))
            {
                _Model.InteralRepositoryPath = settings.InternalRepoPath;
                LoadProducts();
            }
        }        
    }
}
