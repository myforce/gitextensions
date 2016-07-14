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
        public string InteralRepositoryPath { get; set; }
        public IMyForceGitModule Git { get; set; }

        public MyForceReleaser(string InternalRepositoryPath, IMyForceGitModule GitModule)
        {
            InteralRepositoryPath = InternalRepositoryPath;
            Git = GitModule;
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
                    bGood = StaticTools.CompareFileVersions(strGitVersionString, "2.8.2") <= 0; // A <= B
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
                strErrors = string.Format("Repository that should be released cannot be found! Location checked: <{0}>", strToReleaseRepo);
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
        public static void LoadResourceMapsForValidProducts(MyForceReleaser _Model, ref Dictionary<string, List<string>> ResourceFilesPerProduct, ref Dictionary<string, List<string>> SqlFilePerProduct)
        {
            // Create a runspace to host the PowerScript environment:
            Runspace runspace = System.Management.Automation.Runspaces.RunspaceFactory.CreateRunspace();
            runspace.Open();

            // Using the runspace, create a new pipeline for your cmdlets:
            Pipeline pipeline = runspace.CreatePipeline();

            // Invoke the file
            string cmd = System.IO.Path.Combine(_Model.Git.GetWorkingDir(), FILELIST_REPO[FILE_REPO_MAPPRODUCTNAMESTORESOURCEFILES]);
            if (System.IO.File.Exists(cmd))
            {
                pipeline.Commands.AddScript(cmd);
                ICollection<PSObject> collection = pipeline.Invoke();

                //Load the resource files for each product
                ResourceFilesPerProduct.Clear();
                System.Collections.Hashtable ProductsToResources = (System.Collections.Hashtable)runspace.SessionStateProxy.GetVariable("global:MapProductNamesToResourceFiles");
                if (ProductsToResources != null)
                {
                    foreach (var Repo in ProductsToResources.Keys)
                    {
                        //Add key
                        ResourceFilesPerProduct.Add(Repo.ToString(), new List<string>());

                        //Add values
                        object[] Products = (object[])ProductsToResources[Repo];
                        foreach (var Product in Products)
                            ResourceFilesPerProduct[Repo.ToString()].Add(Product.ToString());
                    }
                }

                // Load the SQL files for each product
                SqlFilePerProduct.Clear();
                System.Collections.Hashtable ProductsToSqlFiles = (System.Collections.Hashtable)runspace.SessionStateProxy.GetVariable("global:MapProductNamesToResourceFiles_SqlList");
                if (ProductsToSqlFiles != null)
                {
                    foreach (var Repo in ProductsToSqlFiles.Keys)
                    {
                        //Add key
                        SqlFilePerProduct.Add(Repo.ToString(), new List<string>());

                        //Add values
                        object[] Products = (object[])ProductsToSqlFiles[Repo];
                        foreach (var Product in Products)
                            SqlFilePerProduct[Repo.ToString()].Add(Product.ToString());
                    }
                }
            }
            runspace.Close();
        }
        #endregion

        public static Dictionary<string, List<string>> LoadValidProducts(MyForceReleaser _Model)
        {
            Dictionary<string, List<string>> toReturn = new Dictionary<string, List<string>>();

            // Create a runspace to host the PowerScript environment:
            Runspace runspace = System.Management.Automation.Runspaces.RunspaceFactory.CreateRunspace();
            runspace.Open();
            // Using the runspace, create a new pipeline for your cmdlets:
            Pipeline pipeline = runspace.CreatePipeline();

            string cmd = System.IO.Path.Combine(_Model.InteralRepositoryPath, FILELIST_INTERNAL[FILE_INTERNAL_VALIDPRODUCTS]);
            if (System.IO.File.Exists(cmd))
            {
                pipeline.Commands.AddScript(cmd);
                ICollection<PSObject> collection = pipeline.Invoke();

                System.Collections.Hashtable psValidProducts = (System.Collections.Hashtable)runspace.SessionStateProxy.GetVariable("global:ValidProducts");
                if (psValidProducts != null)
                {
                    foreach (var Repo in psValidProducts.Keys)
                    {
                        //Add key
                        toReturn.Add(Repo.ToString(), new List<string>());

                        //Add values
                        object[] Products = (object[])psValidProducts[Repo];
                        foreach (var Product in Products)
                            toReturn[Repo.ToString()].Add(Product.ToString());
                    }
                }
            }
            runspace.Close();
            return toReturn;
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

        public static string GetRemoteBranchName(MyForceReleaser _Model)
        {
            return _Model.Git.RunGitCmd("rev-parse --abbrev-ref --symbolic-full-name @{u}").Replace("origin/", "").TrimEnd('\n');
        }
        public static string GetTags(MyForceReleaser _Model)
        {
            return _Model.Git.RunGitCmd("ls-remote");
        }
    }
}
