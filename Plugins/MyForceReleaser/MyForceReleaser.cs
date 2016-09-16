using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Text.RegularExpressions;

namespace MyForceReleaser
{

    public class MyForceReleaser
    {
        public string InternalRepositoryPath { get; set; }
        public IMyForceGitModule Git { get; set; }

        private List<string> _CommitMessageIgnoreRegexes;
        public List<string> CommitMessageIgnoreRegexes 
        { 
            get
            {
                return _CommitMessageIgnoreRegexes;
            }
            set 
            {
                if (value == null)
                    _CommitMessageIgnoreRegexes.Clear();
                else
                    _CommitMessageIgnoreRegexes = value;
            } 
        }

        private readonly static bool IsUseCache = false;
        public MyForceReleaser(string InternalRepositoryPath, IMyForceGitModule GitModule)
        {
            this.InternalRepositoryPath = InternalRepositoryPath;
            Git = GitModule;
            _CommitMessageIgnoreRegexes = new List<string>();
        }
        
        public bool Validate(ref string strErrors)
        {
            bool bGood = true;
            string strGitVersionString = Git.RunGitCmd("--version");
            
            Regex regex = new Regex(@"((?:\d+.)+\d+).windows");
            Match match = regex.Match(strGitVersionString);
            bGood = match.Success && match.Groups.Count >= 2;
            if (bGood)
            {
                strGitVersionString = match.Groups[1].Value;
                bGood = StaticTools.IsValidVersionNumber(strGitVersionString); 
                if (bGood)
                {
                    bGood = StaticTools.CompareFileVersions("2.8.2", strGitVersionString) <= 0; // A <= B
                    if (!bGood)
                        strErrors = string.Format("Required git version <2.8.2>. Current version: <{0}>", strGitVersionString); 
                }
                else
                    strErrors = string.Format("Unable to parse git version! Version is not a valid version number: <{0}>", strGitVersionString);
            }
            else
                strErrors = string.Format("Unable to parse git version from <{0}>", strGitVersionString);
                
            return bGood;
        }

