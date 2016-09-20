using GitUIPluginInterfaces;
using ResourceManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyForceReleaser
{
    public class MyForceReleaserPlugin : GitPluginBase, IGitPluginForRepository
    {
        #region Translation
        private readonly TranslationString _currentDirectoryIsNotValidGit = new TranslationString("The current directory is not a valid git repository.\n\nMyForce Release can be only be started from a valid git repository.");
        private readonly TranslationString _myforcereleaser = new TranslationString("MyForce Releaser");
        private readonly TranslationString _resetInternalRepoPath = new TranslationString("Errors are found (see below), do you want to select a new path?\n\nErrors:\n{0}");
        private readonly TranslationString _internalRepoPathNotFound = new TranslationString("No path specified for the Internal repository!"); 
        #endregion

        #region Settings
        private StringSetting settingInternalRepoPath = new StringSetting("Path to internal repository", "");
        private StringSetting settingCommitMessageIgnoreRegexes = new StringSetting("Commit message ignore regexes", "");
        private BoolSetting settingShowLogOnExit = new BoolSetting("Show log on exit value", false);
        #endregion

        public MyForceReleaserPlugin()
        {
            Description = _myforcereleaser.Text;
            Translate();
        }

        public override bool Execute(GitUIBaseEventArgs eventArgs)
        {
            var ownerForm = eventArgs.OwnerForm as IWin32Window;
            if (!eventArgs.GitModule.IsValidGitWorkingDir())
            {
                MessageBox.Show(ownerForm, _currentDirectoryIsNotValidGit.Text);
                return false;
            }
            Logger.GetLogger().LogMessage("Plugin Started");
            
            var pathToInternalRepo = settingInternalRepoPath[Settings];
            Logger.GetLogger().LogMessage(string.Format("Internal repository checks: Stored path of repository: <{0}>", pathToInternalRepo));
            
            string strFileCheckErrors = "";
            while (!MyForceReleaser.AllInternalFilesArePresent(pathToInternalRepo, eventArgs.GitModule.WorkingDir, ref strFileCheckErrors))
            {
                Logger.GetLogger().LogMessage(string.Format("Internal repository checks: Verification checks failed: <{0}>", strFileCheckErrors), Logger.LogType.lt_error);
                if (MessageBox.Show(ownerForm, string.Format("Errors are found (see below). This means this is not a compatible repo or the internal repository is nog configured well. Do you want to select a new internal repository path?\n\nErrors:\n{0}", strFileCheckErrors), _myforcereleaser.Text, MessageBoxButtons.YesNo) != DialogResult.Yes)
                    return false;

                Logger.GetLogger().LogMessage("Internal repository checks: Ask user for new location");
                FolderBrowserDialog dlgSelectFolder = new FolderBrowserDialog();
                dlgSelectFolder.SelectedPath = pathToInternalRepo;
                dlgSelectFolder.ShowDialog();

                pathToInternalRepo = dlgSelectFolder.SelectedPath;                
                Logger.GetLogger().LogMessage(string.Format("Internal repository checks: User selected location <{0}>", pathToInternalRepo));
            }

            Logger.GetLogger().LogMessage("Startup: Creating instance of our model");
            MyForceReleaser Model = new MyForceReleaser(pathToInternalRepo, new MyForceGitExtensions(eventArgs.GitModule));
            
            Model.CommitMessageIgnoreRegexes = StaticTools.Deserialize<List<string>>(settingCommitMessageIgnoreRegexes[Settings]);
            Logger.GetLogger().LogMessage(string.Format("Startup: Loaded previous regexes for the version history commits window <{0}>", string.Join(",",Model.CommitMessageIgnoreRegexes.ToArray())));

            strFileCheckErrors = "";
            Logger.GetLogger().LogMessage("Startup: Validate model & repo");            
            if (!Model.Validate(ref strFileCheckErrors))
            {
                Logger.GetLogger().LogMessage(string.Format("Startup: Found issue in model: <{0}>", strFileCheckErrors), Logger.LogType.lt_error);
                MessageBox.Show(ownerForm, string.Format("This plugin can't run with the current config! Errors:\n{0}", strFileCheckErrors), _myforcereleaser.Text, MessageBoxButtons.OK);
                return false;
            }

            Logger.GetLogger().LogMessage("Startup: Done");
            Logger.GetLogger().LogMessage("Startup: Starting GUI");
            
            using (var releaserStart = new MyForceReleaserGUI(Model))
            {
                releaserStart.SetShowLogRequested(settingShowLogOnExit[Settings].GetValueOrDefault(false));
                releaserStart.ShowDialog(ownerForm);
                Settings.SetBool(settingShowLogOnExit.Name, releaserStart.ShowLogRequested());
               
            }

            Logger.GetLogger().LogMessage("Shutdown: storing settings");

            Logger.GetLogger().LogMessage(string.Format("Shutdown: storing repo path <{0}>", Model.InternalRepositoryPath));
            Settings.SetString(settingInternalRepoPath.Name, Model.InternalRepositoryPath);

            Logger.GetLogger().LogMessage(string.Format("Shutdown: storing regexes for the version history commits window <{0}>", string.Join(",", Model.CommitMessageIgnoreRegexes.ToArray())));
            Settings.SetString(settingCommitMessageIgnoreRegexes.Name, StaticTools.Serialize<List<string>>(Model.CommitMessageIgnoreRegexes));

            Settings.SetString(settingInternalRepoPath.Name, Model.InternalRepositoryPath);
            if (settingShowLogOnExit[Settings].GetValueOrDefault(false))
            {
                string strPath = System.IO.Path.GetTempFileName().Replace(".tmp", ".txt"); ;
                System.IO.File.WriteAllText(strPath, Logger.GetLogger().GetLog());
                System.Diagnostics.Process.Start(strPath);
            }
            return true;
        }        
    }
}
