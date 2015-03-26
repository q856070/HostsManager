using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Specialized;

namespace Utilitys {
    public static class ObjectExtend {

        /// <summary>
        /// ZD加密(类似MD5,不可逆,任何一位更改返回值就不同,结果是32个字符)
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GeZD(this byte[] data, string key) {
            const byte reLen = 16;
            byte[] re = new byte[reLen];
            byte keyLen = (byte)key.Length;

            if (data != null) {
                long i = 0, len = data.Length;
                for (i = 0; i < len; i++) {
                    byte b = data[i];
                    int k = (int)key[(b + reLen) % keyLen];

                    int index1 = (int)(i % reLen);
                    int index2 = b % reLen;

                    byte appD = data[(index1 + index2 + k) % len];

                    re[index1] = (byte)((re[index2] + k + b + len) % 255);
                    re[index2] = (byte)((re[index1] + k + b + len) % 255);
                }
                for (i = len - 1; i >= 0; i--) {
                    byte b = data[i];
                    int k = (int)key[(b + reLen + 1) % keyLen];

                    int index1 = (int)((i + 1) % reLen);
                    int index2 = (b + index1) % reLen;

                    byte appD = data[(index1 + index2 + k) % len];

                    re[index1] = (byte)((re[index2] + k + b + len) % 255);
                    re[index2] = (byte)((re[index1] + k + b + len) % 255);
                }
            }

            //每位转换成16进制(高位补零)
            return System.BitConverter.ToString(re).Replace("-", "");
        }

        /// <summary>
        /// 从对象中取得decimal数据 取不到时,解析错误时,返回 null
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static decimal? GetDecimal(this object obj) {
            if (obj.isDbNullOrNull()) {
                return null;
            } else {
                try {
                    return Convert.ToDecimal(obj);
                } catch { return null; }
            }
        }

        /// <summary>
        /// 从对象中取得字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetString(this object obj, string defaultValue) {
            return obj == null ? defaultValue : obj == DBNull.Value ? defaultValue : obj.ToString();
        }

        /// <summary>
        /// 从对象中取得int数据 取不到时,解析错误时,返回 null
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int? GetInt(this object obj) {
            if (obj.isDbNullOrNull() || obj.GetString(string.Empty).IsEmpty()) {
                return null;
            } else {
                try {
                    return Convert.ToInt32(obj);
                } catch { return null; }
            }
        }

        /// <summary>
        /// 从对象中取得int数据
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int GetInt(this object obj, int defaultValue) {
            return GetInt(obj, defaultValue, false);
        }
        /// <summary>
        /// 从对象中取得int数据
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="isCatch">是否处理异常</param>
        /// <returns></returns>
        public static int GetInt(this object obj, int defaultValue, bool isCatch) {
            try {
                if (obj == null || obj.GetString(string.Empty).IsEmpty()) return defaultValue;
                return Convert.ToInt32(obj);
            } catch (Exception ex) {
                if (isCatch) {
                    throw ex;
                } else {
                    return defaultValue;
                }
            }
        }
        /// <summary>
        /// 从对象中取得double数据
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="isCatch">是否处理异常</param>
        /// <returns></returns>
        public static double GetDouble(this object obj, double defaultValue, bool isCatch) {
            try {
                return Convert.ToDouble(obj);
            } catch (Exception ex) {
                if (isCatch) {
                    throw ex;
                } else {
                    return defaultValue;
                }
            }
        }

        /// <summary>
        /// 判断对象是否是空
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool isDbNullOrNull(this object obj) {
            return obj == null || obj == DBNull.Value;
        }

        /// <summary>
        /// 从对象中取得时间数据,取不到时,返回 null
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DateTime? GetDateTime(this object obj) {
            if (obj.isDbNullOrNull()) {
                return null;
            } else {
                try {
                    return Convert.ToDateTime(obj);
                } catch { return null; }
            }
        }
        /// <summary>
        /// 从对象中取得时间
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DateTime GetDateTime(this object obj, DateTime defaultVal) {
            if (!obj.isDbNullOrNull()) {
                try {
                    return Convert.ToDateTime(obj);
                } catch { }
            }
            return defaultVal;
        }

        /// <summary>
        /// 从对象中取得bool数据
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static bool GetBoolean(this object obj, bool defaultValue) {
            return obj == null ? defaultValue : obj == DBNull.Value ? defaultValue : Convert.ToBoolean(obj);
        }
        /// <summary>
        /// 从对象中取得bool数据
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static bool GetBoolean(this object obj, bool defaultValue, bool isCatch) {
            try {
                return Convert.ToBoolean(obj);
            } catch (Exception ex) {
                if (isCatch) {
                    throw ex;
                } else {
                    return defaultValue;
                }
            }
        }
        /// <summary>
        /// 从对象中取得bool数据
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static bool? GetBoolean(this object obj) {
            return obj == null ? null : (obj == DBNull.Value ? null : (bool?)Convert.ToBoolean(obj));
        }

        /// <summary>
        /// 如果对象为空,取默认值 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static object GetDefault(this object obj, object defaultValue) {
            return obj == null ? defaultValue : obj == DBNull.Value ? defaultValue : obj;
        }

        /// <summary>
        /// 序列化对象为字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeToString(this object obj) {
            IFormatter formatter = new BinaryFormatter();
            string result = string.Empty;
            using (MemoryStream stream = new MemoryStream()) {
                formatter.Serialize(stream, obj);

                byte[] byt = new byte[stream.Length];
                byt = stream.ToArray();

                result = Convert.ToBase64String(byt);
                stream.Flush();
            }
            return result;
        }


        /// <summary>
        /// 保留小数位(不四舍五入,直接截取)
        /// </summary>
        /// <param name="fval"></param>
        /// <param name="decCount"></param>
        /// <returns></returns>
        public static double ToCutDecimal(this double fNum, int decCount) {
            double d = Math.Pow(10, decCount);
            return (Math.Truncate(fNum * d) / d);
        }

        /// <summary>
        /// 将名值对象转换为查询字符串
        /// </summary>
        /// <param name="nameValues"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string GetQuesyString(this NameValueCollection nameValues, Encoding encode) {
            StringBuilder str = new StringBuilder();
            foreach (string key in nameValues.Keys) {
                str.Append(key);
                str.Append("=");
                str.Append(nameValues[key].UrlEncode(encode));
                str.Append("&");
            }
            if (str.Length > 0) str.Remove(str.Length - 1, 1);
            return str.ToString();
        }
    }
}
