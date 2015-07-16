using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;//互动服务 

namespace HostsManager {
    public partial class FrmMain : Form {

        private const string hostFilePath = @"C:\Windows\System32\drivers\etc\hosts";
        private const string sysHostCfgStr = "#请不要改动或删除本行-----------当前HOST方案:";

        private List<MHostScheme> hsList = null;
        private TabPage currUsingHSTabP = null;

        private TabPage lastCMenuTabP = null;

        private string[] _commands = null;

        /// <summary>
        /// 选中文本
        /// </summary>
        /// <param name="start"></param>
        /// <param name="length"></param>
        public delegate void dgSelect(int start, int length);
        /// <summary>
        /// 替换文本
        /// </summary>
        /// <param name="start"></param>
        /// <param name="source">源文本</param>
        /// <param name="target">目标文本</param>
        public delegate void dgReplace(int start, string source, string target);
        /// <summary>
        /// 查找字符串是否存在
        /// </summary>
        /// <param name="value">查找的文本</param>
        /// <param name="startIndex">开始位置</param>
        /// <param name="matchTheCase">是否匹配大小写</param>
        public delegate int dgTextIndexOf(string value, int startIndex, bool matchTheCase);

        public FrmMain(string[] commands) {
            _commands = commands;
            InitializeComponent();
            hsList = HostSchemeMgr.GetAllHostScheme(false);
        }

        TextBox GetCurrentTextBox() {
            TextBox txt = null;
            TabPage page = this.tab_AllHost.SelectedTab;
            if (page != null) {
                if (page.HasChildren) {
                    foreach (Control childControl in page.Controls) {
                        if (childControl is TextBox) {
                            txt = childControl as TextBox;
                            break;
                        }
                    }
                }
            }
            return txt;
        }

