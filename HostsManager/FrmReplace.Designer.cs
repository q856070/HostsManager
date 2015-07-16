namespace HostsManager {
    partial class FrmReplace {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.btnReplaceAll = new System.Windows.Forms.Button();
            this.txtSearchString = new System.Windows.Forms.TextBox();
            this.lblFindStr = new System.Windows.Forms.Label();
            this.btnNext = new System.Windows.Forms.Button();
            this.chkMatchTheCase = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtReplaceString = new System.Windows.Forms.TextBox();
            this.btnReplace = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnReplaceAll
            // 
            this.btnReplaceAll.Location = new System.Drawing.Point(266, 112);
            this.btnReplaceAll.Name = "btnReplaceAll";
            this.btnReplaceAll.Size = new System.Drawing.Size(80, 25);
            this.btnReplaceAll.TabIndex = 0;
            this.btnReplaceAll.Text = "全部替换";
            this.btnReplaceAll.UseVisualStyleBackColor = true;
            // 
            // txtSearchString
            // 
            this.txtSearchString.Location = new System.Drawing.Point(85, 29);
            this.txtSearchString.Name = "txtSearchString";
            this.txtSearchString.Size = new System.Drawing.Size(347, 21);
            this.txtSearchString.TabIndex = 2;
            // 
            // lblFindStr
            // 
            this.lblFindStr.AutoSize = true;
            this.lblFindStr.Location = new System.Drawing.Point(12, 32);
            this.lblFindStr.Name = "lblFindStr";
            this.lblFindStr.Size = new System.Drawing.Size(65, 12);
            this.lblFindStr.TabIndex = 3;
            this.lblFindStr.Text = "查询文本：";
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(352, 112);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(80, 25);
            this.btnNext.TabIndex = 4;
            this.btnNext.Text = "下一个";
            this.btnNext.UseVisualStyleBackColor = true;
            // 
            // chkMatchTheCase
            // 
            this.chkMatchTheCase.AutoSize = true;
            this.chkMatchTheCase.Location = new System.Drawing.Point(85, 117);
            this.chkMatchTheCase.Name = "chkMatchTheCase";
            this.chkMatchTheCase.Size = new System.Drawing.Size(84, 16);
            this.chkMatchTheCase.TabIndex = 5;
            this.chkMatchTheCase.Text = "匹配大小写";
            this.chkMatchTheCase.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "替换文本：";
            // 
            // txtReplaceString
            // 
            this.txtReplaceString.Location = new System.Drawing.Point(85, 66);
            this.txtReplaceString.Name = "txtReplaceString";
            this.txtReplaceString.Size = new System.Drawing.Size(347, 21);
            this.txtReplaceString.TabIndex = 6;
            // 
            // btnReplace
            // 
            this.btnReplace.Location = new System.Drawing.Point(180, 112);
            this.btnReplace.Name = "btnReplace";
            this.btnReplace.Size = new System.Drawing.Size(80, 25);
            this.btnReplace.TabIndex = 8;
            this.btnReplace.Text = "替换";
            this.btnReplace.UseVisualStyleBackColor = true;
            // 
            // FrmReplace
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(444, 153);
            this.Controls.Add(this.btnReplace);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtReplaceString);
            this.Controls.Add(this.chkMatchTheCase);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.lblFindStr);
            this.Controls.Add(this.txtSearchString);
            this.Controls.Add(this.btnReplaceAll);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmReplace";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "替换";
            this.Load += new System.EventHandler(this.FrmFind_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnReplaceAll;
        private System.Windows.Forms.TextBox txtSearchString;
        private System.Windows.Forms.Label lblFindStr;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.CheckBox chkMatchTheCase;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtReplaceString;
        private System.Windows.Forms.Button btnReplace;
    }
}