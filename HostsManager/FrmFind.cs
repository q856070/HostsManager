using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace HostsManager {
    public partial class FrmFind : Form {
        /// <summary>
        /// 是否忽略大小写
        /// </summary>
        private bool _matchTheCase = false;
        private int _index = 0;

        /// <summary>
        /// 选中文本
        /// </summary>
        public FrmMain.dgSelect SelectText { get; set; }
        /// <summary>
        /// 查找字符串
        /// </summary>
        public FrmMain.dgTextIndexOf TextIndexOf { get; set; }

        public FrmFind() {
            InitializeComponent();
            this.ShowInTaskbar = false;
            this.ShowIcon = false;
            this.btnFind.Enabled = this.btnNext.Enabled = false;

            this.btnFind.Click += btnFind_Click;
            this.btnNext.Click += btnNext_Click;
            this.chkMatchTheCase.Click += chkIgnoringTheCase_Click;
            this.txtSearchString.KeyPress += txtSearchString_KeyPress;
            this.txtSearchString.KeyUp += txtSearchString_KeyUp;
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
                btnFind_Click(sender, e);
                e.Handled = true;
                return;
            }
        }
        //启用/禁用查找
        private void txtSearchString_KeyUp(object sender, KeyEventArgs e) {
            string str = this.txtSearchString.Text.Trim();
            this.btnFind.Enabled = str.Length > 0;
        }
        //匹配大小写
        private void chkIgnoringTheCase_Click(object sender, EventArgs e) {
            _matchTheCase = this.chkMatchTheCase.Checked;
        }

        //查找
        private void btnFind_Click(object sender, EventArgs e) {
            if (this.txtSearchString.Text.Trim().Length == 0) {
                MessageBox.Show("请输入查询字符串！");
                return;
            }
            bool exist = this.SearchString();
            this.btnNext.Enabled = exist;
        }
        //下一个
        private void btnNext_Click(object sender, EventArgs e) {
            if (this.txtSearchString.Text.Trim().Length == 0) {
                MessageBox.Show("请输入查询字符串！");
                return;
            }
            this.SearchString();
        }

        /// <summary>
        /// 搜索文本
        /// </summary>
        private bool SearchString() {
            string str = this.txtSearchString.Text.Trim();
            //查找字符串是否存在
            int idx = this.TextIndexOf(str, _index, this._matchTheCase);
            if (idx > 0) {
                this.SelectText(idx, str.Length);
                _index = idx + 1;
            } else {
                if (_index > 0) {
                    MessageBox.Show("已经到达最底部！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    _index = 0;
                    this.SelectText(0, 0);
                } else {
                    MessageBox.Show("没有找到字符串！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            return idx > 0;
        }
    }
}
