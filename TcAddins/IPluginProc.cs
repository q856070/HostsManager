using System;
using System.Collections.Generic;
using System.Text;
using EnvDTE80;
using EnvDTE;

namespace TcAddins {
    public interface IPluginProc {
        /// <summary>
        /// 启动插件
        /// </summary>
        /// <param name="application"></param>
        /// <param name="addInInst"></param>
        /// <returns></returns>
        void Start(DTE2 application, AddIn addInInst, string addinDllPath);

        /// <summary>
        /// 给VS查询当前命令应该的状态(对statusOption,commandText参数进行赋值)
        /// </summary>
        /// <param name="application"></param>
        /// <param name="addInInst"></param>
        /// <param name="addinDllPath"></param>
        /// <returns></returns>
        void QueryCommandStatus(DTE2 application, AddIn addInInst, string addinDllPath, vsCommandStatusTextWanted neededText, ref vsCommandStatus statusOption, ref object commandText);
    }
}
