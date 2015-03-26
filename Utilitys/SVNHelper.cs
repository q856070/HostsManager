using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;

namespace Utilitys {
    public static class SVNHelper {

        public enum EConflictAccept {
            /// <summary>
            /// 忽略
            /// </summary>
            [EnumAttr(Desc = "postpone")]
            Postpone,
            /// <summary>
            /// 使用"他们的"解决冲突
            /// </summary>
            [EnumAttr(Desc = "theirs-conflict")]
            TheirsConflict,
        }
        public enum ESVNItemState {
            /// <summary>
            /// 新增
            /// </summary>
            A = 1,
            /// <summary>
            /// 锁破坏
            /// </summary>
            B,
            /// <summary>
            /// 删除
            /// </summary>
            D,
            /// <summary>
            /// 修改
            /// </summary>
            M,
            /// <summary>
            /// 替代
            /// </summary>
            R,
            /// <summary>
            /// 忽略
            /// </summary>
            I,
            /// <summary>
            /// 更新
            /// </summary>
            U,
            /// <summary>
            /// 冲突
            /// </summary>
            C,
            /// <summary>
            /// 合并
            /// </summary>
            G,
            /// <summary>
            /// 存在的
            /// </summary>
            E,
            /// <summary>
            /// ? 未受控
            /// </summary>
            Un,
            /// <summary>
            /// ! 丢失，一般是将受控文件直接删除导致
            /// </summary>
            Lose,
        }
        public class SVNItem {
            public string Path { get; set; }
            public ESVNItemState State { get; set; }
        }

        private static string ExecExe(string exePath, bool wndHide, string paramsStr) {
            StringBuilder outStr = new StringBuilder();
            using (Process process = new Process()) {
                try {
                    process.StartInfo.FileName = exePath;
                    process.StartInfo.Arguments = paramsStr;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.UseShellExecute = false;
                    if (wndHide) {
                        process.StartInfo.Verb = "hide";
                        process.StartInfo.CreateNoWindow = true;
                        process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    }
                    process.Start();
                    do {
                        outStr.Append(process.StandardOutput.ReadToEnd());
                    } while (!process.HasExited);
                } catch (Exception ex) {
                    outStr.Append("\nErr:" + ex.Message);
                }
            }
            return outStr.ToString();
        }
        private static string ExecSvn(string paramsStr) {
            return ExecExe("svn", true, paramsStr);
        }

        /// <summary>
        /// 提交的原始实现
        /// </summary>
        /// <param name="path"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public static string CommitPrimeval(string path, string log) {
            return ExecSvn("commit -m \"" + log + "\" \"" + path + "\"");
        }
        /// <summary>
        /// 更新的原始实现
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string UpdatePrimeval(string path, string param) {
            return ExecSvn("update " + param + " \"" + path + "\"");
        }
        /// <summary>
        /// 状态的原始实现
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string StatuePrimeval(string path, string param) {
            return ExecSvn("status " + param + " \"" + path + "\"");
        }

        /// <summary>
        /// 判断服务器是否有更新的
        /// </summary>
        /// <param name="path">目录</param>
        /// <returns></returns>
        public static bool CheckServerHasNew(string path) {
            string result = StatuePrimeval(path, "-u");
            string[] resultRows = result.Replace("\r", "").Split('\n');
            foreach (var r in resultRows) {
                if (r.Length > 8 && r[8] == '*') return true;
            }
            return false;
        }
        /// <summary>
        /// 判断本地是否有更新的/未提交的/冲突的/未处理的
        /// </summary>
        /// <param name="path">目录</param>
        /// <param name="errMsg">输出检查状态时的错误异常</param>
        /// <returns></returns>
        public static bool CheckLocalHasUpdate(string path, out string errMsg) {
            errMsg = string.Empty;
            string result = StatuePrimeval(path, "");
            string[] resultRows = result.Replace("\r", "").Split('\n');
            foreach (var r in resultRows) {
                if (!r.Substring(1).StartsWith("       ")) {
                    errMsg = result;
                    return false;
                }
                if (r[0] != '?') return true;
            }
            return false;
        }
        /// <summary>
        /// 判断本地是否有冲突的
        /// </summary>
        /// <param name="path">目录</param>
        /// <param name="errMsg">输出检查状态时的错误异常</param>
        /// <returns></returns>
        public static bool CheckLocalHasConflict(string path, out string errMsg) {
            errMsg = string.Empty;
            string result = StatuePrimeval(path, "");
            string[] resultRows = result.Replace("\r", "").Split('\n');
            foreach (var r in resultRows) {
                if (!r.Substring(1).StartsWith("       ")) {
                    errMsg = result;
                    return false;
                }
                if (r[0] == 'C') return true;
            }
            return false;
        }

