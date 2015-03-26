using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Xml;

namespace Utilitys {
    public static class XmlHelper {
        public static XDocument GetXmlDoc(string xmlPath,string defXmlStr){
            if (!File.Exists(xmlPath)) {
                try {
                    string dir = Path.GetDirectoryName(xmlPath);
                    if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                    File.WriteAllText(xmlPath, defXmlStr, Encoding.UTF8);
                } catch { }
            }
            try {
                return XDocument.Load(xmlPath);
            } catch { }
            return null;
        }

        /// <summary>
        /// 设置元素下指定元素的值,该元素不存在则会自动创建
        /// </summary>
        /// <param name="parentEl"></param>
        /// <param name="elName"></param>
        /// <param name="XCData"></param>
        public static void SetElementXCData(XElement parentEl, string elName, string XCData) {
            var t = parentEl.Element(elName);
            if (t == null) {
                t = new XElement(elName);
                parentEl.Add(t);
            } else {
                t.RemoveNodes();
            }
            t.Add(new XCData(XCData));
        }
        /// <summary>
        /// 获取元素下指定元素的值
        /// </summary>
        /// <param name="parentEl"></param>
        /// <param name="elName"></param>
        /// <param name="XCData"></param>
        public static string GetElementXCData(XElement parentEl, string elName) {
            var t = parentEl.Element(elName);
            if (t == null) {
                return string.Empty;
            } else {
                return t.Value;
            }
        }
        /// <summary>
        /// 获取元素下指定名称的元素列表不会为空
        /// </summary>
        /// <param name="parentEl"></param>
        /// <param name="elName"></param>
        /// <param name="XCData"></param>
        public static IEnumerable<XElement> GetElements(XElement parentEl, string elName) {
            IEnumerable<XElement> t = null;
            if (parentEl == null || (t = parentEl.Elements(elName)) == null) {
                return new List<XElement>();
            } else {
                return t;
            }
        }
        /// <summary>
        /// 获取元素下指定名称的元素
        /// </summary>
        /// <param name="parentEl"></param>
        /// <param name="elName"></param>
        /// <param name="XCData"></param>
        public static XElement GetElement(XElement parentEl, string elName, bool noExistCreate) {
            if (parentEl == null) throw new Exception("父元素不能为空");
            XElement t = parentEl.Element(elName);
            if (t == null && noExistCreate) {
                t = new XElement(elName);
                parentEl.Add(t);
            }
            return t;
        }
    }
}
