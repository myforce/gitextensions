using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyForceReleaser
{
    public partial class MyForceReleaserGUISettings : Form
    {
        public string InternalRepoPath
        {
            get { return txtInternalRepoPath.Text; }
            set { txtInternalRepoPath.Text = value; }
        }

        public MyForceReleaserGUISettings()
        {
            InitializeComponent();
        }

        private void btnSelectInternalRepo_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlgSelectFolder = new FolderBrowserDialog();
            dlgSelectFolder.SelectedPath = InternalRepoPath;
            if (dlgSelectFolder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                InternalRepoPath = dlgSelectFolder.SelectedPath;
        }
    }
}
