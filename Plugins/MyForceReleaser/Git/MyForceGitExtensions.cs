using GitUIPluginInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyForceReleaser
{
    public class MyForceGitExtensions : IMyForceGitModule
    {
        private IGitModule _Git { get; set; }

        public MyForceGitExtensions(IGitModule GitModule)
        {
            _Git = GitModule;
        }

        public string GetCurrentBranchName()
        {
            return _Git.GetSelectedBranch();
        }

        public string GetWorkingDir()
        {
            return _Git.WorkingDir;
        }

        public string RunGitCmd(string strCMD)
        {
            return _Git.RunGitCmd(strCMD);
        }


        public bool IsCurrentBranchMaster()
        {
            string strCurrentBranchName = GetCurrentBranchName();
            return !string.IsNullOrWhiteSpace(strCurrentBranchName)
                && System.Text.RegularExpressions.Regex.Match(strCurrentBranchName, "master", System.Text.RegularExpressions.RegexOptions.IgnoreCase).Success;
        }

        public bool IsCurrentBranchFixBranch()
        {
            string strCurrentBranchName = GetCurrentBranchName();
            return !string.IsNullOrWhiteSpace(strCurrentBranchName)
                && System.Text.RegularExpressions.Regex.Match(strCurrentBranchName, @"fix-(\d+\.)+\d+", System.Text.RegularExpressions.RegexOptions.IgnoreCase).Success;
        }

        public bool IsCurrentBranchVersionBranch()
        {
            string strCurrentBranchName = GetCurrentBranchName();
            return !string.IsNullOrWhiteSpace(strCurrentBranchName)
                && System.Text.RegularExpressions.Regex.Match(strCurrentBranchName, @"version-(\d+\.)+\d+", System.Text.RegularExpressions.RegexOptions.IgnoreCase).Success;
        }
    }
}
