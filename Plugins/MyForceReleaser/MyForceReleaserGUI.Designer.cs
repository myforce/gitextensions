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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnChangeVersions = new System.Windows.Forms.Button();
            this.dataGridViewProducts = new System.Windows.Forms.DataGridView();
            this.contextVersionNumbers = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.setAllVersionNumbersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataGridViewReleases = new System.Windows.Forms.DataGridView();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnRelease = new System.Windows.Forms.Button();
            this.btnSettings = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.ValidProductName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CurrentVersion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NewReleaseVersion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Release = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.productName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.versionToRelease = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRelease = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.HiddenTagBased = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.contextCheckAll = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.checkAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uncheckAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProducts)).BeginInit();
            this.contextVersionNumbers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewReleases)).BeginInit();
            this.flowLayoutPanel2.SuspendLayout();
            this.contextCheckAll.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 32);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.flowLayoutPanel1);
            this.splitContainer1.Panel1.Controls.Add(this.dataGridViewProducts);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dataGridViewReleases);
            this.splitContainer1.Panel2.Controls.Add(this.flowLayoutPanel2);
            this.splitContainer1.Size = new System.Drawing.Size(994, 489);
            this.splitContainer1.SplitterDistance = 523;
            this.splitContainer1.TabIndex = 2;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.btnChangeVersions);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 455);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(523, 34);
            this.flowLayoutPanel1.TabIndex = 3;
            // 
            // btnChangeVersions
            // 
            this.btnChangeVersions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChangeVersions.Location = new System.Drawing.Point(405, 3);
            this.btnChangeVersions.Name = "btnChangeVersions";
            this.btnChangeVersions.Size = new System.Drawing.Size(115, 26);
            this.btnChangeVersions.TabIndex = 2;
            this.btnChangeVersions.Text = "Update Versions";
            this.btnChangeVersions.UseVisualStyleBackColor = true;
            this.btnChangeVersions.Click += new System.EventHandler(this.UpdateVersionNumbers_Click);
            // 
            // dataGridViewProducts
            // 
            this.dataGridViewProducts.AllowUserToAddRows = false;
            this.dataGridViewProducts.AllowUserToDeleteRows = false;
            this.dataGridViewProducts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewProducts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewProducts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ValidProductName,
            this.CurrentVersion,
            this.NewReleaseVersion,
            this.Release});
            this.dataGridViewProducts.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewProducts.Name = "dataGridViewProducts";
            this.dataGridViewProducts.Size = new System.Drawing.Size(523, 452);
            this.dataGridViewProducts.TabIndex = 1;
            this.dataGridViewProducts.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewProducts_CellContentClick);
            this.dataGridViewProducts.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewProducts_ColumnHeaderMouseClick);
            // 
            // contextVersionNumbers
            // 
            this.contextVersionNumbers.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setAllVersionNumbersToolStripMenuItem});
            this.contextVersionNumbers.Name = "contextVersionNumbers";
            this.contextVersionNumbers.Size = new System.Drawing.Size(197, 26);
            // 
            // setAllVersionNumbersToolStripMenuItem
            // 
            this.setAllVersionNumbersToolStripMenuItem.Name = "setAllVersionNumbersToolStripMenuItem";
            this.setAllVersionNumbersToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.setAllVersionNumbersToolStripMenuItem.Text = "Set all version numbers";
            this.setAllVersionNumbersToolStripMenuItem.Click += new System.EventHandler(this.setAllVersionNumbersToolStripMenuItem_Click);
            // 
            // dataGridViewReleases
            // 
            this.dataGridViewReleases.AllowUserToAddRows = false;
            this.dataGridViewReleases.AllowUserToDeleteRows = false;
            this.dataGridViewReleases.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewReleases.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewReleases.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.productName,
            this.versionToRelease,
            this.colRelease,
            this.HiddenTagBased});
            this.dataGridViewReleases.Location = new System.Drawing.Point(3, 0);
            this.dataGridViewReleases.Name = "dataGridViewReleases";
            this.dataGridViewReleases.Size = new System.Drawing.Size(461, 452);
            this.dataGridViewReleases.TabIndex = 1;
            this.dataGridViewReleases.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewReleases_CellContentClick);
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.btnRelease);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(0, 455);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(467, 34);
            this.flowLayoutPanel2.TabIndex = 0;
            // 
            // btnRelease
            // 
            this.btnRelease.Location = new System.Drawing.Point(349, 3);
            this.btnRelease.Name = "btnRelease";
            this.btnRelease.Size = new System.Drawing.Size(115, 26);
            this.btnRelease.TabIndex = 0;
            this.btnRelease.Text = "Release Products";
            this.btnRelease.UseVisualStyleBackColor = true;
            this.btnRelease.Click += new System.EventHandler(this.ReleasePrograms_Click);
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
            this.progressBar.Location = new System.Drawing.Point(94, 3);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(909, 23);
            this.progressBar.TabIndex = 4;
            // 
            // ValidProductName
            // 
            this.ValidProductName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ValidProductName.HeaderText = "ProductName";
            this.ValidProductName.Name = "ValidProductName";
            this.ValidProductName.ReadOnly = true;
            this.ValidProductName.Width = 97;
            // 
            // CurrentVersion
            // 
            this.CurrentVersion.HeaderText = "Current Version";
            this.CurrentVersion.Name = "CurrentVersion";
            this.CurrentVersion.ReadOnly = true;
            // 
            // NewReleaseVersion
            // 
            this.NewReleaseVersion.HeaderText = "New Release Version";
            this.NewReleaseVersion.Name = "NewReleaseVersion";
            // 
            // Release
            // 
            this.Release.HeaderText = "Update Versions?";
            this.Release.Name = "Release";
            // 
            // productName
            // 
            this.productName.HeaderText = "Product Name";
            this.productName.Name = "productName";
            this.productName.ReadOnly = true;
            // 
            // versionToRelease
            // 
            this.versionToRelease.HeaderText = "Version Ready to Release";
            this.versionToRelease.Name = "versionToRelease";
            // 
            // colRelease
            // 
            this.colRelease.HeaderText = "Release?";
            this.colRelease.Name = "colRelease";
            // 
            // HiddenTagBased
            // 
            this.HiddenTagBased.HeaderText = "HiddenTagBased";
            this.HiddenTagBased.Name = "HiddenTagBased";
            this.HiddenTagBased.Visible = false;
            // 
            // contextCheckAll
            // 
            this.contextCheckAll.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.checkAllToolStripMenuItem,
            this.uncheckAllToolStripMenuItem});
            this.contextCheckAll.Name = "contextCheckAll";
            this.contextCheckAll.Size = new System.Drawing.Size(153, 70);
            // 
            // checkAllToolStripMenuItem
            // 
            this.checkAllToolStripMenuItem.Name = "checkAllToolStripMenuItem";
            this.checkAllToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.checkAllToolStripMenuItem.Text = "Check all";
            this.checkAllToolStripMenuItem.Click += new System.EventHandler(this.checkAllToolStripMenuItem_Click);
            // 
            // uncheckAllToolStripMenuItem
            // 
            this.uncheckAllToolStripMenuItem.Name = "uncheckAllToolStripMenuItem";
            this.uncheckAllToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.uncheckAllToolStripMenuItem.Text = "Uncheck all";
            this.uncheckAllToolStripMenuItem.Click += new System.EventHandler(this.uncheckAllToolStripMenuItem_Click);
            // 
            // MyForceReleaserGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1018, 533);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.btnSettings);
            this.Controls.Add(this.splitContainer1);
            this.Name = "MyForceReleaserGUI";
            this.Text = "MyForce Releaser";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProducts)).EndInit();
            this.contextVersionNumbers.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewReleases)).EndInit();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.contextCheckAll.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dataGridViewProducts;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnChangeVersions;
        private System.Windows.Forms.DataGridView dataGridViewReleases;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Button btnRelease;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.ContextMenuStrip contextVersionNumbers;
        private System.Windows.Forms.ToolStripMenuItem setAllVersionNumbersToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn ValidProductName;
        private System.Windows.Forms.DataGridViewTextBoxColumn CurrentVersion;
        private System.Windows.Forms.DataGridViewTextBoxColumn NewReleaseVersion;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Release;
        private System.Windows.Forms.DataGridViewTextBoxColumn productName;
        private System.Windows.Forms.DataGridViewTextBoxColumn versionToRelease;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colRelease;
        private System.Windows.Forms.DataGridViewCheckBoxColumn HiddenTagBased;
        private System.Windows.Forms.ContextMenuStrip contextCheckAll;
        private System.Windows.Forms.ToolStripMenuItem checkAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uncheckAllToolStripMenuItem;

    }
}

