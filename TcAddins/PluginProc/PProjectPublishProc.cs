using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using EnvDTE;
using System.IO;
using System.Xml.Linq;
using System.Xml;
using System.Linq;
using Utilitys;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace TcAddins.PluginProc {
    public class PProjectPublishProc : IPluginProc {

        private const string path = @"\plugin\WebPublisher.exe";  

        private int ProcProject(Project project, ref StringBuilder projectListStr) {
            int count = 0;
            count += AddProjectDir(project, project.FullName, ref projectListStr) ? 1 : 0;
            if (project.ProjectItems != null) {
                foreach (ProjectItem proItem in project.ProjectItems) {
                    if (proItem.SubProject != null)
                        count += ProcProject(proItem.SubProject, ref projectListStr);
                }
            }
            return count;
        }
        private bool AddProjectDir(Project project, string FullName, ref StringBuilder projectListStr) {
            if (IsWebAppPorject(project)) {
                string publishDir = Path.GetDirectoryName(FullName);
                string userFile = FullName + ".user";
                if (File.Exists(userFile)) {
                    try {
                        XDocument doc = XDocument.Load(userFile);
                        var ptNode = doc.Root.Elements().FirstOrDefault(e => e.Name.LocalName.Equals("ProjectExtensions"))
                            .Elements().FirstOrDefault(e => e.Name.LocalName.Equals("VisualStudio"))
                            .Elements().FirstOrDefault(e => e.Name.LocalName.Equals("FlavorProperties"))
                            .Elements().FirstOrDefault(e => e.Name.LocalName.Equals("WebProjectProperties"))
                            .Elements().FirstOrDefault(e => e.Name.LocalName.Equals("PublishTargetLocation"));
                        if (ptNode != null) {
                            string tmpFilePath = ptNode.Value;
                            if (Directory.Exists(tmpFilePath)) {
                                projectListStr.AppendFormat(" \"{0} \"", tmpFilePath);
                            }
                        }

                    } catch { }
                }
                projectListStr.AppendFormat(" \"{0} \"", publishDir);
                return true;
            }
            return false;
        }
        private bool IsWebAppPorject(Project project) {
            return ((string[])project.ExtenderNames).Contains("WebApplication");
        }

        /// <summary>
        /// vs发布当前项目(仅支持中文版本)
        /// </summary>
        /// <returns>null:用户未发布,或者取消发布,false:发布失败,true:发布成功</returns>
        public bool? VSPubilshCurrProject(EnvDTE80.DTE2 application, EnvDTE.AddIn addInInst, string addinDllPath) {
            try {
                OutputWindowPane buildPane = application.ToolWindows.OutputWindow.OutputWindowPanes.Item("{1BD8A850-02D1-11D1-BEE7-00A0C913D1F8}");
                var tDoc = buildPane.TextDocument;
                application.ExecuteCommand("生成.清理选定内容", string.Empty);
                Application.DoEvents();
                buildPane.Clear();
                application.ExecuteCommand("生成.发布选定内容", string.Empty);
                Application.DoEvents();
                var text = tDoc.StartPoint.CreateEditPoint().GetText(tDoc.EndPoint);

                if (text == "")
                    return null;

                Regex buildErrRe = new Regex(@"\n编译完成 \-\- (\d)+ 个错误，(\d)+ 个警告", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                var buildMatchs = buildErrRe.Matches(text);
                if (buildMatchs.Count <= 0) {
                    //一个项目的编译都没有,说明没确定发布
                    return null;
                }
                Regex buildCancelRe = new Regex(@"\n错误 CS1600: 编译被用户取消", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                if (buildCancelRe.IsMatch(text)) {
                    return null;
                }
                for (var i = 0; i < buildMatchs.Count; i++) {
                    var errCount = buildMatchs[i].Groups[1].Value.GetInt(0, false);
                    //任意一个项目编译出错.就不通过
                    if (errCount > 0)
                        return false;
                }

                //等待一段时间,让发布完成操作,定时去看是否有结果
                var allWaitMs = 30000;
                var oneSleepMs = 10;
                var forCount = allWaitMs / oneSleepMs;
                Regex publishRe = new Regex(@"\n========== 发布: 成功 (\d)+ 个，失败 (\d)+ 个，跳过 (\d)+ 个 ==========", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                Regex publishCancelRe = new Regex(@"\n发布被用户取消", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                for (var i = 0; i < forCount; i++) {
                    text = tDoc.StartPoint.CreateEditPoint().GetText(tDoc.EndPoint);
                    var publishMatch = publishRe.Match(text);
                    if (publishMatch.Success) {
                        var succeed = publishMatch.Groups[1].Value.GetInt(0, false);
                        var err = publishMatch.Groups[2].Value.GetInt(0, false);
                        if (succeed > 0 && err <= 0) return true;
                        else return false;
                    }
                    if (publishCancelRe.IsMatch(text)) {
                        return null;
                    }
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(oneSleepMs);
                }
                //发布超时
                return false;
            } catch {
                return false;
            }
        }

        #region IPluginProc 成员

        public void Start(EnvDTE80.DTE2 application, EnvDTE.AddIn addInInst, string addinDllPath) {


            //File.WriteAllText(proFileName, File.ReadAllText(proFileName));

            return;


            string addinPath = addinDllPath;

            string solutionName = Path.GetFileNameWithoutExtension(application.Solution.FullName);
            if (solutionName.IsEmpty()) {
                MessageBox.Show("请先打开解决方案！", Utilitys.Config.SystemCNName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string solutionPath = Path.GetDirectoryName(application.Solution.FullName);

            StringBuilder projectListStr = new StringBuilder();
            int webCount = 0;
            foreach (Project project in application.Solution.Projects) {
                webCount += ProcProject(project, ref projectListStr);
            }
            if (webCount <= 0) {
                MessageBox.Show("当前解决方案中不存在Web应用程序！", Utilitys.Config.SystemCNName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            VSPubilshCurrProject(application, addInInst, addinDllPath);
            return;

            FrmProPublishSVNCheck f = new FrmProPublishSVNCheck(addinPath, solutionPath);
            PublishHelper.ShowModelForm(f);
            if (f.Result != DialogResult.OK) {
                return;
            }

            Utilitys.Shell.ExeShell(addinPath + path, "-solution \"" + solutionName + " \" -projectlist" + projectListStr.ToString() + " -currProject");
        }


        #endregion

        #region IPluginProc 成员


        public void QueryCommandStatus(EnvDTE80.DTE2 application, AddIn addInInst, string addinDllPath, vsCommandStatusTextWanted neededText, ref vsCommandStatus statusOption, ref object commandText) {
            Project currProject = ((Project)((Array)application.ActiveSolutionProjects).GetValue(0));
            if (currProject == null || !IsWebAppPorject(currProject)) {
                statusOption = vsCommandStatus.vsCommandStatusInvisible;
            } else {
                statusOption = vsCommandStatus.vsCommandStatusSupported | vsCommandStatus.vsCommandStatusEnabled;
            }
        }

        #endregion
    }
}
