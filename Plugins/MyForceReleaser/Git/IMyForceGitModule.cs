using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyForceReleaser
{
    public interface IMyForceGitModule
    {
        string GetCurrentBranchName();
        bool IsCurrentBranchMaster();
        bool IsCurrentBranchFixBranch();
        bool IsCurrentBranchVersionBranch();
        string GetCurrentBranchVersion();
        string GetWorkingDir();
        string RunGitCmd(string strCMD);
    }
}