        public static string ReadVersionHistoryFile(MyForceReleaser _Model, string strProduct, string strRequestedTrack)
        {
            string strToReturn = null;
            strProduct = GetVersionHistoryFilePathForProduct(_Model, strProduct, strRequestedTrack);
            if (System.IO.File.Exists(strProduct))
                strToReturn = System.IO.File.ReadAllText(strProduct);
            return strToReturn;
        }
        public static void SaveVersionHistoryFile(MyForceReleaser _Model, string strProduct, string strRequestedTrack, string strCurrentText)
        {
            strProduct = GetVersionHistoryFilePathForProduct(_Model, strProduct, strRequestedTrack);
            if (System.IO.File.Exists(strProduct))
                System.IO.File.Delete(strProduct);
            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(strProduct));
            System.IO.File.WriteAllText(strProduct, strCurrentText);
        }
        public static bool ProductHasVersionHistoryFile(MyForceReleaser _Model, string strProduct, string strRequestedTrack)
        {
            strProduct = GetVersionHistoryFilePathForProduct(_Model, strProduct, strRequestedTrack);
            return System.IO.File.Exists(strProduct);
        }
        public static string GetVersionHistoryFilePathForProductForPreviousTrack(MyForceReleaser _Model, string strProduct, string strCurrentTrack)
        {
            string strPath = GetVersionHistoryFilePathForProduct(_Model, strProduct, "*");
            string strMaxTrack = "";
            try
            {
                string[] arrFoundFiles = System.IO.Directory.GetFiles(System.IO.Path.GetDirectoryName(strPath), System.IO.Path.GetFileName(strPath));
                string strFileTrack = "";
                string strRegex = string.Format("{0}-(.*?).md", strProduct);
                foreach (var FilePath in arrFoundFiles)
                {
                    Match result = Regex.Match(FilePath, strRegex, RegexOptions.IgnoreCase);
                    if (result.Success && result.Groups.Count > 1)
                    {
                        strFileTrack = result.Groups[1].Value;
                        if (StaticTools.CompareFileVersions(strFileTrack, strCurrentTrack) < 0)
                            if (string.IsNullOrWhiteSpace(strMaxTrack) || StaticTools.CompareFileVersions(strFileTrack, strMaxTrack) > 0)
                                strMaxTrack = strFileTrack;
                    }
                }
            }
            catch (Exception) { };            
            return !string.IsNullOrWhiteSpace(strMaxTrack) ? GetVersionHistoryFilePathForProduct(_Model, strProduct, strMaxTrack) : "";
        }
        public static string GetVersionHistoryFilePathForProduct(MyForceReleaser _Model, string strProduct, string strRequestedTrack)
        {
            return System.IO.Path.Combine(_Model.Git.GetWorkingDir(), GetVersionHistoryRelativeFilePathForProduct(strProduct, strRequestedTrack));
        }
        public static string GetVersionHistoryRelativeFilePathForProduct(string strProduct, string strRequestedTrack)
        {
            return string.Format("{0}\\{1}-{2}.md", MyForceReleaser.GetVersionHistoryRelativeRootFolder(), strProduct, strRequestedTrack);
        }
        public static string GetVersionHistoryRelativeRootFolder()
        {
            return "VersionHistory";
        }
        public static bool IsVersionHistoryChanged(MyForceReleaser _Model, string strProduct, string strRequestedTrack, string strCurrentText)
        {
            string strOnDisk = MyForceReleaser.ReadVersionHistoryFile(_Model, strProduct, strRequestedTrack);
            return string.IsNullOrWhiteSpace(strOnDisk) ? !string.IsNullOrWhiteSpace(strCurrentText) : !strCurrentText.Equals(strOnDisk);
        }
        public static string GetDemoMarkDown(string strProductName, string strVersionToSet)
        {
            int nLengthPos1 = -1, nLengthPos2 = -1;
            StringBuilder builder = new StringBuilder();
            //Header
            builder.Append(strProductName);
            builder.Append(" version history");
            nLengthPos1 = builder.Length;
            builder.Append("\n");
            
            //Header underline
            for (int nIndex = 0; nIndex < nLengthPos1; nIndex++)
                builder.Append("=");
            builder.Append("\n");
           
            int nVersionsToPrint = 1;
            int nDemoVersionCount = nVersionsToPrint;
            do
            {
                string strDemoVersion = strVersionToSet;
                for (int nToIncrement = 1; nToIncrement < nDemoVersionCount; nToIncrement++)
                    strDemoVersion = StaticTools.IncrementVersionNumber(strDemoVersion);

                //Blank
                builder.Append("\n");
                
                //Version specific header
                nLengthPos1 = builder.Length;
                builder.Append("Version ");
                builder.Append(strDemoVersion);
                builder.Append(" (Not yet released");
                //builder.Append(System.DateTime.Now.AddDays(nDemoVersionCount - nVersionsToPrint).ToString("dd/MM/yyyy"));
                builder.Append(")");
                nLengthPos2 = builder.Length;
                builder.Append("\n");
                for (int nIndex = nLengthPos1; nIndex < nLengthPos2; nIndex++)
                    builder.Append("-");
                builder.Append("\n");

                //Version info
                builder.Append("- This a demo bug/feature description");
                builder.Append("\n");
                builder.Append("- This is another bug/feature description");
                builder.Append("\n");
                builder.Append("  - This is a subitem of the previous bug/feature description");
                builder.Append("\n");
                builder.Append("- This is a final item on the root level");
                builder.Append("\n");

            } while (--nDemoVersionCount > 0);

            return builder.ToString();
        }

        #region Files
        //Files that should be available in the "Internal" repository
        public static readonly string FILE_INTERNAL_VALIDPRODUCTS = "ValidProducts.ps1";
        public static readonly string FILE_INTERNAL_CHANGEVERSIONS = "ChangeVersions.ps1";
        public static readonly string FILE_INTERNAL_CREATETAG = "CreateTag.ps1";
        public static Dictionary<string, string> FILELIST_INTERNAL = new Dictionary<string, string>
            {                
                { FILE_INTERNAL_CREATETAG, @"BuildScripts\SanityChecks\CreateTag.ps1" }
                , { FILE_INTERNAL_VALIDPRODUCTS, @"BuildScripts\SanityChecks\ValidProducts.ps1" }
                , { FILE_INTERNAL_CHANGEVERSIONS, @"UsefulScripts\ChangeVersions.ps1" }
            };

