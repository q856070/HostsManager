using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;
using Utilitys;

namespace FTPPublish.FlashFXP {
    public partial class FrmMain : Form {

        /// <summary>
        /// 提取的文件源目录
        /// </summary>
        protected string _sourceDir;

        /// <summary>
        /// 发布key[项目名称_发布目录]
        /// </summary>
        protected string _publishKey;
        protected string _publishCurrDir;
        protected string _uploadDir;
        protected string _backDir;

        /// <summary>
        /// 配置路径
        /// </summary>
        protected static string xmlFile = Application.StartupPath + @"\config\FTPPublish.FlashFXP.xml";
        protected static string webPublisherXmlFile = Application.StartupPath + @"\config\WebPublisher.xml";
        protected static string tmpFqfDir = Application.StartupPath + @"\tmpFqf";
        protected const string defXmlStr = "<?xml version=\"1.0\" encoding=\"utf-8\"?><config></config>";
        protected static Encoding FqfFileEncoding = Encoding.GetEncoding("gb2312");
        private const string flashFXPPwdCfgKey = "sdfaKLJ8ASDI3FJNkl4jlidjsa6";

        public FrmMain(string sourceDir, string publishKey, string publishCurrDir, string uploadDir, string backDir) {

            _sourceDir = sourceDir;

            _publishKey = publishKey;
            _publishCurrDir = publishCurrDir;
            _uploadDir = uploadDir;
            _backDir = backDir;
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e) {
            txt_FlashFXPPwd.GotFocus += new EventHandler(txt_FlashFXPPwd_GotFocus);
            txt_FlashFXPPwd.LostFocus += new EventHandler(txt_FlashFXPPwd_LostFocus);

            this.Text = _publishKey + " - " + this.Text;
            if (!string.IsNullOrEmpty(_uploadDir)) {
                txt_localPath.Text = _uploadDir;
                txt_localPath.ReadOnly = true;
                txt_BackPath.Text = _backDir;
                txt_BackPath.ReadOnly = true;

                var allFiles = Directory.GetFiles(_uploadDir, "*", SearchOption.AllDirectories);
                ckList_AllFiles.Items.Clear();
                foreach (var filePath in allFiles) {
                    ckList_AllFiles.Items.Add(filePath, true);
                }
                ReadCfg();
            }
            try { if (Directory.Exists(tmpFqfDir)) Directory.Delete(tmpFqfDir); } catch { }
        }
        #region 配置相关
        private void WritePwdCfg(TextBox txtFlashFXPPwd) {
            var FlashFXPPwd = txtFlashFXPPwd.Text;
            if (FlashFXPPwd != "(自动加密)") {
                try {
                    XDocument xdoc = XmlHelper.GetXmlDoc(xmlFile, defXmlStr);
                    XmlHelper.SetElementXCData(xdoc.Root, "flashFXPPwd", FlashFXPPwd.DESEncryptWithCBCZeros(flashFXPPwdCfgKey));
                    // 保存xml
                    xdoc.Save(xmlFile);
                } catch { }
            }
            if (FlashFXPPwd != "") {
                txtFlashFXPPwd.Text = "(自动加密)";
            }
        }
        private string ReadPwdCfg(TextBox txtFlashFXPPwd) {
            var FlashFXPPwd = "";
            txtFlashFXPPwd.Text = "";
            try {
                XDocument xdoc = XmlHelper.GetXmlDoc(xmlFile, defXmlStr);
                var pwd = XmlHelper.GetElementXCData(xdoc.Root, "flashFXPPwd");
                if (!string.IsNullOrEmpty(pwd)) {
                    FlashFXPPwd = pwd.DESDecryptWithCBCZeros(flashFXPPwdCfgKey);
                    txtFlashFXPPwd.Text =  "(自动加密)";
                }
            } catch { }
            return FlashFXPPwd;
        }
        private void txt_FlashFXPPwd_GotFocus(object sender, EventArgs e) {
            TextBox txt_Pwd = (TextBox)sender;
            txt_Pwd.SelectAll();
        }
        private void txt_FlashFXPPwd_LostFocus(object sender, EventArgs e) {
            TextBox txt_Pwd = (TextBox)sender;
            WritePwdCfg(txt_Pwd);
        }
        private void ReadCfg() {
            string backDir, ftpDir, flashFXPSiteName, flashFXPPath;
            backDir = ftpDir = flashFXPSiteName = flashFXPPath = "";
            if (File.Exists(xmlFile)) {
                try {
                    XDocument xdoc = XmlHelper.GetXmlDoc(xmlFile, defXmlStr);
                    flashFXPPath = XmlHelper.GetElementXCData(xdoc.Root, "flashFXPPath");
                    flashFXPSiteName = XmlHelper.GetElementXCData(xdoc.Root, "flashFXPSiteName");
                    var items = xdoc.Root.Elements("item");
                    if (items != null) {
                        var currCfgEl = items.FirstOrDefault(
                            delegate(XElement el) {
                                var pEl = el.Element("publishKey");
                                if (pEl != null && pEl.Value.Equals(_publishKey)) return true;
                                return false;
                            }
                        );
                        if (currCfgEl != null) {
                            ftpDir = XmlHelper.GetElementXCData(currCfgEl, "ftpDir");
                        }
                    }
                } catch { }
            }
            txt_FTPPath.Text = ftpDir;
            txt_FTPSiteName.Text = flashFXPSiteName;
            txt_FlashFXPPath.Text = flashFXPPath;

            ReadPwdCfg(txt_FlashFXPPwd);
        }
        private void WriteCfg(string FlashFXPExePath, string flashFXPSiteName, string publishKey, string ftpDir) {
            try {
                XDocument xdoc = XmlHelper.GetXmlDoc(xmlFile, defXmlStr);
                XmlHelper.SetElementXCData(xdoc.Root, "flashFXPPath", FlashFXPExePath);
                XmlHelper.SetElementXCData(xdoc.Root, "flashFXPSiteName", flashFXPSiteName);

                var items = xdoc.Root.Elements("item");
                XElement cfgItem = null;
                if (items == null
                    || 
                    (cfgItem = 
                        items.FirstOrDefault(
                            delegate(XElement el){
                                var pEl = el.Element("publishKey");
                                if (pEl != null && pEl.Value.Equals(publishKey)) return true;
                                return false;
                            }
                        )
                    ) == null) {
                    cfgItem = new XElement("item");
                    xdoc.Root.Add(cfgItem);
                }
                XmlHelper.SetElementXCData(cfgItem, "publishKey", publishKey);
                XmlHelper.SetElementXCData(cfgItem, "ftpDir", ftpDir);
                //保存xml
                xdoc.Save(xmlFile);
            } catch { }
        }
        #endregion

