using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace HostsManager {
    public partial class FrmReplace : Form {
        /// <summary>
        /// 是否忽略大小写
        /// </summary>
        private bool _matchTheCase = false;
        private int _index = 0;

        /// <summary>
        /// 选中文本
        /// </summary>
        public FrmMain.dgReplace ReplaceText { get; set; }
        /// <summary>
        /// 查找字符串
        /// </summary>
        public FrmMain.dgTextIndexOf TextIndexOf { get; set; }
        public FrmReplace() {
            InitializeComponent();

            this.btnReplace.Click += btnReplace_Click;
            this.ShowInTaskbar = false;
            this.ShowIcon = false;
            this.CancelButton = new Button();
            (this.CancelButton as Button).Click += (sender, e) => { this.Close(); };
        }

        private void FrmFind_Load(object sender, EventArgs e) {
            if (this.Owner == null) {
                MessageBox.Show("无法获取父窗体");
                this.Close();
            }
        }

        //回车响应搜索
        private void txtSearchString_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == (char)Keys.Enter) {
                btnReplace_Click(sender, e);
                e.Handled = true;
                return;
            }
        }
        //启用/禁用查找
        private void txtSearchString_KeyUp(object sender, KeyEventArgs e) {
            string str = this.txtSearchString.Text.Trim();
            this.btnReplace.Enabled = str.Length > 0;
        }
        //匹配大小写
        private void chkIgnoringTheCase_Click(object sender, EventArgs e) {
            _matchTheCase = this.chkMatchTheCase.Checked;
        }

        //查找
        private void btnReplace_Click(object sender, EventArgs e) {
            if (this.txtSearchString.Text.Trim().Length == 0) {
                MessageBox.Show("请输入查询字符串！");
                return;
            } else if (this.txtReplaceString.Text.Trim().Length == 0) {
                MessageBox.Show("请输入替换字符串！");
                return;
            }
            bool exist = this.ReplaceString();
            this.btnNext.Enabled = exist;
        }
        //下一个
        private void btnNext_Click(object sender, EventArgs e) {
            if (this.txtSearchString.Text.Trim().Length == 0) {
                MessageBox.Show("请输入查询字符串！");
                return;
            } 
            else if (this.txtReplaceString.Text.Trim().Length == 0) {
                MessageBox.Show("请输入替换字符串！");
                return;
            }
            this.ReplaceString();
        }

        /// <summary>
        /// 搜索文本
        /// </summary>
        private bool ReplaceString() {
            string source = this.txtSearchString.Text.Trim();
            string target = this.txtReplaceString.Text.Trim();
            //查找字符串是否存在
            int idx = this.TextIndexOf(source, _index, this._matchTheCase);
            if (idx > 0) {
                this.ReplaceText(idx, source, target);
                _index = idx + 1;
            } else {
                if (_index > 0) {
                    MessageBox.Show("已经到达最底部！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    _index = 0;
                    this.ReplaceText(0, source, "");
                } else {
                    MessageBox.Show("没有可替换的字符串！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            return idx > 0;
        }
    }
}