        //Files that should be available in the repository we wish to release
        public static readonly string FILE_REPO_MAPPRODUCTNAMESTORESOURCEFILES = "MapProductNamesToResourceFiles.ps1";
        public static Dictionary<string, string> FILELIST_REPO = new Dictionary<string, string>
            {
                { FILE_REPO_MAPPRODUCTNAMESTORESOURCEFILES, @"BuildScripts\MapProductNamesToResourceFiles.ps1" }
            };

        public static bool AllInternalFilesArePresent(string strInternalRepo, string strToReleaseRepo, ref string strErrors)
        {
            strErrors = "";

            // Check the Internal repo is configured
            if (string.IsNullOrEmpty(strInternalRepo) || !System.IO.Directory.Exists(strInternalRepo))
            {
                strErrors = string.Format("Internal repository cannot be found! Location checked: <{0}>", strInternalRepo);
                return false;
            }

            // Check the release repo is valid
            if (string.IsNullOrEmpty(strToReleaseRepo) || !System.IO.Directory.Exists(strToReleaseRepo))
            {
                strErrors = string.Format("No repository openend, please open an repository first! Location checked: <{0}>", strToReleaseRepo);
                return false;
            }

            // Verify the internal files we need
            foreach (var File in FILELIST_INTERNAL.Values)
            {
                if (!System.IO.File.Exists(System.IO.Path.Combine(strInternalRepo, File)))
                    strErrors = string.Concat(strErrors, string.Format("Found missing file in the Internal repository: {0}", File), System.Environment.NewLine);
            }
            return string.IsNullOrWhiteSpace(strErrors);
        }
        
        //We cache these things since these can't change during run of plug-in
        private static Dictionary<string, List<string>> _ResourceFilesPerProduct = new Dictionary<string, List<string>>();
        private static Dictionary<string, List<string>> _SqlFilePerProduct = new Dictionary<string, List<string>>();
        public static void LoadResourceMapsForValidProducts(MyForceReleaser _Model, ref Dictionary<string, List<string>> ResourceFilesPerProduct, ref Dictionary<string, List<string>> SqlFilePerProduct)
        {
            if (!IsUseCache || (_ResourceFilesPerProduct.Count == 0 && _SqlFilePerProduct.Count == 0))
            {
                _ResourceFilesPerProduct.Clear();
                _SqlFilePerProduct.Clear();

                // Create a runspace to host the PowerScript environment:
                Runspace runspace = System.Management.Automation.Runspaces.RunspaceFactory.CreateRunspace();
                runspace.Open();

                // Using the runspace, create a new pipeline for your cmdlets:
                Pipeline pipeline = runspace.CreatePipeline();

                // Invoke the file
                string cmd = System.IO.Path.Combine(_Model.Git.GetWorkingDir(), FILELIST_REPO[FILE_REPO_MAPPRODUCTNAMESTORESOURCEFILES]);
                if (System.IO.File.Exists(cmd))
                {
                    pipeline.Commands.AddScript("& \"" + cmd + "\"");
                    ICollection<PSObject> collection = pipeline.Invoke();

                    //Load the resource files for each product
                    _ResourceFilesPerProduct.Clear();
                    System.Collections.Hashtable ProductsToResources = (System.Collections.Hashtable)runspace.SessionStateProxy.GetVariable("global:MapProductNamesToResourceFiles");
                    if (ProductsToResources != null)
                    {
                        foreach (var Repo in ProductsToResources.Keys)
                        {
                            //Add key
                            _ResourceFilesPerProduct.Add(Repo.ToString(), new List<string>());

                            //Add values
                            object[] Products = (object[])ProductsToResources[Repo];
                            foreach (var Product in Products)
                                _ResourceFilesPerProduct[Repo.ToString()].Add(Product.ToString());
                        }
                    }

                    // Load the SQL files for each product
                    _SqlFilePerProduct.Clear();
                    System.Collections.Hashtable ProductsToSqlFiles = (System.Collections.Hashtable)runspace.SessionStateProxy.GetVariable("global:MapProductNamesToResourceFiles_SqlList");
                    if (ProductsToSqlFiles != null)
                    {
                        foreach (var Repo in ProductsToSqlFiles.Keys)
                        {
                            //Add key
                            _SqlFilePerProduct.Add(Repo.ToString(), new List<string>());

                            //Add values
                            object[] Products = (object[])ProductsToSqlFiles[Repo];
                            foreach (var Product in Products)
                                _SqlFilePerProduct[Repo.ToString()].Add(Product.ToString());
                        }
                    }
                }
                runspace.Close();
            }

            //Copy the items
            ResourceFilesPerProduct.Clear();
            SqlFilePerProduct.Clear();

            foreach (var item in _ResourceFilesPerProduct)
                ResourceFilesPerProduct.Add(item.Key, item.Value);

            foreach (var item in _SqlFilePerProduct)
                SqlFilePerProduct.Add(item.Key, item.Value);
        }