        private void btn_Go_Click(object sender, EventArgs e) {

            if (!ChkFlashFXPPath(txt_FlashFXPPath.Text)) {
                MessageBox.Show("FlashFXP软件路径选择错误。", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txt_FlashFXPPath.Focus();
                return;
            }

            string flashFXPPath = txt_FlashFXPPath.Text;
            string setSiteName = txt_FTPSiteName.Text;

            string ftpPath = txt_FTPPath.Text;
            //处理ftp路径,方便操作
            if (ftpPath.EndsWith(@"/")) ftpPath = ftpPath.Substring(0, ftpPath.Length - 1);//FTP路径末尾去掉"/"
            if (!ftpPath.StartsWith(@"/")) ftpPath = "/" + ftpPath;//FTP路径开头加上"/"

            string localPath = txt_localPath.Text;
            if (localPath.EndsWith(@"\")) localPath = localPath.Substring(0, localPath.Length - 1);//本地路径末尾去掉"\"

            //保存配置,这里用填写的ftp路径(不用处理后的)
            WriteCfg(flashFXPPath, setSiteName, _publishKey, txt_FTPPath.Text);

            string tmpBackFqfPath = Path.GetTempFileName();
            string tmpMKDFqfPath = Path.GetTempFileName();
            string tmpUpFqfPath = Path.GetTempFileName();

            List<MPublishItem> publishList = new List<MPublishItem>();

            FileStream backFqfFS = new FileStream(tmpBackFqfPath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
            FileStream mkdFqfFS = new FileStream(tmpMKDFqfPath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
            FileStream upFqfFS = new FileStream(tmpUpFqfPath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
            try {
                List<string> dirList = new List<string>();
                int localPathLen = localPath.Length;
                foreach (var filePathChk in ckList_AllFiles.CheckedItems) {
                    string filePath = filePathChk.ToString();
                    //本地路径
                    string fileLocalPath = filePath.Replace("\r", "");
                    //以"\"开头的相对路径
                    string fileRePath = fileLocalPath.Substring(localPathLen);
                    //在ftp的路径
                    string fileFtpPath = ftpPath + fileRePath.Replace(@"\", "/");
                    //备份到本地的路径
                    string fileBackPath = _backDir + fileRePath;

                    //备份队列
                    WriteDownFqf(backFqfFS, setSiteName, fileFtpPath, fileBackPath);
                    //上传队列
                    if (fileRePath.ToLower().Equals(@"\web.config")) {
                        //上传的文件中过滤Web.Config文件,这个文件需要手动去修改
                    } else {
                        //上传队列
                        WriteUpFqf(upFqfFS, setSiteName, fileLocalPath, fileFtpPath);

                        publishList.Add(new MPublishItem {
                            SourceFilePath = fileLocalPath,
                            BackFilePath = fileBackPath,
                            FtpFilePath = fileFtpPath,
                            FileRePath = fileRePath
                        });
                    }
                    string[] ftpDirPaths = GetFtpDirPathByFtpFilePath(fileFtpPath);
                    if (ftpDirPaths!=null) {
                        foreach (var ftpDirPath in ftpDirPaths) {
                            if (!dirList.Contains(ftpDirPath)) dirList.Add(ftpDirPath);
                        }
                    }
                }
                var allFtpDirs = dirList.OrderBy(s => s);
                foreach (var ftpDir in allFtpDirs) {
                    WriteCreateFtpDirFqf(mkdFqfFS, setSiteName, ftpDir);
                }
            } catch {
            } finally {
                backFqfFS.Close();
                backFqfFS.Dispose();
                mkdFqfFS.Close();
                mkdFqfFS.Dispose();
                upFqfFS.Close();
                upFqfFS.Dispose();
            }
            string backFqfPath = Application.StartupPath + @"\tmpFqf\back_" + Guid.NewGuid() + ".fqf";
            string mkdFqfPath = Application.StartupPath + @"\tmpFqf\mkd_" + Guid.NewGuid() + ".fqf";
            string upFqfPath = Application.StartupPath + @"\tmpFqf\up_" + Guid.NewGuid() + ".fqf";

            string backFqfDir = Path.GetDirectoryName(backFqfPath);
            if (!Directory.Exists(backFqfDir)) Directory.CreateDirectory(backFqfDir);

            string flashFXPPwd = ReadPwdCfg(txt_FlashFXPPwd);
            string ffAr = string.IsNullOrEmpty(flashFXPPwd) ? "" : "-pass=\"" + flashFXPPwd + "\" ";

            using (Process process = new Process()) {
                process.StartInfo.FileName = flashFXPPath;
                process.StartInfo.Verb = "open";

                //备份
                File.Move(tmpBackFqfPath, backFqfPath);
                process.StartInfo.Arguments = ffAr + @"-c4 """ + backFqfPath + @"""";
                process.Start();
                process.WaitForExit();
                File.Delete(backFqfPath);

                FrmBackCmp frmBackCmp = new FrmBackCmp(flashFXPPath, flashFXPPwd, setSiteName, _publishCurrDir, localPath, _backDir, ftpPath, publishList);
                if (frmBackCmp.ShowDialog() == DialogResult.Cancel) {
                    MessageBox.Show("完成备份，但未上传。", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (publishList.Count <= 0) {
                    FinishPross(flashFXPPath, setSiteName, _backDir, ftpPath);
                    return;
                }

                //上传前的创建目录
                File.Move(tmpMKDFqfPath, mkdFqfPath);
                process.StartInfo.Arguments = ffAr + @"-c4 """ + mkdFqfPath + @"""";
                process.Start();
                process.WaitForExit();
                File.Delete(mkdFqfPath);
                //上传
                File.Move(tmpUpFqfPath, upFqfPath);
                process.StartInfo.Arguments = ffAr + @"-c4 """ + upFqfPath + @"""";
                process.Start();
                process.WaitForExit();
                File.Delete(upFqfPath);

            }

            FinishPross(flashFXPPath, setSiteName, _backDir, ftpPath);
        }
        private void FinishPross(string flashFXPPath, string ftpSiteName, string backCurrPath, string ftpPath) {

            //发布成功了,将当前时间更新到文件提取的配置中.
            //下次发布,时间就默认为当前时间之后修改的文件了.
            UpdateLastReleaseTime();

            if (MessageBox.Show("备份/发布已完成。是否在FTP中打开本次备份目录？(方便还原操作)", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes) {
                string flashFXPPwd = ReadPwdCfg(txt_FlashFXPPwd);
                string ffAr = ftpSiteName+" ";
                ffAr += string.IsNullOrEmpty(flashFXPPwd) ? "" : "-pass=\"" + flashFXPPwd + "\" ";
                Utilitys.Shell.ExeShell(flashFXPPath, ffAr + "-local=\"" + backCurrPath + "\" -remotepath=\"" + ftpPath + "\"");
            }
        }
        /// <summary>
        /// 发布成功后,将本站点的提取时间配置为当前时间(WebPublisher的配置)
        /// </summary>
        private void UpdateLastReleaseTime() {
            try {
                XDocument xdoc = XmlHelper.GetXmlDoc(webPublisherXmlFile, defXmlStr);
                var items = XmlHelper.GetElements(xdoc.Root, "item");

                var cfgItem = items.FirstOrDefault(p => p.Element("sourceDir").Value.Equals(_sourceDir));

                // 更新指定的XElement对象
                if (cfgItem != null) {
                    XmlHelper.SetElementXCData(cfgItem, "LastReleaseTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                // 保存xml
                xdoc.Save(webPublisherXmlFile);
            } catch { }
        }


        private void btn_SelFlashFXP_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "FlashFXP.exe|FlashFXP.exe";
            string tmpPath = "";
            do {
                if (openFileDialog.ShowDialog() != DialogResult.OK || openFileDialog.FileName.IsEmpty()) {
                    return;
                }
                tmpPath = openFileDialog.FileName;
            } while (!ChkFlashFXPPath(tmpPath));
            this.txt_FlashFXPPath.Text = tmpPath;
        }

        private void btn_SelBackupDirectory_Click(object sender, EventArgs e) {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            DialogResult dr = folderBrowserDialog.ShowDialog();
            if (dr == DialogResult.OK) {
                this.txt_BackPath.Text = folderBrowserDialog.SelectedPath;
            }
        }

        #region FlashFXP队列文件格式处理
        private void WriteDownFqf(Stream fqfFileStream, string setSiteName, string fileFtpPath, string fileBackPath) {
            byte[] tmpData;
            tmpData = FqfFileEncoding.GetBytes("0"); fqfFileStream.Write(tmpData, 0, (int)tmpData.Length);
            fqfFileStream.WriteByte(1);//SOH
            tmpData = FqfFileEncoding.GetBytes("5"); fqfFileStream.Write(tmpData, 0, (int)tmpData.Length);
            fqfFileStream.WriteByte(1);//SOH
            fqfFileStream.WriteByte(3);//ETX
            tmpData = FqfFileEncoding.GetBytes("00000000"); fqfFileStream.Write(tmpData, 0, (int)tmpData.Length);
            tmpData = FqfFileEncoding.GetBytes(setSiteName); fqfFileStream.Write(tmpData, 0, (int)tmpData.Length);
            fqfFileStream.WriteByte(1);//SOH
            fqfFileStream.WriteByte(2);//STX
            tmpData = FqfFileEncoding.GetBytes("-"); fqfFileStream.Write(tmpData, 0, (int)tmpData.Length);
            fqfFileStream.WriteByte(1);//SOH
            tmpData = FqfFileEncoding.GetBytes(fileFtpPath); fqfFileStream.Write(tmpData, 0, (int)tmpData.Length);
            fqfFileStream.WriteByte(1);//SOH
            tmpData = FqfFileEncoding.GetBytes(fileBackPath); fqfFileStream.Write(tmpData, 0, (int)tmpData.Length);
            fqfFileStream.WriteByte(1);//SOH
            tmpData = FqfFileEncoding.GetBytes("0"); fqfFileStream.Write(tmpData, 0, (int)tmpData.Length);
            tmpData = FqfFileEncoding.GetBytes("\r\n"); fqfFileStream.Write(tmpData, 0, (int)tmpData.Length);
        }
        private void WriteCreateFtpDirFqf(Stream fqfFileStream, string setSiteName, string ftpDirPath) {
            byte[] tmpData;
            tmpData = FqfFileEncoding.GetBytes("0"); fqfFileStream.Write(tmpData, 0, (int)tmpData.Length);
            fqfFileStream.WriteByte(1);//SOH
            tmpData = FqfFileEncoding.GetBytes("17"); fqfFileStream.Write(tmpData, 0, (int)tmpData.Length);
            //tmpData = FqfFileEncoding.GetBytes("16"); fqfFileStream.Write(tmpData, 0, (int)tmpData.Length);
            fqfFileStream.WriteByte(1);//SOH
            fqfFileStream.WriteByte(2);//STX
            tmpData = FqfFileEncoding.GetBytes(setSiteName); fqfFileStream.Write(tmpData, 0, (int)tmpData.Length);
            fqfFileStream.WriteByte(1);//SOH
            fqfFileStream.WriteByte(2);//STX
            tmpData = FqfFileEncoding.GetBytes("-"); fqfFileStream.Write(tmpData, 0, (int)tmpData.Length);
            fqfFileStream.WriteByte(1);//SOH
            tmpData = FqfFileEncoding.GetBytes("MKD " + ftpDirPath); fqfFileStream.Write(tmpData, 0, (int)tmpData.Length);
            fqfFileStream.WriteByte(1);//SOH
            fqfFileStream.WriteByte(1);//SOH
            tmpData = FqfFileEncoding.GetBytes("0"); fqfFileStream.Write(tmpData, 0, (int)tmpData.Length);
            tmpData = FqfFileEncoding.GetBytes("\r\n"); fqfFileStream.Write(tmpData, 0, (int)tmpData.Length);
        }
        private void WriteUpFqf(Stream fqfFileStream, string setSiteName, string fileLocalPath, string fileFtpPath) {
            byte[] tmpData;
            tmpData = FqfFileEncoding.GetBytes("0"); fqfFileStream.Write(tmpData, 0, (int)tmpData.Length);
            fqfFileStream.WriteByte(1);//SOH
            tmpData = FqfFileEncoding.GetBytes("4"); fqfFileStream.Write(tmpData, 0, (int)tmpData.Length);
            fqfFileStream.WriteByte(1);//SOH
            fqfFileStream.WriteByte(2);//STX
            tmpData = FqfFileEncoding.GetBytes("-"); fqfFileStream.Write(tmpData, 0, (int)tmpData.Length);
            fqfFileStream.WriteByte(1);//SOH
            fqfFileStream.WriteByte(3);//ETX
            tmpData = FqfFileEncoding.GetBytes("00000000"); fqfFileStream.Write(tmpData, 0, (int)tmpData.Length);
            tmpData = FqfFileEncoding.GetBytes(setSiteName); fqfFileStream.Write(tmpData, 0, (int)tmpData.Length);
            fqfFileStream.WriteByte(1);//SOH
            tmpData = FqfFileEncoding.GetBytes(fileLocalPath); fqfFileStream.Write(tmpData, 0, (int)tmpData.Length);
            fqfFileStream.WriteByte(1);//SOH
            tmpData = FqfFileEncoding.GetBytes(fileFtpPath); fqfFileStream.Write(tmpData, 0, (int)tmpData.Length);
            fqfFileStream.WriteByte(1);//SOH
            tmpData = FqfFileEncoding.GetBytes(new FileInfo(fileLocalPath).Length.ToString()); fqfFileStream.Write(tmpData, 0, (int)tmpData.Length);
            tmpData = FqfFileEncoding.GetBytes("\r\n"); fqfFileStream.Write(tmpData, 0, (int)tmpData.Length);
        }
        #endregion

        /// <summary>
        /// 根据ftp文件路径获取ftp的目录路径,末尾无"/"
        /// </summary>
        /// <param name="ftpPath">必须起始有带"/"</param>
        /// <returns></returns>
        protected string[] GetFtpDirPathByFtpFilePath(string ftpPath) {
            int i = ftpPath.LastIndexOf("/");
            if (i < 0) return null;
            else if (i == 0) return null;
            string path = ftpPath.Substring(0, i);
            string[] frDirs = path.Split('/');
            if (frDirs.Length > 1) {
                string[] allDirs = new string[frDirs.Length - 1];
                string lastDir = "";
                for (i = 1; i < frDirs.Length; i++) {
                    lastDir = lastDir + "/" + frDirs[i];
                    allDirs[i - 1] = lastDir;
                }
                return allDirs;
            }
            return null;
        }

        protected bool ChkFlashFXPPath(string flashFXPPath) {
            if (File.Exists(flashFXPPath) && Path.GetFileName(flashFXPPath).ToLower().Equals("flashfxp.exe")) {
                return true;
            }
            return false;
        }

        private void btn_OpenFtp_Click(object sender, EventArgs e) {
            if (!ChkFlashFXPPath(txt_FlashFXPPath.Text)) {
                MessageBox.Show("FlashFXP软件路径选择错误。", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txt_FlashFXPPath.Focus();
                return;
            }
            string flashFXPPwd = ReadPwdCfg(txt_FlashFXPPwd);
            string flashFXPPath = txt_FlashFXPPath.Text.GetString("");
            string ftpSiteName = txt_FTPSiteName.Text.GetString("");


            if (string.IsNullOrEmpty(ftpSiteName)) {
                MessageBox.Show("请输入ftp站点", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txt_FTPSiteName.Focus();
                return;
            }
            string ffAr = ftpSiteName + " ";
            ffAr += string.IsNullOrEmpty(flashFXPPwd) ? "" : "-pass=\"" + flashFXPPwd + "\" ";
            Utilitys.Shell.ExeShell(flashFXPPath, ffAr);
        }

        private void button1_Click(object sender, EventArgs e) {
            SVNHelper.UpdateGUI(@"C:\Users\Administrator\Desktop\新建文件夹\test", false);
        }

    }
}
