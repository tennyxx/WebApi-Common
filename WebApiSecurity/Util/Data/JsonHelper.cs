using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace WebApi.Common.Util
{
    /// <summary>
    /// JSON序列化和反序列化辅助类
    /// </summary>
    public class JsonHelper
    {

        /// <summary>
        /// 将对象序列化成 json 字符串  
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static string ObjectToJson(object obj)
        {
            Newtonsoft.Json.Converters.IsoDateTimeConverter jc = new Newtonsoft.Json.Converters.IsoDateTimeConverter();
            jc.DateTimeFormat = "yyyy-MM-dd HH:mm:ss.fff";
            return JsonConvert.SerializeObject(obj, Formatting.None, jc);
        }
        /// <summary>
        /// 将对象序列化成 json 字符串  
        /// </summary>
        /// <param name="obj">序列化的对象</param>
        /// <param name="format">日期数据格式化</param>
        /// <returns>返回Json</returns>
        public static string ObjectToJson(object obj, string format)
        {
            Newtonsoft.Json.Converters.IsoDateTimeConverter jc = new Newtonsoft.Json.Converters.IsoDateTimeConverter();
            jc.DateTimeFormat = format;
            return JsonConvert.SerializeObject(obj, Formatting.None, jc);
        }
        /// <summary>
        /// 将对象序列化成 json 字符串  
        /// </summary>
        /// <param name="obj">序列化的对象</param>
        /// <param name="Count">列表数据总记录数</param>
        /// <returns>{\"jsondata\":" + Json + ",\"rowcount\":\"" + Count + "\"}</returns>
        public static string ObjectToJsonAndCount(object obj, int Count)
        {
            Newtonsoft.Json.Converters.IsoDateTimeConverter jc = new Newtonsoft.Json.Converters.IsoDateTimeConverter();
            jc.DateTimeFormat = "yyyy-MM-dd HH:mm:ss.fff";
            string Json = JsonConvert.SerializeObject(obj, Formatting.None, jc);
            return "{\"jsondata\":" + Json + ",\"rowcount\":\"" + Count + "\"}";
        }

        /// <summary>
        /// 将对象序列化成 json 字符串  
        /// </summary>
        /// <param name="obj">序列化的对象</param>
        /// <param name="format">日期数据格式化</param>
        /// <param name="Count">列表数据总记录数</param>
        /// <returns>{\"jsondata\":" + Json + ",\"rowcount\":\"" + Count + "\"}</returns>
        public static string ObjectToJsonAndCount(object obj, string format, int Count)
        {
            Newtonsoft.Json.Converters.IsoDateTimeConverter jc = new Newtonsoft.Json.Converters.IsoDateTimeConverter();
            jc.DateTimeFormat = format;
            string Json = JsonConvert.SerializeObject(obj, Formatting.None, jc);
            return "{\"jsondata\":" + Json + ",\"rowcount\":\"" + Count + "\"}";
        }

        /// <summary>
        /// 将对象序列化成 json 字符串  
        /// </summary>
        /// <param name="obj">序列化的对象</param>
        /// <param name="format">日期数据格式化</param>
        /// <param name="Count">列表数据总记录数</param>
        /// <returns>{\"jsondata\":" + Json + ",\"rowcount\":\"" + Count + "\"}</returns>
        public static string ObjectToJsonAndCount(object obj, string format, int Count, int immediatelyoverduecount, int alreadyoverduecount)
        {
            Newtonsoft.Json.Converters.IsoDateTimeConverter jc = new Newtonsoft.Json.Converters.IsoDateTimeConverter();
            jc.DateTimeFormat = format;
            string Json = JsonConvert.SerializeObject(obj, Formatting.None, jc);
            return "{\"jsondata\":" + Json + ",\"rowcount\":\"" + Count + "\",\"immediatelyoverduecount\":\"" + immediatelyoverduecount + "\",\"alreadyoverduecount\":\"" + alreadyoverduecount + "\"}";
        }
        /// <summary>
        /// 将json 字符串反序列化成对象 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        public static T JsonToObject<T>(string str)
        {
            return (T)Newtonsoft.Json.JsonConvert.DeserializeObject<T>(str);
        }
        /// <summary>
        /// 将对象序列化为JSON格式
        /// </summary>
        /// <param name="o">对象</param>
        /// <returns>json字符串</returns>
        public static string SerializeObject(object o)
        {
            string json = JsonConvert.SerializeObject(o);
            return json;
        }
        /// <summary>
        /// 解析JSON数组生成对象实体集合
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">json数组字符串(eg.[{"ID":"112","Name":"石子儿"}])</param>
        /// <returns>对象实体集合</returns>
        public static List<T> DeserializeJsonToList<T>(string json) where T : class
        {
            JsonSerializer serializer = new JsonSerializer();
            StringReader sr = new StringReader(json);
            object o = serializer.Deserialize(new JsonTextReader(sr), typeof(List<T>));
            List<T> list = o as List<T>;
            return list;
        }
        /// <summary>
        /// 解析JSON字符串生成对象实体
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">json字符串(eg.{"ID":"112","Name":"石子儿"})</param>
        /// <returns>对象实体</returns>
        public static T DeserializeJsonToObject<T>(string json) where T : class
        {
            JsonSerializer serializer = new JsonSerializer();
            StringReader sr = new StringReader(json);
            object o = serializer.Deserialize(new JsonTextReader(sr), typeof(T));
            T t = o as T;
            return t;
        }
        /// <summary>
        /// 将datatable转换为json  
        /// </summary>
        /// <param name="dtb">Dt</param>
        /// <returns>JSON字符串</returns>
        public static string Dtb2Json(DataTable dtb)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            System.Collections.ArrayList dic = new System.Collections.ArrayList();
            foreach (DataRow dr in dtb.Rows)
            {
                System.Collections.Generic.Dictionary<string, object> drow = new System.Collections.Generic.Dictionary<string, object>();
                foreach (DataColumn dc in dtb.Columns)
                {
                    drow.Add(dc.ColumnName, dr[dc.ColumnName]);
                }
                dic.Add(drow);

            }
            //序列化  
            return jss.Serialize(dic);
        }

        public static dynamic JsonToDynamic(string requestStr)
        {
            try
            {
                JavaScriptSerializer Serializer = new JavaScriptSerializer();
                dynamic obj = Serializer.Deserialize<dynamic>(requestStr);
                return obj;

            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }

}