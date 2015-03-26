namespace HostsManager {
    partial class FrmFind {
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
            this.btnFind = new System.Windows.Forms.Button();
            this.txtSearchString = new System.Windows.Forms.TextBox();
            this.lblFindStr = new System.Windows.Forms.Label();
            this.btnNext = new System.Windows.Forms.Button();
            this.chkMatchTheCase = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnFind
            // 
            this.btnFind.Location = new System.Drawing.Point(248, 81);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(80, 25);
            this.btnFind.TabIndex = 0;
            this.btnFind.Text = "查找";
            this.btnFind.UseVisualStyleBackColor = true;
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
            this.btnNext.Location = new System.Drawing.Point(352, 81);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(80, 25);
            this.btnNext.TabIndex = 4;
            this.btnNext.Text = "下一个";
            this.btnNext.UseVisualStyleBackColor = true;
            // 
            // chkMatchTheCase
            // 
            this.chkMatchTheCase.AutoSize = true;
            this.chkMatchTheCase.Location = new System.Drawing.Point(85, 86);
            this.chkMatchTheCase.Name = "chkMatchTheCase";
            this.chkMatchTheCase.Size = new System.Drawing.Size(84, 16);
            this.chkMatchTheCase.TabIndex = 5;
            this.chkMatchTheCase.Text = "匹配大小写";
            this.chkMatchTheCase.UseVisualStyleBackColor = true;
            // 
            // FrmFind
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(444, 128);
            this.Controls.Add(this.chkMatchTheCase);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.lblFindStr);
            this.Controls.Add(this.txtSearchString);
            this.Controls.Add(this.btnFind);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmFind";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "查找";
            this.Load += new System.EventHandler(this.FrmFind_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.TextBox txtSearchString;
        private System.Windows.Forms.Label lblFindStr;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.CheckBox chkMatchTheCase;
    }
}