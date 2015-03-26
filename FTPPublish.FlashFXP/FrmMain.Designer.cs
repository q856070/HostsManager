namespace FTPPublish.FlashFXP {
    partial class FrmMain {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent() {
            this.label1 = new System.Windows.Forms.Label();
            this.txt_FlashFXPPath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_FTPSiteName = new System.Windows.Forms.TextBox();
            this.btn_Go = new System.Windows.Forms.Button();
            this.btn_SelFlashFXP = new System.Windows.Forms.Button();
            this.txt_BackPath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_FTPPath = new System.Windows.Forms.TextBox();
            this.txt_localPath = new System.Windows.Forms.TextBox();
            this.ckList_AllFiles = new System.Windows.Forms.CheckedListBox();
            this.btn_SelBackupDirectory = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_OpenFtp = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.txt_FlashFXPPwd = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(42, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "FlashFXP路径:";
            // 
            // txt_FlashFXPPath
            // 
            this.txt_FlashFXPPath.Location = new System.Drawing.Point(131, 24);
            this.txt_FlashFXPPath.Name = "txt_FlashFXPPath";
            this.txt_FlashFXPPath.Size = new System.Drawing.Size(275, 21);
            this.txt_FlashFXPPath.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "站点管理器中站点名:";
            // 
            // txt_FTPSiteName
            // 
            this.txt_FTPSiteName.Location = new System.Drawing.Point(131, 78);
            this.txt_FTPSiteName.Name = "txt_FTPSiteName";
            this.txt_FTPSiteName.Size = new System.Drawing.Size(166, 21);
            this.txt_FTPSiteName.TabIndex = 2;
            // 
            // btn_Go
            // 
            this.btn_Go.Location = new System.Drawing.Point(449, 3);
            this.btn_Go.Name = "btn_Go";
            this.btn_Go.Size = new System.Drawing.Size(75, 23);
            this.btn_Go.TabIndex = 7;
            this.btn_Go.Text = "备份并上传";
            this.btn_Go.UseVisualStyleBackColor = true;
            this.btn_Go.Click += new System.EventHandler(this.btn_Go_Click);
            // 
            // btn_SelFlashFXP
            // 
            this.btn_SelFlashFXP.Location = new System.Drawing.Point(412, 22);
            this.btn_SelFlashFXP.Name = "btn_SelFlashFXP";
            this.btn_SelFlashFXP.Size = new System.Drawing.Size(75, 23);
            this.btn_SelFlashFXP.TabIndex = 111;
            this.btn_SelFlashFXP.Text = "...";
            this.btn_SelFlashFXP.UseVisualStyleBackColor = true;
            this.btn_SelFlashFXP.Click += new System.EventHandler(this.btn_SelFlashFXP_Click);
            // 
            // txt_BackPath
            // 
            this.txt_BackPath.Location = new System.Drawing.Point(114, 42);
            this.txt_BackPath.Name = "txt_BackPath";
            this.txt_BackPath.Size = new System.Drawing.Size(404, 21);
            this.txt_BackPath.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "本地备份文件路径";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 12);
            this.label4.TabIndex = 1;
            this.label4.Text = "本地上传文件路径";
            // 
            // txt_FTPPath
            // 
            this.txt_FTPPath.Location = new System.Drawing.Point(114, 69);
            this.txt_FTPPath.Name = "txt_FTPPath";
            this.txt_FTPPath.Size = new System.Drawing.Size(183, 21);
            this.txt_FTPPath.TabIndex = 5;
            // 
            // txt_localPath
            // 
            this.txt_localPath.Location = new System.Drawing.Point(114, 15);
            this.txt_localPath.Name = "txt_localPath";
            this.txt_localPath.Size = new System.Drawing.Size(404, 21);
            this.txt_localPath.TabIndex = 3;
            // 
            // ckList_AllFiles
            // 
            this.ckList_AllFiles.CheckOnClick = true;
            this.ckList_AllFiles.FormattingEnabled = true;
            this.ckList_AllFiles.HorizontalScrollbar = true;
            this.ckList_AllFiles.Location = new System.Drawing.Point(3, 227);
            this.ckList_AllFiles.Name = "ckList_AllFiles";
            this.ckList_AllFiles.Size = new System.Drawing.Size(527, 276);
            this.ckList_AllFiles.TabIndex = 6;
            // 
            // btn_SelBackupDirectory
            // 
            this.btn_SelBackupDirectory.Location = new System.Drawing.Point(443, 67);
            this.btn_SelBackupDirectory.Name = "btn_SelBackupDirectory";
            this.btn_SelBackupDirectory.Size = new System.Drawing.Size(75, 23);
            this.btn_SelBackupDirectory.TabIndex = 111;
            this.btn_SelBackupDirectory.Text = "...";
            this.btn_SelBackupDirectory.UseVisualStyleBackColor = true;
            this.btn_SelBackupDirectory.Visible = false;
            this.btn_SelBackupDirectory.Click += new System.EventHandler(this.btn_SelBackupDirectory_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.btn_OpenFtp);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.txt_FlashFXPPwd);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btn_SelFlashFXP);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txt_FlashFXPPath);
            this.groupBox1.Controls.Add(this.txt_FTPSiteName);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(527, 112);
            this.groupBox1.TabIndex = 51;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "FTP软件配置(FlashFXP)";
            // 
            // btn_OpenFtp
            // 
            this.btn_OpenFtp.Location = new System.Drawing.Point(303, 76);
            this.btn_OpenFtp.Name = "btn_OpenFtp";
            this.btn_OpenFtp.Size = new System.Drawing.Size(75, 23);
            this.btn_OpenFtp.TabIndex = 112;
            this.btn_OpenFtp.Text = "打开FTP";
            this.btn_OpenFtp.UseVisualStyleBackColor = true;
            this.btn_OpenFtp.Click += new System.EventHandler(this.btn_OpenFtp_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(408, 54);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(89, 12);
            this.label7.TabIndex = 4;
            this.label7.Text = "(没有可不填写)";
            // 
            // txt_FlashFXPPwd
            // 
            this.txt_FlashFXPPwd.Location = new System.Drawing.Point(131, 51);
            this.txt_FlashFXPPwd.Name = "txt_FlashFXPPwd";
            this.txt_FlashFXPPwd.Size = new System.Drawing.Size(275, 21);
            this.txt_FlashFXPPwd.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(30, 54);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(95, 12);
            this.label6.TabIndex = 1;
            this.label6.Text = "FlashFXP的密码:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(25, 72);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(83, 12);
            this.label5.TabIndex = 1;
            this.label5.Text = "FTP的文件路径";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.Controls.Add(this.groupBox1);
            this.flowLayoutPanel1.Controls.Add(this.panel1);
            this.flowLayoutPanel1.Controls.Add(this.ckList_AllFiles);
            this.flowLayoutPanel1.Controls.Add(this.panel2);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(533, 545);
            this.flowLayoutPanel1.TabIndex = 61;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.btn_SelBackupDirectory);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txt_FTPPath);
            this.panel1.Controls.Add(this.txt_BackPath);
            this.panel1.Controls.Add(this.txt_localPath);
            this.panel1.Location = new System.Drawing.Point(3, 121);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(527, 100);
            this.panel1.TabIndex = 70;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btn_Go);
            this.panel2.Location = new System.Drawing.Point(3, 509);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(527, 29);
            this.panel2.TabIndex = 71;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(431, 69);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 113;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(533, 545);
            this.Controls.Add(this.flowLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ftp备份上传工具（FlashFXP）";
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_FlashFXPPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_FTPSiteName;
        private System.Windows.Forms.Button btn_Go;
        private System.Windows.Forms.Button btn_SelFlashFXP;
        private System.Windows.Forms.TextBox txt_BackPath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txt_FTPPath;
        private System.Windows.Forms.TextBox txt_localPath;
        private System.Windows.Forms.CheckedListBox ckList_AllFiles;
        private System.Windows.Forms.Button btn_SelBackupDirectory;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txt_FlashFXPPwd;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btn_OpenFtp;
        private System.Windows.Forms.Button button1;
    }
}