        /// <summary>
        /// 获取一个版本范围的日志更新项列表
        /// </summary>
        /// <param name="path"></param>
        /// <param name="startVer"></param>
        /// <param name="endVer"></param>
        /// <returns></returns>
        public static List<SVNItem> GetLogUpdateItems(string localPath, int startVer, int endVer) {
            var resultStr = ExecSvn("log -v -q -r" + startVer + ":" + endVer + " \"" + localPath + "\"");
            var resultRows = resultStr.Replace("\r", "").Split('\n');

            List<SVNItem> list = new List<SVNItem>();
            Dictionary<string, bool> existPath = new Dictionary<string, bool>();
            bool inPath = false;
            for (int i = 0; i < resultRows.Length; i++) {
                if (resultRows[i].Equals("Changed paths:")) {
                    inPath = true;
                    continue;
                } else if (!resultRows[i].StartsWith("   ") || resultRows[i].Length < 6) {
                    inPath = false;
                    continue;
                }

                if (inPath) {
                    var path = resultRows[i].Substring(5);
                    var state = ParseItemState(resultRows[i][3]);
                    if (!state.HasValue) continue;
                    if (existPath.ContainsKey(path)) continue;
                    existPath.Add(path, true);
                    list.Add(new SVNItem {
                        Path = path,
                        State = state.Value,
                    });
                }
            }
            return list;
        }

        /// <summary>
        /// 获取本地目录对应SVN中的最新版本号
        /// </summary>
        /// <param name="path"></param>
        /// <param name="startVer"></param>
        /// <param name="endVer"></param>
        /// <returns></returns>
        public static int GetHeadVersion(string localPath) {
            var resultStr = ExecSvn("status -u --depth empty \"" + localPath + "\"");
            //Status against revision:  1
            var resultRows = resultStr.Replace("\r", "").Split('\n');
            if (resultRows.Length < 1) return 0;
            if (resultRows[0].StartsWith("Status against revision:  ")) {
                return resultRows[0].Substring(26).GetInt(0, false);
            }
            return 0;
        }

        /// <summary>
        /// 判断本地是否有更新的/未提交的/冲突的/未处理的
        /// </summary>
        /// <param name="path">目录</param>
        /// <param name="errMsg">输出检查状态时的错误异常</param>
        /// <returns></returns>
        public static List<SVNItem> GetLocalStatus(string path, out string errMsg) {
            var list = new List<SVNItem>();
            errMsg = string.Empty;
            string result = StatuePrimeval(path, "");
            string[] resultRows = result.Replace("\r", "").Split('\n');
            foreach (var r in resultRows) {
                if (!r.Substring(1).StartsWith("       ")) {
                    errMsg += r + "\n";
                } else {
                    SVNItem item = new SVNItem { Path = r.Substring(8) };
                    var state = ParseItemState(r[0]);
                    if (!state.HasValue) continue;
                    item.State = state.Value;
                    list.Add(item);
                }
            }
            return list;
        }

        private static ESVNItemState? ParseItemState(char chr) {
            switch (chr) {
                case '?':
                    return ESVNItemState.Un;
                case '!':
                    return ESVNItemState.Lose;
                default:
                    var state = chr.ToString().ParseEnum<ESVNItemState>();
                    if (state.IsEnumEmpty()) {
                        return null;
                    }
                    return state;
            }
        }

        /// <summary>
        /// 提交,完全成功,才返回true,失败的有:被锁定,需要清理,有冲突等.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="log"></param>
        /// <param name="revision"></param>
        /// <param name="commotList"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static bool Commit(string path, string log, out int revision, List<SVNItem> commotList, out string errMsg) {
            revision = -1;
            commotList = new List<SVNItem>();
            errMsg = string.Empty;

