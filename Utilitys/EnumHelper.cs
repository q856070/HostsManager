using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Utilitys {
    /// <summary>
    /// 枚举工具类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class EnumHelper<T> {
        /// <summary>
        /// 获取所有枚举
        /// </summary>
        /// <returns></returns>
        public static List<T> GetAllItem() {
            List<T> list = new List<T>();
            Type enType = typeof(T);
            var enProArr = enType.GetFields();
            foreach (var en in enProArr) {
                if (en.FieldType.Equals(enType)) {
                    list.Add((T)Enum.Parse(enType, en.Name));
                }
            }
            return list;
        }

        /// <summary>
        /// 获取所有枚举和对应的属性
        /// </summary>
        /// <returns></returns>
        public static List<EnumItem<T>> GetAllEnumItem() {
            List<EnumItem<T>> list = new List<EnumItem<T>>();
            Type enType = typeof(T);
            var enProArr = enType.GetFields();
            foreach (var en in enProArr) {
                if (en.FieldType.Equals(enType)) {
                    T em = (T)Enum.Parse(enType, en.Name);
                    object[] attrs = en.GetCustomAttributes(false);
                    EnumAttr attr = (attrs == null || attrs.Length <= 0 ? null : (EnumAttr)attrs[0]);
                    list.Add(new EnumItem<T> { Enum = em, Attr = attr });
                }
            }
            return list;
        }

    }
    /// <summary>
    /// 枚举工具类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class EnumHelper {
        /// <summary> 
        /// 获取枚举项的Attribute 
        /// </summary> 
        /// <typeparam name="T">自定义的Attribute</typeparam> 
        /// <param name="source">枚举</param> 
        /// <returns>返回枚举,否则返回null</returns> 
        public static T GetCustomAttribute<T>(this Enum source) where T : Attribute {
            Type sourceType = source.GetType();
            string sourceName = Enum.GetName(sourceType, source);
            FieldInfo field = sourceType.GetField(sourceName);
            object[] attributes = field.GetCustomAttributes(typeof(T), false);
            foreach (object attribute in attributes) {
                if (attribute is T)
                    return (T)attribute;
            }
            return null;
        }

        public static T ParseEnum<T>(this string enumName) {
            return (T)Enum.Parse(typeof(T), enumName);
        }

        /// <summary>
        /// 是否枚举为空
        /// </summary>
        /// <param name="enu"></param>
        /// <returns></returns>
        public static bool IsEnumEmpty(this object enu) {
            try {
                return !Enum.IsDefined(enu.GetType(), enu);
            } catch { }
            return true;
        }
        /// <summary>
        /// 是否枚举为空
        /// </summary>
        /// <param name="enu"></param>
        /// <returns></returns>
        public static T GetEnum<T>(this object enu) {
            try {
                return (T)enu;
            } catch { }
            return default(T);
        }
    }
    /// <summary>
    /// 枚举的属性
    /// </summary>
    public class EnumAttr : Attribute {
        /// <summary>
        /// 文本
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// 说明字符串(比Text长)
        /// </summary>
        public string Desc { get; set; }
    }

    /// <summary>
    /// 枚举项(包括枚举和对应的属性)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EnumItem<T> {
        public T Enum { get; set; }
        public EnumAttr Attr { get; set; }
    }
}
