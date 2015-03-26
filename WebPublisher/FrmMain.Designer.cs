namespace WebPublisher {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.cobPathValue = new System.Windows.Forms.ComboBox();
            this.btnSelectPath = new System.Windows.Forms.Button();
            this.btnDestinPath = new System.Windows.Forms.Button();
            this.btnCopyFile = new System.Windows.Forms.Button();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.txt_WorkspaceDir = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_ignoreDirs = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.ckl_Files = new System.Windows.Forms.CheckedListBox();
            this.btn_OK = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.btn_CopyFiles = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cobPathValue
            // 
            this.cobPathValue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cobPathValue.FormattingEnabled = true;
            this.cobPathValue.Location = new System.Drawing.Point(147, 90);
            this.cobPathValue.Name = "cobPathValue";
            this.cobPathValue.Size = new System.Drawing.Size(357, 20);
            this.cobPathValue.TabIndex = 1;
            this.cobPathValue.SelectedIndexChanged += new System.EventHandler(this.cobPathValue_SelectedIndexChanged);
            this.cobPathValue.KeyUp += new System.Windows.Forms.KeyEventHandler(this.cobPathValue_KeyUp);
            // 
            // btnSelectPath
            // 
            this.btnSelectPath.Location = new System.Drawing.Point(677, 88);
            this.btnSelectPath.Name = "btnSelectPath";
            this.btnSelectPath.Size = new System.Drawing.Size(59, 23);
            this.btnSelectPath.TabIndex = 111;
            this.btnSelectPath.Text = "...";
            this.btnSelectPath.UseVisualStyleBackColor = true;
            this.btnSelectPath.Click += new System.EventHandler(this.btnSelectPath_Click);
            // 
            // btnDestinPath
            // 
            this.btnDestinPath.Location = new System.Drawing.Point(445, 23);
            this.btnDestinPath.Name = "btnDestinPath";
            this.btnDestinPath.Size = new System.Drawing.Size(59, 23);
            this.btnDestinPath.TabIndex = 111;
            this.btnDestinPath.Text = "...";
            this.btnDestinPath.UseVisualStyleBackColor = true;
            this.btnDestinPath.Click += new System.EventHandler(this.btnDestinPath_Click);
            // 
            // btnCopyFile
            // 
            this.btnCopyFile.Location = new System.Drawing.Point(374, 262);
            this.btnCopyFile.Name = "btnCopyFile";
            this.btnCopyFile.Size = new System.Drawing.Size(130, 35);
            this.btnCopyFile.TabIndex = 0;
            this.btnCopyFile.Text = "检查更新↓";
            this.btnCopyFile.UseVisualStyleBackColor = true;
            this.btnCopyFile.Click += new System.EventHandler(this.btnCopyFile_Click);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(208, 264);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(160, 21);
            this.dateTimePicker1.TabIndex = 4;
            // 
            // txt_WorkspaceDir
            // 
            this.txt_WorkspaceDir.Location = new System.Drawing.Point(75, 25);
            this.txt_WorkspaceDir.Name = "txt_WorkspaceDir";
            this.txt_WorkspaceDir.Size = new System.Drawing.Size(357, 21);
            this.txt_WorkspaceDir.TabIndex = 2;
            this.txt_WorkspaceDir.TextChanged += new System.EventHandler(this.txtTargetPath_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 120);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(293, 12);
            this.label2.TabIndex = 81;
            this.label2.Text = "忽略的目录(相对路径,一个一行,用\"\\\",前后不带\"\\\"):";
            // 
            // txt_ignoreDirs
            // 
            this.txt_ignoreDirs.Location = new System.Drawing.Point(12, 135);
            this.txt_ignoreDirs.Multiline = true;
            this.txt_ignoreDirs.Name = "txt_ignoreDirs";
            this.txt_ignoreDirs.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txt_ignoreDirs.Size = new System.Drawing.Size(724, 121);
            this.txt_ignoreDirs.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 93);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(131, 12);
            this.label3.TabIndex = 101;
            this.label3.Text = "提取源(来自解决方案):";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(510, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(161, 12);
            this.label4.TabIndex = 101;
            this.label4.Text = "不在下拉中?请选择其他位置:";
            // 
            // ckl_Files
            // 
            this.ckl_Files.CheckOnClick = true;
            this.ckl_Files.FormattingEnabled = true;
            this.ckl_Files.HorizontalScrollbar = true;
            this.ckl_Files.Location = new System.Drawing.Point(13, 303);
            this.ckl_Files.Name = "ckl_Files";
            this.ckl_Files.Size = new System.Drawing.Size(724, 244);
            this.ckl_Files.TabIndex = 5;
            // 
            // btn_OK
            // 
            this.btn_OK.Location = new System.Drawing.Point(610, 553);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(126, 40);
            this.btn_OK.TabIndex = 6;
            this.btn_OK.Text = "提取文件并发布...";
            this.btn_OK.UseVisualStyleBackColor = true;
            this.btn_OK.Click += new System.EventHandler(this.btn_OK_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(73, 49);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(371, 12);
            this.label6.TabIndex = 101;
            this.label6.Text = "该目录为所有项目发布/备份文件的根目录，自动创建项目对应目录。";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(206, 285);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(149, 12);
            this.label9.TabIndex = 112;
            this.label9.Text = "↑该时间之后有修改的文件";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 28);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 12);
            this.label5.TabIndex = 101;
            this.label5.Text = "工作空间:";
            // 
            // label10
            // 
            this.label10.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label10.Location = new System.Drawing.Point(-55, 75);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(1317, 3);
            this.label10.TabIndex = 101;
            // 
            // btn_CopyFiles
            // 
            this.btn_CopyFiles.Location = new System.Drawing.Point(488, 553);
            this.btn_CopyFiles.Name = "btn_CopyFiles";
            this.btn_CopyFiles.Size = new System.Drawing.Size(116, 40);
            this.btn_CopyFiles.TabIndex = 113;
            this.btn_CopyFiles.Text = "提取这些文件";
            this.btn_CopyFiles.UseVisualStyleBackColor = true;
            this.btn_CopyFiles.Click += new System.EventHandler(this.btn_CopyFiles_Click);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(749, 619);
            this.Controls.Add(this.btn_CopyFiles);
            this.Controls.Add(this.btnDestinPath);
            this.Controls.Add(this.txt_WorkspaceDir);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.btn_OK);
            this.Controls.Add(this.ckl_Files);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txt_ignoreDirs);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.btnCopyFile);
            this.Controls.Add(this.btnSelectPath);
            this.Controls.Add(this.cobPathValue);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "提取发布文件";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cobPathValue;
        private System.Windows.Forms.Button btnSelectPath;
        private System.Windows.Forms.Button btnDestinPath;
        private System.Windows.Forms.Button btnCopyFile;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.TextBox txt_WorkspaceDir;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_ignoreDirs;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckedListBox ckl_Files;
        private System.Windows.Forms.Button btn_OK;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btn_CopyFiles;
    }
}