            string result = CommitPrimeval(path, log);
            string[] resultRows = result.Replace("\r", "").Split('\n');
            foreach (var r in resultRows) {

                if (r.Trim().IsEmpty()) continue;

                if (r.StartsWith("Committed revision ")) {
                    revision = r.Substring(19).GetInt(-1, false);
                    //svn的提交命令这个信息一出来,就表示完全提交成功了,把错误信息清除掉
                    errMsg = string.Empty;
                    break;
                }
                if (r.StartsWith("Transmitting file data .")) {
                    continue;
                }
                

                ESVNItemState state = (ESVNItemState)0;
                if (r.StartsWith("Sending")) {
                    state = ESVNItemState.M;
                } else if (r.StartsWith("Adding")) {
                    state = ESVNItemState.A;
                } else if (r.StartsWith("Deleting")) {
                    state = ESVNItemState.D;
                }
                if (!state.IsEnumEmpty()) {
                    commotList.Add(new SVNItem {
                        Path = r.Substring(5),
                        State = state,
                    });
                    continue;
                }


                //剩下的是未知的信息
                errMsg += r + "\n";
            }

            if (errMsg.IsEmpty() && commotList.FirstOrDefault(i => i.State == ESVNItemState.C) == null) {
                //没有错误文本,且没有冲突的项,则返回完全成功
                return true;
            }

            return false;
        }


        /// <summary>
        /// 使用GUI来提交,返回错误信息,如果没错误则返回空字符串
        /// </summary>
        /// <param name="path"></param>
        /// <param name="noErrAutoClose">没错误是否自动关闭窗口</param>
        public static string CommitGUI(string path, bool noErrAutoClose, string logmsg) {
            return ExecExe("TortoiseProc", false, "/command:commit /path:\"" + path + "\" /closeonend:" + (noErrAutoClose ? "2" : "0") + " /logmsg:\"" + logmsg + "\"");
        }

        
        /// <summary>
        /// 更新,完全成功,才返回true,失败的有:被锁定,需要清理,更新后有冲突等.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="revision">如果更新成功,则输出更新到的版本号,更新失败则输出-1</param>
        /// <param name="updateList">输出更新列表</param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static bool Update(string path, EConflictAccept? confilictAccept, out int revision, out List<SVNItem> updateList, out string errMsg) {
            revision = -1;
            updateList = new List<SVNItem>();
            errMsg = string.Empty;

            string updParam = "";
            if (confilictAccept != null) {
                updParam += " --accept " + confilictAccept.GetCustomAttribute<EnumAttr>().Desc;
            }

            string result = UpdatePrimeval(path, updParam);
            string[] resultRows = result.Replace("\r", "").Split('\n');
            foreach (var r in resultRows) {

                if (r.Trim().IsEmpty()) continue;

                if (r.Length > 5 && r.Substring(1).StartsWith("    ")) {
                    var state = r.Substring(0, 1).ParseEnum<ESVNItemState>();
                    if (!state.IsEnumEmpty()) {
                        updateList.Add(new SVNItem {
                            Path = r.Substring(5),
                            State = state,
                        });
                        continue;
                    }
                }
                if (r.StartsWith("Updated to revision ")) {
                    revision = r.Substring(20).GetInt(-1, false);
                    continue;
                }
                if (r.StartsWith("At to revision ")) {
                    revision = r.Substring(15).GetInt(-1, false);
                    continue;
                }
                if (r.StartsWith("Summary of conflicts:")) {
                    errMsg += "冲突摘要:";
                    continue;
                }
                if (r.StartsWith("  Text conflicts: ")) {
                    errMsg += r.Substring(18).GetInt(0, false) + "个文本冲突.";
                    continue;
                }

                //剩下的是未知的信息
                errMsg += r + "\n";
            }

            if (errMsg.IsEmpty() && updateList.FirstOrDefault(i => i.State == ESVNItemState.C) == null) {
                //没有错误文本,且没有冲突的项,则返回完全成功
                return true;
            }
            return false;
        }

        /// <summary>
        /// 使用GUI来更新,返回错误信息,如果没错误则返回空字符串
        /// </summary>
        /// <param name="path"></param>
        /// <param name="noErrAutoClose">没错误是否自动关闭窗口</param>
        public static string UpdateGUI(string path, bool noErrAutoClose) {
            return ExecExe("TortoiseProc", false, "/command:update /path:\"" + path + "\" /closeonend:" + (noErrAutoClose ? "2" : "0") + "");
        }
    }
}
