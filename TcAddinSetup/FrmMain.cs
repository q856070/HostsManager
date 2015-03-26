using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Utilitys;
using System.IO;
using System.Diagnostics;
using System.Collections;
using System.ServiceProcess;
using System.Xml.Linq;

namespace TcAddinSetup {
    public partial class FrmMain : Form {

        /// <summary>
        /// 安装文件列表,只许往后增加.不允许中间插入
        /// </summary>
        protected static string[] packFiles = new string[] { 
            Config.SystemENName+@".dll",
            @"Utilitys.dll",
            @"TcAddins.AddIn",
            @"plugin\FTPPublish.FlashFXP.exe",
            @"plugin\HostsManager.exe",
            @"plugin\HostsManager.exe.config",
            @"plugin\WebPublisher.exe",
            @"plugin\Utilitys.dll",
            @"TcWorkClient.exe",
            @"plugin\Interop.IWshRuntimeLibrary.dll",
        };
        private static bool mustCloseVS = true;

        private const string CONST_signKey = "asdfkjasou80w3pqwjfiasud9g7qwue9tjilsduv8qwu";

        private string _currVer = string.Empty;
        protected string CurrVer {
            get {
                if (_currVer.IsEmpty() && File.Exists(packDir + addinDllFName)) {
                    _currVer = FileVersionInfo.GetVersionInfo(packDir + addinDllFName).FileVersion;
                }
                return _currVer;
            }
        }

        protected static string packDir = Application.StartupPath + "\\pack\\";
        protected static string addinXmlTargetDir = Environment.GetEnvironmentVariable("APPDATA") + @"\Microsoft\MSEnvShared\Addins\";
        protected static string addinDllFName = packFiles[0];
        protected static string addinXmlFName = packFiles[2];
        protected static string tcWorkClientFName = packFiles[8];

        private Regedit reg = new Regedit(Config.SystemENName);
        private string preSoftPath = "";
        private string preSoftVer = "";
        private string preSoftSign = "";

        private IDictionary stateSaver = new Hashtable();  


        public FrmMain() {
            InitializeComponent();
        }

