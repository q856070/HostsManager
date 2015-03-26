namespace ProjectsSvnCfg {
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
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.btn_SaveGO = new System.Windows.Forms.Button();
            this.txt_CodeDir = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_CodeDirSel = new System.Windows.Forms.Button();
            this.txt_ServerAddress = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btn_ServerAddressCheck = new System.Windows.Forms.Button();
            this.txt_CoderName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btn_CoderNameCheck = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_SaveGO
            // 
            this.btn_SaveGO.Location = new System.Drawing.Point(203, 127);
            this.btn_SaveGO.Name = "btn_SaveGO";
            this.btn_SaveGO.Size = new System.Drawing.Size(98, 53);
            this.btn_SaveGO.TabIndex = 20;
            this.btn_SaveGO.Text = "保存";
            this.btn_SaveGO.UseVisualStyleBackColor = true;
            this.btn_SaveGO.Click += new System.EventHandler(this.btn_SaveGO_Click);
            // 
            // txt_CodeDir
            // 
            this.txt_CodeDir.Location = new System.Drawing.Point(113, 54);
            this.txt_CodeDir.Name = "txt_CodeDir";
            this.txt_CodeDir.Size = new System.Drawing.Size(280, 21);
            this.txt_CodeDir.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 12);
            this.label2.TabIndex = 200;
            this.label2.Text = "Code本地目录:";
            // 
            // btn_CodeDirSel
            // 
            this.btn_CodeDirSel.Location = new System.Drawing.Point(399, 52);
            this.btn_CodeDirSel.Name = "btn_CodeDirSel";
            this.btn_CodeDirSel.Size = new System.Drawing.Size(75, 23);
            this.btn_CodeDirSel.TabIndex = 100;
            this.btn_CodeDirSel.Text = "浏览...";
            this.btn_CodeDirSel.UseVisualStyleBackColor = true;
            this.btn_CodeDirSel.Click += new System.EventHandler(this.btn_CodeDirSel_Click);
            // 
            // txt_ServerAddress
            // 
            this.txt_ServerAddress.Location = new System.Drawing.Point(113, 27);
            this.txt_ServerAddress.Name = "txt_ServerAddress";
            this.txt_ServerAddress.Size = new System.Drawing.Size(280, 21);
            this.txt_ServerAddress.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 12);
            this.label3.TabIndex = 200;
            this.label3.Text = "发布器服务地址:";
            // 
            // btn_ServerAddressCheck
            // 
            this.btn_ServerAddressCheck.Location = new System.Drawing.Point(399, 25);
            this.btn_ServerAddressCheck.Name = "btn_ServerAddressCheck";
            this.btn_ServerAddressCheck.Size = new System.Drawing.Size(75, 23);
            this.btn_ServerAddressCheck.TabIndex = 100;
            this.btn_ServerAddressCheck.Text = "检查";
            this.btn_ServerAddressCheck.UseVisualStyleBackColor = true;
            this.btn_ServerAddressCheck.Click += new System.EventHandler(this.btn_ServerAddressCheck_Click);
            // 
            // txt_CoderName
            // 
            this.txt_CoderName.Location = new System.Drawing.Point(113, 81);
            this.txt_CoderName.Name = "txt_CoderName";
            this.txt_CoderName.Size = new System.Drawing.Size(280, 21);
            this.txt_CoderName.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(48, 84);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 200;
            this.label4.Text = "你的姓名:";
            // 
            // btn_CoderNameCheck
            // 
            this.btn_CoderNameCheck.Location = new System.Drawing.Point(399, 79);
            this.btn_CoderNameCheck.Name = "btn_CoderNameCheck";
            this.btn_CoderNameCheck.Size = new System.Drawing.Size(75, 23);
            this.btn_CoderNameCheck.TabIndex = 100;
            this.btn_CoderNameCheck.Text = "检查";
            this.btn_CoderNameCheck.UseVisualStyleBackColor = true;
            this.btn_CoderNameCheck.Click += new System.EventHandler(this.btn_CoderNameCheck_Click);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(502, 192);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txt_CodeDir);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txt_CoderName);
            this.Controls.Add(this.txt_ServerAddress);
            this.Controls.Add(this.btn_CodeDirSel);
            this.Controls.Add(this.btn_CoderNameCheck);
            this.Controls.Add(this.btn_ServerAddressCheck);
            this.Controls.Add(this.btn_SaveGO);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "发布器设置";
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button btn_SaveGO;
        private System.Windows.Forms.TextBox txt_CodeDir;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_CodeDirSel;
        private System.Windows.Forms.TextBox txt_ServerAddress;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_ServerAddressCheck;
        private System.Windows.Forms.TextBox txt_CoderName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btn_CoderNameCheck;
    }
}

