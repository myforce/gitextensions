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
    }
}
