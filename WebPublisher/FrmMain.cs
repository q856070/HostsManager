using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml.Linq;
using Utilitys;
namespace WebPublisher {
    public partial class FrmMain : Form {
        protected string _solutionName;
        protected List<string> _projectDirs;

        public FrmMain(string solutionName, List<string> projectDirs) {
            _solutionName = solutionName;
            _projectDirs = projectDirs;
            InitializeComponent();
        }
        /// <summary>
        /// 当前的配置项
        /// </summary>
        Dictionary<string, MCfgItem> PathList = new Dictionary<string, MCfgItem>();
        /// <summary>
        /// 配置路径
        /// </summary>
        protected static string xmlFile = Application.StartupPath + @"\config\WebPublisher.xml";
        protected const string defXmlStr = "<?xml version=\"1.0\" encoding=\"utf-8\"?><config></config>";

        private void MainForm_Load(object sender, EventArgs e) {
            LoadPathList();
            this.Text = _solutionName + " - " + this.Text;
        }

        /// <summary>
        /// 初始化目录列表
        /// </summary>
        private void LoadPathList() {
            try {
                XDocument xdoc = XmlHelper.GetXmlDoc(xmlFile, defXmlStr);
                var items = XmlHelper.GetElements(xdoc.Root, "item");
                var historys = XmlHelper.GetElements(XmlHelper.GetElement(xdoc.Root, "historys", true), "path");
                
                //加载所有配置
                PathList = (from item in items select item).ToDictionary(item => item.Element("sourceDir").Value, item => new MCfgItem {
                    SourceDir = XmlHelper.GetElementXCData(item, "sourceDir"),
                    IgnoreDirs = XmlHelper.GetElementXCData(item, "IgnoreDirs"),
                    LastReleaseTime = XmlHelper.GetElementXCData(item, "LastReleaseTime").GetDateTime()
                });
                this.txt_WorkspaceDir.Text = XmlHelper.GetElementXCData(xdoc.Root, "WorkspaceDir");

                this.cobPathValue.Items.Clear();

                //加载外部传入的
                foreach (var item in _projectDirs) {
                    this.cobPathValue.Items.Add(GetPath(item));
                }

                //加载历史的
                foreach (var path in historys) {
                    var p = GetPath(path.Value);
                    if (!this.cobPathValue.Items.Contains(p)) {
                        this.cobPathValue.Items.Add(p);
                    }
                }

                if (this.cobPathValue.Items.Count > 0) {
                    this.cobPathValue.SelectedIndex = 0;
                    cobPathValue_SelectedIndexChanged(this.cobPathValue, EventArgs.Empty);
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// 更新当前站点的提取配置
        /// </summary>
        private void UpdatePathList() {
            try {
                string workspaceDir = GetPath(this.txt_WorkspaceDir.Text);
                string sourceDir = GetPath(this.cobPathValue.SelectedItem.GetString(""));
                string sourceDirLower = sourceDir.ToLower();
                string ignoreDirs = this.txt_ignoreDirs.Text.Replace("\r", "");

                XDocument xdoc = XmlHelper.GetXmlDoc(xmlFile, defXmlStr);

                var items = XmlHelper.GetElements(xdoc.Root, "item");
                var cfgItem = items.FirstOrDefault(p => p.Element("sourceDir").Value.Equals(sourceDir));
                // 更新指定的XElement对象
                if (cfgItem == null) {
                    cfgItem = new XElement("item");
                    xdoc.Root.Add(cfgItem);
                }
                XmlHelper.SetElementXCData(xdoc.Root, "WorkspaceDir", workspaceDir);
                XmlHelper.SetElementXCData(cfgItem, "sourceDir", sourceDir);
                XmlHelper.SetElementXCData(cfgItem, "IgnoreDirs", ignoreDirs);

                MCfgItem dirs = new MCfgItem();
                if (PathList.TryGetValue(this.cobPathValue.SelectedItem.GetString(""), out dirs)) {
                    dirs.IgnoreDirs = ignoreDirs;
                }
                // 保存xml
                xdoc.Save(xmlFile);
            } catch { }
        }
        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="SourcePath">原始路径</param>
        /// <returns></returns>
        public static bool CreateFolder(string SourcePath) {
            try {
                Directory.CreateDirectory(SourcePath);
                return true;
            } catch {
                return false;
            }
        }
        /// <summary>
        /// 过滤前后的"\"
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string GetPath(string path) {
            path = path.Replace("/", @"\");
            if (path.EndsWith(@"\")) {
                path = path.Substring(0, path.Length - 1);
            }
            if (path.StartsWith(@"\")) {
                path = path.Substring(1);
            }
            return path;
        }
        /// <summary>
        /// 获取更新文件列表[循环遍历]
        /// </summary>
        /// <param name="SourcePath">原始路径</param>
        /// <param name="DestinPath">目地的路径</param>
        /// <returns></returns>
        public bool GetUpdFileList(string SourcePath) {

            if (!Directory.Exists(SourcePath)) {
                MessageBox.Show("不存在需提取文件的目录", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            string sourceDir = GetPath(SourcePath);
            DateTime lastTime = this.dateTimePicker1.Value;
            
            List<string> tmpDirs = new List<string>();
            List<string> ignoreDirs = new List<string>();
            string[] list = this.txt_ignoreDirs.Text.Replace("\r\n", "|").Replace("\n", "").Split('|');
            for (int i = 0; i < list.Length; i++) {
                string path = sourceDir + @"\" + GetPath(list[i]);
                ignoreDirs.Add(path.ToLower());
            }

            var paiChuDirList = new string[] { 
                sourceDir+"\\obj"
            };

            ckl_Files.Items.Clear();
            
            tmpDirs.Add(sourceDir);

            List<string> tmpDirs3 = new List<string>();
            while (tmpDirs.Count > 0) {
                List<string> tmpDirs2 = new List<string>();
                //处理上层的目录列表
                foreach (var dir in tmpDirs) {
                    var tmpFiles = Directory.GetFiles(dir, "*", SearchOption.TopDirectoryOnly).Where(FilterFile);
                    string[] tmp3Dirs = Directory.GetDirectories(dir, "*", SearchOption.TopDirectoryOnly);

                    tmpDirs2.AddRange(tmp3Dirs.Where(d =>
                        !Path.GetFileName(d).ToLower().Equals(".svn")
                        && !ignoreDirs.Contains(d.ToLower())
                        && !paiChuDirList.Contains(d)
                        ));
                    foreach (var f in tmpFiles) {
                        if (File.GetLastWriteTime(f) >= lastTime) {
                            ckl_Files.Items.Add(f, true);
                        }
                    }
                }
                tmpDirs = tmpDirs2;
            }
            UpdatePathList();

            if (ckl_Files.Items.Count <= 0) {
                MessageBox.Show("[" + lastTime.ToString("yyyy-MM-dd HH:mm:ss") + "]之后没有修改文件.", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            MessageBox.Show("文件提取完毕！", "提示信息");
            return true;
        }
        private bool FilterFile(string filePath) {
            string exName = Path.GetExtension(filePath).ToLower();
            if (exName.Equals(".pdb")) return false;
            else if (exName.Equals(".cs")) return false;
            else if (exName.Equals(".csproj")) return false;
            return true;
        }
        private void btnSelectPath_Click(object sender, EventArgs e) {
            FolderBrowserDialog fbSel = new FolderBrowserDialog();
            if (fbSel.ShowDialog() == DialogResult.OK) {
                string path = GetPath(fbSel.SelectedPath);
                int preIndex = this.cobPathValue.Items.IndexOf(path);
                if (preIndex > -1) {
                    this.cobPathValue.SelectedIndex = preIndex;
                } else {
                    this.cobPathValue.Items.Insert(0, path);
                    this.cobPathValue.SelectedIndex = 0;
                }
                cobPathValue_SelectedIndexChanged(this.cobPathValue, EventArgs.Empty);

                XDocument xdoc = XmlHelper.GetXmlDoc(xmlFile, defXmlStr);
                var historysEl = XmlHelper.GetElement(xdoc.Root, "historys", true);
                var pathEls = XmlHelper.GetElements(historysEl, "path").Where(p => GetPath(p.Value.ToLower()).Equals(path.ToLower()));
                foreach (var pE in pathEls) pE.Remove();
                historysEl.AddFirst(new XElement("path", new XCData(path)));
                xdoc.Save(xmlFile);
            }
        }

        private void cobPathValue_KeyUp(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Delete) {
                var path = this.cobPathValue.SelectedItem.GetString("");
                if (path.IsNoEmpty()) {
                    this.cobPathValue.Items.Remove(path);

                    XDocument xdoc = XmlHelper.GetXmlDoc(xmlFile, defXmlStr);
                    var historysEl = XmlHelper.GetElement(xdoc.Root, "historys", true);
                    var pathEls = XmlHelper.GetElements(historysEl, "path").Where(p => GetPath(p.Value.ToLower()).Equals(path.ToLower()));
                    foreach (var pE in pathEls) pE.Remove();
                    xdoc.Save(xmlFile);
                }
            }
        }

        private void btnDestinPath_Click(object sender, EventArgs e) {
            FolderBrowserDialog path = new FolderBrowserDialog();
            DialogResult dr = path.ShowDialog();
            if (dr == DialogResult.OK) {
                this.txt_WorkspaceDir.Text = path.SelectedPath;
            }
        }

        private void btnCopyFile_Click(object sender, EventArgs e) {
            string SourcePath = this.cobPathValue.SelectedItem.GetString("");
            string WorkspaceDir = this.txt_WorkspaceDir.Text.GetString("");
            if (SourcePath.IsEmpty()) {
                MessageBox.Show("请选择需提取文件的目录", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (WorkspaceDir.IsEmpty()) {
                MessageBox.Show("请选择工作空间的位置", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            GetUpdFileList(SourcePath);
        }

        private void cobPathValue_SelectedIndexChanged(object sender, EventArgs e) {
            ckl_Files.Items.Clear();
            MCfgItem dirs = new MCfgItem();
            if (PathList.TryGetValue(GetPath(this.cobPathValue.SelectedItem.GetString("")), out dirs)) {
                this.txt_ignoreDirs.Text = dirs.IgnoreDirs.Replace("\r", "").Replace("\n", "\r\n");
                this.dateTimePicker1.Value = dirs.LastReleaseTime.HasValue ? dirs.LastReleaseTime.Value : DateTime.Today;
            } else {
                this.txt_ignoreDirs.Text = "";
                this.dateTimePicker1.Value = DateTime.Today;
            }
        }

        private void btn_OK_Click(object sender, EventArgs e) {
            string sourceDir, workspaceDir, publishKey, nowStr, publishCurrDir, uploadDir, backDir;
            if (!CopyFiles(out sourceDir, out workspaceDir, out publishKey, out  nowStr, out  publishCurrDir, out  uploadDir, out backDir)) return;
            Utilitys.Shell.ExeShell(Application.StartupPath + "\\FTPPublish.FlashFXP.exe"
                , "-sourceDir \"" + sourceDir + " \" -publishKey \"" + publishKey + " \" -publishCurrDir \"" + publishCurrDir + " \" -uploadDir \"" + uploadDir + " \" -backDir \"" + backDir + " \"");
            Application.Exit();
        }

        private void txtTargetPath_TextChanged(object sender, EventArgs e) {
            ckl_Files.Items.Clear();
        }

        private bool CopyFiles(out string sourceDir, out  string workspaceDir, out  string publishKey, out string nowStr, out string publishCurrDir, out string uploadDir, out  string backDir) {
            sourceDir = workspaceDir = publishKey = nowStr = publishCurrDir = uploadDir = backDir = null;
            if (ckl_Files.Items.Count <= 0) {
                MessageBox.Show("没有更新的文件，请重新选择时间检查。", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            sourceDir = GetPath(this.cobPathValue.SelectedItem.GetString(""));
            workspaceDir = GetPath(this.txt_WorkspaceDir.Text.GetString(""));
            publishKey = _solutionName + "_" + Path.GetFileName(sourceDir);
            nowStr = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
            publishCurrDir = workspaceDir + @"\" + publishKey + @"\" + nowStr;
            uploadDir = publishCurrDir + @"\upload";
            backDir = publishCurrDir + @"\back";

            if (Directory.Exists(uploadDir) && Directory.GetFileSystemEntries(uploadDir, "*").Length > 0) {
                if (MessageBox.Show("本次发布文件的存放目录中已经存在文件。\r\n需要清除，是否继续？", "提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK) {
                    Directory.Delete(uploadDir, true);
                    Directory.CreateDirectory(uploadDir);
                } else {
                    MessageBox.Show("发布被中止：本次发布文件的存放目录中已经存在文件未清除。", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }

            foreach (var sourceFile in ckl_Files.CheckedItems) {
                string f = sourceFile.ToString();
                FileInfo fileInfo = new FileInfo(uploadDir + f.Substring(sourceDir.Length));
                if (!fileInfo.Directory.Exists) fileInfo.Directory.Create();
                File.Copy(f, fileInfo.FullName, true);//复制文件
            }
            return true;
        }

        private void btn_CopyFiles_Click(object sender, EventArgs e) {
            string sourceDir, workspaceDir, publishKey, nowStr, publishCurrDir, uploadDir, backDir;
            if (!CopyFiles(out sourceDir, out workspaceDir, out publishKey, out  nowStr, out  publishCurrDir, out  uploadDir, out backDir)) return;
            Utilitys.Shell.ExeShell(publishCurrDir, "");
        }
    }
}
