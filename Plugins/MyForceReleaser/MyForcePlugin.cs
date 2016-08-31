﻿using GitUIPluginInterfaces;
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

            var pathToInternalRepo = settingInternalRepoPath[Settings];

            string strFileCheckErrors = "";
            while (!MyForceReleaser.AllInternalFilesArePresent(pathToInternalRepo, eventArgs.GitModule.WorkingDir, ref strFileCheckErrors))
            {
                if (MessageBox.Show(ownerForm, string.Format("Errors are found (see below). This means this is not a compatible repo or the internal repository is nog configured well. Do you want to select a new internal repository path?\n\nErrors:\n{0}", strFileCheckErrors), _myforcereleaser.Text, MessageBoxButtons.YesNo) != DialogResult.Yes)
                    return false;

                FolderBrowserDialog dlgSelectFolder = new FolderBrowserDialog();
                dlgSelectFolder.SelectedPath = pathToInternalRepo;
                dlgSelectFolder.ShowDialog();

                pathToInternalRepo = dlgSelectFolder.SelectedPath;
            }

            MyForceReleaser Model = new MyForceReleaser(pathToInternalRepo, new MyForceGitExtensions(eventArgs.GitModule));
            Model.CommitMessageIgnoreRegexes = StaticTools.Deserialize<List<string>>(settingCommitMessageIgnoreRegexes[Settings]);

            strFileCheckErrors = "";
            if (!Model.Validate(ref strFileCheckErrors))
            {
                MessageBox.Show(ownerForm, string.Format("This plugin can't run with the current config! Errors:\n{0}", strFileCheckErrors), _myforcereleaser.Text, MessageBoxButtons.OK);
                return false;
            }

            using (var releaserStart = new MyForceReleaserGUI(Model))
            {
                releaserStart.ShowDialog(ownerForm);
            }

            Settings.SetValue<string>(settingInternalRepoPath.Name, Model.InternalRepositoryPath, s => s);
            Settings.SetValue<string>(settingCommitMessageIgnoreRegexes.Name, StaticTools.Serialize<List<string>>(Model.CommitMessageIgnoreRegexes), s => s);
            return true;
        }        
    }
}