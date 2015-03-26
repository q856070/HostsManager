using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace HostsManager {
    public partial class FrmCreateHS : Form {
        private static string[] fileNameNoChr = new string[] { "\\", "/", ":", "*", "?", "\"", "<", ">", "|" };
        private static string[] sysHSName = new string[] { "(默认)" };//小写

        public string HSName { get; set; }

        private List<MHostScheme> _allHSList = null;
        private EAddHSMode _addMode;
        private MHostScheme _oldHS;

        public FrmCreateHS(List<MHostScheme> allHSList, EAddHSMode addMode, MHostScheme oldHS) {
            _allHSList = allHSList;
            _addMode = addMode;
            _oldHS = oldHS;
            InitializeComponent();
        }

        private void btn_Cancel_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void FrmCreateHS_Load(object sender, EventArgs e) {
            switch (_addMode) {
                case EAddHSMode.Create:
                    this.Text = "添加HOST方案";
                    break;
                case EAddHSMode.Rename:
                    this.Text = "编辑HOST方案[" + _oldHS.SchemeName + "]";
                    txt_HSName.Text = _oldHS.SchemeName;
                    txt_HSName.SelectAll();
                    break;
            }
        }

        private void btn_OK_Click(object sender, EventArgs e) {
            string hsName = txt_HSName.Text;
            string lowerHSName = hsName.ToLower();

            if (hsName.Length < 1 || hsName.Length > 20) {
                MessageBox.Show("HOST方案名长度限制为1~20，请重新输入", "添加HOST方案");
                txt_HSName.Focus();
                txt_HSName.SelectAll();
                return;
            }
            
            foreach (var noChr in sysHSName) {
                string noChrLower = noChr.ToLower();
                if (lowerHSName.Equals(noChrLower)) {
                    StringBuilder str = new StringBuilder();
                    foreach (var shN in sysHSName) {
                        if (!shN.Equals(noChrLower)) {
                            str.Append(shN);
                            str.Append("，");
                        }
                    }
                    if (str.Length > 0) {
                        str.Remove(str.Length - 1, 1);
                        str.Insert(0, "同样不可使用的有：\n");
                    }
                    MessageBox.Show("方案名[" + hsName + "]为系统保留名称，请重新输入。" + str.ToString(), "添加HOST方案");
                    txt_HSName.Focus();
                    txt_HSName.SelectAll();
                    return;
                }
            }

            foreach (var noChr in fileNameNoChr) {
                if (hsName.Contains(noChr)) {
                    MessageBox.Show(lab_HSNoChr.Text + "，请重新输入", "添加HOST方案");
                    txt_HSName.Focus();
                    txt_HSName.SelectAll();
                    return;
                }
            }


            bool hasRe = false;//是否有重名
            switch (_addMode) {
                case EAddHSMode.Create:
                    hasRe = (_allHSList.Find(h => h.SchemeName.ToLower().Equals(lowerHSName)) != null);
                    break;
                case EAddHSMode.Rename:
                    string oldHSNameLower = _oldHS.SchemeName.ToLower();
                    if (lowerHSName.Equals(oldHSNameLower)) {
                        MessageBox.Show("HOST方案名未修改，请重新输入", "添加HOST方案");
                        txt_HSName.Focus();
                        txt_HSName.Select();
                        return;
                    }
                    hasRe = (_allHSList.Find(h => !h.SchemeName.ToLower().Equals(oldHSNameLower) && h.SchemeName.ToLower().Equals(lowerHSName)) != null);
                    break;
            }
            if (hasRe) {
                MessageBox.Show("HOST方案名[" + hsName + "]已存在，请重新输入", "添加HOST方案");
                txt_HSName.Focus();
                txt_HSName.Select();
                return;
            }

            HSName = hsName;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
    public enum EAddHSMode {
        Create,
        Rename
    }
}