        //void FrmMain_KeyDown(object sender, KeyEventArgs e) {
        //    if ((e.KeyCode == Keys.F) && e.Control) {
        //        MessageBox.Show("1");
        //        //FrmFind win = new FrmFind();
        //        //win.ShowDialog(this);
        //    }
        //}
        private void FrmMain_Load(object sender, EventArgs e) {
            try {
                this.WindowState = (FormWindowState)HostsManager.Properties.Settings.Default.LastWindowState_FrmMain;
            } catch { }

            MHostScheme currUsingHS = null;
            string currHSName;
            string currSysHostStr = GetSysHostStr(out currHSName);
            if (string.IsNullOrEmpty(currHSName)) {
                //host文件中没有配置信息,则加一个默认的方案
                currUsingHS = SaveAndGetDefHS(currSysHostStr);
            } else {
                currUsingHS = hsList.Find(h => h.SchemeName.Equals(currHSName));
                if (currUsingHS == null) {
                    //有配置信息,但是该方案找不到,一样加一个默认的方案
                    currUsingHS = SaveAndGetDefHS(currSysHostStr);
                }
            }
            if (currUsingHS == null) {
                //找不到当前方案,连默认的都创建不成功
                splitC_Main.Enabled = false;
                MessageBox.Show("初始化失败，软件将关闭。可能问题有：\n1.HOSTS文件或方案(.hs)文件被独占，无法打开\n2.权限不足\n3.磁盘空间已满", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            } else {
                foreach (var hsItem in hsList) {
                    TabPage tp = AddHSToTabP(hsItem);
                    if (hsItem == currUsingHS) {
                        tab_AllHost.SelectedTab = tp;
                        currUsingHSTabP = tp;
                    }
                    SetTabPageStyle(tp);
                }
                tab_AllHost_SelectedProc(currUsingHSTabP);
            }

            if (_commands != null) {
                foreach (var cmd in _commands) {
                    ProcStartArg(cmd);
                }
            }
        }
        private TabPage AddHSToTabP(MHostScheme hsItem) {
            TabPage tp = new TabPage(hsItem.SchemeName);
            hsItem.ShowTabPage = tp;
            tp.Tag = hsItem;
            TextBox txt = new TextBox();
            txt.HideSelection = false;
            txt.Multiline = true;
            txt.Dock = DockStyle.Fill;
            txt.ScrollBars = ScrollBars.Both;
            txt.Name = "txt_HostStr";
            txt.AcceptsReturn = true;
            txt.KeyDown += new KeyEventHandler(txt_HostStr_KeyDown);
            tp.Controls.Add(txt);
            tab_AllHost.TabPages.Add(tp);

            //add by qq
            txt.MouseWheel += txt_MouseWheel;
            txt.Font = this.TextFont;

            return tp;

        }

        void txt_MouseWheel(object sender, MouseEventArgs e) {
            //滚轮时间，更改字体大小
            if (Control.ModifierKeys == Keys.Control) {
                var txt = sender as TextBox;
                if (txt != null) {
                    //滚轮每次滑动一格为120
                    var fontSize = txt.Font.Size + (e.Delta / 120);
                    if (fontSize < 5) {
                        fontSize = 5;
                    }
                    txt.Font = new Font(txt.Font.Name, fontSize);
                }
            }
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e) {

            //有方案未保存需要提示
            foreach (TabPage tabPage in tab_AllHost.TabPages) {
                MHostScheme hs = (MHostScheme)tabPage.Tag;
                if (hs.Changed) {
                    DialogResult dr = MessageBox.Show("HOST方案[" + hs.SchemeName + "]未保存，是否保存", "系统提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Asterisk);
                    if (dr == DialogResult.Yes) {
                        SaveTabPageHS(tabPage);
                    } else if (dr == DialogResult.Cancel) {
                        e.Cancel = true;
                        return;
                    }
                }
            }

            try {
                HostsManager.Properties.Settings.Default.LastWindowState_FrmMain = (int)this.WindowState;
                HostsManager.Properties.Settings.Default.Save();
            } catch { }
        }

        /// <summary>
        /// 自动出现未保存的标识
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_HostStr_TextChanged(object sender, EventArgs e) {
            TextBox txt = (TextBox)sender;
            TabPage tp = (TabPage)txt.Parent;
            MHostScheme hs = (MHostScheme)tp.Tag;
            hs.Changed = true;
            SetTabPageStyle(tp);
        }
        /// <summary>
        /// 选中加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tab_AllHost_Selected(object sender, TabControlEventArgs e) {
            tab_AllHost_SelectedProc(e.TabPage);
        }
        private void tab_AllHost_SelectedProc(TabPage selTabPage) {

            //切换后才进行加载
            MHostScheme hs = (MHostScheme)selTabPage.Tag;
            if (!hs.HostLoaded) {
                TextBox txt = null;
                try {
                    Control[] searchCtl = selTabPage.Controls.Find("txt_HostStr", false);
                    if (searchCtl != null && searchCtl.Length > 0) {
                        txt = (TextBox)searchCtl[0];
                    }
                } catch (Exception ex) {
                    MessageBox.Show("加载Host方案[" + hs.SchemeName + "]失败:" + ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (txt != null) {
                    if (HostSchemeMgr.LoadHostScheme(hs)) {
                        txt.Text = hs.HostStr.Replace("\n", "\r\n");
                        txt.Enabled = true;
                        txt.SelectionStart = 0;
                        txt.SelectionLength = 0;
                        //同时开始监视改变
                        txt.TextChanged += new EventHandler(txt_HostStr_TextChanged);
                        txt.Focus();
                    } else {
                        MessageBox.Show("加载Host方案[" + hs.SchemeName + "]失败:方案文件打开失败:" + hs.HSFilePath, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txt.Enabled = false;
                    }
                } else {
                    MessageBox.Show("加载Host方案[" + hs.SchemeName + "]失败:未知异常", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            //启用或禁用"启用当前方案"
            if (selTabPage == currUsingHSTabP) {
                tsbtn_UseCurrHS.Enabled = false;
            } else {
                if (hs.HostLoaded) {
                    //只有加载成功才能启用
                    tsbtn_UseCurrHS.Enabled = true;
                }
            }
        }

        /// <summary>
        /// 注释或取消注释
        /// </summary>
        /// <param name="txt">当前文本框</param>
        void NotesOpearation(TextBox txt) {
            string noteFlag = "# ";
            //选中行的集合
            List<int> selectedRows = new List<int>();
            int startRow = 0;
            int endRow = 0;
            int startIndex = txt.SelectionStart;
            int endIndex = startIndex;

            //1. 获取选中的行或者当前焦点所在的行
            int charCount = 0;//字符总数,用于计算开始字符位置和结束字符位置
            for (int i = 0; i < txt.Lines.Length; i++) {
                charCount += txt.Lines[i].Length;
                if (charCount >= txt.SelectionStart) {
                    //开始和结束行,默认为同一行
                    startRow = endRow = i;
                    //选中字符开始索引
                    startIndex = charCount - txt.Lines[i].Length;
                    selectedRows.Add(startRow);

                    //有选中文本               
                    if (txt.SelectedText.Length > 0) {
                        //选中行总数,使用换行符区分共选中几行
                        endRow = startRow + txt.SelectedText.Split(new string[] { "\r\n" }, StringSplitOptions.None).Length - 1;
                        i++;
                        while (i <= endRow) {
                            selectedRows.Add(i);
                            charCount += txt.Lines[i].Length + 2;
                            i++;
                        }
                    }

                    endIndex = charCount;
                    break;
                }
                //每行结尾需要添加换行符,所以需要加2
                if (i < txt.Lines.Length - 1) {
                    charCount += 2;
                }
            }

            //2. 判断注释还是取消注释
            bool allNote = true;
            foreach (var index in selectedRows) {
                var str = txt.Lines[index].Trim();
                if (str.Length >= 2) {
                    if (!str.StartsWith(noteFlag.Trim())) {
                        allNote = false;
                        break;
                    }
                }
            }

            //3. txt.Lines为只读数组,所有只能替换注释或取消注释后的字符数组
            var lines = txt.Lines.Clone() as string[];
            int operationLinesCount = 0;
            if (allNote) {
                //移除注释
                foreach (var index in selectedRows) {
                    if (txt.Lines[index].Trim().Length - 2 > 0) {
                        lines[index] = txt.Lines[index].Remove(0, noteFlag.Length);
                        operationLinesCount++;
                    }
                }
            } else {
                //添加注释
                foreach (var index in selectedRows) {
                    if (txt.Lines[index].Trim().Length - 2 > 0) {
                        lines[index] = noteFlag + txt.Lines[index];
                        operationLinesCount++;
                    }
                }
            }

            //加上或移除注释符长度
            endIndex += operationLinesCount * noteFlag.Length * (allNote ? -1 : 1);
            txt.Lines = lines;

            //4. 选中行
            txt.Select(startIndex, endIndex - startIndex);
        }

        /// <summary>
        /// 获取或设置文本框字体
        /// </summary>
        public Font TextFont {
            get {
                Font font = this.Font.Clone() as Font;
                if (HostsManager.Properties.Settings.Default.Font.Length > 0) {
                    try {
                        var arr = HostsManager.Properties.Settings.Default.Font.Split(',');
                        var name = arr[0];
                        var size = float.Parse(arr[1]);
                        FontStyle fontStyle = (FontStyle)Enum.Parse(typeof(FontStyle), arr[2]);
                        font = new System.Drawing.Font(name, size, fontStyle);
                    } catch (Exception) {
                        font = this.Font;
                    }
                }
                return font;
            }
            set {
                var font = value;
                foreach (TabPage page in tab_AllHost.TabPages) {
                    foreach (Control item in page.Controls) {
                        item.Font = font;
                    }
                }
                HostsManager.Properties.Settings.Default.Font = string.Format("{0},{1},{2}", font.Name, font.Size, font.Style);
                HostsManager.Properties.Settings.Default.Save();
            }
        }

        /// <summary>
        /// 快捷键保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_HostStr_KeyDown(object sender, KeyEventArgs e) {
            //MessageBox.Show(string.Format("{0},{1},{2}", e.KeyCode, e.KeyData, e.KeyValue));
            //return;
            var txt = (TextBox)sender;
            if (e.KeyCode == Keys.F1) {
                string caption = "帮助提示";
                string text = @"Ctrl + S        : 保存当前文本" + Environment.NewLine +
                              @"Ctrl + A        : 全选当前文本框" + Environment.NewLine +
                              @"Ctrl + /        : 注释或取消注释选中行" + Environment.NewLine +
                              @"Ctrl + F        : 搜索文本" + Environment.NewLine +
                              @"Ctrl + Alt + F: 更换为文本框字体" + Environment.NewLine;
                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                e.Handled = true;
                return;
            }
            if (e.Control) {

                if (e.KeyCode == Keys.S) {
                    (txt.Parent as TabPage).Select();
                    tsbtn_UseCurrHS_Click(this.tsbtn_UseCurrHS, null);
                    //SaveTabPageHS((TabPage)txt.Parent);
                    txt.Focus();

                    e.Handled = true;
                    if (e.Alt) {
                        this.Close();
                    }
                } else if (e.KeyCode == Keys.A) {
                    //全选
                    txt.SelectAll();
                    e.Handled = true;
                } else if (e.KeyCode == Keys.OemQuestion || e.KeyCode == Keys.Divide) {
                    NotesOpearation(sender as TextBox);
                    e.Handled = true;
                } else if (e.Alt && e.KeyCode == Keys.F) {
                    FontDialog fontDialog = new FontDialog();
                    fontDialog.Font = (sender as Control).Font;

                    if (fontDialog.ShowDialog() == DialogResult.OK) {
                        var font = fontDialog.Font;
                        this.TextFont = font;
                    }
                    e.Handled = true;
                } else if (e.KeyCode == Keys.G) {
                    if (txt.Text.Length > 0 && txt.Lines.Length > 0) {
                        FrmGoLine win = new FrmGoLine(txt);
                        win.Owner = this;

                        win.ShowDialog();
                    }
                } else if (e.KeyCode == Keys.F) {
                    e.Handled = true;
                    //ctrl+F响应搜索
                    //已存在则不弹出
                    if (this.OwnedForms.Length > 0) {
                        foreach (var item in this.OwnedForms) {
                            if (item is FrmFind) {
                                (item as FrmFind).Focus();
                                return;
                            }
                        }
                    }
                    FrmFind win = new FrmFind();
                    win.Owner = this;
                    win.SelectText = delegate(int start, int length) {
                        //选中文本
                        txt.Select(start, length);
                        //滚动条到焦点
                        txt.ScrollToCaret();
                    };
                    win.TextIndexOf = delegate(string value, int startIndex, bool matchTheCase) {
                        string content = txt.Text;
                        if (!matchTheCase) {
                            value = value.ToUpper();
                            content = content.ToUpper();
                        }
                        return content.IndexOf(value, startIndex);
                    };
                    win.Show(this);
                } else if (e.KeyCode == Keys.H) {
                    e.Handled = true;
                    //ctrl+F响应搜索
                    //已存在则不弹出
                    if (this.OwnedForms.Length > 0) {
                        foreach (var item in this.OwnedForms) {
                            if (item is FrmReplace) {
                                (item as FrmReplace).Focus();
                                return;
                            }
                        }
                    }
                    FrmReplace win = new FrmReplace();
                    win.Owner = this;
                    win.ReplaceText = delegate(int start, string source, string target) {
                        //选中文本
                        txt.Select(start, source.Length);
                        //txt.Text.Replace()
                        //滚动条到焦点
                        txt.ScrollToCaret();
                    };
                    win.TextIndexOf = delegate(string value, int startIndex, bool matchTheCase) {
                        string content = txt.Text;
                        if (!matchTheCase) {
                            value = value.ToUpper();
                            content = content.ToUpper();
                        }
                        return content.IndexOf(value, startIndex);
                    };
                    win.Show(this);
                }
            }
        }


        private void SetTabPageStyle(TabPage tp) {
            MHostScheme hs = (MHostScheme)tp.Tag;
            if (tp == currUsingHSTabP) {
                tp.Text = "√" + hs.SchemeName + (hs.Changed ? " *" : "");
            } else {
                tp.Text = hs.SchemeName + (hs.Changed ? " *" : "");
            }
        }
        private void SaveTabPageHS(TabPage tp) {
            MHostScheme hs = (MHostScheme)tp.Tag;
            if (hs.HostLoaded) {
                //只有已加载的才能/需要保存
                string hostStr = string.Empty;
                Control[] searchCtl = tp.Controls.Find("txt_HostStr", false);
                if (searchCtl != null && searchCtl.Length > 0) {
                    hs.HostStr = ProcHostTxt(searchCtl[0].Text);
                }
                bool saveS = false;
                if (tp == currUsingHSTabP) {
                    saveS = UseHSToSysHost(hs);
                } else {
                    saveS = HostSchemeMgr.SaveHostScheme(hs);
                }
                if (saveS) {
                    hs.Changed = false;
                    SetTabPageStyle(tp);
                } else {
                    MessageBox.Show("保存Host方案[" + hs.SchemeName + "]失败:方案文件打开失败:" + hs.HSFilePath, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void tsbtn_UseCurrHS_Click(object sender, EventArgs e) {
            TabPage tp = tab_AllHost.SelectedTab;
            if (tp != null) {
                tsbtn_UseCurrHS.Enabled = false;

                TabPage preCurrTP = currUsingHSTabP;
                currUsingHSTabP = tp;
                SaveTabPageHS(preCurrTP);
                SaveTabPageHS(currUsingHSTabP);
            }
        }

        private void tsbtn_SaveAllHS_Click(object sender, EventArgs e) {
            foreach (TabPage tabPage in tab_AllHost.TabPages) {
                SaveTabPageHS(tabPage);
            }
        }

        private void TSMenuItem_Del_Click(object sender, EventArgs e) {
            if (lastCMenuTabP != null) {
                MHostScheme hs = (MHostScheme)lastCMenuTabP.Tag;

                tab_AllHost.TabPages.Remove(lastCMenuTabP);
                hsList.Remove(hs);
                HostSchemeMgr.DeleteHostScheme(hs);
            }
        }

        private void TSMenuItem_Rename_Click(object sender, EventArgs e) {
            if (lastCMenuTabP != null) {
                MHostScheme hs = (MHostScheme)lastCMenuTabP.Tag;
                if (hs.Changed) {
                    if (MessageBox.Show("方案[" + hs.SchemeName + "]需要保存后才能重命名，是否保存？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK) {
                        SaveTabPageHS(lastCMenuTabP);
                    } else {
                        return;
                    }
                }
                FrmCreateHS fc = new FrmCreateHS(hsList, EAddHSMode.Rename, hs);
                if (fc.ShowDialog() == DialogResult.OK) {
                    if (HostSchemeMgr.RenameHostScheme(hs, fc.HSName)) {
                        SetTabPageStyle(lastCMenuTabP);
                        if (lastCMenuTabP == currUsingHSTabP) {
                            SetHSToSysHost(hs);
                        }
                    } else {
                        MessageBox.Show("编辑Host方案[" + hs.SchemeName + "]=>[" + fc.HSName + "]失败！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void tsbtn_AddHS_Click(object sender, EventArgs e) {
            FrmCreateHS fc = new FrmCreateHS(hsList, EAddHSMode.Create, null);
            if (fc.ShowDialog() == DialogResult.OK) {
                var hs = HostSchemeMgr.AddHostScheme(fc.HSName);
                if (hs != null) {
                    TabPage tp = AddHSToTabP(hs);
                    hsList.Add(hs);
                } else {
                    MessageBox.Show("添加Host方案[" + fc.HSName + "]失败！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void cmenu_Tab_Opening(object sender, CancelEventArgs e) {
            ContextMenuStrip ct = (ContextMenuStrip)sender;
            lastCMenuTabP = GetTabPByScreenPoint(new Point(ct.Left, ct.Top));
            if (lastCMenuTabP != null) {
                //启用中的方案不让删除
                if (lastCMenuTabP == currUsingHSTabP) {
                    TSMenuItem_Del.Enabled = false;
                } else {
                    TSMenuItem_Del.Enabled = true;
                }

            } else {
                e.Cancel = true;
            }
        }

        private MHostScheme SaveAndGetDefHS(string hostStr) {
            string defHSName = "(默认)";
            MHostScheme currHS = hsList.Find(h => h.SchemeName.Equals(defHSName));
            if (currHS == null) {
                currHS = HostSchemeMgr.AddHostScheme(defHSName);
                if (currHS != null) {
                    hsList.Add(currHS);
                }
            }
            if (currHS != null) {
                currHS.HostStr = hostStr;
                if (!UseHSToSysHost(currHS)) {
                    return null;
                }
            }
            return currHS;
        }
        private string GetSysHostStr(out string currHSName) {
            string sysHostStr = ProcHostTxt(File.ReadAllText(hostFilePath));
            string[] hostRows = sysHostStr.Split('\n');
            string firstRow = hostRows[0];
            if (firstRow.StartsWith(sysHostCfgStr)) {
                currHSName = firstRow.Substring(sysHostCfgStr.Length);
                StringBuilder str = new StringBuilder();
                for (var i = 1; i < hostRows.Length; i++) {
                    str.Append(hostRows[i]);
                    str.Append("\n");
                }
                return str.ToString();
            } else {
                currHSName = string.Empty;
                return sysHostStr;
            }
        }
        private bool UseHSToSysHost(MHostScheme hs) {
            if (!HostSchemeMgr.SaveHostScheme(hs)) return false;
            SetHSToSysHost(hs);
            return true;
        }
        private void SetHSToSysHost(MHostScheme hs) {
            File.WriteAllText(hostFilePath, sysHostCfgStr + hs.SchemeName + "\n" + ProcHostTxt(hs.HostStr));
        }
        private string ProcHostTxt(string txt) {
            return txt.Replace("\r\n", "\n").Replace("\r", "\n");
        }
        private TabPage GetTabPByScreenPoint(Point screenPoint) {
            TabPage currTP = null;
            Point p0 = tab_AllHost.PointToScreen(Point.Empty);
            Point p = new Point(screenPoint.X - p0.X, screenPoint.Y - p0.Y);
            for (int i = 0; i < tab_AllHost.TabPages.Count; i++) {
                TabPage tp = tab_AllHost.TabPages[i];
                if (tab_AllHost.GetTabRect(i).Contains(p)) {
                    currTP = tp;
                    break;
                }
            }
            return currTP;
        }

        private void TSMenuItem_CreateDesktop_Click(object sender, EventArgs e) {
            if (lastCMenuTabP != null) {
                MHostScheme hs = (MHostScheme)lastCMenuTabP.Tag;
                Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

                //实例化WshShell对象 
                IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();

                //通过该对象的 CreateShortcut 方法来创建 IWshShortcut 接口的实例对象 
                IWshRuntimeLibrary.IWshShortcut shortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "//" + hs.SchemeName + "(快速切换HOST).lnk");

                //设置快捷方式的目标所在的位置(源程序完整路径) 
                shortcut.TargetPath = System.Reflection.Assembly.GetExecutingAssembly().Location;

                //应用程序的工作目录 
                //当用户没有指定一个具体的目录时，快捷方式的目标应用程序将使用该属性所指定的目录来装载或保存文件。 
                shortcut.WorkingDirectory = System.Environment.CurrentDirectory;

                //目标应用程序窗口类型(1.Normal window普通窗口,3.Maximized最大化窗口,7.Minimized最小化) 
                shortcut.WindowStyle = 1;

                //快捷方式的描述 
                shortcut.Description = "快速切换HOST";

                //可以自定义快捷方式图标.(如果不设置,则将默认源文件图标.) 
                //shortcut.IconLocation = System.Environment.SystemDirectory + "\\" + "shell32.dll, 165"; 

                //设置应用程序的启动参数(如果应用程序支持的话) 
                shortcut.Arguments = "-UseScheme:" + hs.SchemeName;

                //设置快捷键(如果有必要的话.) 
                //shortcut.Hotkey = "CTRL+ALT+D"; 

                //保存快捷方式 
                shortcut.Save();

            }
        }

        protected override void WndProc(ref Message m) {
            switch (m.Msg) {
                case WinMessageUtil.WM_COPYDATA:
                    ProcStartArg(WinMessageUtil.ReceiveMessage(ref m));
                    break;
                default:
                    break;
            }
            base.WndProc(ref m);
        }

        protected void ProcStartArg(string cmd) {

            var useSchemeCMD = "-UseScheme:";

            var cmdVal = "";

            if (cmd.StartsWith(useSchemeCMD)) {
                cmdVal = cmd.Substring(useSchemeCMD.Length);
                for (var i = 0; i < 100; i++) {
                    if (hsList == null) {
                        Thread.Sleep(100);
                        continue;
                    }
                    var hs = hsList.Find(f => f.SchemeName == cmdVal);
                    if (hs != null && hs.ShowTabPage != null) {
                        tab_AllHost.SelectedTab = hs.ShowTabPage;
                        tsbtn_UseCurrHS_Click(tab_AllHost, EventArgs.Empty);
                        this.Show();
                    }
                    break;
                }
            }
        }
    }
}
