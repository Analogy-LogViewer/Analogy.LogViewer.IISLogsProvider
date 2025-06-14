namespace Analogy.LogViewer.IISLogsProvider
{
    partial class IISUserSettingsUC
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tabControlFiles = new System.Windows.Forms.TabControl();
            tabPageFilesGeneral = new System.Windows.Forms.TabPage();
            groupBox2 = new System.Windows.Forms.GroupBox();
            rbtnApplicationFolder = new System.Windows.Forms.RadioButton();
            rbtnPerUser = new System.Windows.Forms.RadioButton();
            tabPageFileParsingSettings = new System.Windows.Forms.TabPage();
            panel1 = new System.Windows.Forms.Panel();
            pnlBottom = new System.Windows.Forms.Panel();
            btnSave = new System.Windows.Forms.Button();
            cbGeoService = new System.Windows.Forms.CheckBox();
            tabControlFiles.SuspendLayout();
            tabPageFilesGeneral.SuspendLayout();
            groupBox2.SuspendLayout();
            tabPageFileParsingSettings.SuspendLayout();
            panel1.SuspendLayout();
            pnlBottom.SuspendLayout();
            SuspendLayout();
            // 
            // tabControlFiles
            // 
            tabControlFiles.Controls.Add(tabPageFilesGeneral);
            tabControlFiles.Controls.Add(tabPageFileParsingSettings);
            tabControlFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            tabControlFiles.Location = new System.Drawing.Point(0, 0);
            tabControlFiles.Name = "tabControlFiles";
            tabControlFiles.SelectedIndex = 0;
            tabControlFiles.Size = new System.Drawing.Size(832, 360);
            tabControlFiles.TabIndex = 50;
            // 
            // tabPageFilesGeneral
            // 
            tabPageFilesGeneral.Controls.Add(groupBox2);
            tabPageFilesGeneral.ImageKey = "Technology_32x32.png";
            tabPageFilesGeneral.Location = new System.Drawing.Point(4, 28);
            tabPageFilesGeneral.Name = "tabPageFilesGeneral";
            tabPageFilesGeneral.Size = new System.Drawing.Size(824, 328);
            tabPageFilesGeneral.TabIndex = 2;
            tabPageFilesGeneral.Text = "General Settings";
            tabPageFilesGeneral.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(rbtnApplicationFolder);
            groupBox2.Controls.Add(rbtnPerUser);
            groupBox2.Location = new System.Drawing.Point(14, 22);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new System.Drawing.Size(683, 94);
            groupBox2.TabIndex = 0;
            groupBox2.TabStop = false;
            groupBox2.Text = "Storage and Location";
            // 
            // rbtnApplicationFolder
            // 
            rbtnApplicationFolder.AutoSize = true;
            rbtnApplicationFolder.Location = new System.Drawing.Point(14, 54);
            rbtnApplicationFolder.Name = "rbtnApplicationFolder";
            rbtnApplicationFolder.Size = new System.Drawing.Size(621, 25);
            rbtnApplicationFolder.TabIndex = 1;
            rbtnApplicationFolder.TabStop = true;
            rbtnApplicationFolder.Text = "Portable: Store settings in the Application Folder (May need folder permissions)";
            rbtnApplicationFolder.UseVisualStyleBackColor = true;
            // 
            // rbtnPerUser
            // 
            rbtnPerUser.AutoSize = true;
            rbtnPerUser.Location = new System.Drawing.Point(14, 22);
            rbtnPerUser.Name = "rbtnPerUser";
            rbtnPerUser.Size = new System.Drawing.Size(605, 25);
            rbtnPerUser.TabIndex = 0;
            rbtnPerUser.TabStop = true;
            rbtnPerUser.Text = "Per User: Store settings in: %userprofile%\\appdata\\local\\Analogy.LogViewer";
            rbtnPerUser.UseVisualStyleBackColor = true;
            // 
            // tabPageFileParsingSettings
            // 
            tabPageFileParsingSettings.Controls.Add(cbGeoService);
            tabPageFileParsingSettings.ImageKey = "EmptyTableRowSeparator_32x32.png";
            tabPageFileParsingSettings.Location = new System.Drawing.Point(4, 28);
            tabPageFileParsingSettings.Name = "tabPageFileParsingSettings";
            tabPageFileParsingSettings.Padding = new System.Windows.Forms.Padding(3);
            tabPageFileParsingSettings.Size = new System.Drawing.Size(824, 328);
            tabPageFileParsingSettings.TabIndex = 0;
            tabPageFileParsingSettings.Text = "Geo Location Service";
            tabPageFileParsingSettings.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            panel1.Controls.Add(tabControlFiles);
            panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            panel1.Location = new System.Drawing.Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(832, 360);
            panel1.TabIndex = 51;
            // 
            // pnlBottom
            // 
            pnlBottom.Controls.Add(btnSave);
            pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            pnlBottom.Location = new System.Drawing.Point(0, 360);
            pnlBottom.Name = "pnlBottom";
            pnlBottom.Size = new System.Drawing.Size(832, 60);
            pnlBottom.TabIndex = 52;
            // 
            // btnSave
            // 
            btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnSave.Location = new System.Drawing.Point(695, 7);
            btnSave.Name = "btnSave";
            btnSave.Size = new System.Drawing.Size(133, 47);
            btnSave.TabIndex = 2;
            btnSave.Text = "Save Settings";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // cbGeoService
            // 
            cbGeoService.AutoSize = true;
            cbGeoService.Location = new System.Drawing.Point(21, 18);
            cbGeoService.Name = "cbGeoService";
            cbGeoService.Size = new System.Drawing.Size(235, 25);
            cbGeoService.TabIndex = 50;
            cbGeoService.Text = "Enable GeoLocation service";
            cbGeoService.UseVisualStyleBackColor = true;
            // 
            // IISUserSettingsUC
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(panel1);
            Controls.Add(pnlBottom);
            Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            Name = "IISUserSettingsUC";
            Size = new System.Drawing.Size(832, 420);
            Load += IISUserSettingsUC_Load;
            tabControlFiles.ResumeLayout(false);
            tabPageFilesGeneral.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            tabPageFileParsingSettings.ResumeLayout(false);
            tabPageFileParsingSettings.PerformLayout();
            panel1.ResumeLayout(false);
            pnlBottom.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlFiles;
        private System.Windows.Forms.TabPage tabPageFilesGeneral;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rbtnApplicationFolder;
        private System.Windows.Forms.RadioButton rbtnPerUser;
        private System.Windows.Forms.TabPage tabPageFileParsingSettings;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.CheckBox cbGeoService;
    }
}
