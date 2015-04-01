using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE;
using Utilitys;
using System.Windows.Forms;
using System.IO;

namespace TcAddins.PluginProc.Publish {
    /// <summary>
    /// 发布到准生产环境测试
    /// </summary>
    public class PPublishToTestProc : IPluginProc {

        private static string _configFilePath = Application.StartupPath + @"\\Publish.Cfg";

        private void MsgWarning(string msg) {
            MessageBox.Show(msg, "发布器提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        #region IPluginProc 成员

        public void Start(EnvDTE80.DTE2 application, EnvDTE.AddIn addInInst, string addinDllPath) {

            MessageBox.Show(SVNHelper.GetLogUpdateItems(@"C:\Users\Administrator\Desktop\新建文件夹\Web", 14510, 14545).Count.ToString());
            return;

            //从*.Publish.xml中,把发布的文件覆盖到WebSVN目录(需要排除设置的忽略项),并纳入SVN

            //把内定需要加入WebSV


            Project currProject = VSHelper.GetCurrProject(application);
            string sDir = Path.GetDirectoryName(application.Solution.FileName);
            //下面用户输入完发布日志后得到
            string log = string.Empty;
            //下面所有操作,随时有错误消息
            string errMsg = string.Empty;
            //下面发布完后得到
            string publishTargetPath = string.Empty;

            #region 先让源码最新
            if (SVNHelper.CheckServerHasNew(sDir)) {
                if (MessageBox.Show("本解决方案SVN在服务器上有更新，是否需要更新SVN？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes) {
                    SVNHelper.UpdateGUI(sDir, true);
                    if (SVNHelper.CheckLocalHasConflict(sDir, out errMsg)) {
                        MsgWarning("有未解决的冲突，发布被中止，请解决完这些冲突再发布！");
                        return;
                    }
                    if (errMsg.IsNoEmpty()) {
                        MsgWarning("更新源码失败:" + errMsg);
                        return;
                    }
                }
            }
            #endregion

            #region 用户输入发布日志(必填)
            MsgInput msgInp = new MsgInput();
            msgInp.Text = "请输入发布日志(必填)";
            int checkI = 0;
            do{
                //让用户输入日志,取消则退出发布
                if (msgInp.ShowDialog() != DialogResult.OK) return;
                //有输入日志则进入下一步
                if ((log = msgInp.Msg.Trim()).IsNoEmpty()) break;
                else MsgWarning("必须输入发布日志!");
            } while (++checkI < 100);
            #endregion

            #region 提交SVN代码
            errMsg = SVNHelper.CommitGUI(sDir, false, log);
            if (errMsg.IsNoEmpty()) {
                MsgWarning("提交源码失败:" + errMsg);
                return;
            }
            if (SVNHelper.CheckLocalHasUpdate(sDir, out errMsg)) {
                MsgWarning("提交未完成,还有未提交的文件!");
                return;
            }
            if (errMsg.IsNoEmpty()) {
                MsgWarning("提交源码失败:" + errMsg);
                return;
            }
            #endregion

            #region vs发布
            var vsPublish = VSHelper.VSPubilshCurrProject(application, addInInst, addinDllPath);
            if (vsPublish == null) {
                //取消发布
                return;
            }
            if (!vsPublish.Value) {
                MsgWarning("发布失败！请到输出查看原因！");
                application.ToolWindows.OutputWindow.ActivePane.Activate();
                return;
            }
            VSHelper.SaveAll(application);
            publishTargetPath = VSHelper.VSGetPublishTargetLocation(currProject);
            if (publishTargetPath.IsEmpty()) {
                MsgWarning("发布失败！无法获取到发布目标，请重新尝试。");
                return;
            }
            #endregion

            //

            //*.Publish.xml发布前后的对比

            //提取非忽略且有更新的项,到SVN目录

            //有更新的项,如果未受控,那么加入SVN.

            
        }

        public void QueryCommandStatus(EnvDTE80.DTE2 application, EnvDTE.AddIn addInInst, string addinDllPath, EnvDTE.vsCommandStatusTextWanted neededText, ref EnvDTE.vsCommandStatus statusOption, ref object commandText) {
            Project currProject = ((Project)((Array)application.ActiveSolutionProjects).GetValue(0));
            if (currProject == null || !VSHelper.IsWebAppPorject(currProject)) {
                statusOption = vsCommandStatus.vsCommandStatusInvisible;
            } else {
                statusOption = vsCommandStatus.vsCommandStatusSupported | vsCommandStatus.vsCommandStatusEnabled;
            }
        }

        #endregion
    }
}