        private static Dictionary<string, List<string>> _LoadValidProducts = new Dictionary<string, List<string>>();
        public static Dictionary<string, List<string>> LoadValidProducts(MyForceReleaser _Model)
        {
            if (!IsUseCache || _LoadValidProducts.Count <= 0)
            {
                _LoadValidProducts.Clear();

                // Create a runspace to host the PowerScript environment:
                Runspace runspace = System.Management.Automation.Runspaces.RunspaceFactory.CreateRunspace();
                runspace.Open();
                // Using the runspace, create a new pipeline for your cmdlets:
                Pipeline pipeline = runspace.CreatePipeline();

                string cmd = System.IO.Path.Combine(_Model.InternalRepositoryPath, FILELIST_INTERNAL[FILE_INTERNAL_VALIDPRODUCTS]);
                if (System.IO.File.Exists(cmd))
                {
                    pipeline.Commands.AddScript("& \"" + cmd + "\"");
                    ICollection<PSObject> collection = pipeline.Invoke();

                    System.Collections.Hashtable psValidProducts = (System.Collections.Hashtable)runspace.SessionStateProxy.GetVariable("global:ValidProducts");
                    if (psValidProducts != null)
                    {
                        foreach (var Repo in psValidProducts.Keys)
                        {
                            //Add key
                            _LoadValidProducts.Add(Repo.ToString(), new List<string>());

                            //Add values
                            object[] Products = (object[])psValidProducts[Repo];
                            foreach (var Product in Products)
                                _LoadValidProducts[Repo.ToString()].Add(Product.ToString());
                        }
                    }
                }
                runspace.Close();
            }
            return _LoadValidProducts;
        }
        public static Dictionary<string, List<string>> LoadResourceFilesForValidProducts(MyForceReleaser _Model)
        {
            Dictionary<string, List<string>> toReturn = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> toReturn2 = new Dictionary<string, List<string>>();
            LoadResourceMapsForValidProducts(_Model, ref toReturn, ref toReturn2);
            return toReturn;
        }
        public static Dictionary<string, List<string>> LoadResourceSqlFilesForValidProducts(MyForceReleaser _Model)
        {
            Dictionary<string, List<string>> toReturn = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> toReturn2 = new Dictionary<string, List<string>>();
            LoadResourceMapsForValidProducts(_Model, ref toReturn, ref toReturn2);
            return toReturn2;
        }
        #endregion

        #region Git
        private static string _GetRemoteBranchName;
        public static string GetRemoteBranchName(MyForceReleaser _Model)
        {
            if (!IsUseCache || string.IsNullOrWhiteSpace(_GetRemoteBranchName))
                _GetRemoteBranchName = _Model.Git.RunGitCmd("rev-parse --abbrev-ref --symbolic-full-name @{u}").Replace("origin/", "").TrimEnd('\n');
            return _GetRemoteBranchName;
        }

