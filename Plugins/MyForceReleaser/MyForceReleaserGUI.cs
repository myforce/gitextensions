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
        private MyForceReleaser _Model;
        public MyForceReleaserGUI(MyForceReleaser Model)
        {
            InitializeComponent();
            _Model = Model;

            progressBar.Visible = false;
            progressBar.Minimum = 0;
            m_nVHCurrentSelectedVersionRow = -1;
            m_nVHPreviousSelectedVersionRow = -1;
            m_lstNonFilteredCommitMessages = new List<string>();

            //Load the commit message regexes from the model
            this.CommitMessageIgnoreRegexes = _Model.CommitMessageIgnoreRegexes;

            //Parse all products
            LoadProducts();

            tabVersions.Enabled = _Model.Git.IsCurrentBranchMaster() || _Model.Git.IsCurrentBranchVersionBranch();
            tabReleasing.Enabled = _Model.Git.IsCurrentBranchVersionBranch() || _Model.Git.IsCurrentBranchMaster();
            tabVersionHistory.Enabled = !_Model.Git.IsCurrentBranchVersionBranch(); //Enable history on all except version branches! So they can be merged in!

            //Open the correct tab
            tabMainWindow_Selected(null, new TabControlEventArgs(tabMainWindow.SelectedTab, 0, TabControlAction.Selected));
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            //Store the commit message regexes on the model
            _Model.CommitMessageIgnoreRegexes = this.CommitMessageIgnoreRegexes;            
        }

        public bool ShowLogRequested()
        {
            return chkShowLogOnExit.Checked;
        }
        public void SetShowLogRequested(bool bValue)
        {
            chkShowLogOnExit.Checked = bValue;
        }

        //Regulate settings
        private void btnSettings_Click(object sender, EventArgs e)
        {
            MyForceReleaserGUISettings settings = new MyForceReleaserGUISettings();
            settings.InternalRepoPath = _Model.InternalRepositoryPath;

            settings.ShowDialog();
            if (settings.InternalRepoPath.Equals(_Model.InternalRepositoryPath))
            {
                _Model.InternalRepositoryPath = settings.InternalRepoPath;
                LoadProducts();
            }
        }
        
        #region VersionTab

        #region VERSIONTAB_PROPERTIES
        /// <summary>
        /// Wrapper to retrieve/store list of string from/to txtCommitMessagesFilters
        /// </summary>
        private List<string> CommitMessageIgnoreRegexes
        {
            get
            {
                List<string> lstRegexes = new List<string>();
                var Matches = Regex.Matches(txtCommitMessagesFilters.Text, @"^(.*?)(?:\r|\n)*$", RegexOptions.Multiline);
                foreach (Match item in Matches)
                {
                    if (item.Groups.Count > 1
                        && !string.IsNullOrWhiteSpace(item.Groups[1].Value)
                        && !"<regex patterns>".Equals(item.Groups[1].Value))
                        lstRegexes.Add(item.Groups[1].Value);
                }
                return lstRegexes;
            }
            set
            {
                StringBuilder strBuilder = new StringBuilder();
                if (value != null)
                    foreach (var strRegex in value)
                    {
                        if (!string.IsNullOrWhiteSpace(strRegex))
                            strBuilder.AppendFormat(@"{0}{1}", strRegex, System.Environment.NewLine);
                    }
                txtCommitMessagesFilters.Text = strBuilder.ToString();
                if (string.IsNullOrWhiteSpace(txtCommitMessagesFilters.Text))
                    txtCommitMessagesFilters.Text = "<regex patterns>";
            }
        }

        /// <summary>
        /// List of all commit messages. This can be used (re-)apply filters on the views
        /// </summary>
        private List<string> m_lstNonFilteredCommitMessages;
        private int m_nVHPreviousSelectedVersionRow;
        private int m_nVHCurrentSelectedVersionRow;
        #endregion

        #region VERSIONTAB_CONSTANTS
        private class VersionHistoryCache
        {
            public VersionHistoryCache(string version = "", string historytext = "")
            {
                Version = version;
                HistoryText = historytext;
            }
            public string Version { get; set; }
            public string HistoryText { get; set; }
        }
        private readonly string PRODUCTS_COL_PRODUCT = "colProduct";
        private readonly string PRODUCTS_COL_CURRENTVERSION = "colCurrentVersion";
        private readonly string PRODUCTS_COL_VERSIONTOSET = "colVersionToSet";
        private readonly string PRODUCTS_COL_UPDATEVERSION = "colUpdateVersion";
        private readonly string PRODUCTS_COL_VERSIONHISTORYUPDATED = "colVersionHistoryUpdated";
        #endregion      

        //Actions
        private void UpdateVersionNumbers_Click(object sender, EventArgs e)
        {
            Logger.GetLogger().LogMessage("UpdateVersionNumbers: Start");

            if (dataGridViewProducts.Rows.Count < 0)
            {
                Logger.GetLogger().LogMessage("UpdateVersionNumbers: No rows available!");
                return; //No use to update rows
            }

            if (!ValidateVersionNumbersInColumn(ref dataGridViewProducts, 
                dataGridViewProducts[PRODUCTS_COL_VERSIONTOSET, 0].ColumnIndex, 
                dataGridViewProducts[PRODUCTS_COL_UPDATEVERSION, 0].ColumnIndex))
            {
                Logger.GetLogger().LogMessage("UpdateVersionNumbers: End");
                return;
            }
            int nProductsFoundToRelease = 0;
            
            //Load the resource files for each valid product
            Dictionary<string, List<string>> ResourceFilesForProducts = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> SqlFilesForProducs = new Dictionary<string, List<string>>();

            MyForceReleaser.LoadResourceMapsForValidProducts(_Model, ref ResourceFilesForProducts, ref SqlFilesForProducs);
            if (ResourceFilesForProducts.Count <= 0 && SqlFilesForProducs.Count <= 0)
            {
                Logger.GetLogger().LogMessage("UpdateVersionNumbers: End");
                return; //Already notified so just abort once here
            }

            //Change all version numbers
            List<PowerShell> lstToExecute = new List<PowerShell>();
            for (int RowIndex = 0; RowIndex < dataGridViewProducts.Rows.Count; RowIndex++)
            {
                string strProduct = (string)dataGridViewProducts[PRODUCTS_COL_PRODUCT, RowIndex].Value;
                string strVersionToSet = (string)dataGridViewProducts[PRODUCTS_COL_VERSIONTOSET, RowIndex].Value;
                string strTrack = StaticTools.GetMainTrackVersionNumber(strVersionToSet);                
                
                if ((bool)dataGridViewProducts[PRODUCTS_COL_UPDATEVERSION, RowIndex].Value)
                {
                    StringBuilder strbLogCommand = new StringBuilder();

                    nProductsFoundToRelease++;
                    PowerShell ps = StaticTools.GetWriteHostDisabledPowershell();

                    strbLogCommand.Append("\"" + System.IO.Path.Combine(_Model.InternalRepositoryPath, MyForceReleaser.FILELIST_INTERNAL[MyForceReleaser.FILE_INTERNAL_CHANGEVERSIONS]) + "\" ");
                    ps.AddCommand(System.IO.Path.Combine(_Model.InternalRepositoryPath, MyForceReleaser.FILELIST_INTERNAL[MyForceReleaser.FILE_INTERNAL_CHANGEVERSIONS]));

                    strbLogCommand.AppendFormat("-Directory {0} ", _Model.Git.GetWorkingDir().TrimEnd('\\'));
                    ps.AddParameter("Directory", _Model.Git.GetWorkingDir().TrimEnd('\\'));
                    
                    strbLogCommand.AppendFormat("-Version {0} ", strVersionToSet);
                    ps.AddParameter("Version", strVersionToSet);

                    strbLogCommand.Append("-DisableChecks:$true ");
                    ps.AddParameter("DisableChecks", true);

                    if (ResourceFilesForProducts.ContainsKey(strProduct))
                    {
                        strbLogCommand.AppendFormat("-IncludeList: {0} ", string.Join("," , ResourceFilesForProducts[strProduct].ToArray()));
                        ps.AddParameter("IncludeList", ResourceFilesForProducts[strProduct]);
                    }

                    if (SqlFilesForProducs.ContainsKey(strProduct))
                    {
                        strbLogCommand.AppendFormat("-SqlList: {0} ", string.Join(",", SqlFilesForProducs[strProduct].ToArray()));
                        ps.AddParameter("SqlList", SqlFilesForProducs[strProduct]);
                    }

                    Logger.GetLogger().LogMessage(string.Format("UpdateVersionNumbers: Product <{0}> marked for update versions execute command <{1}> "
                        , strProduct
                        , strbLogCommand.ToString()));
                    
                    lstToExecute.Add(ps);
                } else
                    Logger.GetLogger().LogMessage("UpdateVersionNumbers: Product not marked for update of its versions => " + strProduct);

            }

            if (nProductsFoundToRelease <= 0)
            {
                string strError = "No products checked to update the version numbers!";
                MessageBox.Show(strError);
                Logger.GetLogger().LogMessage("UpdateVersionNumbers: " + strError);
                return;
            }

            // Create a runspace to host the PowerScript environment:
            try
            {
                progressBar.Visible = true;
                progressBar.Value = 0;
                progressBar.Maximum = lstToExecute.Count;
                foreach (var ps in lstToExecute)
                {
                    progressBar.Increment(1);
                    ps.Invoke();
                }
                progressBar.Visible = false;
            }
            catch (Exception ex)
            {
                progressBar.Visible = false;
                MessageBox.Show(ex.Message);
                Logger.GetLogger().LogMessage("UpdateVersionNumbers: " + ex.Message);
                return;
            }

            //Commit the changes
            Logger.GetLogger().LogMessage("UpdateVersionNumbers: Commit changes");
            _Model.Git.RunGitCmd("commit -am \"GitExtensions MyForce Releaser Plugin: Updated version numbers for release\"");
            MessageBox.Show(string.Format("{0}/{1} program versions are updated. Please verify them and then re-open this plugin to actually release the programs!", nProductsFoundToRelease, dataGridViewProducts.Rows.Count));

            //Close the GUI so user can verify his actions
            Logger.GetLogger().LogMessage("UpdateVersionNumbers: Done");
            Close();
        }
        private void dataGridViewProducts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (dataGridViewProducts[PRODUCTS_COL_UPDATEVERSION, e.RowIndex].ColumnIndex == e.ColumnIndex)
            {
                var dataGridView = (DataGridView)sender;
                var cell = dataGridViewProducts[e.ColumnIndex, e.RowIndex];
                if (cell.Value == null)
                    cell.Value = false;
                cell.Value = !(bool)cell.Value;
            }
        }
        private void dataGridViewProducts_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dataGridViewProducts.Rows.Count <= 0)
                return; //No use if there are no rows

            if (e.ColumnIndex == dataGridViewProducts[PRODUCTS_COL_VERSIONTOSET, 0].ColumnIndex)
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    contextVersionToSet.Show(Cursor.Position);
            }
            else if (e.ColumnIndex == dataGridViewProducts[PRODUCTS_COL_UPDATEVERSION, 0].ColumnIndex)
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    contextShouldUpdateVersions.Show(Cursor.Position);
            }
        }
        private void dataGridViewProducts_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (dataGridViewProducts[PRODUCTS_COL_VERSIONTOSET, 0].ColumnIndex == e.ColumnIndex)
            {
                string strNewVersion = (string)e.FormattedValue;
                if (string.IsNullOrWhiteSpace(strNewVersion) || !StaticTools.IsValidVersionNumber(strNewVersion))
                {
                    MessageBox.Show("Invalid version number! Please respect the format: X.X.X.X where (X = digits)!");
                    e.Cancel = true;
                }
            }
        }

        //context menu actions
        private void ContextVersionToSet_SetAllVersionNumbers_Click(object sender, EventArgs e)
        {
            bool bDone = true;
            string strVersionToSet = "";
            Point p = new Point(Cursor.Position.X, Cursor.Position.Y);
            do
            {
                bDone = true;
                InputBoxResult dlg = InputBox.Show("Please provide a version to set on all projects: ", "Version to set?", "X.X.X.X", p.X, p.Y);
                if (dlg.ReturnCode == System.Windows.Forms.DialogResult.OK)
                {
                    if (string.IsNullOrWhiteSpace(dlg.Text) || !StaticTools.IsValidVersionNumber(dlg.Text))
                    {
                        MessageBox.Show("Invalid version number! Please respect the format: X.X.X.X where (X = digits)!");
                        bDone = false;
                    }
                    else
                        strVersionToSet = dlg.Text;
                }
            } while (!bDone);

            if (!string.IsNullOrWhiteSpace(strVersionToSet))
            {
                for (int index = 0; index < dataGridViewProducts.Rows.Count; index++)
                    dataGridViewProducts[PRODUCTS_COL_VERSIONTOSET, index].Value = strVersionToSet;
                SyncVersionHistory(true);
            }
        }
        private void ContextUpdateVersion_CheckAll_Click(object sender, EventArgs e)
        {
            for (int nIndex = 0; nIndex < dataGridViewProducts.Rows.Count; nIndex++)
                dataGridViewProducts[PRODUCTS_COL_UPDATEVERSION, nIndex].Value = true;
        }
        private void ContextUpdateVersion_UncheckAll_Click(object sender, EventArgs e)
        {
            for (int nIndex = 0; nIndex < dataGridViewProducts.Rows.Count; nIndex++)
                dataGridViewProducts[PRODUCTS_COL_UPDATEVERSION, nIndex].Value = false;
        }        
        #endregion

        #region ReleaseTab

        #region RELEASETAB_CONSTANTS
        private readonly string RELEASE_COL_PRODUCT = "colReleaseProduct";
        private readonly string RELEASE_COL_VERSIONTORELEASE = "colReleaseVersionToRelease";
        private readonly string RELEASE_COL_RELEASE = "colReleaseRelease";
        private readonly string RELEASE_COL_INTERNALISTAGBASED = "colReleaseInternalIsTagBased";
        #endregion

        //Actions
        private void ReleasePrograms_Click(object sender, EventArgs e)
        {
            Logger.GetLogger().LogMessage("ReleasePrograms: Start");
            if (dataGridViewReleases.RowCount <= 0)
            {
                Logger.GetLogger().LogMessage("ReleasePrograms: No rows in the gui...");
                return; //No use to release nothing
            }

            bool ProductsFoundToRelease = false;

            if (!ValidateVersionNumbersInColumn(ref dataGridViewReleases,
                dataGridViewReleases[RELEASE_COL_VERSIONTORELEASE, 0].ColumnIndex,
                dataGridViewReleases[RELEASE_COL_RELEASE, 0].ColumnIndex))
            {
                Logger.GetLogger().LogMessage("ReleasePrograms: End");
                return;
            }

            //Change all version numbers
            //bool DoDummyCommitSinceOnlyTagBasedRelease = true;
            List<PowerShell> lstToExecute = new List<PowerShell>();
            for (int RowIndex = 0; RowIndex < dataGridViewReleases.Rows.Count; RowIndex++)
            {
                string strProduct = (string)dataGridViewReleases[RELEASE_COL_PRODUCT, RowIndex].Value;
                if ((bool)dataGridViewReleases[RELEASE_COL_RELEASE, RowIndex].Value)
                {
                    //if (!((bool)dataGridViewReleases[3, RowIndex].Value))
                    //    DoDummyCommitSinceOnlyTagBasedRelease = false; //We have program that actually has changed resources
                    ProductsFoundToRelease = true;
                    Logger.GetLogger().LogMessage(string.Format("ReleasePrograms: Product <{0}> marked for release", strProduct));

                    StringBuilder strbLogCommand = new StringBuilder();

                    PowerShell psTag = StaticTools.GetWriteHostDisabledPowershell();                    
                    strbLogCommand.AppendFormat("\"{0}\" ", System.IO.Path.Combine(_Model.InternalRepositoryPath, MyForceReleaser.FILELIST_INTERNAL[MyForceReleaser.FILE_INTERNAL_CREATETAG]));
                    psTag.AddCommand(System.IO.Path.Combine(_Model.InternalRepositoryPath, MyForceReleaser.FILELIST_INTERNAL[MyForceReleaser.FILE_INTERNAL_CREATETAG]));
                    psTag.Runspace.SessionStateProxy.Path.SetLocation(_Model.Git.GetWorkingDir().TrimEnd('\\'));

                    strbLogCommand.AppendFormat("-ProductNames {0} ", strProduct);
                    psTag.AddParameter("ProductNames", strProduct);

                    strbLogCommand.AppendFormat("-Version {0} ", (string)dataGridViewReleases[RELEASE_COL_VERSIONTORELEASE, RowIndex].Value);
                    psTag.AddParameter("Version", (string)dataGridViewReleases[RELEASE_COL_VERSIONTORELEASE, RowIndex].Value);

                    lstToExecute.Add(psTag);
                    strbLogCommand.Append(System.Environment.NewLine);

                    PowerShell psPush = StaticTools.GetWriteHostDisabledPowershell();
                    strbLogCommand.AppendFormat("\"{0}\" ", System.IO.Path.Combine(_Model.InternalRepositoryPath, MyForceReleaser.FILELIST_INTERNAL[MyForceReleaser.FILE_INTERNAL_CREATETAG]));
                    psPush.Runspace.SessionStateProxy.Path.SetLocation(_Model.Git.GetWorkingDir().TrimEnd('\\'));
                    psPush.AddCommand(System.IO.Path.Combine(_Model.InternalRepositoryPath, MyForceReleaser.FILELIST_INTERNAL[MyForceReleaser.FILE_INTERNAL_CREATETAG]));

                    strbLogCommand.AppendFormat("-ProductNames {0} ", strProduct);
                    psPush.AddParameter("ProductNames", strProduct);

                    strbLogCommand.AppendFormat("-Version {0} ", (string)dataGridViewReleases[RELEASE_COL_VERSIONTORELEASE, RowIndex].Value);
                    psPush.AddParameter("Version", (string)dataGridViewReleases[RELEASE_COL_VERSIONTORELEASE, RowIndex].Value);

                    strbLogCommand.Append("-Push=$true");
                    psPush.AddParameter("Push", true);

                    lstToExecute.Add(psPush);

                    Logger.GetLogger().LogMessage(string.Format("ReleasePrograms: Product <{0}> marked for release execute command <{1}>"
                        , strProduct
                        , strbLogCommand.ToString()));
                } 
                else
                    Logger.GetLogger().LogMessage(string.Format("ReleasePrograms: Product <{0}> isn't marked for release", strProduct));
            }

            if (!ProductsFoundToRelease)
            {
                string strWarning = "No products checked to release!";
                MessageBox.Show(strWarning);
                Logger.GetLogger().LogMessage(string.Format("ReleasePrograms: {0}", strWarning), Logger.LogType.lt_warning);
                return;
            }

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
                foreach (var ps in lstToExecute)
                {
                    progressBar.Increment(1);
                    ps.Invoke();
                }
                progressBar.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                progressBar.Visible = false;
                Logger.GetLogger().LogMessage(string.Format("ReleasePrograms: {0}", ex.Message), Logger.LogType.lt_error);
                return;
            }

            MessageBox.Show("Programs are released!");
            Logger.GetLogger().LogMessage("ReleasePrograms: Done");
            //Close the GUI
            Close();
        }

        //Data grid view events
        private void dataGridViewReleases_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dataGridViewReleases.Rows.Count <= 0)
                return; //No use if there are no rows

            if (e.ColumnIndex == dataGridViewReleases[RELEASE_COL_VERSIONTORELEASE, 0].ColumnIndex)
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    contextReleaseToSet.Show(Cursor.Position);
            }
            else if (e.ColumnIndex == dataGridViewReleases[RELEASE_COL_RELEASE, 0].ColumnIndex)
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    contextShouldRelease.Show(Cursor.Position);
            }
        }
        private void dataGridViewReleases_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dataGridViewReleases[RELEASE_COL_RELEASE, e.RowIndex].ColumnIndex)
            {
                var dataGridView = (DataGridView)sender;
                var cell = dataGridViewReleases[e.ColumnIndex, e.RowIndex];
                if (cell.Value == null)
                    cell.Value = false;
                cell.Value = !(bool)cell.Value;
            }
        }
        
        //Context menu actions
        private void ContextReleases_SetAllVersionNumbers_Click(object sender, EventArgs e)
        {
            bool bDone = true, bIsMasterBranch = _Model.Git.IsCurrentBranchMaster();
            string strVersionToSet = "";
            Point p = new Point(Cursor.Position.X, Cursor.Position.Y);
            do
            {
                bDone = true;
                InputBoxResult dlg = InputBox.Show("Please provide a version to set on all projects: ", "Version to set?", "X.X.X.X", p.X, p.Y);
                if (dlg.ReturnCode == System.Windows.Forms.DialogResult.OK)
                {
                    if (string.IsNullOrWhiteSpace(dlg.Text) || !StaticTools.IsValidVersionNumber(dlg.Text, bIsMasterBranch))
                    {
                        string strMessage = "Invalid version number! Please respect the format: X.X.X.X where (X = digits)!";
                        if (bIsMasterBranch)
                            strMessage += " Note: This is a master branch => You can only release the initial versions X.X.X.0!";
                        MessageBox.Show(strMessage);
                        bDone = false;
                    }
                    else
                        strVersionToSet = dlg.Text;
                }
            } while (!bDone);

            if (!string.IsNullOrWhiteSpace(strVersionToSet))
            {
                for (int index = 0; index < dataGridViewReleases.Rows.Count; index++)
                    dataGridViewReleases[RELEASE_COL_VERSIONTORELEASE, index].Value = strVersionToSet;
            }
        }
        private void ContextReleases_CheckAll_Click(object sender, EventArgs e)
        {
            for (int nIndex = 0; nIndex < dataGridViewReleases.Rows.Count; nIndex++)
                dataGridViewReleases[RELEASE_COL_RELEASE, nIndex].Value = true;
        }
        private void ContextReleases_UncheckAll_Click(object sender, EventArgs e)
        {
            for (int nIndex = 0; nIndex < dataGridViewReleases.Rows.Count; nIndex++)
                dataGridViewReleases[RELEASE_COL_RELEASE, nIndex].Value = false;
        }  
        #endregion

        #region VersionHistoryTab
        
        #region VERSIONHISTORYTAB_CONSTANTS
        private readonly string VERSIONHISTORY_COL_PRODUCT = "colVHProduct";
        private readonly string VERSIONHISTORY_COL_CURRENTVERSION = "colVHCurrentVersion";
        private readonly string VERSIONHISTORY_COL_VERSIONHISTORY = "colVHInternalVersionHistory";
        private readonly string VERSIONHISTORY_COL_VERSIONHISTORYUPDATED = "colVHVersionHistoryUpdated";
        #endregion

        private void UpdateVersionHistory_Click(object sender, EventArgs e)
        {
            Logger.GetLogger().LogMessage("UpdateVersionHistory: Start");

            if (dataGridViewVersionHist.Rows.Count < 0)
            {
                Logger.GetLogger().LogMessage("UpdateVersionHistory: No rows in the view... Stop pushing the damn button!");
                return; //No use to update rows
            }

            int nVersionHistoriesUpdated = 0;

            //Trigger selection changed to make sure version history is synced with data grid!
            dataGridViewVersionHist_SelectionChanged(null, new EventArgs());

            //Change all version numbers
            List<PowerShell> lstToExecute = new List<PowerShell>();
            for (int RowIndex = 0; RowIndex < dataGridViewVersionHist.Rows.Count; RowIndex++)
            {
                string strProduct = (string)dataGridViewVersionHist[VERSIONHISTORY_COL_PRODUCT, RowIndex].Value;
                string strVersionToSet = (string)dataGridViewVersionHist[VERSIONHISTORY_COL_CURRENTVERSION, RowIndex].Value;
                string strTrack = StaticTools.GetMainTrackVersionNumber(strVersionToSet);

                //Check if we need to save the version history file
                VersionHistoryCache cache = (VersionHistoryCache)dataGridViewVersionHist[VERSIONHISTORY_COL_VERSIONHISTORY, RowIndex].Value;
                if ((bool)dataGridViewVersionHist[VERSIONHISTORY_COL_VERSIONHISTORYUPDATED, RowIndex].Value && cache != null && cache.Version.Equals(strTrack))
                {
                    Logger.GetLogger().LogMessage(string.Format("UpdateVersionHistory: {0} Version history changed!", strProduct));
                    MyForceReleaser.SaveVersionHistoryFile(_Model, strProduct, strTrack, cache.HistoryText);
                    nVersionHistoriesUpdated++;
                }
                else
                    Logger.GetLogger().LogMessage(string.Format("UpdateVersionHistory: {0} Version history not changed!", strProduct));
            }
            MessageBox.Show(string.Format("{0}/{1} version history files have changed and are saved.", nVersionHistoriesUpdated, dataGridViewVersionHist.Rows.Count));

            //Commit the changes
            Logger.GetLogger().LogMessage("UpdateVersionHistory: Adding files & commit changes!");
            _Model.Git.RunGitCmd(string.Format("add \"{0}\"/", MyForceReleaser.GetVersionHistoryRelativeRootFolder()));
            _Model.Git.RunGitCmd("commit -am \"GitExtensions MyForce Releaser Plugin: Updated version history\"");

            Logger.GetLogger().LogMessage("UpdateVersionHistory: Done");

            //Close the GUI so user can verify his actions
            Close();
        }
        private void SyncVersionHistory(bool bFullReload = false)
        {
            bool bSelectionChanged = m_nVHCurrentSelectedVersionRow != m_nVHPreviousSelectedVersionRow;

            // Update current version history visual
            if (m_nVHCurrentSelectedVersionRow >= 0 && m_nVHCurrentSelectedVersionRow < dataGridViewVersionHist.Rows.Count)
            {
                //Process version history file
                string strProduct = (string)dataGridViewVersionHist[VERSIONHISTORY_COL_PRODUCT, m_nVHCurrentSelectedVersionRow].Value;
                string strTrack = StaticTools.GetMainTrackVersionNumber((string)dataGridViewVersionHist[VERSIONHISTORY_COL_CURRENTVERSION, m_nVHCurrentSelectedVersionRow].Value);

                //Assure cache object
                VersionHistoryCache cache = (VersionHistoryCache)dataGridViewVersionHist[VERSIONHISTORY_COL_VERSIONHISTORY, m_nVHCurrentSelectedVersionRow].Value;
                if (cache == null || !cache.Version.Equals(strTrack))
                {
                    //Reset content just in case the track didn't equals
                    dataGridViewVersionHist[VERSIONHISTORY_COL_VERSIONHISTORY, m_nVHCurrentSelectedVersionRow].Value = null;

                    //Get the file content and place 
                    string strFileContent = MyForceReleaser.ReadVersionHistoryFile(_Model, strProduct, strTrack);
                    if (!string.IsNullOrWhiteSpace(strFileContent))
                        dataGridViewVersionHist[VERSIONHISTORY_COL_VERSIONHISTORY, m_nVHCurrentSelectedVersionRow].Value = new VersionHistoryCache(strTrack, strFileContent);

                    cache = (VersionHistoryCache)dataGridViewVersionHist[VERSIONHISTORY_COL_VERSIONHISTORY, m_nVHCurrentSelectedVersionRow].Value;
                }
                txtCurVersionHistory.Text = cache != null ? cache.HistoryText : "";

                //Process previous version history file
                string strPrevPath = MyForceReleaser.GetVersionHistoryFilePathForProductForPreviousTrack(_Model, strProduct, strTrack);
                if (System.IO.File.Exists(strPrevPath))
                    txtPreviousVersionHistory.Text = string.Format("Path: {0}\n{1}", strPrevPath, System.IO.File.ReadAllText(strPrevPath));
                else
                    txtPreviousVersionHistory.Text = "No version history file found for the previous track!";
            }
            else
            {
                txtCurVersionHistory.Text = "";
                txtPreviousVersionHistory.Text = "";
            }

            // Update status for each row
            bool bRefreshAnyWay = false;
            for (int nRowIndex = 0; nRowIndex < dataGridViewVersionHist.Rows.Count; nRowIndex++)
            {
                bRefreshAnyWay = bFullReload || (bSelectionChanged && m_nVHPreviousSelectedVersionRow == nRowIndex); //Also update previous in case index changed
                string strProductName = (string)dataGridViewVersionHist[VERSIONHISTORY_COL_PRODUCT, nRowIndex].Value;
                string strTrack = StaticTools.GetMainTrackVersionNumber((string)dataGridViewVersionHist[VERSIONHISTORY_COL_CURRENTVERSION, nRowIndex].Value);
                VersionHistoryCache cache = (VersionHistoryCache)dataGridViewVersionHist[VERSIONHISTORY_COL_VERSIONHISTORY, nRowIndex].Value;

                if (cache != null && !cache.Version.Equals(strTrack))
                {
                    //Version were updated so cache doesn't fit anymore
                    if (nRowIndex != m_nVHCurrentSelectedVersionRow) //Not needed for current selection since that just got replaced during activating context for this product
                        dataGridViewVersionHist[VERSIONHISTORY_COL_VERSIONHISTORY, nRowIndex].Value = null;
                    bRefreshAnyWay = true;
                }

                //Only sync files for the current displayed item
                if (bRefreshAnyWay)
                {
                    dataGridViewVersionHist.Rows[nRowIndex].DefaultCellStyle.BackColor = dataGridViewVersionHist[VERSIONHISTORY_COL_VERSIONHISTORY, nRowIndex].Value == null && !MyForceReleaser.ProductHasVersionHistoryFile(_Model, strProductName, strTrack) ? Color.Red : Color.White;
                    
                    //if (MyForceReleaser.ProductHasVersionHistoryFile(_Model, strProductName, strTrack))
                    //{
                    //    //We have a version file => check if the history file was changed since last tag                    
                    //    string strOutput = _Model.Git.RunGitCmd(string.Format("diff {0}-{1} HEAD -- \"{2}\"", strProductName, dataGridViewVersionHist[VERSIONHISTORY_COL_CURRENTVERSION, nRowIndex].Value, MyForceReleaser.GetVersionHistoryRelativeFilePathForProduct(strProductName, strTrack)));
                    //    if (string.IsNullOrWhiteSpace(strOutput))
                    //        dataGridViewVersionHist.Rows[nRowIndex].DefaultCellStyle.BackColor = Color.Orange; //No changes since last tag
                    //    else
                    //        dataGridViewVersionHist.Rows[nRowIndex].DefaultCellStyle.BackColor = Color.White; //Changes since last tag                   
                    //}
                    //else
                    //    dataGridViewVersionHist.Rows[nRowIndex].DefaultCellStyle.BackColor = dataGridViewVersionHist[VERSIONHISTORY_COL_VERSIONHISTORY, nRowIndex].Value == null ? Color.Red : Color.Orange;
                }

                //Update the marker that tells if version history is updated or not (If there is no text yet => no component has been selected yet)
                dataGridViewVersionHist[VERSIONHISTORY_COL_VERSIONHISTORYUPDATED, nRowIndex].Value = cache != null ? MyForceReleaser.IsVersionHistoryChanged(_Model, strProductName, strTrack, cache.HistoryText) : false;
            }
        }
        private void SyncCommitMessages()
        {
            bool bSelectionChanged = m_nVHCurrentSelectedVersionRow != m_nVHPreviousSelectedVersionRow;
            if (bSelectionChanged)
            {
                // Update the list of non-filtered commit messages
                m_lstNonFilteredCommitMessages.Clear();
                if (m_nVHCurrentSelectedVersionRow >= 0 && m_nVHCurrentSelectedVersionRow < dataGridViewVersionHist.Rows.Count)
                {
                    //git log --pretty=oneline Cca-5.3.5.1..Cca-5.3.5.2
                    //7a3af222936702d3f5298b95c071a13b9ae86446 Changed Version Numbers to 5.3.5.2
                    //5af1d3eb5a913e9b1ea012dbeb8afac681b4f37a Merge branch 'Version-5.3.5' of https://github.com/Askia/ContactCentre into Version-5.3.5
                    //40f55635ac9107054dedc1449d43308e8571d7da 5.3.5 Kodim & Design Survey Service preliminary setups Added missing semi-colon to CcaConnectionPage.iss Set version numbers
                    //d482cdf68a87a6a4fce73b32942a34ce8b78c35f Auto-merge Subversion
                    //c0195fa7d5fb15fe356d040f55185292f9ced28b Merged revision(s) 36015-36099 from Project/branches/Version 5.3.4/Wave: 5.3.4.2 Preliminary checkin
                    //2f47ecbb9be160e15a5e0aa6b203757f80762438 all properties of ADC in property view supported
                    //4dde330d56330e32a9512f858f435e3cb6da77c3 Few devs on 5.3.5.X, property view for elements
                    //bed40145dc3b7eb21b638e0879c28e0a515b532c XML reading and writing of ADC properties moved to AskCmn
                    string strResult = _Model.Git.RunGitCmd(string.Format("log --pretty=oneline {0}-{1}..{2}", dataGridViewVersionHist[VERSIONHISTORY_COL_PRODUCT, m_nVHCurrentSelectedVersionRow].Value, dataGridViewVersionHist[VERSIONHISTORY_COL_CURRENTVERSION, m_nVHCurrentSelectedVersionRow].Value, _Model.Git.GetCurrentBranchName()));
                    if (strResult.ToLower().StartsWith("fatal:"))
                    {
                        //Do a second attempt since some of our tags are created with the v prefix before the version
                        strResult = _Model.Git.RunGitCmd(string.Format("log --pretty=oneline {0}-v{1}..{2}", dataGridViewVersionHist[VERSIONHISTORY_COL_PRODUCT, m_nVHCurrentSelectedVersionRow].Value, dataGridViewVersionHist[VERSIONHISTORY_COL_CURRENTVERSION, m_nVHCurrentSelectedVersionRow].Value, _Model.Git.GetCurrentBranchName()));
                    }

                    //Do some processing (Eg: Strip of commit ISH)
                    var Matches = Regex.Matches(strResult, @"^.*?\s+(.*)$", RegexOptions.Multiline);
                    foreach (Match match in Matches)
                    {
                        if (match.Groups.Count < 2)
                            continue;
                        m_lstNonFilteredCommitMessages.Add(match.Groups[1].Value);
                    }
                }
            }

            // Update the views

            // Remove any notice of commits/filters on the screen controls
            grpBoxCommitMessages.Text = grpBoxCommitMessages.Text.Replace(" --- (filtered)", "");
            toolTipCommitMessages.SetToolTip(grpBoxCommitMessages, "");
            txtCommitMessages.Text = "";

            //Try catch to handle the case where regexes are (still) invalid or incomplete
            try
            {
                //Filter the complete list of commit messages with filters the user has entered
                StringBuilder finalStr = new StringBuilder();
                StringBuilder finalStrFiltered = new StringBuilder();
                List<string> lstCommitMessageIgnoreRegexes = this.CommitMessageIgnoreRegexes;
                foreach (string Line in m_lstNonFilteredCommitMessages)
                {
                    bool bValid = true;
                    int nRegexesCount = lstCommitMessageIgnoreRegexes.Count;
                    while (bValid && --nRegexesCount >= 0)
                    {
                        //First bValid just tells us if we match
                        bValid = Regex.Match(Line, lstCommitMessageIgnoreRegexes[nRegexesCount], chkIgnoreCaseForRegex.Checked ? RegexOptions.IgnoreCase : RegexOptions.None).Success;

                        if (!chkInclusiveRegex.Checked)
                            bValid = !bValid; //Invert matched => In case of exclusive not matched is good!
                    }

                    if (bValid)
                        finalStr.AppendFormat(@"{0}{1}", Line, System.Environment.NewLine);
                    else
                        finalStrFiltered.AppendFormat(@"{0}{1}", Line, System.Environment.NewLine);
                }

                if (finalStrFiltered.Length > 0)
                {
                    //Filters active => Set tooltip & changed titles
                    if (grpBoxCommitMessages.Text.IndexOf(" --- (filtered)") < 0)
                        grpBoxCommitMessages.Text = grpBoxCommitMessages.Text + " --- (filtered)";
                    toolTipCommitMessages.SetToolTip(grpBoxCommitMessages, finalStrFiltered.ToString());
                }

                //Show the differences
                txtCommitMessages.Text = finalStr.ToString();
            }
            catch (Exception)
            { }
        }

        //Version history filtering
        private void txtCommitMessagesFilters_TextChanged(object sender, EventArgs e)
        {
            SyncCommitMessages();
        }
        private void chkRegexOption_CheckedChanged(object sender, EventArgs e)
        {
            SyncCommitMessages();
        }
        private void txtCurVersionHistory_TextChanged(object sender, EventArgs e)
        {
            if (dataGridViewVersionHist.SelectedCells.Count > 0)
            {
                //Assure object cache
                bool bHasTextOnScreen = !string.IsNullOrWhiteSpace(txtCurVersionHistory.Text);
                VersionHistoryCache cache = (VersionHistoryCache)dataGridViewVersionHist[VERSIONHISTORY_COL_VERSIONHISTORY, dataGridViewVersionHist.SelectedCells[0].RowIndex].Value;
                if (cache == null && bHasTextOnScreen)
                {
                    cache = new VersionHistoryCache(StaticTools.GetMainTrackVersionNumber((string)dataGridViewVersionHist[VERSIONHISTORY_COL_CURRENTVERSION, dataGridViewVersionHist.SelectedCells[0].RowIndex].Value));
                    dataGridViewVersionHist[VERSIONHISTORY_COL_VERSIONHISTORY, dataGridViewVersionHist.SelectedCells[0].RowIndex].Value = cache;
                }

                //Save text in cache
                if (bHasTextOnScreen)
                    cache.HistoryText = txtCurVersionHistory.Text;
            }
        }

        //Data grid view events
        private void dataGridViewVersionHist_SelectionChanged(object sender, EventArgs e)
        {
            m_nVHCurrentSelectedVersionRow = -1;
            if (dataGridViewVersionHist.SelectedCells.Count > 0)
                m_nVHCurrentSelectedVersionRow = dataGridViewVersionHist.SelectedCells[0].RowIndex;

            SyncVersionHistory();
            SyncCommitMessages();

            m_nVHPreviousSelectedVersionRow = m_nVHCurrentSelectedVersionRow;
        }  
        private void dataGridViewVersionHist_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dataGridViewVersionHist.Rows.Count <= 0)
                return; //No use if there are no rows

           if (e.ColumnIndex == dataGridViewVersionHist[VERSIONHISTORY_COL_VERSIONHISTORYUPDATED, 0].ColumnIndex)
           {
               if (e.Button == System.Windows.Forms.MouseButtons.Right)
                   contextVersionHistory.Show(Cursor.Position);
           }
        }
        private void dataGridViewVersionHist_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dataGridViewVersionHist.Rows.Count)
                return;

            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                if (e.ColumnIndex == dataGridViewVersionHist[VERSIONHISTORY_COL_VERSIONHISTORYUPDATED, e.RowIndex].ColumnIndex)
                    contextVersionHistory.Show(Cursor.Position);
            }
        }

        //Context menu's
        private void ContextVersionHistory_GenerateMissingVersionFiles_Click(object sender, EventArgs e)
        {
            Point pointOnScreen = new Point(contextVersionHistory.Bounds.Location.X, contextVersionHistory.Bounds.Location.Y);
            Point pointOnClient = dataGridViewVersionHist.PointToClient(pointOnScreen);
            DataGridView.HitTestInfo info = dataGridViewVersionHist.HitTest(pointOnClient.X, pointOnClient.Y);

            int nStartIndex = 0;
            int nCount = dataGridViewVersionHist.Rows.Count;

            //Check if specific row is selected
            if (info.RowIndex > 0)
            {
                //We support only single row selection => hence we can take the first selected item
                nStartIndex = info.RowIndex;
                nCount = 1;
            }

            for (int nIndex = nStartIndex; nIndex < nStartIndex + nCount; nIndex++)
            {
                string strProductName = (string)dataGridViewVersionHist[VERSIONHISTORY_COL_PRODUCT, nIndex].Value;
                string strVersion = (string)dataGridViewVersionHist[VERSIONHISTORY_COL_CURRENTVERSION, nIndex].Value;
                string strTrack = StaticTools.GetMainTrackVersionNumber(strVersion);
                if (!MyForceReleaser.ProductHasVersionHistoryFile(_Model, strProductName, strTrack))
                {
                    VersionHistoryCache cache = new VersionHistoryCache(strTrack, MyForceReleaser.GetDemoMarkDown(strProductName, strVersion));
                    dataGridViewVersionHist[VERSIONHISTORY_COL_VERSIONHISTORY, nIndex].Value = cache;
                }
            }

            if (nCount > 0)
                SyncVersionHistory(true);
        }
        private void ContextVersionHistory_ReplaceNotYetReleasedWithCurrentDate_Click(object sender, EventArgs e)
        {
            Point pointOnScreen = new Point(contextVersionHistory.Bounds.Location.X, contextVersionHistory.Bounds.Location.Y);
            Point pointOnClient = dataGridViewVersionHist.PointToClient(pointOnScreen);
            DataGridView.HitTestInfo info = dataGridViewVersionHist.HitTest(pointOnClient.X, pointOnClient.Y);

            int nStartIndex = 0;
            int nCount = dataGridViewVersionHist.Rows.Count;

            //Check if specific row is selected
            if (info.RowIndex >= 0)
            {
                //We support only single row selection => hence we can take the first selected item
                nStartIndex = info.RowIndex;
                nCount = 1;
            }

            //Update the version history for the requested rows
            for (int nRowIndex = nStartIndex; nRowIndex < nStartIndex + nCount; nRowIndex++)
            {
                VersionHistoryCache cache = (VersionHistoryCache)dataGridViewVersionHist[VERSIONHISTORY_COL_VERSIONHISTORY, nRowIndex].Value;
                if (cache == null)
                {
                    //In case where the cache is missing we need to load the cache from disk
                    cache = new VersionHistoryCache(StaticTools.GetMainTrackVersionNumber((string)dataGridViewVersionHist[VERSIONHISTORY_COL_CURRENTVERSION, nRowIndex].Value));
                    cache.HistoryText = MyForceReleaser.ReadVersionHistoryFile(_Model, (string)dataGridViewVersionHist[VERSIONHISTORY_COL_PRODUCT, nRowIndex].Value, StaticTools.GetMainTrackVersionNumber((string)dataGridViewVersionHist[VERSIONHISTORY_COL_CURRENTVERSION, nRowIndex].Value));
                    dataGridViewVersionHist[VERSIONHISTORY_COL_VERSIONHISTORY, nRowIndex].Value = cache;
                }
                if (!string.IsNullOrWhiteSpace(cache.HistoryText))
                    cache.HistoryText = Regex.Replace(cache.HistoryText, @"\(not yet released\)", string.Format("({0})", DateTime.Now.ToString("dd/MM/yyyy")), RegexOptions.IgnoreCase | RegexOptions.Multiline);
            }
            if (nCount > 0)
                SyncVersionHistory(true);
        }
        private void ContextVersionHistory_ResetVersionFiles_Click(object sender, EventArgs e)
        {
            Point pointOnScreen = new Point(contextVersionHistory.Bounds.Location.X, contextVersionHistory.Bounds.Location.Y);
            Point pointOnClient = dataGridViewVersionHist.PointToClient(pointOnScreen);
            DataGridView.HitTestInfo info = dataGridViewVersionHist.HitTest(pointOnClient.X, pointOnClient.Y);

            int nStartIndex = 0;
            int nCount = dataGridViewVersionHist.Rows.Count;

            //Check if specific row is selected
            if (info.RowIndex > 0)
            {
                //We support only single row selection => hence we can take the first selected item
                nStartIndex = info.RowIndex;
                nCount = 1;
            }

            //Update the version history for the requested rows
            for (int nRowIndex = nStartIndex; nRowIndex < nStartIndex + nCount; nRowIndex++)
                dataGridViewVersionHist[VERSIONHISTORY_COL_VERSIONHISTORY, nRowIndex].Value = null;

            if (nCount > 0)
                SyncVersionHistory(true);
        }
        
        #endregion

        #region Shared Tab Logic
        public void LoadProducts()
        {
            dataGridViewVersionHist.SelectionChanged -= dataGridViewVersionHist_SelectionChanged;

            //Clear the current products
            dataGridViewProducts.Rows.Clear();
            dataGridViewReleases.Rows.Clear();
            dataGridViewVersionHist.Rows.Clear();

            //Load all available tags
            string strAllTags = MyForceReleaser.GetTags(_Model);

            //Fill the grids
            string strErrors = "";
            Dictionary<string, List<KeyValuePair<string, bool>>> dicProductsToMaxVersionPerTrack = new Dictionary<string,List<KeyValuePair<string,bool>>>();
            bool bSucces = MyForceReleaser.GetLatestVersionNumbersForAllProducts(_Model, ref dicProductsToMaxVersionPerTrack, ref strErrors);
            if (!string.IsNullOrWhiteSpace(strErrors))
                MessageBox.Show(strErrors);

            if (!bSucces)
                return;

            foreach (var ProductToVersion in dicProductsToMaxVersionPerTrack)
            {
                string ProductName = ProductToVersion.Key;
                foreach (var VersionInfo in ProductToVersion.Value)
                {
                    bool bReleaseAbleProductsFound = false;

                    //Increment the next version number
                    string versionNumber = VersionInfo.Key;
                    bool bRetrievedFromTag = VersionInfo.Value;
                    if (StaticTools.IsValidVersionNumber(versionNumber))
                    {
                        bReleaseAbleProductsFound = true;
                        string strNextVersion = StaticTools.IncrementVersionNumber(versionNumber);

                        //Verify if the tag already exists => if not this product is ready to release
                        int nNewRowIndex = 0;
                        if (bRetrievedFromTag || !Regex.Match(strAllTags, "refs/tags/" + ProductName + "-v?" + versionNumber + @"\^{}").Success)
                        {
                            nNewRowIndex = dataGridViewReleases.Rows.Add();
                            dataGridViewReleases[RELEASE_COL_PRODUCT, nNewRowIndex].Value = ProductName;
                            dataGridViewReleases[RELEASE_COL_VERSIONTORELEASE, nNewRowIndex].Value = bRetrievedFromTag ? strNextVersion : versionNumber;
                            dataGridViewReleases[RELEASE_COL_RELEASE, nNewRowIndex].Value = !bRetrievedFromTag;
                            dataGridViewReleases[RELEASE_COL_INTERNALISTAGBASED, nNewRowIndex].Value = bRetrievedFromTag;
                        }
                        else
                        {
                            nNewRowIndex = dataGridViewProducts.Rows.Add();
                            dataGridViewProducts[PRODUCTS_COL_PRODUCT, nNewRowIndex].Value = ProductName;
                            dataGridViewProducts[PRODUCTS_COL_CURRENTVERSION, nNewRowIndex].Value = versionNumber;
                            dataGridViewProducts[PRODUCTS_COL_VERSIONTOSET, nNewRowIndex].Value = strNextVersion;
                            dataGridViewProducts[PRODUCTS_COL_UPDATEVERSION, nNewRowIndex].Value = false;
                            dataGridViewProducts[PRODUCTS_COL_VERSIONHISTORYUPDATED, nNewRowIndex].Value = false;
                        }

                        //Fill the version history tab
                        nNewRowIndex = dataGridViewVersionHist.Rows.Add();
                        dataGridViewVersionHist[VERSIONHISTORY_COL_PRODUCT, nNewRowIndex].Value = ProductName;
                        dataGridViewVersionHist[VERSIONHISTORY_COL_CURRENTVERSION, nNewRowIndex].Value = versionNumber;
                        dataGridViewVersionHist[VERSIONHISTORY_COL_VERSIONHISTORYUPDATED, nNewRowIndex].Value = false;
                    }

                    //Warn the user not any version could be parsed
                    if (!bReleaseAbleProductsFound)
                        MessageBox.Show("Couldn't determine any version for <" + ProductName + ">. Releasing will work but not for this product!");
                }
            }
            dataGridViewVersionHist.SelectionChanged += dataGridViewVersionHist_SelectionChanged;
            SyncVersionHistory(true);
        }
        private bool ValidateVersionNumbersInColumn(ref DataGridView gridView, 
            int nColumnIndexVersionToCheck, int nColumnIndexEnabled, int nColumnIndexRowIdentifier = 0)
        {
            if (nColumnIndexVersionToCheck < 0 || nColumnIndexVersionToCheck > gridView.ColumnCount
                || nColumnIndexRowIdentifier < 0 || nColumnIndexRowIdentifier > gridView.ColumnCount
                || nColumnIndexEnabled < 0 || nColumnIndexEnabled > gridView.ColumnCount)
                return false; //During testing this won't even work so should never get released. Hence no warning!

            bool bValid = true, bIsMasterBranch = _Model.Git.IsCurrentBranchMaster();
            int nRowIndex = gridView.Rows.Count;
            while (bValid && --nRowIndex >= 0)
                if ((bool)gridView[nColumnIndexEnabled, nRowIndex].Value)
                    bValid = StaticTools.IsValidVersionNumber(gridView[nColumnIndexVersionToCheck, nRowIndex].Value.ToString(), bIsMasterBranch);
  
            if (!bValid && nRowIndex >= 0 && nRowIndex < gridView.Rows.Count)
            {
                string strError = "Invalid version number <" + gridView[nColumnIndexVersionToCheck, nRowIndex].Value.ToString()
                    + "> found for <" + gridView[nColumnIndexRowIdentifier, nRowIndex].Value.ToString() + ">!";
                if (bIsMasterBranch)
                    strError += " Note: This is a master branch => you may only create/release the intital X.X.X.0 version tags!";
                MessageBox.Show(strError);
                Logger.GetLogger().LogMessage("ValidateVersionNumbersInColumn: " + strError);
            }
            return bValid;
        }


        #endregion        

        private void tabMainWindow_Selected(object sender, TabControlEventArgs e)
        {
            if (e.Action == TabControlAction.Selected)
            {
                //Build debug string
                string strInfo = string.Format("IsMasterBranch: {0}, IsFixBranch: {1}, IsVersionBranch: {2}",
                  _Model.Git.IsCurrentBranchMaster() ? "true" : "false"
                  , _Model.Git.IsCurrentBranchFixBranch() ? "true" : "false"
                  , _Model.Git.IsCurrentBranchVersionBranch() ? "true" : "false"
                );

                //Check if tab is allowed
                bool bForceOtherAllowedPage = false;
                if (e.TabPage.Name.Equals(tabVersionHistory.Name) && !tabVersionHistory.Enabled)
                {
                    //Not allowed to update version history
                    bForceOtherAllowedPage = true;
                    MessageBox.Show(string.Format("It's not allowed to update version history directly on a version branch! Please use the fix branches (or master for a new track)! {0}Info for developper: {1}"
                        , System.Environment.NewLine + System.Environment.NewLine
                        , strInfo));
                }
                else if (e.TabPage.Name.Equals(tabReleasing.Name) && !tabReleasing.Enabled)
                {
                    //Not allowed to release from current branch
                    bForceOtherAllowedPage = true;
                    MessageBox.Show(string.Format("It's only allowed to release from a version branch! {0}Info for the developper: {1}"
                        , System.Environment.NewLine + System.Environment.NewLine
                        , strInfo));
                }
                else if (e.TabPage.Name.Equals(tabVersions.Name) && !tabVersions.Enabled)
                {
                    //Not allowed to update version on current branch
                    bForceOtherAllowedPage = true;
                    MessageBox.Show(string.Format("It's only allowed to update versions on a version or a master branch! {0}Info for the developper: {1}"
                        , System.Environment.NewLine + System.Environment.NewLine
                        , strInfo));
                };

                if (bForceOtherAllowedPage)
                {
                    bool bFoundPage = false;
                    int nIndex = 0;
                    while (!bFoundPage && nIndex < tabMainWindow.TabPages.Count)
                    {
                        bFoundPage = tabMainWindow.TabPages[nIndex].Enabled;
                        nIndex++;
                    }

                    if (bFoundPage)
                        tabMainWindow.SelectedTab = tabMainWindow.TabPages[--nIndex];
                    else
                    {
                        MessageBox.Show("Unexpected error during tab selection!");
                        Close();
                    }
                }
            }
        }
    }
}
