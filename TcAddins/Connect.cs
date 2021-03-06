using System;
using Extensibility;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.CommandBars;
using System.Windows.Forms;
using System.Collections.Generic;
using Utilitys;
using System.IO;
using System.Text;
using System.Security.Permissions;
namespace TcAddins
{
	/// <summary>用于实现外接程序的对象。</summary>
	/// <seealso class='IDTExtensibility2' />
    public class Connect : IDTExtensibility2, IDTCommandTarget {

        private static string MENU_ROOT_TEXT = Config.SystemCNName + "(&C)";


        private static object[] contextGUIDS = new object[] { };
        private static List<TCommandItem> _allCmdItem = new List<TCommandItem> {
            new TCommandItem { CommandName = "ProjectPublishToTest",CommandText="发布到准生产环境测试(&P)",CommandTips="发布到准生产环境测试",IconID=127,PluginProc=new TcAddins.PluginProc.Publish.PPublishToTestProc()} ,
            new TCommandItem { CommandName = "ProjectPublishCom",CommandText="准生产环境测试完发布(&C)",CommandTips="准生产环境测试完发布",IconID=127,PluginProc=new TcAddins.PluginProc.Publish.PPublishComProc()} ,
            new TCommandItem { CommandName = "HostsManager",CommandText="Host管理器(&H)",CommandTips="Host管理器",IconID=127,PluginProc=new TcAddins.PluginProc.PHostsManagerProc() } ,
        };

        private static List<TCommandBarItem> _menuItemNames = new List<TCommandBarItem> {
            new TCommandBarItem{CommandName= "ProjectPublishToTest"},
            new TCommandBarItem{CommandName= "ProjectPublishCom"},
            new TCommandBarItem{CommandName= "HostsManager"}
        };
        private static List<TCommandBarItem> _projectItemNames = new List<TCommandBarItem> {
            new TCommandBarItem{CommandName= "ProjectPublishToTest"},
            new TCommandBarItem{CommandName= "ProjectPublishCom"},
        };


        private DTE2 _applicationObject;
        private AddIn _addInInstance;
        private string _addinDllPath = null;
        private Regedit _reg = new Regedit(Config.SystemENName);

        private EventTrigger _eventTg = null;

		/// <summary>实现外接程序对象的构造函数。请将您的初始化代码置于此方法内。</summary>
		public Connect()
		{
		}