        private static string _GetTags;
        public static string GetTags(MyForceReleaser _Model)
        {
            if (!IsUseCache || string.IsNullOrWhiteSpace(_GetTags))
                _GetTags = _Model.Git.RunGitCmd("ls-remote");
            return _GetTags;
        }

        private static string _GetRepoName;
        public static string GetRepoName(MyForceReleaser _Model, ref string strErrors)
        {
            if (!IsUseCache || string.IsNullOrWhiteSpace(_GetRepoName))
            {
                string originURL = _Model.Git.RunGitCmd("remote get-url origin");
                Regex regex = new Regex(@"^.*?([^/]+)$");
                Match match = regex.Match(originURL);
                if (!match.Success || match.Groups.Count < 2)
                {
                    strErrors = string.Format("Unable to parse repo name from: {0}", originURL);
                    return "";
                }
                _GetRepoName = "";
                try
                {
                    _GetRepoName = System.IO.Path.GetFileNameWithoutExtension(match.Groups[1].Value.Trim());
                }
                catch (Exception ex)
                {
                    strErrors = ex.Message;
                    return "";
                }
            }
            return _GetRepoName;
        }

        private static Dictionary<string, List<KeyValuePair<string, bool>>> _GetLatestVersionNumbersForAllProducts = new Dictionary<string, List<KeyValuePair<string, bool>>>();
        public static bool GetLatestVersionNumbersForAllProducts(MyForceReleaser _Model, ref Dictionary<string, List<KeyValuePair<string, bool>>> dicToFill, ref string strErrorsAndWarnings)
        {
            if (!IsUseCache || _GetLatestVersionNumbersForAllProducts.Count <= 0)
            {
                _GetLatestVersionNumbersForAllProducts.Clear();

                //Determine the repo name from the remote URL (Eg: Bison, ContactCentre, ...
                string repoName = MyForceReleaser.GetRepoName(_Model, ref strErrorsAndWarnings);
                if (!string.IsNullOrWhiteSpace(strErrorsAndWarnings))
                    return false;

                //Load the valid products from the internal repo
                var ValidProducts = MyForceReleaser.LoadValidProducts(_Model);
                if (ValidProducts.Count <= 0)
                {
                    strErrorsAndWarnings = string.Format("Unable to load ValidProducts from the internal repo! Please verify the file (internal repo): <{0}>", MyForceReleaser.FILELIST_INTERNAL[MyForceReleaser.FILE_INTERNAL_VALIDPRODUCTS]);
                    return false;
                }
                if (!ValidProducts.ContainsKey(repoName))
                {
                    strErrorsAndWarnings = string.Format("Repository: <{0}> not found in the list of valid products!", repoName);
                    return false;
                }

                //Load the resource files for each valid product
                Dictionary<string, List<string>> dicResourceFiles = new Dictionary<string, List<string>>();
                Dictionary<string, List<string>> dicSqlFiles = new Dictionary<string, List<string>>();
                MyForceReleaser.LoadResourceMapsForValidProducts(_Model, ref dicResourceFiles, ref dicSqlFiles);
                if (dicResourceFiles.Count <= 0 && dicSqlFiles.Count <= 0)
                    strErrorsAndWarnings = string.Format("Unable to load resource files for this repo! Please verify the file (current repo): <{0}>{1}", MyForceReleaser.FILELIST_REPO[MyForceReleaser.FILE_REPO_MAPPRODUCTNAMESTORESOURCEFILES], System.Environment.NewLine);

                //Load all available tags
                string strAllTags = MyForceReleaser.GetTags(_Model);

                //Fill the dictionary
                foreach (var ProductName in ValidProducts[repoName])
                {
                    bool bRetrievedFromTag = false;
                    Dictionary<string, string> dicTrackMaxVersion = new Dictionary<string, string>();

                    //Check if there are any regular resource files where we can get versions from
                    if (dicResourceFiles.ContainsKey(ProductName) && dicResourceFiles[ProductName].Count > 0)
                    {
                        string FilePath = System.IO.Path.Combine(_Model.Git.GetWorkingDir(), dicResourceFiles[ProductName][0]);
                        string strCurrentVersion = "";
                        if (FilePath.ToLower().EndsWith(".rc"))
                        {
                            //Parse from cpp file
                            strCurrentVersion = StaticTools.FindInFile(FilePath, @"FILEVERSION\s+((?:\d+,)+\d+)", 1).Replace(',', '.');
                        }
                        else
                        {
                            //Parse from assembly info
                            strCurrentVersion = StaticTools.FindInFile(FilePath, "\\[assembly:\\s+AssemblyVersion\\(\"([^\"\\*]*)\"\\)\\]", 1);
                        }
                        dicTrackMaxVersion.Add(StaticTools.GetMainTrackVersionNumber(strCurrentVersion), strCurrentVersion);
                    }
                    else
                    {
                        //No regular resource files... Maybe sql files with versions in it ?
                        if (dicSqlFiles.ContainsKey(ProductName))
                        {
                            string FilePath = System.IO.Path.Combine(_Model.Git.GetWorkingDir(), dicSqlFiles[ProductName][0]);
                            string strCurrentVersion = StaticTools.FindInFile(FilePath, @"((?:\d+\.){3}\d+)", 1).Replace(',', '.');
                            dicTrackMaxVersion.Add(StaticTools.GetMainTrackVersionNumber(strCurrentVersion), strCurrentVersion);
                        }
                        else
                        {
                            //No resource or SQL => We use the version from the previous tag
                            bRetrievedFromTag = true;
                            string strRemoteBranch = MyForceReleaser.GetRemoteBranchName(_Model);
                            if (!string.IsNullOrWhiteSpace(strRemoteBranch))
                            {
                                //Parse version from the current branch                            
                                bool bSpecificVersionFound = strRemoteBranch.Contains("Version-");
                                string strRemoteVersion = "";
                                if (bSpecificVersionFound)
                                    strRemoteVersion = strRemoteBranch.Replace("Version-", "") + "."; //Find specific track version number
                                else
                                    strRemoteVersion = @"(?:\d+.)+"; //Find any version number

                                //If we don't have a specific version warn the user we aren't working on specific version!
                                if (!bSpecificVersionFound)
                                    strErrorsAndWarnings += string.Format("No version found for product <{0}> for branch <{1}>! Please be sure this is intended!{2}", ProductName, strRemoteBranch, System.Environment.NewLine);

                                var matches = Regex.Matches(strAllTags, "refs/tags/" + ProductName + "-v?(" + strRemoteVersion + @"\d+)\^{}");
                                foreach (Match tagmatch in matches)
                                {
                                    string strCurrentTagMatch = "";
                                    if (tagmatch.Groups.Count >= 2)
                                        strCurrentTagMatch = tagmatch.Groups[1].Value.Trim();

                                    if (StaticTools.IsValidVersionNumber(strCurrentTagMatch))
                                    {
                                        //Extract the track
                                        string strTrack = StaticTools.GetMainTrackVersionNumber(strCurrentTagMatch);
                                        if (!dicTrackMaxVersion.ContainsKey(strTrack))
                                            dicTrackMaxVersion.Add(strTrack, strCurrentTagMatch);
                                        else if (StaticTools.CompareFileVersions(dicTrackMaxVersion[strTrack], strCurrentTagMatch) < 0)
                                            dicTrackMaxVersion[strTrack] = strCurrentTagMatch;
                                    }
                                }
                            }
                        }
                    }

                    foreach (var strActualVersion in dicTrackMaxVersion)
                    {
                        //Increment the next version number
                        string versionNumber = strActualVersion.Value;
                        if (StaticTools.IsValidVersionNumber(versionNumber))
                        {
                            if (!_GetLatestVersionNumbersForAllProducts.ContainsKey(ProductName))
                                _GetLatestVersionNumbersForAllProducts.Add(ProductName, new List<KeyValuePair<string, bool>>());
                            _GetLatestVersionNumbersForAllProducts[ProductName].Add(new KeyValuePair<string, bool>(versionNumber, bRetrievedFromTag));
                        }
                    }
                }
            }

            dicToFill.Clear();
            foreach (var item in _GetLatestVersionNumbersForAllProducts)
                dicToFill.Add(item.Key, item.Value);
            return true;
        }
        #endregion
    }
}