        private string GetSign(params string[] pas) {
            StringBuilder str = new StringBuilder();
            foreach (var pa in pas) {
                str.Append(pa);
            }
            str.Append(CONST_signKey);
            return str.ToString().MD5();
        }
        /// <summary>
        /// 验证之前是否有安装
        /// </summary>
        /// <param name="path"></param>
        /// <param name="ver"></param>
        /// <param name="sign"></param>
        /// <returns></returns>
        private bool ChkRegData(string path, string ver, string sign) {
            if (path.IsNoEmpty()
                && ver.IsNoEmpty()
                && sign.IsNoEmpty()
                && GetSign(path, ver).Equals(sign)) {
                preSoftPath = path;
                preSoftVer = ver;
                preSoftSign = sign;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 版本1是否比2版本新,新返回1,旧返回-1,相等返回0
        /// </summary>
        /// <param name="ver1"></param>
        /// <param name="ver2"></param>
        /// <returns></returns>
        private int CVerNew(string ver1, string ver2) {
            string[] vs1 = ver1.Split('.');
            string[] vs2 = ver2.Split('.');
            for (int i = 0; i < vs1.Length; i++) {
                if (i >= vs2.Length) {
                    //前面一样,2没1长
                    return 1;
                } else {
                    int vp1 = vs1[i].GetInt(0);
                    int vp2 = vs2[i].GetInt(0);
                    if (vp1 > vp2) {
                        return 1;
                    } else if (vp2 > vp1) {
                        return -1;
                    }
                }
            }
            return 0;
        }

        private void FrmMain_Load(object sender, EventArgs e) {

            if (!ChkFileComp()) {
                MessageBox.Show("安装文件缺失！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            this.Text += " - " + CurrVer;

            var path = reg.GetRegistData("Path").GetString(string.Empty);
            var ver = reg.GetRegistData("Ver").GetString(string.Empty);
            var sign = reg.GetRegistData("Sign").GetString(string.Empty);
            if (ChkRegData(path, ver, sign)) {
                int cp = CVerNew(preSoftVer, CurrVer);
                txt_SoftPath.Text = preSoftPath;
                if (cp == 1) {
                    if (MessageBox.Show("已安装了较新的版本，是否用当前较旧的版本覆盖？", "已存在版本提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.No) {
                        this.Close();
                    }
                } else if (cp == 0) {
                    if (MessageBox.Show("已安装了相同的版本，是否修复？", "已存在版本提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.No) {
                        this.Close();
                    }
                } else if (cp == -1) {
                    txt_SoftPath.ReadOnly = true;
                    btn_SelectSoftPath.Enabled = false;
                    btn_GO.Text = "升级";
                }
            }
        }

        private void btn_SelectSoftPath_Click(object sender, EventArgs e) {
            FolderBrowserDialog f = new FolderBrowserDialog();
            if (f.ShowDialog() == DialogResult.OK && f.SelectedPath.IsNoEmpty() && Directory.Exists(f.SelectedPath)) {
                txt_SoftPath.Text = f.SelectedPath;
            }
        }

        private void btn_GO_Click(object sender, EventArgs e) {
            try {
                string softPath = GetPath(txt_SoftPath.Text);


                //安装需要先关闭VS
                if (mustCloseVS) {
                    while (HasVSExe()) {
                        if (MessageBox.Show("需要先关闭VS，关闭后点击确定。", "安装", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.Cancel) {
                            return;
                        }
                    }
                }

                ////原先是将客户端做成服务,发现无法调用外部进程
                //using (ServiceInstaller serviceInst = new ServiceInstaller(CONST_TcClientServiceName, softPath + CONST_TcClientFileName)) {
                //    if (serviceInst.Exist) {
                //        if (serviceInst.Running) {
                //            serviceInst.Stop();
                //        }
                //        serviceInst.UnInstallService();
                //    }
                //}
                
                //如果当前客户端运行着,需要关闭
                Process[] runningExes = Process.GetProcessesByName("TcWorkClient");
                if (runningExes != null) {
                    foreach (var exe in runningExes) {
                        try {
                            exe.Kill();
                            exe.WaitForExit();
                            exe.Close();
                        } catch { }
                    }
                }

                //如果当前客户端运行着,需要关闭
                runningExes = Process.GetProcessesByName("HostsManager");
                if (runningExes != null) {
                    foreach (var exe in runningExes) {
                        try {
                            exe.Kill();
                            exe.WaitForExit();
                            exe.Close();
                        } catch { }
                    }
                }

                //将包里的文件拷贝到输出目录
                if (!Directory.Exists(softPath)) Directory.CreateDirectory(softPath);
                var allFiles = Directory.GetFiles(packDir, "*", SearchOption.AllDirectories);
                foreach (var file in allFiles) {
                    string targetFilePath = softPath + file.Substring(packDir.Length);
                    string dir = Path.GetDirectoryName(targetFilePath);
                    if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                    File.Copy(file, targetFilePath, true);
                }

                //之后将插件配置文件拷贝到对应目录,并且根据安装路径做出修改
                var sourceAddinFilePath = softPath + addinXmlFName;
                var targetAddinFilePath = addinXmlTargetDir + addinXmlFName;
                if (!Directory.Exists(addinXmlTargetDir)) Directory.CreateDirectory(addinXmlTargetDir);
                string fileStr = File.ReadAllText(sourceAddinFilePath, Encoding.Unicode);
                fileStr = fileStr.Replace("{SystemPath}", softPath);
                fileStr = fileStr.Replace("{SystemCNName}", Config.SystemCNName);
                fileStr = fileStr.Replace("{SystemENName}", Config.SystemENName);
                fileStr = fileStr.Replace("{CompanyCNName}", Config.CompanyCNName);

                File.WriteAllText(targetAddinFilePath, fileStr, Encoding.Unicode);

                //if (!CfgFull(softPath)) {
                //    MessageBox.Show("安装被中断：必须配置各项目的SVN目录。", "安装结果", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                //    return;
                //}

                Shell.ExeShell(softPath + tcWorkClientFName);

                reg.WTRegedit("Path", softPath);
                reg.WTRegedit("Ver", CurrVer);
                reg.WTRegedit("Sign", GetSign(softPath, CurrVer));

                MessageBox.Show("安装成功", "安装结果", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                this.Close();

            } catch (Exception ex) {
                MessageBox.Show("安装遇到错误：" + ex.Message, "安装结果", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool CfgFull(string softPath) {
            string procOutResult = "";
            using (Process process = new Process()) {
                process.StartInfo.FileName = softPath + "ProjectsSvnCfg.exe";
                process.StartInfo.Verb = "open";
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false; 

                process.Start();
                process.WaitForExit();

                procOutResult = process.StandardOutput.ReadToEnd();
            }
            if (procOutResult == "Finished") return true;

            return false;
        }

        /// <summary>
        /// 检测安装文件的完整性
        /// </summary>
        /// <returns></returns>
        private bool ChkFileComp() {
            foreach (var file in packFiles) {
                if (!File.Exists(packDir + file)) {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 结尾补"\"
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string GetPath(string path) {
            if (path == null) return "";
            if (!path.EndsWith(@"\")) return path + "\\";
            return path;
        }
        private bool HasVSExe() {
            System.Diagnostics.Process[] processList = System.Diagnostics.Process.GetProcesses();
            foreach (System.Diagnostics.Process process in processList) {
                if (process.ProcessName == "devenv") {
                    return true;
                }
            }
            return false;
        }
    }
}