		/// <summary>实现 IDTExtensibility2 接口的 OnConnection 方法。接收正在加载外接程序的通知。</summary>
		/// <param term='application'>宿主应用程序的根对象。</param>
		/// <param term='connectMode'>描述外接程序的加载方式。</param>
		/// <param term='addInInst'>表示此外接程序的对象。</param>
		/// <seealso class='IDTExtensibility2' />
		public void OnConnection(object application, ext_ConnectMode connectMode, object addInInst, ref Array custom)
		{
			_applicationObject = (DTE2)application;
			_addInInstance = (AddIn)addInInst;

            if (connectMode == ext_ConnectMode.ext_cm_Startup || connectMode == ext_ConnectMode.ext_cm_UISetup) {


                Uninstall();

                StringBuilder initErrMsg = new StringBuilder();

                //命令集合
                Commands2 commands = (Commands2)_applicationObject.Commands;
                //命令栏集合
                CommandBars appCmdBars = (CommandBars)_applicationObject.CommandBars;

                //查找 MenuBar 命令栏，该命令栏是容纳所有主菜单项的顶级命令栏:
                CommandBar menuCmdBar = appCmdBars["MenuBar"];
                //查找解决方案资源管理器的项目右键菜单的命令栏
                CommandBar pjCmdBar = appCmdBars["Project"];

                //将配置中的命令全部注册到VS命令集合中
                try {
                    foreach (var cmd in _allCmdItem) {
                        cmd.CurrVSCommand = commands.AddNamedCommand(_addInInstance, cmd.CommandName, cmd.CommandText, cmd.CommandTips, true, cmd.IconID
                                                , ref contextGUIDS, (int)vsCommandStatus.vsCommandStatusSupported + (int)vsCommandStatus.vsCommandStatusEnabled);
                    }
                } catch (Exception ex) {
                    initErrMsg.Append("命令集合初始化失败:" + ex.Message + "\n");
                }

                if (menuCmdBar != null) {
                    //如果希望添加多个由您的外接程序处理的命令，可以重复此 try/catch 块，
                    //  只需确保更新 QueryStatus/Exec 方法，使其包含新的命令名。
                    try {
                        
                        //创建顶级菜单
                        CommandBar menuObj = (CommandBar)_applicationObject.Commands.AddCommandBar(MENU_ROOT_TEXT, vsCommandBarType.vsCommandBarTypeMenu, menuCmdBar, 1);

                        foreach (var mItemName in _menuItemNames) {
                            var commandObj = _allCmdItem.Find(c => c.CommandName.Equals(mItemName.CommandName));
                            if (commandObj == null || commandObj.CurrVSCommand == null) throw new Exception("_menuItemNames中配置错误");
                            mItemName.CurrVSCommandBarBtn = (CommandBarButton)commandObj.CurrVSCommand.AddControl(menuObj, menuObj.Controls.Count + 1);
                        }

                    } catch (Exception ex) {
                        //如果出现此异常，原因很可能是由于具有该名称的命令
                        //  已存在。如果确实如此，则无需重新创建此命令，并且
                        //  可以放心忽略此异常。
                        initErrMsg.Append("命令菜单初始化失败:" + ex.Message + "\n");
                    }
                    
                }

                if (pjCmdBar != null) {
                    try {

                        //创建顶级菜单
                        CommandBar menuObj = (CommandBar)_applicationObject.Commands.AddCommandBar(MENU_ROOT_TEXT, vsCommandBarType.vsCommandBarTypeMenu, pjCmdBar, 1);

                        foreach (var mItemName in _projectItemNames) {
                            var commandObj = _allCmdItem.Find(c => c.CommandName.Equals(mItemName.CommandName));
                            if (commandObj == null || commandObj.CurrVSCommand == null) throw new Exception("_projectItemNames中配置错误");
                            mItemName.CurrVSCommandBarBtn = (CommandBarButton)commandObj.CurrVSCommand.AddControl(menuObj, menuObj.Controls.Count + 1);
                        }

                    } catch (Exception ex) {
                        //如果出现此异常，原因很可能是由于具有该名称的命令
                        //  已存在。如果确实如此，则无需重新创建此命令，并且
                        //  可以放心忽略此异常。
                        initErrMsg.Append("命令菜单初始化失败:" + ex.Message + "\n");
                    }
                }


                string tmpGetAddinDllPathErr = "";
                if (_addinDllPath.IsEmpty()) {
                    try {
                        _addinDllPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                        if (!Directory.Exists(_addinDllPath)) _addinDllPath = string.Empty;
                    } catch (Exception ex){
                        tmpGetAddinDllPathErr += "用Assembly.GetExecutingAssembly获取插件路径失败:" + ex.Message + "\n";
                    }
                }
                if (_addinDllPath.IsEmpty()) {
                    try {
                        _addinDllPath = Path.GetDirectoryName(_addInInstance.SatelliteDllPath);
                    } catch (Exception ex) {
                        tmpGetAddinDllPathErr += "用addInInstance.SatelliteDllPath获取插件路径失败:" + ex.Message + "\n";
                    }
                }
                if (_addinDllPath.IsEmpty()) {
                    try {
                        string path = _reg.GetRegistData("Path").GetString(string.Empty);
                        if (path.IsNoEmpty()) _addinDllPath = Path.GetDirectoryName(path);
                    } catch (Exception ex) {
                        tmpGetAddinDllPathErr += "读取注册表获取插件路径失败:" + ex.Message + "\n";
                    }
                }
                if (_addinDllPath.IsEmpty()) {
                    initErrMsg.Append(tmpGetAddinDllPathErr);
                }
                if (initErrMsg.Length > 0) {
                    MessageBox.Show("初始化失败!\n" + initErrMsg.ToString(), "系统异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


                if (_eventTg == null) {
                    _eventTg = new EventTrigger(_applicationObject, _addInInstance);
                    _eventTg.Opened += new EventTrigger.SolutionEvents_OpenedEventHandler(EventTg_Opened);
                    _eventTg.Start();
                }
            }
			
		}

        void EventTg_Opened() {
            string solutionPath = Path.GetDirectoryName(_applicationObject.Solution.FullName);
            if (SVNHelper.CheckServerHasNew(solutionPath)) {
                if (MessageBox.Show("本解决方案SVN在服务器上有更新，是否需要更新SVN？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes) {
                    SVNHelper.UpdateGUI(solutionPath, true);
                }
            }
        }

		/// <summary>实现 IDTExtensibility2 接口的 OnDisconnection 方法。接收正在卸载外接程序的通知。</summary>
		/// <param term='disconnectMode'>描述外接程序的卸载方式。</param>
		/// <param term='custom'>特定于宿主应用程序的参数数组。</param>
		/// <seealso class='IDTExtensibility2' />
        public void OnDisconnection(ext_DisconnectMode disconnectMode, ref Array custom) {
            if (disconnectMode == Extensibility.ext_DisconnectMode.ext_dm_HostShutdown
                || disconnectMode == Extensibility.ext_DisconnectMode.ext_dm_UserClosed) {
                Uninstall();
            }
		}

		/// <summary>实现 IDTExtensibility2 接口的 OnAddInsUpdate 方法。当外接程序集合已发生更改时接收通知。</summary>
		/// <param term='custom'>特定于宿主应用程序的参数数组。</param>
		/// <seealso class='IDTExtensibility2' />		
		public void OnAddInsUpdate(ref Array custom)
		{
		}

		/// <summary>实现 IDTExtensibility2 接口的 OnStartupComplete 方法。接收宿主应用程序已完成加载的通知。</summary>
		/// <param term='custom'>特定于宿主应用程序的参数数组。</param>
		/// <seealso class='IDTExtensibility2' />
		public void OnStartupComplete(ref Array custom) {
		}

		/// <summary>实现 IDTExtensibility2 接口的 OnBeginShutdown 方法。接收正在卸载宿主应用程序的通知。</summary>
		/// <param term='custom'>特定于宿主应用程序的参数数组。</param>
		/// <seealso class='IDTExtensibility2' />
		public void OnBeginShutdown(ref Array custom)
		{
            Uninstall();
		}

        #region IDTCommandTarget 成员

        public void Exec(string commandName, vsCommandExecOption executeOption, ref object variantIn, ref object variantOut, ref bool handled) {
            handled = false;
            if (executeOption == vsCommandExecOption.vsCommandExecOptionDoDefault) {
                var mItem = _allCmdItem.Find(d => commandName.Equals("TcAddins.Connect." + d.CommandName));
                if (mItem != null) {
                    mItem.PluginProc.Start(_applicationObject, _addInInstance, _addinDllPath);
                    handled = true;
                    return;
                }
            }
        }

        public void QueryStatus(string commandName, vsCommandStatusTextWanted neededText, ref vsCommandStatus statusOption, ref object commandText) {
            if (neededText == vsCommandStatusTextWanted.vsCommandStatusTextWantedNone) {
                var mItem = _allCmdItem.Find(d => commandName.Equals("TcAddins.Connect." + d.CommandName));
                if (mItem != null) {
                    mItem.PluginProc.QueryCommandStatus(_applicationObject, _addInInstance, _addinDllPath, neededText, ref statusOption, ref commandText);
                }
            }
        }

        #endregion

        private void Uninstall() {

            if (_eventTg != null) {
                _eventTg.Dispose();
            }

            //先删除所有按钮/命令栏
            CommandBars appCmdBars = (CommandBars)_applicationObject.CommandBars;
            try {
                CommandBar menuItem = (CommandBar)appCmdBars[MENU_ROOT_TEXT];
                if (menuItem != null) {
                    _applicationObject.Commands.RemoveCommandBar(menuItem);
                }
            } catch { }
            foreach (var mItemName in _menuItemNames) {
                try {
                    mItemName.CurrVSCommandBarBtn.Delete(true);
                } catch { }
            }
            foreach (var mItemName in _projectItemNames) {
                try {
                    mItemName.CurrVSCommandBarBtn.Delete(true);
                } catch { }
            }

            //删除所有命令
            foreach (var mItem in _allCmdItem) {
                try {
                    Command cmd = _applicationObject.Commands.Item("TcAddins.Connect." + mItem.CommandName, -1);
                    if (cmd != null) {
                        cmd.Delete();
                    }
                } catch { }
            }

        }
    }
}