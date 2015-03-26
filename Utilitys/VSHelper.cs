using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml.Linq;

namespace Utilitys {
    public static class VSHelper {

        /// <summary>
        /// 获取当前项目
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        public static Project GetCurrProject(EnvDTE80.DTE2 application) {
            return ((Project)((Array)application.ActiveSolutionProjects).GetValue(0));
        }

        /// <summary>
        /// 项目是否Web应用程序类型
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public static bool IsWebAppPorject(Project project) {
            return ((string[])project.ExtenderNames).Contains("WebApplication");
        }


        /// <summary>
        /// vs发布当前项目(仅支持中文版本),调用位置不能锁住VS窗口的进程.
        /// </summary>
        /// <returns>null:用户未发布,或者取消发布,false:发布失败,true:发布成功</returns>
        public static bool? VSPubilshCurrProject(EnvDTE80.DTE2 application, EnvDTE.AddIn addInInst, string addinDllPath) {
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
                var oneWaitMs = 10;
                var forCount = allWaitMs / oneWaitMs;
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
                    System.Threading.Thread.Sleep(oneWaitMs);
                }
                //发布超时
                return false;
            } catch {
                return false;
            }
        }

        /// <summary>
        /// 全部保存(和VS工具栏上的"全部保存"按钮一样的效果)
        /// </summary>
        /// <param name="application"></param>
        public static void SaveAll(EnvDTE80.DTE2 application) {
            application.ExecuteCommand("文件.全部保存", string.Empty);
            System.Threading.Thread.Sleep(100);
            Application.DoEvents();
            System.Threading.Thread.Sleep(100);
            Application.DoEvents();
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public static string VSGetPublishTargetLocation(Project project) {
            string projectName = Path.GetFileNameWithoutExtension(project.FullName);
            string publishDir = Path.GetDirectoryName(project.FileName);
            string userFile = publishDir + "\\" + projectName + ".csproj.user";
            if (File.Exists(userFile)) {
                try {
                    XDocument doc = XDocument.Load(userFile);
                    var ptNode = doc.Root.Element("ProjectExtensions").Element("VisualStudio").Element("FlavorProperties").Element("WebProjectProperties").Element("PublishTargetLocation");
                    if (ptNode != null) {
                        string tmpFilePath = ptNode.Value;
                        if (Directory.Exists(tmpFilePath)) {
                            return tmpFilePath;
                        }
                    }
                } catch { }
            }
            return null;
        }
    }
}
