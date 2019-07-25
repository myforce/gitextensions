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
        private bool _Cached { get; set; }

        private enum CacheElement { BranchName, WorkingDir, IsMasterBranch, IsFixBranch, IsVersionBranch, BranchVersion }
        private Dictionary<CacheElement, object> _Chache;
        private delegate object Retriever();
        private T GetItemFromCache<T>(CacheElement element, Retriever ret)
        {
            //Check if we should use the cache
            if (!_Cached)
                _Chache.Remove(element);

            //Get the element if its missing
            if (!_Chache.ContainsKey(element))
            {
                _Chache.Add(element, ret());
                Logger.GetLogger().LogMessage(string.Format("GIT: Get {0} => {1}", element.ToString("f"), (T)_Chache[element]));
            }
            return (T)_Chache[element];
        }

        public MyForceGitExtensions(IGitModule GitModule)
        {
            _Git = GitModule;
            _Chache = new Dictionary<CacheElement, object>();
            _Cached = true; //As long as the plug in is loaded we don't expect to change branches
            if (_Cached)
                Logger.GetLogger().LogMessage("MyForceGitExtensions: Running in cached mode! We don't expect git branch switches during the period the plugin is loaded!");
        }

        public string GetCurrentBranchName()
        {
            return GetItemFromCache<string>(CacheElement.BranchName, 
                delegate() { 
                    return _Git.GetSelectedBranch(); 
                });
        }

        public string GetWorkingDir()
        {
            return GetItemFromCache<string>(CacheElement.WorkingDir,
                delegate()
                {
                    return _Git.WorkingDir;
                });
        }

        public string RunGitCmd(string strCMD)
        {
            string strOutPut = _Git.RunGitCmd(strCMD);
            Logger.GetLogger().LogMessage(string.Format("GIT: RunGitCmd({0}) => {1}", strCMD, strOutPut, strOutPut.StartsWith("fatal") ? Logger.LogType.lt_error : Logger.LogType.lt_info));
            return strOutPut;
        }

        public bool IsCurrentBranchMaster()
        {
            return GetItemFromCache<bool>(CacheElement.IsMasterBranch,
                delegate()
                {
                    string strCurrentBranchName = GetCurrentBranchName();
                    return !string.IsNullOrWhiteSpace(strCurrentBranchName)
                        && System.Text.RegularExpressions.Regex.Match(strCurrentBranchName, "master", System.Text.RegularExpressions.RegexOptions.IgnoreCase).Success;
                });
        }

        public bool IsCurrentBranchFixBranch()
        {
            return GetItemFromCache<bool>(CacheElement.IsFixBranch,
                delegate()
                {
                    string strCurrentBranchName = GetCurrentBranchName();
                    return !string.IsNullOrWhiteSpace(strCurrentBranchName)
                        && System.Text.RegularExpressions.Regex.Match(strCurrentBranchName, @"fix-(\d+\.)+\d+", System.Text.RegularExpressions.RegexOptions.IgnoreCase).Success;
                });
        }

        public bool IsCurrentBranchVersionBranch()
        {
            return GetItemFromCache<bool>(CacheElement.IsVersionBranch,
               delegate ()
               {
                   string strCurrentBranchName = GetCurrentBranchName();
                   return !string.IsNullOrWhiteSpace(strCurrentBranchName)
                       && System.Text.RegularExpressions.Regex.Match(strCurrentBranchName, @"version-(\d+\.)+\d+", System.Text.RegularExpressions.RegexOptions.IgnoreCase).Success;
               });
        }

        public string GetCurrentBranchVersion()
        {
            return GetItemFromCache<string>(CacheElement.BranchVersion,
               delegate ()
               {
                   string strCurrentBranchName = GetCurrentBranchName();
                   if (string.IsNullOrWhiteSpace(strCurrentBranchName))
                       return null;
                   var match = System.Text.RegularExpressions.Regex.Match(strCurrentBranchName, @"(?:version|fix)-((?:\d+\.)+\d+)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                   if (match == null || !match.Success)
                       return null;
                   return match.Groups[1].Value;
               });
        }
    }
}
