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
    /// 准生产环境测试完发布
    /// </summary>
    public class PPublishComProc : IPluginProc {

        private void MsgWarning(string msg) {
            MessageBox.Show(msg, "发布器提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        #region IPluginProc 成员

        public void Start(EnvDTE80.DTE2 application, EnvDTE.AddIn addInInst, string addinDllPath) {

            Project currProject = VSHelper.GetCurrProject(application);
            string sDir = Path.GetDirectoryName(application.Solution.FileName);
            //下面用户输入完发布日志后得到
            string log = string.Empty;
            //下面所有操作,随时有错误消息
            string errMsg = string.Empty;
            //下面发布完后得到
            string publishTargetPath = string.Empty;

            #region 用户输入发布日志(必填)
            MsgInput msgInp = new MsgInput();
            msgInp.Text = "请输入发布日志(必填)";
            int checkI = 0;
            do {
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
