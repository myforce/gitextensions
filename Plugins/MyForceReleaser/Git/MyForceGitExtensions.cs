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
            string strBranchName = _Git.GetSelectedBranch();
            Logger.GetLogger().LogMessage(string.Format("GIT: GetCurrentBranchName() => {0}", strBranchName));
            return strBranchName;
        }

        public string GetWorkingDir()
        {
            string strWorkDir = _Git.WorkingDir;
            Logger.GetLogger().LogMessage(string.Format("GIT: GetWorkingDir() => {0}", strWorkDir));
            return strWorkDir;
        }

        public string RunGitCmd(string strCMD)
        {
            string strOutPut = _Git.RunGitCmd(strCMD);
            Logger.GetLogger().LogMessage(string.Format("GIT: RunGitCmd({0}) => {1}", strCMD, strOutPut, strOutPut.StartsWith("fatal") ? Logger.LogType.lt_error : Logger.LogType.lt_info));
            return strOutPut;
        }

        public bool IsCurrentBranchMaster()
        {
            string strCurrentBranchName = GetCurrentBranchName();
            bool bResult = !string.IsNullOrWhiteSpace(strCurrentBranchName)
                && System.Text.RegularExpressions.Regex.Match(strCurrentBranchName, "master", System.Text.RegularExpressions.RegexOptions.IgnoreCase).Success;
            Logger.GetLogger().LogMessage(string.Format("GIT: IsCurrentBranchMaster() => {0}", bResult ? "true" : "false"));
            return bResult;
        }

        public bool IsCurrentBranchFixBranch()
        {
            string strCurrentBranchName = GetCurrentBranchName();
            bool bResult = !string.IsNullOrWhiteSpace(strCurrentBranchName)
                && System.Text.RegularExpressions.Regex.Match(strCurrentBranchName, @"fix-(\d+\.)+\d+", System.Text.RegularExpressions.RegexOptions.IgnoreCase).Success;
            Logger.GetLogger().LogMessage(string.Format("GIT: IsCurrentBranchFixBranch() => {0}", bResult ? "true" : "false"));
            return bResult;
        }

        public bool IsCurrentBranchVersionBranch()
        {
            string strCurrentBranchName = GetCurrentBranchName();
            bool bResult = !string.IsNullOrWhiteSpace(strCurrentBranchName)
                && System.Text.RegularExpressions.Regex.Match(strCurrentBranchName, @"version-(\d+\.)+\d+", System.Text.RegularExpressions.RegexOptions.IgnoreCase).Success;
            Logger.GetLogger().LogMessage(string.Format("GIT: IsCurrentBranchVersionBranch() => {0}", bResult ? "true" : "false"));
            return bResult;
        }
    }
}
