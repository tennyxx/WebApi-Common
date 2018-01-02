using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace WebApi.Common.Util
{
    /// <summary>
    ///  公共处理，转换去空值 
    /// </summary>
    public static class ExConvert
    {
        private static readonly Regex INTEGER_REGEX = new Regex(@"^\-?\d{1,9}$", RegexOptions.Compiled);
        private static readonly Regex LONG_REGEX = new Regex(@"^\-?\d{1,18}$", RegexOptions.Compiled);
        private static readonly Regex SHORT_REGEX = new Regex(@"^\-?\d{1,5}$", RegexOptions.Compiled);
        private static readonly Regex FLOAT_REGEX = new Regex(@"^\-?\d+(\.\d+)?$", RegexOptions.Compiled);

        public static string ToString(object data, string def_value)
        {
            if (null == data)
                return def_value;

            return Convert.ToString(data).Trim();
        }
        public static string ToString(object data)
        {
            return ToString(data, string.Empty);
        }

        public static int ToInt(object data, int def_value)
        {
            if (data == null || data == DBNull.Value)
                return def_value;

            if (data is int)
                return (int)data;

            if (INTEGER_REGEX.IsMatch(data.ToString()))
            {
                return Convert.ToInt32(data);
            }
            else
            {
                try
                {
                    return Convert.ToInt32(data);
                }
                catch
                {
                    return def_value;
                }
            }
        }
        public static int ToInt(object data)
        {
            return ToInt(data, 0);
        }

        public static long ToLong(object data)
        {
            if (data == null || data == DBNull.Value)
                return 0L;

            if (data is long)
                return (long)data;

            if (LONG_REGEX.IsMatch(data.ToString()))
            {
                return Convert.ToInt64(data);
            }
            else
            {
                try
                {
                    return Convert.ToInt32(data);
                }
                catch
                {
                    return 0L;
                }
            }
        }

        public static short ToShort(object data)
        {
            if (data == null || data == DBNull.Value)
                return 0;

            if (data is short)
                return (short)data;

            if (SHORT_REGEX.IsMatch(data.ToString()))
            {
                return Convert.ToInt16(data);
            }
            else
            {
                try
                {
                    return Convert.ToInt16(data);
                }
                catch
                {
                    return 0;
                }
            }
        }

        public static DateTime ToDateTime(object data)
        {
            return ToDateTime(data, Convert.ToDateTime("1900-01-01"));
        }
        public static DateTime ToDateTime(object data, DateTime def_value)
        {
            if (data == null || data == DBNull.Value)
                return def_value;

            if (data is DateTime)
                return (DateTime)data;

            try
            {
                return Convert.ToDateTime(data);
            }
            catch
            {
                return def_value;
            }
        }
        public static decimal ToDecimal(object data)
        {
            if (data == null || data == DBNull.Value)
                return 0;

            if (data is decimal)
                return (decimal)data;

            try
            {
                //string str = Convert.ToDecimal(data).ToString("n");
                return Convert.ToDecimal(data);
            }
            catch
            {
                return 0;
            }
        }
        public static bool ToBoolean(object d)
        {
            return ToBoolean(d, false);
        }
        public static bool ToBoolean(object d, bool def_value)
        {
            if (d == null || d == DBNull.Value)
                return def_value;

            if (d is bool)
                return (bool)d;

            try
            {
                return Convert.ToBoolean(d);
            }
            catch
            {
                return def_value;
            }
        }

        public static byte ToByte(object data)
        {
            if (data == null || data == DBNull.Value)
                return 0;

            if (data is byte)
                return (byte)data;

            try
            {
                return Convert.ToByte(data);
            }
            catch
            {
                return 0;
            }
        }

        public static byte[] ToBinary(object data)
        {
            try
            {
                return (byte[])data;
            }
            catch
            {
                return null;
            }
        }
        public static float ToFloat(object data)
        {
            if (data == null || data == DBNull.Value)
                return 0f;

            if (data is float)
                return (float)data;

            if (FLOAT_REGEX.IsMatch(data.ToString()))
            {
                return Convert.ToSingle(data);
            }
            else
            {
                return 0f;
            }
        }

        public static string ToBase64(string strnormal)
        {
            try
            {
                return Convert.ToBase64String(Encoding.Default.GetBytes(strnormal));
            }
            catch
            {
                return string.Empty;
            }
        }
        public static string FromBase64(string strbase64)
        {
            try
            {
                return Encoding.Default.GetString(Convert.FromBase64String(strbase64));
            }
            catch
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// 解码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string URLDecode(string str)
        {
            return System.Web.HttpUtility.UrlDecode(str, Encoding.UTF8).Replace("\"", "&quot;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "%27");
        }
        /// <summary>
        /// 编码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string URLEncode(string str)
        {
            return System.Web.HttpUtility.UrlEncode(str, Encoding.UTF8).Replace("'", "%27"); ;
        }

        /// <summary>
        /// 包括特殊 sql 脚本
        /// </summary>
        /// <returns></returns>
        public static bool CheckContainsSqlCommand(NameValueCollection QueryString)
        {
            string SqlCommandString = "or |and |exec |insert |select |delete |update |count |* |chr |mid |master |truncate |char |declare ";
            string[] SqlCommandArr = SqlCommandString.Split('|');
            for (int k = 0; k < QueryString.Count; k++)
            {
                for (int i = 0; i < SqlCommandArr.Length; i++)
                {
                    if (QueryString[k].IndexOf(SqlCommandArr[i]) >= 0)
                    {
                        return true;
                    }
                }
            }
            //foreach (KeyValuePair<string, string> kvp in QueryString)
            //{
            //}
            return false;
        }

        /// <summary>
        /// 格式化性别
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string FormartSex(string val)
        {
            if (string.IsNullOrEmpty(val))
                return string.Empty;

            return val.Equals("1", StringComparison.Ordinal) ? "男" : "女";
        }

        public static string FormatReadState(string val)
        {
            if (string.IsNullOrEmpty(val))
                return string.Empty;

            return val.Equals("1", StringComparison.Ordinal) ? "已读" : "未读";
        }

        /// <summary>
        /// 格式化时间为yyyy-MM-dd HH:mm
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string FormatDateTime(string val)
        {
            DateTime time;
            if (string.IsNullOrEmpty(val) || !DateTime.TryParse(val, out time))
                return string.Empty;

            return time.ToString("yyyy-MM-dd HH:mm");
        }

        /// <summary>
        /// 格式化时间
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string FormatDateTime(DateTime? time)
        {
            if (Equals(null, time))
                return string.Empty;

            return time.Value.ToString("yyyy-MM-dd HH:mm");
        }

        /// <summary>
        /// 格式化时间
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string FormatDateTime(DateTime time)
        {
            return time.ToString("yyyy-MM-dd HH:mm");
        }

        /// <summary>
        /// int类型转枚举
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <returns></returns>
        public static T ToEnum<T>(int val) where T : struct
        {
            return ToEnum<T>(val.ToString());
        }

        /// <summary>
        /// string类型转枚举
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <returns></returns>
        public static T ToEnum<T>(string val) where T : struct
        {
            if (string.IsNullOrEmpty(val))
                throw new ArgumentException("无法将空值转换为枚举类型！");

            T t;
            Enum.TryParse(val, out t);
            return t;
        }

        /// <summary>
        /// 枚举转字典
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Dictionary<int, string> EnumToDictionary<T>()
        {
            var type = typeof(T);
            if (!type.IsEnum)
                throw new ArgumentException("待转换的类型必须是枚举！", type.Name);

            var enumDic = new Dictionary<int, string>();
            Array enumValues = Enum.GetValues(type);
            foreach (Enum enumValue in enumValues)
            {
                int key = enumValue.GetHashCode();
                string name = Enum.GetName(type, enumValue);
                var field = type.GetField(name);
                var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                string value = attribute == null ? string.Empty : attribute.Description;
                enumDic.Add(key, value);
            }

            return enumDic;
        }

        /// <summary>
        /// 获取枚举的描述
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string GetEnumDes<T>(int val) where T : struct
        {
            return GetEnumDes<T>(val.ToString());
        }

        /// <summary>
        /// 获取枚举描述
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string GetEnumDes<T>(short val) where T : struct
        {
            return GetEnumDes<T>(val.ToString());
        }

        /// <summary>
        /// 获取枚举的描述
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string GetEnumDes<T>(string val) where T : struct
        {
            T en;
            Enum.TryParse(val, out en);
            //获取类型
            Type type = en.GetType();
            //获取成员
            MemberInfo[] memberInfos = type.GetMember(en.ToString());
            if (memberInfos != null && memberInfos.Length > 0)
            {
                //获取描述特性  
                DescriptionAttribute[] attrs = memberInfos[0].GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];
                if (attrs != null && attrs.Length > 0)
                {
                    //返回当前描述  
                    return attrs[0].Description;
                }
            }

            return en.ToString();
        }

    }
}
