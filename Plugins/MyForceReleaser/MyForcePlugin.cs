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
        private StringSetting InternalRepoPath = new StringSetting("Path to internal repository", "");
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

            var pathToInternalRepo = InternalRepoPath[Settings];

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
            using (var releaserStart = new MyForceReleaserGUI(Model))
            {
                releaserStart.ShowDialog(ownerForm);
            }
            Settings.SetValue<string>(InternalRepoPath.Name, Model.InteralRepositoryPath, s => s);
            return true;
        }        
    }
}
