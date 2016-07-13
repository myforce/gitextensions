using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyForceReleaser
{
    public interface IMyForceGitModule
    {
        string GetWorkingDir();
        string RunGitCmd(string strCMD);
    }
}
