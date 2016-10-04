namespace MyForceReleaser
{
    partial class MyForceReleaserGUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextVersionToSet = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.setAllVersionNumbersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSettings = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.contextShouldUpdateVersions = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.checkAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uncheckAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTipCommitMessages = new System.Windows.Forms.ToolTip(this.components);
            this.tabReleasing = new System.Windows.Forms.TabPage();
            this.dataGridViewReleases = new System.Windows.Forms.DataGridView();
            this.colReleaseProduct = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colReleaseVersionToRelease = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colReleaseRelease = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colReleaseInternalIsTagBased = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnRelease = new System.Windows.Forms.Button();
            this.tabVersions = new System.Windows.Forms.TabPage();
            this.dataGridViewProducts = new System.Windows.Forms.DataGridView();
            this.colProduct = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCurrentVersion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVersionToSet = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUpdateVersion = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colVersionHistoryUpdated = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnChangeVersions = new System.Windows.Forms.Button();
            this.tabMainWindow = new System.Windows.Forms.TabControl();
            this.tabVersionHistory = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dataGridViewVersionHist = new System.Windows.Forms.DataGridView();
            this.colVHProduct = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVHCurrentVersion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVHVersionHistoryUpdated = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVHInternalVersionHistory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.splitHistoryInfoWithVersionHistory = new System.Windows.Forms.SplitContainer();
            this.splitCommitMessagesWithPrevHist = new System.Windows.Forms.SplitContainer();
            this.grpBoxCommitMessages = new System.Windows.Forms.GroupBox();
            this.chkIgnoreCaseForRegex = new System.Windows.Forms.CheckBox();
            this.chkInclusiveRegex = new System.Windows.Forms.CheckBox();
            this.txtCommitMessagesFilters = new System.Windows.Forms.TextBox();
            this.txtCommitMessages = new System.Windows.Forms.RichTextBox();
            this.grpPreviousVersionHist = new System.Windows.Forms.GroupBox();
            this.txtPreviousVersionHistory = new System.Windows.Forms.RichTextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtCurVersionHistory = new System.Windows.Forms.RichTextBox();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnUpdateVersionHistory = new System.Windows.Forms.Button();
            this.contextVersionHistory = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.generateMissingFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceNotYetReleasedWithCurrentDateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetVersionFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.chkShowLogOnExit = new System.Windows.Forms.CheckBox();
            this.contextReleaseToSet = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ContextRelease_SetAllVersionNumbers = new System.Windows.Forms.ToolStripMenuItem();
            this.contextShouldRelease = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.contextVersionToSet.SuspendLayout();
            this.contextShouldUpdateVersions.SuspendLayout();
            this.tabReleasing.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewReleases)).BeginInit();
            this.flowLayoutPanel2.SuspendLayout();
            this.tabVersions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProducts)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.tabMainWindow.SuspendLayout();
            this.tabVersionHistory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewVersionHist)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitHistoryInfoWithVersionHistory)).BeginInit();
            this.splitHistoryInfoWithVersionHistory.Panel1.SuspendLayout();
            this.splitHistoryInfoWithVersionHistory.Panel2.SuspendLayout();
            this.splitHistoryInfoWithVersionHistory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitCommitMessagesWithPrevHist)).BeginInit();
            this.splitCommitMessagesWithPrevHist.Panel1.SuspendLayout();
            this.splitCommitMessagesWithPrevHist.Panel2.SuspendLayout();
            this.splitCommitMessagesWithPrevHist.SuspendLayout();
            this.grpBoxCommitMessages.SuspendLayout();
            this.grpPreviousVersionHist.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.flowLayoutPanel3.SuspendLayout();
            this.contextVersionHistory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.contextReleaseToSet.SuspendLayout();
            this.contextShouldRelease.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextVersionToSet
            // 
            this.contextVersionToSet.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setAllVersionNumbersToolStripMenuItem});
            this.contextVersionToSet.Name = "contextVersionNumbers";
            this.contextVersionToSet.Size = new System.Drawing.Size(197, 26);
            // 
            // setAllVersionNumbersToolStripMenuItem
            // 
            this.setAllVersionNumbersToolStripMenuItem.Name = "setAllVersionNumbersToolStripMenuItem";
            this.setAllVersionNumbersToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.setAllVersionNumbersToolStripMenuItem.Text = "Set all version numbers";
            this.setAllVersionNumbersToolStripMenuItem.Click += new System.EventHandler(this.ContextVersionToSet_SetAllVersionNumbers_Click);
            // 
            // btnSettings
            // 
            this.btnSettings.Location = new System.Drawing.Point(13, 3);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(75, 23);
            this.btnSettings.TabIndex = 3;
            this.btnSettings.Text = "Settings";
            this.btnSettings.UseVisualStyleBackColor = true;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(204, 3);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(799, 23);
            this.progressBar.TabIndex = 4;
            // 
            // contextShouldUpdateVersions
            // 
            this.contextShouldUpdateVersions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.checkAllToolStripMenuItem,
            this.uncheckAllToolStripMenuItem});
            this.contextShouldUpdateVersions.Name = "contextCheckAll";
            this.contextShouldUpdateVersions.Size = new System.Drawing.Size(136, 48);
            // 
            // checkAllToolStripMenuItem
            // 
            this.checkAllToolStripMenuItem.Name = "checkAllToolStripMenuItem";
            this.checkAllToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.checkAllToolStripMenuItem.Text = "Check all";
            this.checkAllToolStripMenuItem.Click += new System.EventHandler(this.ContextUpdateVersion_CheckAll_Click);
            // 
            // uncheckAllToolStripMenuItem
            // 
            this.uncheckAllToolStripMenuItem.Name = "uncheckAllToolStripMenuItem";
            this.uncheckAllToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.uncheckAllToolStripMenuItem.Text = "Uncheck all";
            this.uncheckAllToolStripMenuItem.Click += new System.EventHandler(this.ContextUpdateVersion_UncheckAll_Click);
            // 
            // tabReleasing
            // 
            this.tabReleasing.Controls.Add(this.dataGridViewReleases);
            this.tabReleasing.Controls.Add(this.flowLayoutPanel2);
            this.tabReleasing.Location = new System.Drawing.Point(4, 22);
            this.tabReleasing.Name = "tabReleasing";
            this.tabReleasing.Padding = new System.Windows.Forms.Padding(3);
            this.tabReleasing.Size = new System.Drawing.Size(985, 463);
            this.tabReleasing.TabIndex = 1;
            this.tabReleasing.Text = "Release Products";
            this.tabReleasing.UseVisualStyleBackColor = true;
            // 
            // dataGridViewReleases
            // 
            this.dataGridViewReleases.AllowUserToAddRows = false;
            this.dataGridViewReleases.AllowUserToDeleteRows = false;
            this.dataGridViewReleases.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewReleases.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colReleaseProduct,
            this.colReleaseVersionToRelease,
            this.colReleaseRelease,
            this.colReleaseInternalIsTagBased});
            this.dataGridViewReleases.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewReleases.Location = new System.Drawing.Point(3, 3);
            this.dataGridViewReleases.Name = "dataGridViewReleases";
            this.dataGridViewReleases.Size = new System.Drawing.Size(979, 423);
            this.dataGridViewReleases.TabIndex = 1;
            this.dataGridViewReleases.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewReleases_CellContentClick);
            this.dataGridViewReleases.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewReleases_ColumnHeaderMouseClick);
            // 
            // colReleaseProduct
            // 
            this.colReleaseProduct.HeaderText = "Product";
            this.colReleaseProduct.Name = "colReleaseProduct";
            this.colReleaseProduct.ReadOnly = true;
            // 
            // colReleaseVersionToRelease
            // 
            this.colReleaseVersionToRelease.HeaderText = "Version to release";
            this.colReleaseVersionToRelease.Name = "colReleaseVersionToRelease";
            // 
            // colReleaseRelease
            // 
            this.colReleaseRelease.HeaderText = "Release?";
            this.colReleaseRelease.Name = "colReleaseRelease";
            // 
            // colReleaseInternalIsTagBased
            // 
            this.colReleaseInternalIsTagBased.HeaderText = "internal_TagBased";
            this.colReleaseInternalIsTagBased.Name = "colReleaseInternalIsTagBased";
            this.colReleaseInternalIsTagBased.Visible = false;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.btnRelease);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(3, 426);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(979, 34);
            this.flowLayoutPanel2.TabIndex = 0;
            // 
            // btnRelease
            // 
            this.btnRelease.Location = new System.Drawing.Point(861, 3);
            this.btnRelease.Name = "btnRelease";
            this.btnRelease.Size = new System.Drawing.Size(115, 26);
            this.btnRelease.TabIndex = 0;
            this.btnRelease.Text = "Release Products";
            this.btnRelease.UseVisualStyleBackColor = true;
            this.btnRelease.Click += new System.EventHandler(this.ReleasePrograms_Click);
            // 
            // tabVersions
            // 
            this.tabVersions.Controls.Add(this.dataGridViewProducts);
            this.tabVersions.Controls.Add(this.flowLayoutPanel1);
            this.tabVersions.Location = new System.Drawing.Point(4, 22);
            this.tabVersions.Name = "tabVersions";
            this.tabVersions.Padding = new System.Windows.Forms.Padding(3);
            this.tabVersions.Size = new System.Drawing.Size(985, 463);
            this.tabVersions.TabIndex = 0;
            this.tabVersions.Text = "Update Versions";
            this.tabVersions.UseVisualStyleBackColor = true;
            // 
            // dataGridViewProducts
            // 
            this.dataGridViewProducts.AllowUserToAddRows = false;
            this.dataGridViewProducts.AllowUserToDeleteRows = false;
            this.dataGridViewProducts.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            this.dataGridViewProducts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewProducts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colProduct,
            this.colCurrentVersion,
            this.colVersionToSet,
            this.colUpdateVersion,
            this.colVersionHistoryUpdated});
            this.dataGridViewProducts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewProducts.Location = new System.Drawing.Point(3, 3);
            this.dataGridViewProducts.MultiSelect = false;
            this.dataGridViewProducts.Name = "dataGridViewProducts";
            this.dataGridViewProducts.Size = new System.Drawing.Size(979, 423);
            this.dataGridViewProducts.TabIndex = 5;
            this.dataGridViewProducts.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewProducts_CellContentClick);
            this.dataGridViewProducts.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dataGridViewProducts_CellValidating);
            this.dataGridViewProducts.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewProducts_ColumnHeaderMouseClick);
            // 
            // colProduct
            // 
            this.colProduct.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colProduct.HeaderText = "Product";
            this.colProduct.Name = "colProduct";
            this.colProduct.ReadOnly = true;
            this.colProduct.Width = 69;
            // 
            // colCurrentVersion
            // 
            this.colCurrentVersion.HeaderText = "Current version";
            this.colCurrentVersion.Name = "colCurrentVersion";
            this.colCurrentVersion.ReadOnly = true;
            // 
            // colVersionToSet
            // 
            this.colVersionToSet.HeaderText = "Version to set";
            this.colVersionToSet.Name = "colVersionToSet";
            // 
            // colUpdateVersion
            // 
            this.colUpdateVersion.HeaderText = "Update version?";
            this.colUpdateVersion.Name = "colUpdateVersion";
            // 
            // colVersionHistoryUpdated
            // 
            this.colVersionHistoryUpdated.HeaderText = "Version history updated?";
            this.colVersionHistoryUpdated.Name = "colVersionHistoryUpdated";
            this.colVersionHistoryUpdated.ReadOnly = true;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.btnChangeVersions);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 426);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(979, 34);
            this.flowLayoutPanel1.TabIndex = 3;
            // 
            // btnChangeVersions
            // 
            this.btnChangeVersions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnChangeVersions.Location = new System.Drawing.Point(861, 3);
            this.btnChangeVersions.Name = "btnChangeVersions";
            this.btnChangeVersions.Size = new System.Drawing.Size(115, 26);
            this.btnChangeVersions.TabIndex = 2;
            this.btnChangeVersions.Text = "Update Versions";
            this.btnChangeVersions.UseVisualStyleBackColor = true;
            this.btnChangeVersions.Click += new System.EventHandler(this.UpdateVersionNumbers_Click);
            // 
            // tabMainWindow
            // 
            this.tabMainWindow.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabMainWindow.Controls.Add(this.tabVersions);
            this.tabMainWindow.Controls.Add(this.tabReleasing);
            this.tabMainWindow.Controls.Add(this.tabVersionHistory);
            this.tabMainWindow.Location = new System.Drawing.Point(13, 32);
            this.tabMainWindow.Name = "tabMainWindow";
            this.tabMainWindow.SelectedIndex = 0;
            this.tabMainWindow.Size = new System.Drawing.Size(993, 489);
            this.tabMainWindow.TabIndex = 4;
            this.tabMainWindow.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabMainWindow_Selected);
            // 
            // tabVersionHistory
            // 
            this.tabVersionHistory.Controls.Add(this.splitContainer1);
            this.tabVersionHistory.Controls.Add(this.flowLayoutPanel3);
            this.tabVersionHistory.Location = new System.Drawing.Point(4, 22);
            this.tabVersionHistory.Name = "tabVersionHistory";
            this.tabVersionHistory.Padding = new System.Windows.Forms.Padding(3);
            this.tabVersionHistory.Size = new System.Drawing.Size(985, 463);
            this.tabVersionHistory.TabIndex = 2;
            this.tabVersionHistory.Text = "Version History";
            this.tabVersionHistory.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dataGridViewVersionHist);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitHistoryInfoWithVersionHistory);
            this.splitContainer1.Size = new System.Drawing.Size(979, 423);
            this.splitContainer1.SplitterDistance = 336;
            this.splitContainer1.TabIndex = 5;
            // 
            // dataGridViewVersionHist
            // 
            this.dataGridViewVersionHist.AllowUserToAddRows = false;
            this.dataGridViewVersionHist.AllowUserToDeleteRows = false;
            this.dataGridViewVersionHist.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            this.dataGridViewVersionHist.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewVersionHist.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colVHProduct,
            this.colVHCurrentVersion,
            this.colVHVersionHistoryUpdated,
            this.colVHInternalVersionHistory});
            this.dataGridViewVersionHist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewVersionHist.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewVersionHist.MultiSelect = false;
            this.dataGridViewVersionHist.Name = "dataGridViewVersionHist";
            this.dataGridViewVersionHist.Size = new System.Drawing.Size(336, 423);
            this.dataGridViewVersionHist.TabIndex = 2;
            this.dataGridViewVersionHist.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewVersionHist_CellMouseClick);
            this.dataGridViewVersionHist.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewVersionHist_ColumnHeaderMouseClick);
            // 
            // colVHProduct
            // 
            this.colVHProduct.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colVHProduct.HeaderText = "Product";
            this.colVHProduct.Name = "colVHProduct";
            this.colVHProduct.ReadOnly = true;
            this.colVHProduct.Width = 69;
            // 
            // colVHCurrentVersion
            // 
            this.colVHCurrentVersion.HeaderText = "Current version";
            this.colVHCurrentVersion.Name = "colVHCurrentVersion";
            this.colVHCurrentVersion.ReadOnly = true;
            // 
            // colVHVersionHistoryUpdated
            // 
            this.colVHVersionHistoryUpdated.HeaderText = "Version history updated?";
            this.colVHVersionHistoryUpdated.Name = "colVHVersionHistoryUpdated";
            this.colVHVersionHistoryUpdated.ReadOnly = true;
            // 
            // colVHInternalVersionHistory
            // 
            this.colVHInternalVersionHistory.HeaderText = "internal_VersionHistory";
            this.colVHInternalVersionHistory.Name = "colVHInternalVersionHistory";
            this.colVHInternalVersionHistory.Visible = false;
            // 
            // splitHistoryInfoWithVersionHistory
            // 
            this.splitHistoryInfoWithVersionHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitHistoryInfoWithVersionHistory.Location = new System.Drawing.Point(0, 0);
            this.splitHistoryInfoWithVersionHistory.Name = "splitHistoryInfoWithVersionHistory";
            this.splitHistoryInfoWithVersionHistory.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitHistoryInfoWithVersionHistory.Panel1
            // 
            this.splitHistoryInfoWithVersionHistory.Panel1.Controls.Add(this.splitCommitMessagesWithPrevHist);
            // 
            // splitHistoryInfoWithVersionHistory.Panel2
            // 
            this.splitHistoryInfoWithVersionHistory.Panel2.Controls.Add(this.groupBox2);
            this.splitHistoryInfoWithVersionHistory.Size = new System.Drawing.Size(639, 423);
            this.splitHistoryInfoWithVersionHistory.SplitterDistance = 199;
            this.splitHistoryInfoWithVersionHistory.TabIndex = 1;
            // 
            // splitCommitMessagesWithPrevHist
            // 
            this.splitCommitMessagesWithPrevHist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitCommitMessagesWithPrevHist.Location = new System.Drawing.Point(0, 0);
            this.splitCommitMessagesWithPrevHist.Name = "splitCommitMessagesWithPrevHist";
            // 
            // splitCommitMessagesWithPrevHist.Panel1
            // 
            this.splitCommitMessagesWithPrevHist.Panel1.Controls.Add(this.grpBoxCommitMessages);
            // 
            // splitCommitMessagesWithPrevHist.Panel2
            // 
            this.splitCommitMessagesWithPrevHist.Panel2.Controls.Add(this.grpPreviousVersionHist);
            this.splitCommitMessagesWithPrevHist.Size = new System.Drawing.Size(639, 199);
            this.splitCommitMessagesWithPrevHist.SplitterDistance = 327;
            this.splitCommitMessagesWithPrevHist.TabIndex = 0;
            // 
            // grpBoxCommitMessages
            // 
            this.grpBoxCommitMessages.Controls.Add(this.chkIgnoreCaseForRegex);
            this.grpBoxCommitMessages.Controls.Add(this.chkInclusiveRegex);
            this.grpBoxCommitMessages.Controls.Add(this.txtCommitMessagesFilters);
            this.grpBoxCommitMessages.Controls.Add(this.txtCommitMessages);
            this.grpBoxCommitMessages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpBoxCommitMessages.Location = new System.Drawing.Point(0, 0);
            this.grpBoxCommitMessages.Name = "grpBoxCommitMessages";
            this.grpBoxCommitMessages.Size = new System.Drawing.Size(327, 199);
            this.grpBoxCommitMessages.TabIndex = 1;
            this.grpBoxCommitMessages.TabStop = false;
            this.grpBoxCommitMessages.Text = "Commit Messages";
            // 
            // chkIgnoreCaseForRegex
            // 
            this.chkIgnoreCaseForRegex.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkIgnoreCaseForRegex.AutoSize = true;
            this.chkIgnoreCaseForRegex.Checked = true;
            this.chkIgnoreCaseForRegex.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIgnoreCaseForRegex.Location = new System.Drawing.Point(218, 37);
            this.chkIgnoreCaseForRegex.Name = "chkIgnoreCaseForRegex";
            this.chkIgnoreCaseForRegex.Size = new System.Drawing.Size(82, 17);
            this.chkIgnoreCaseForRegex.TabIndex = 3;
            this.chkIgnoreCaseForRegex.Text = "Ignore case";
            this.chkIgnoreCaseForRegex.UseVisualStyleBackColor = true;
            this.chkIgnoreCaseForRegex.CheckedChanged += new System.EventHandler(this.chkRegexOption_CheckedChanged);
            // 
            // chkInclusiveRegex
            // 
            this.chkInclusiveRegex.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkInclusiveRegex.AutoSize = true;
            this.chkInclusiveRegex.Location = new System.Drawing.Point(218, 14);
            this.chkInclusiveRegex.Name = "chkInclusiveRegex";
            this.chkInclusiveRegex.Size = new System.Drawing.Size(67, 17);
            this.chkInclusiveRegex.TabIndex = 2;
            this.chkInclusiveRegex.Text = "Include?";
            this.chkInclusiveRegex.UseVisualStyleBackColor = true;
            this.chkInclusiveRegex.CheckedChanged += new System.EventHandler(this.chkRegexOption_CheckedChanged);
            // 
            // txtCommitMessagesFilters
            // 
            this.txtCommitMessagesFilters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCommitMessagesFilters.Location = new System.Drawing.Point(7, 20);
            this.txtCommitMessagesFilters.Multiline = true;
            this.txtCommitMessagesFilters.Name = "txtCommitMessagesFilters";
            this.txtCommitMessagesFilters.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtCommitMessagesFilters.Size = new System.Drawing.Size(206, 34);
            this.txtCommitMessagesFilters.TabIndex = 1;
            this.txtCommitMessagesFilters.TextChanged += new System.EventHandler(this.txtCommitMessagesFilters_TextChanged);
            // 
            // txtCommitMessages
            // 
            this.txtCommitMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCommitMessages.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCommitMessages.Location = new System.Drawing.Point(6, 60);
            this.txtCommitMessages.Name = "txtCommitMessages";
            this.txtCommitMessages.ReadOnly = true;
            this.txtCommitMessages.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            this.txtCommitMessages.Size = new System.Drawing.Size(315, 133);
            this.txtCommitMessages.TabIndex = 0;
            this.txtCommitMessages.Text = "";
            this.txtCommitMessages.WordWrap = false;
            // 
            // grpPreviousVersionHist
            // 
            this.grpPreviousVersionHist.Controls.Add(this.txtPreviousVersionHistory);
            this.grpPreviousVersionHist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpPreviousVersionHist.Location = new System.Drawing.Point(0, 0);
            this.grpPreviousVersionHist.Name = "grpPreviousVersionHist";
            this.grpPreviousVersionHist.Size = new System.Drawing.Size(308, 199);
            this.grpPreviousVersionHist.TabIndex = 0;
            this.grpPreviousVersionHist.TabStop = false;
            this.grpPreviousVersionHist.Text = "Previous Track Version History";
            // 
            // txtPreviousVersionHistory
            // 
            this.txtPreviousVersionHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPreviousVersionHistory.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPreviousVersionHistory.Location = new System.Drawing.Point(3, 16);
            this.txtPreviousVersionHistory.Name = "txtPreviousVersionHistory";
            this.txtPreviousVersionHistory.ReadOnly = true;
            this.txtPreviousVersionHistory.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            this.txtPreviousVersionHistory.Size = new System.Drawing.Size(302, 180);
            this.txtPreviousVersionHistory.TabIndex = 0;
            this.txtPreviousVersionHistory.Text = "";
            this.txtPreviousVersionHistory.WordWrap = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtCurVersionHistory);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(639, 220);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Current Track Version History";
            // 
            // txtCurVersionHistory
            // 
            this.txtCurVersionHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCurVersionHistory.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCurVersionHistory.Location = new System.Drawing.Point(3, 16);
            this.txtCurVersionHistory.Name = "txtCurVersionHistory";
            this.txtCurVersionHistory.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            this.txtCurVersionHistory.Size = new System.Drawing.Size(633, 201);
            this.txtCurVersionHistory.TabIndex = 0;
            this.txtCurVersionHistory.Text = "";
            this.txtCurVersionHistory.WordWrap = false;
            this.txtCurVersionHistory.TextChanged += new System.EventHandler(this.txtCurVersionHistory_TextChanged);
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.Controls.Add(this.btnUpdateVersionHistory);
            this.flowLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel3.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel3.Location = new System.Drawing.Point(3, 426);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(979, 34);
            this.flowLayoutPanel3.TabIndex = 6;
            // 
            // btnUpdateVersionHistory
            // 
            this.btnUpdateVersionHistory.Location = new System.Drawing.Point(861, 3);
            this.btnUpdateVersionHistory.Name = "btnUpdateVersionHistory";
            this.btnUpdateVersionHistory.Size = new System.Drawing.Size(115, 26);
            this.btnUpdateVersionHistory.TabIndex = 0;
            this.btnUpdateVersionHistory.Text = "Update history";
            this.btnUpdateVersionHistory.UseVisualStyleBackColor = true;
            this.btnUpdateVersionHistory.Click += new System.EventHandler(this.UpdateVersionHistory_Click);
            // 
            // contextVersionHistory
            // 
            this.contextVersionHistory.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.generateMissingFilesToolStripMenuItem,
            this.replaceNotYetReleasedWithCurrentDateToolStripMenuItem,
            this.resetVersionFilesToolStripMenuItem});
            this.contextVersionHistory.Name = "contextVersionHistory";
            this.contextVersionHistory.Size = new System.Drawing.Size(303, 70);
            // 
            // generateMissingFilesToolStripMenuItem
            // 
            this.generateMissingFilesToolStripMenuItem.Name = "generateMissingFilesToolStripMenuItem";
            this.generateMissingFilesToolStripMenuItem.Size = new System.Drawing.Size(302, 22);
            this.generateMissingFilesToolStripMenuItem.Text = "Generate missing file(s)";
            this.generateMissingFilesToolStripMenuItem.Click += new System.EventHandler(this.ContextVersionHistory_GenerateMissingVersionFiles_Click);
            // 
            // replaceNotYetReleasedWithCurrentDateToolStripMenuItem
            // 
            this.replaceNotYetReleasedWithCurrentDateToolStripMenuItem.Name = "replaceNotYetReleasedWithCurrentDateToolStripMenuItem";
            this.replaceNotYetReleasedWithCurrentDateToolStripMenuItem.Size = new System.Drawing.Size(302, 22);
            this.replaceNotYetReleasedWithCurrentDateToolStripMenuItem.Text = "Replace \'Not yet released\' with current date";
            this.replaceNotYetReleasedWithCurrentDateToolStripMenuItem.Click += new System.EventHandler(this.ContextVersionHistory_ReplaceNotYetReleasedWithCurrentDate_Click);
            // 
            // resetVersionFilesToolStripMenuItem
            // 
            this.resetVersionFilesToolStripMenuItem.Name = "resetVersionFilesToolStripMenuItem";
            this.resetVersionFilesToolStripMenuItem.Size = new System.Drawing.Size(302, 22);
            this.resetVersionFilesToolStripMenuItem.Text = "Reload version file(s) from disk";
            this.resetVersionFilesToolStripMenuItem.Click += new System.EventHandler(this.ContextVersionHistory_ResetVersionFiles_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // chkShowLogOnExit
            // 
            this.chkShowLogOnExit.AutoSize = true;
            this.chkShowLogOnExit.Location = new System.Drawing.Point(94, 7);
            this.chkShowLogOnExit.Name = "chkShowLogOnExit";
            this.chkShowLogOnExit.Size = new System.Drawing.Size(104, 17);
            this.chkShowLogOnExit.TabIndex = 5;
            this.chkShowLogOnExit.Text = "Show log on exit";
            this.chkShowLogOnExit.UseVisualStyleBackColor = true;
            // 
            // contextReleaseToSet
            // 
            this.contextReleaseToSet.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ContextRelease_SetAllVersionNumbers});
            this.contextReleaseToSet.Name = "contextVersionNumbers";
            this.contextReleaseToSet.Size = new System.Drawing.Size(197, 26);
            // 
            // ContextRelease_SetAllVersionNumbers
            // 
            this.ContextRelease_SetAllVersionNumbers.Name = "ContextRelease_SetAllVersionNumbers";
            this.ContextRelease_SetAllVersionNumbers.Size = new System.Drawing.Size(196, 22);
            this.ContextRelease_SetAllVersionNumbers.Text = "Set all version numbers";
            this.ContextRelease_SetAllVersionNumbers.Click += new System.EventHandler(this.ContextReleases_SetAllVersionNumbers_Click);
            // 
            // contextShouldRelease
            // 
            this.contextShouldRelease.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2});
            this.contextShouldRelease.Name = "contextCheckAll";
            this.contextShouldRelease.Size = new System.Drawing.Size(136, 48);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem1.Text = "Check all";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.ContextReleases_CheckAll_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem2.Text = "Uncheck all";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.ContextReleases_UncheckAll_Click);
            // 
            // MyForceReleaserGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1018, 533);
            this.Controls.Add(this.chkShowLogOnExit);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.btnSettings);
            this.Controls.Add(this.tabMainWindow);
            this.Name = "MyForceReleaserGUI";
            this.Text = "MyForce Releaser";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.contextVersionToSet.ResumeLayout(false);
            this.contextShouldUpdateVersions.ResumeLayout(false);
            this.tabReleasing.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewReleases)).EndInit();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.tabVersions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProducts)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.tabMainWindow.ResumeLayout(false);
            this.tabVersionHistory.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewVersionHist)).EndInit();
            this.splitHistoryInfoWithVersionHistory.Panel1.ResumeLayout(false);
            this.splitHistoryInfoWithVersionHistory.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitHistoryInfoWithVersionHistory)).EndInit();
            this.splitHistoryInfoWithVersionHistory.ResumeLayout(false);
            this.splitCommitMessagesWithPrevHist.Panel1.ResumeLayout(false);
            this.splitCommitMessagesWithPrevHist.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitCommitMessagesWithPrevHist)).EndInit();
            this.splitCommitMessagesWithPrevHist.ResumeLayout(false);
            this.grpBoxCommitMessages.ResumeLayout(false);
            this.grpBoxCommitMessages.PerformLayout();
            this.grpPreviousVersionHist.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.flowLayoutPanel3.ResumeLayout(false);
            this.contextVersionHistory.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.contextReleaseToSet.ResumeLayout(false);
            this.contextShouldRelease.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.ContextMenuStrip contextVersionToSet;
        private System.Windows.Forms.ToolStripMenuItem setAllVersionNumbersToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextShouldUpdateVersions;
        private System.Windows.Forms.ToolStripMenuItem checkAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uncheckAllToolStripMenuItem;
        private System.Windows.Forms.ToolTip toolTipCommitMessages;
        private System.Windows.Forms.TabPage tabReleasing;
        private System.Windows.Forms.DataGridView dataGridViewReleases;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Button btnRelease;
        private System.Windows.Forms.TabPage tabVersions;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnChangeVersions;
        private System.Windows.Forms.TabControl tabMainWindow;
        private System.Windows.Forms.DataGridViewTextBoxColumn colReleaseProduct;
        private System.Windows.Forms.DataGridViewTextBoxColumn colReleaseVersionToRelease;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colReleaseRelease;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colReleaseInternalIsTagBased;
        private System.Windows.Forms.ContextMenuStrip contextVersionHistory;
        private System.Windows.Forms.ToolStripMenuItem generateMissingFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem replaceNotYetReleasedWithCurrentDateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetVersionFilesToolStripMenuItem;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.DataGridView dataGridViewProducts;
        private System.Windows.Forms.TabPage tabVersionHistory;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dataGridViewVersionHist;
        private System.Windows.Forms.SplitContainer splitHistoryInfoWithVersionHistory;
        private System.Windows.Forms.SplitContainer splitCommitMessagesWithPrevHist;
        private System.Windows.Forms.GroupBox grpBoxCommitMessages;
        private System.Windows.Forms.CheckBox chkIgnoreCaseForRegex;
        private System.Windows.Forms.CheckBox chkInclusiveRegex;
        private System.Windows.Forms.TextBox txtCommitMessagesFilters;
        private System.Windows.Forms.RichTextBox txtCommitMessages;
        private System.Windows.Forms.GroupBox grpPreviousVersionHist;
        private System.Windows.Forms.RichTextBox txtPreviousVersionHistory;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RichTextBox txtCurVersionHistory;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProduct;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCurrentVersion;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVersionToSet;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colUpdateVersion;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVersionHistoryUpdated;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVHProduct;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVHCurrentVersion;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVHVersionHistoryUpdated;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVHInternalVersionHistory;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
        private System.Windows.Forms.Button btnUpdateVersionHistory;
        private System.Windows.Forms.CheckBox chkShowLogOnExit;
        private System.Windows.Forms.ContextMenuStrip contextReleaseToSet;
        private System.Windows.Forms.ToolStripMenuItem ContextRelease_SetAllVersionNumbers;
        private System.Windows.Forms.ContextMenuStrip contextShouldRelease;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;

    }
}

