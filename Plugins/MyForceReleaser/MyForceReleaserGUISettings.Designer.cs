namespace MyForceReleaser
{
    partial class MyForceReleaserGUISettings
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
            this.lblInternalRepoPath = new System.Windows.Forms.Label();
            this.txtInternalRepoPath = new System.Windows.Forms.TextBox();
            this.btnSelectInternalRepo = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblInternalRepoPath
            // 
            this.lblInternalRepoPath.AutoSize = true;
            this.lblInternalRepoPath.Location = new System.Drawing.Point(13, 13);
            this.lblInternalRepoPath.Name = "lblInternalRepoPath";
            this.lblInternalRepoPath.Size = new System.Drawing.Size(96, 13);
            this.lblInternalRepoPath.TabIndex = 0;
            this.lblInternalRepoPath.Text = "Internal Repo Path";
            // 
            // txtInternalRepoPath
            // 
            this.txtInternalRepoPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInternalRepoPath.Location = new System.Drawing.Point(115, 10);
            this.txtInternalRepoPath.Name = "txtInternalRepoPath";
            this.txtInternalRepoPath.ReadOnly = true;
            this.txtInternalRepoPath.Size = new System.Drawing.Size(294, 20);
            this.txtInternalRepoPath.TabIndex = 1;
            // 
            // btnSelectInternalRepo
            // 
            this.btnSelectInternalRepo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectInternalRepo.Location = new System.Drawing.Point(415, 8);
            this.btnSelectInternalRepo.Name = "btnSelectInternalRepo";
            this.btnSelectInternalRepo.Size = new System.Drawing.Size(30, 23);
            this.btnSelectInternalRepo.TabIndex = 2;
            this.btnSelectInternalRepo.Text = "...";
            this.btnSelectInternalRepo.UseVisualStyleBackColor = true;
            this.btnSelectInternalRepo.Click += new System.EventHandler(this.btnSelectInternalRepo_Click);
            // 
            // MyForceReleaserGUISettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(457, 44);
            this.Controls.Add(this.btnSelectInternalRepo);
            this.Controls.Add(this.txtInternalRepoPath);
            this.Controls.Add(this.lblInternalRepoPath);
            this.Name = "MyForceReleaserGUISettings";
            this.Text = "MyForceReleaserGUISettings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblInternalRepoPath;
        private System.Windows.Forms.TextBox txtInternalRepoPath;
        private System.Windows.Forms.Button btnSelectInternalRepo;
    }
}