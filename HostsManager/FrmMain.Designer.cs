namespace HostsManager {
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.splitC_Main = new System.Windows.Forms.SplitContainer();
            this.tab_AllHost = new System.Windows.Forms.TabControl();
            this.cmenu_Tab = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.TSMenuItem_Del = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMenuItem_Rename = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMenuItem_CreateDesktop = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbtn_SaveAllHS = new System.Windows.Forms.ToolStripButton();
            this.tsbtn_UseCurrHS = new System.Windows.Forms.ToolStripButton();
            this.tsbtn_AddHS = new System.Windows.Forms.ToolStripButton();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.splitC_Main.Panel1.SuspendLayout();
            this.splitC_Main.Panel2.SuspendLayout();
            this.splitC_Main.SuspendLayout();
            this.cmenu_Tab.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitC_Main
            // 
            this.splitC_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitC_Main.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitC_Main.Location = new System.Drawing.Point(0, 0);
            this.splitC_Main.Name = "splitC_Main";
            this.splitC_Main.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitC_Main.Panel1
            // 
            this.splitC_Main.Panel1.Controls.Add(this.tab_AllHost);
            // 
            // splitC_Main.Panel2
            // 
            this.splitC_Main.Panel2.Controls.Add(this.label1);
            this.splitC_Main.Size = new System.Drawing.Size(669, 601);
            this.splitC_Main.SplitterDistance = 575;
            this.splitC_Main.SplitterWidth = 1;
            this.splitC_Main.TabIndex = 100;
            // 
            // tab_AllHost
            // 
            this.tab_AllHost.ContextMenuStrip = this.cmenu_Tab;
            this.tab_AllHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tab_AllHost.Location = new System.Drawing.Point(0, 0);
            this.tab_AllHost.Multiline = true;
            this.tab_AllHost.Name = "tab_AllHost";
            this.tab_AllHost.SelectedIndex = 0;
            this.tab_AllHost.Size = new System.Drawing.Size(669, 575);
            this.tab_AllHost.TabIndex = 2;
            this.tab_AllHost.Selected += new System.Windows.Forms.TabControlEventHandler(this.tab_AllHost_Selected);
            // 
            // cmenu_Tab
            // 
            this.cmenu_Tab.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMenuItem_Del,
            this.TSMenuItem_Rename,
            this.TSMenuItem_CreateDesktop});
            this.cmenu_Tab.Name = "cmenu_Tab";
            this.cmenu_Tab.Size = new System.Drawing.Size(213, 70);
            this.cmenu_Tab.Opening += new System.ComponentModel.CancelEventHandler(this.cmenu_Tab_Opening);
            // 
            // TSMenuItem_Del
            // 
            this.TSMenuItem_Del.Name = "TSMenuItem_Del";
            this.TSMenuItem_Del.Size = new System.Drawing.Size(212, 22);
            this.TSMenuItem_Del.Text = "删除方案(&D)";
            this.TSMenuItem_Del.Click += new System.EventHandler(this.TSMenuItem_Del_Click);
            // 
            // TSMenuItem_Rename
            // 
            this.TSMenuItem_Rename.Name = "TSMenuItem_Rename";
            this.TSMenuItem_Rename.Size = new System.Drawing.Size(212, 22);
            this.TSMenuItem_Rename.Text = "重命名方案(&R)";
            this.TSMenuItem_Rename.Click += new System.EventHandler(this.TSMenuItem_Rename_Click);
            // 
            // TSMenuItem_CreateDesktop
            // 
            this.TSMenuItem_CreateDesktop.Name = "TSMenuItem_CreateDesktop";
            this.TSMenuItem_CreateDesktop.Size = new System.Drawing.Size(212, 22);
            this.TSMenuItem_CreateDesktop.Text = "创建方案桌面快捷方式(&C)";
            this.TSMenuItem_CreateDesktop.Click += new System.EventHandler(this.TSMenuItem_CreateDesktop_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(329, 12);
            this.label1.TabIndex = 21;
            this.label1.Text = "提示:使用快捷键\"Ctrl+S\"可保存,\"Ctrl+Alt+S\"保存并且关闭";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbtn_SaveAllHS,
            this.tsbtn_UseCurrHS,
            this.tsbtn_AddHS});
            this.toolStrip1.Location = new System.Drawing.Point(3, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.toolStrip1.Size = new System.Drawing.Size(264, 25);
            this.toolStrip1.TabIndex = 101;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbtn_SaveAllHS
            // 
            this.tsbtn_SaveAllHS.Image = ((System.Drawing.Image)(resources.GetObject("tsbtn_SaveAllHS.Image")));
            this.tsbtn_SaveAllHS.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtn_SaveAllHS.Name = "tsbtn_SaveAllHS";
            this.tsbtn_SaveAllHS.Size = new System.Drawing.Size(76, 22);
            this.tsbtn_SaveAllHS.Text = "保存全部";
            this.tsbtn_SaveAllHS.Click += new System.EventHandler(this.tsbtn_SaveAllHS_Click);
            // 
            // tsbtn_UseCurrHS
            // 
            this.tsbtn_UseCurrHS.Enabled = false;
            this.tsbtn_UseCurrHS.Image = ((System.Drawing.Image)(resources.GetObject("tsbtn_UseCurrHS.Image")));
            this.tsbtn_UseCurrHS.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtn_UseCurrHS.Name = "tsbtn_UseCurrHS";
            this.tsbtn_UseCurrHS.Size = new System.Drawing.Size(100, 22);
            this.tsbtn_UseCurrHS.Text = "启用当前方案";
            this.tsbtn_UseCurrHS.Click += new System.EventHandler(this.tsbtn_UseCurrHS_Click);
            // 
            // tsbtn_AddHS
            // 
            this.tsbtn_AddHS.Image = ((System.Drawing.Image)(resources.GetObject("tsbtn_AddHS.Image")));
            this.tsbtn_AddHS.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtn_AddHS.Name = "tsbtn_AddHS";
            this.tsbtn_AddHS.Size = new System.Drawing.Size(76, 22);
            this.tsbtn_AddHS.Text = "添加方案";
            this.tsbtn_AddHS.Click += new System.EventHandler(this.tsbtn_AddHS_Click);
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.AutoScroll = true;
            this.toolStripContainer1.ContentPanel.Controls.Add(this.splitC_Main);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(669, 601);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(669, 626);
            this.toolStripContainer1.TabIndex = 102;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip1);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(669, 626);
            this.Controls.Add(this.toolStripContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Host管理器";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.splitC_Main.Panel1.ResumeLayout(false);
            this.splitC_Main.Panel2.ResumeLayout(false);
            this.splitC_Main.Panel2.PerformLayout();
            this.splitC_Main.ResumeLayout(false);
            this.cmenu_Tab.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitC_Main;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tab_AllHost;
        private System.Windows.Forms.ContextMenuStrip cmenu_Tab;
        private System.Windows.Forms.ToolStripMenuItem TSMenuItem_Del;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbtn_SaveAllHS;
        private System.Windows.Forms.ToolStripButton tsbtn_UseCurrHS;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStripButton tsbtn_AddHS;
        private System.Windows.Forms.ToolStripMenuItem TSMenuItem_Rename;
        private System.Windows.Forms.ToolStripMenuItem TSMenuItem_CreateDesktop;
    }
}

