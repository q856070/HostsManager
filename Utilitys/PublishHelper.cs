using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Windows.Forms;
using System.Collections.Specialized;
using System.Web;
using Utilitys.Modul;
using System.ComponentModel;
using System.Threading;
using System.IO;
using System.Net.Sockets;

namespace Utilitys {
    public static class PublishHelper {

        [System.Runtime.InteropServices.DllImport("user32")]
        private static extern System.IntPtr GetForegroundWindow();


        public static List<MCoder> GetAllCoder(string tcAddinsRootPath, string serverAddress, out string errMsg) {
            errMsg = null;
            List<MCoder> allCoder = null;
            if (!string.IsNullOrEmpty(serverAddress)) {
                string allCoderCfgPath = "http://" + serverAddress + "/TcAddinsServer/allCoderCfg.xml";
                XDocument xCfgDoc = null;
                try { xCfgDoc = XDocument.Load(allCoderCfgPath); } catch { }
                if (xCfgDoc != null) {
                    var allCoderEls = XmlHelper.GetElements(xCfgDoc.Root, "coder");
                    allCoder = new List<MCoder>();
                    foreach (var item in allCoderEls) {
                        allCoder.Add(new MCoder {
                            Name = XmlHelper.GetElementXCData(item, "name"),
                            IP = XmlHelper.GetElementXCData(item, "ip")
                        });
                    }
                } else {
                    errMsg = "无法连接到主机,需要将[" + serverAddress + "]打开";
                }
            } else {
                errMsg = "主机配置错误";
            }

            return allCoder;
        }
        public static void GetAllCoder(string tcAddinsRootPath, string serverAddress, DgAsynGetData<List<MCoder>> dg, ISynchronizeInvoke invokeObj) {
            ThreadPool.QueueUserWorkItem(new WaitCallback(delegate(object obj) {
                string errMsg;
                var re = GetAllCoder(tcAddinsRootPath, serverAddress, out errMsg);
                invokeObj.Invoke(dg, new object[] { re, errMsg });
            }));
        }

        public static string GetCodeDir(string tcAddinsRootPath) {
            string xmlFile = tcAddinsRootPath + @"\config.xml";
            string defXmlStr = "<?xml version=\"1.0\" encoding=\"utf-8\"?><config></config>";
            XDocument xdoc = XmlHelper.GetXmlDoc(xmlFile, defXmlStr);
            var val = XmlHelper.GetElementXCData(xdoc.Root, "CodeDir");
            return val;
        }
        public static void SetCodeDir(string tcAddinsRootPath, string val) {
            string xmlFile = tcAddinsRootPath + @"\config.xml";
            string defXmlStr = "<?xml version=\"1.0\" encoding=\"utf-8\"?><config></config>";
            XDocument xdoc = XmlHelper.GetXmlDoc(xmlFile, defXmlStr);
            XmlHelper.SetElementXCData(xdoc.Root, "CodeDir", val);
            xdoc.Save(xmlFile);
        }

        public static string GetCurrCoderName(string tcAddinsRootPath) {
            string xmlFile = tcAddinsRootPath + @"\config.xml";
            string defXmlStr = "<?xml version=\"1.0\" encoding=\"utf-8\"?><config></config>";
            XDocument xdoc = XmlHelper.GetXmlDoc(xmlFile, defXmlStr);
            var name = XmlHelper.GetElementXCData(xdoc.Root, "CoderName");
            return name;
        }
        public static void SetCurrCoderName(string tcAddinsRootPath, string name) {
            string xmlFile = tcAddinsRootPath + @"\config.xml";
            string defXmlStr = "<?xml version=\"1.0\" encoding=\"utf-8\"?><config></config>";
            XDocument xdoc = XmlHelper.GetXmlDoc(xmlFile, defXmlStr);
            XmlHelper.SetElementXCData(xdoc.Root, "CoderName", name);
            xdoc.Save(xmlFile);
        }

        public static string GetServerAddress(string tcAddinsRootPath) {
            string xmlFile = tcAddinsRootPath + @"\config.xml";
            string defXmlStr = "<?xml version=\"1.0\" encoding=\"utf-8\"?><config></config>";
            XDocument xdoc = XmlHelper.GetXmlDoc(xmlFile, defXmlStr);
            var val = XmlHelper.GetElementXCData(xdoc.Root, "ServerAddress");
            return val;
        }
        public static void SetServerAddress(string tcAddinsRootPath, string val) {
            string xmlFile = tcAddinsRootPath + @"\config.xml";
            string defXmlStr = "<?xml version=\"1.0\" encoding=\"utf-8\"?><config></config>";
            XDocument xdoc = XmlHelper.GetXmlDoc(xmlFile, defXmlStr);
            XmlHelper.SetElementXCData(xdoc.Root, "ServerAddress", val);
            xdoc.Save(xmlFile);
        }


        public static void ShowModelForm(FormIsClosed form) {
            form.Show(new WindowWrapper(GetForegroundWindow()));
            while (!form.IsClosed) {
                Application.DoEvents();
                System.Threading.Thread.Sleep(10);
            }
        }

        public static NameValueCollection GetNameValue(string paramStr) {
            string[] p = paramStr.Split('&');
            NameValueCollection re = new NameValueCollection();
            foreach (var pStr in p) {
                if (!string.IsNullOrEmpty(pStr)) {
                    var nv = pStr.Split('=');
                    re[HttpUtility.UrlDecode(nv[0], Encoding.UTF8)] = nv.Length > 1 ? HttpUtility.UrlDecode(nv[1], Encoding.UTF8) : "";
                }
            }
            return re;
        }
        public static string GetNameValue(NameValueCollection nameValue) {
            StringBuilder str = new StringBuilder();
            foreach (string key in nameValue.Keys) {
                str.Append(HttpUtility.UrlEncode(key, Encoding.UTF8));
                str.Append("=");
                str.Append(HttpUtility.UrlEncode(nameValue[key], Encoding.UTF8));
                str.Append("&");
            }
            if (str.Length > 0) str.Remove(str.Length - 1, 1);
            return str.ToString();
        }


        public static string ReadStringInStream(Socket socket) {
            byte[] _WriteBytes = new byte[socket.ReceiveBufferSize];
            for (int i = 0; i < 1000; i++) {
                socket.Receive(_WriteBytes);
                string re = Encoding.UTF8.GetString(_WriteBytes).Trim('\0');
                if (!string.IsNullOrEmpty(re)) return re;
                Thread.Sleep(10);
            }
            return string.Empty;
        }
        public static void WriteStringInStream(Stream stream, string str) {
            byte[] data = Encoding.UTF8.GetBytes(str);
            stream.Write(data, 0, data.Length);
            stream.Flush();
        }
        
    }
    public interface FormIsClosed {
        bool IsClosed { get; }
        void Show(IWin32Window owner);
    }
    public delegate void DgAsynGetData<T>(T data, string errMsg);
    public delegate void DgInvoke();
}
