using System;
using System.Web;

namespace WebApi.Common.Util
{
    //请求 通用方法
    public class RequestUtility
    {
        /// <summary>
        /// 判断当前页面是否接收到了Post请求
        /// </summary>
        /// <returns>是否接收到了Post请求</returns>
        public static bool IsPost()
        {
            return HttpContext.Current.Request.HttpMethod.Equals("POST");
        }
        /// <summary>
        /// 判断当前页面是否接收到了Get请求
        /// </summary>
        /// <returns>是否接收到了Get请求</returns>
        public static bool IsGet()
        {
            return HttpContext.Current.Request.HttpMethod.Equals("GET");
        }

        /// <summary>
        /// 返回指定的服务器变量信息
        /// </summary>
        /// <param name="strName">服务器变量名</param>
        /// <returns>服务器变量信息</returns>
        public static string GetServerString(string strName)
        {
            //
            if (HttpContext.Current.Request.ServerVariables[strName] == null)
            {
                return "";
            }
            return HttpContext.Current.Request.ServerVariables[strName].ToString();
        }

        /// <summary>
        /// 返回上一个页面的地址
        /// </summary>
        /// <returns>上一个页面的地址</returns>
        public static string GetUrlReferrer()
        {
            string retVal = null;

            try
            {
                retVal = HttpContext.Current.Request.UrlReferrer.ToString();
            }
            catch { }

            if (retVal == null)
                return "";

            return retVal;

        }

        /// <summary>
        /// 得到当前完整主机头
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentFullHost()
        {
            HttpRequest request = System.Web.HttpContext.Current.Request;
            if (!request.Url.IsDefaultPort)
            {
                return string.Format("{0}:{1}", request.Url.Host, request.Url.Port.ToString());
            }
            return request.Url.Host;
        }

        /// <summary>
        /// 得到主机头
        /// </summary>
        /// <returns></returns>
        public static string GetHost()
        {
            return HttpContext.Current.Request.Url.Host;
        }


        /// <summary>
        /// 获取当前请求的原始 URL(URL 中域信息之后的部分,包括查询字符串(如果存在))
        /// </summary>
        /// <returns>原始 URL</returns>
        public static string GetRawUrl()
        {
            return HttpContext.Current.Request.RawUrl;
        }

        /// <summary>
        /// 判断当前访问是否来自浏览器软件
        /// </summary>
        /// <returns>当前访问是否来自浏览器软件</returns>
        public static bool IsBrowserGet()
        {
            string[] BrowserName = { "ie", "opera", "netscape", "mozilla", "konqueror", "firefox" };
            string curBrowser = HttpContext.Current.Request.Browser.Type.ToLower();
            for (int i = 0; i < BrowserName.Length; i++)
            {
                if (curBrowser.IndexOf(BrowserName[i]) >= 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 判断是否来自搜索引擎链接
        /// </summary>
        /// <returns>是否来自搜索引擎链接</returns>
        public static bool IsSearchEnginesGet()
        {
            if (HttpContext.Current.Request.UrlReferrer == null)
            {
                return false;
            }
            string[] SearchEngine = { "google", "yahoo", "msn", "baidu", "sogou", "sohu", "sina", "163", "lycos", "tom", "yisou", "iask", "soso", "gougou", "zhongsou" };
            string tmpReferrer = HttpContext.Current.Request.UrlReferrer.ToString().ToLower();
            for (int i = 0; i < SearchEngine.Length; i++)
            {
                if (tmpReferrer.IndexOf(SearchEngine[i]) >= 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 获得当前完整Url地址
        /// </summary>
        /// <returns>当前完整Url地址</returns>
        public static string GetUrl()
        {
            return HttpContext.Current.Request.Url.ToString();
        }


        /// <summary>
        /// 获得指定Url参数的值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <returns>Url参数的值</returns>
        public static string GetQueryString(string strName)
        {
            if (HttpContext.Current.Request.QueryString[strName] == null)
            {
                return "";
            }
            return HttpContext.Current.Request.QueryString[strName];
        }

        /// <summary>
        /// 获得指定Url参数的值
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="defVal"></param>
        /// <returns></returns>
        public static short GetQueryShort(string strName, short defVal)
        {
            string queryParam = HttpContext.Current.Request.QueryString[strName];
            if (string.IsNullOrEmpty(queryParam))
            {
                return defVal;
            }

            return StrToShort(queryParam, defVal);
        }



        /// <summary>
        /// 获得当前页面的名称
        /// </summary>
        /// <returns>当前页面的名称</returns>
        public static string GetPageName()
        {
            string[] urlArr = HttpContext.Current.Request.Url.AbsolutePath.Split('/');
            return urlArr[urlArr.Length - 1].ToLower();
        }

        /// <summary>
        /// 返回表单或Url参数的总个数
        /// </summary>
        /// <returns></returns>
        public static int GetParamCount()
        {
            return HttpContext.Current.Request.Form.Count + HttpContext.Current.Request.QueryString.Count;
        }


        /// <summary>
        /// 获得指定表单参数的值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <returns>表单参数的值</returns>
        public static string GetMapPathString()
        {

            return HttpContext.Current.Server.MapPath("~");
        }

        /// <summary>
        /// 获得指定表单参数的值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <returns>表单参数的值</returns>
        public static string GetFormString(string strName)
        {
            if (HttpContext.Current.Request.Form[strName] == null)
            {
                return "";
            }
            return HttpContext.Current.Request.Form[strName];
        }
        /// <summary>
        /// 获得指定Post参数的值
        /// </summary>
        /// <param name="strName">Post参数</param>
        /// <returns>Post参数的值</returns>
        public static string GetParamsString(string strName)
        {
            if (HttpContext.Current.Request.Params[strName] == null)
            {
                return "";
            }
            return HttpContext.Current.Request.Params[strName];
        }
        /// <summary>
        /// 获得Url或表单参数的值, 先判断Url参数是否为空字符串, 如为True则返回表单参数的值
        /// </summary>
        /// <param name="strName">参数</param>
        /// <returns>Url或表单参数的值</returns>
        public static string GetString(string strName)
        {
            if ("".Equals(GetQueryString(strName)))
            {
                return GetFormString(strName);
            }
            else
            {
                return GetQueryString(strName);
            }
        }

        /// <summary>
        /// 获得指定Url参数的值 如果时间格式不对，返回的值 为1900-1-1
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <returns>Url参数的值</returns>
        public static DateTime GetQueryDate(string strName)
        {
            try
            {
                string QDate = GetString(strName);
                if (string.IsNullOrEmpty(strName))
                    return new DateTime(1900, 1, 1);
                else
                    return Convert.ToDateTime(QDate);
            }
            catch
            {
                return new DateTime(1900, 1, 1);
            }
        }

        /// <summary>
        /// 获得指定Url参数的值 如果时间格式不对，返回的值为null
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static DateTime? GetQueryNullableDateTime(string str)
        {
            string strVal = HttpContext.Current.Request.QueryString[str];
            DateTime time;
            if (string.IsNullOrEmpty(strVal))
                return null;

            if (DateTime.TryParse(strVal, out time))
                return time;

            return null;
        }

        /// <summary>
        /// 获得指定Url参数的int类型值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>Url参数的int类型值</returns>
        public static int GetQueryInt(string strName, int defValue)
        {

            return StrToInt(HttpContext.Current.Request.QueryString[strName], defValue);
        }
        /// <summary>
        /// 获得指定Url参数的int类型值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>Url参数的int类型值</returns>
        public static long GetQueryLong(string strName, long defValue)
        {

            return StrToLong(HttpContext.Current.Request.QueryString[strName], defValue);
        }

        /// <summary>
        /// 获得指定表单参数的int类型值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>表单参数的int类型值</returns>
        public static int GetFormInt(string strName, int defValue)
        {
            return StrToInt(HttpContext.Current.Request.Form[strName], defValue);

        }

        public static decimal GetFormDecimal(string strName, int defValue)
        {
            return StrToDecimal(HttpContext.Current.Request.Form[strName], defValue);
        }

        public static long GetFormLong(string strName, int defValue)
        {
            return StrToLong(HttpContext.Current.Request.Form[strName], defValue);
        }

        public static short GetFormShort(string strName, short defValue)
        {
            return StrToShort(HttpContext.Current.Request.Form[strName], defValue);
        }

        public static DateTime GetFormDateTime(string strName, DateTime defValue)
        {
            DateTime? time = StrToNullableDateTime(HttpContext.Current.Request.Form[strName]);
            if (time == null)
                return defValue;

            return time.Value;
        }
        public static DateTime GetFormDateTime(string strName, string defValue)
        {
            DateTime? time = StrToNullableDateTime(HttpContext.Current.Request.Form[strName]);
            if (time == null)
                return ExConvert.ToDateTime(defValue);

            return time.Value;
        }
        public static DateTime? GetFormNullableDateTime(string strName)
        {
            return StrToNullableDateTime(HttpContext.Current.Request.Form[strName]);
        }

        /// <summary>
        /// 获得指定表单参数的int类型值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>表单参数的int类型值</returns>
        public static int GetParamsInt(string strName, int defValue)
        {
            return StrToInt(HttpContext.Current.Request.Params[strName], defValue);
        }
        /// <summary>
        /// 获得指定表单参数的int类型值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>表单参数的int类型值</returns>
        public static short GetParamsShort(string strName, short defValue)
        {
            return StrToShort(HttpContext.Current.Request.Params[strName], defValue);
        }
        /// <summary>
        /// 获得指定Url或表单参数的int类型值, 先判断Url参数是否为缺省值, 如为True则返回表单参数的值
        /// </summary>
        /// <param name="strName">Url或表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>Url或表单参数的int类型值</returns>
        public static int GetInt(string strName, int defValue)
        {
            if (GetQueryInt(strName, defValue) == defValue)
            {
                return GetFormInt(strName, defValue);
            }
            else
            {
                return GetQueryInt(strName, defValue);
            }
        }

        /// <summary>
        /// 获得指定Url参数的float类型值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>Url参数的int类型值</returns>
        public static float GetQueryFloat(string strName, float defValue)
        {
            return StrToFloat(HttpContext.Current.Request.QueryString[strName], defValue);
        }


        /// <summary>
        /// 获得指定表单参数的float类型值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>表单参数的float类型值</returns>
        public static float GetFormFloat(string strName, float defValue)
        {
            return StrToFloat(HttpContext.Current.Request.Form[strName], defValue);
        }
        /// <summary>
        /// 获得指定表单参数的float类型值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>表单参数的float类型值</returns>
        public static float GetParamsFloat(string strName, float defValue)
        {
            return StrToFloat(HttpContext.Current.Request.Params[strName], defValue);
        }
        /// <summary>
        /// 获得指定Url或表单参数的float类型值, 先判断Url参数是否为缺省值, 如为True则返回表单参数的值
        /// </summary>
        /// <param name="strName">Url或表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>Url或表单参数的int类型值</returns>
        public static float GetFloat(string strName, float defValue)
        {
            if (GetQueryFloat(strName, defValue) == defValue)
            {
                return GetFormFloat(strName, defValue);
            }
            else
            {
                return GetQueryFloat(strName, defValue);
            }
        }
        /// <summary>
        /// 获得指定表单参数的float类型值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>表单参数的float类型值</returns>
        public static decimal GetParamsDecimal(string strName, decimal defValue)
        {
            return StrToDecimal(HttpContext.Current.Request.Params[strName], defValue);
        }

        /// <summary>
        /// 获得指定表单参数的DateTime类型值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>表单参数的float类型值</returns>
        public static DateTime GetParamsDateTime(string strName, DateTime defValue)
        {
            DateTime? time = StrToNullableDateTime(HttpContext.Current.Request.Params[strName]);
            if (time == null)
                return defValue;

            return time.Value;
        }
        public static DateTime? GetParamsNullableDateTime(string strName)
        {
            return StrToNullableDateTime(HttpContext.Current.Request.Params[strName]);
        }

        /// <summary>
        /// 获得当前页面客户端的IP
        /// </summary>
        /// <returns>当前页面客户端的IP</returns>
        public static string GetIP()
        {


            string result = String.Empty;

            result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (null == result || result == String.Empty)
            {
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            if (null == result || result == String.Empty)
            {
                result = HttpContext.Current.Request.UserHostAddress;
            }


            return result;

        }

        public static short StrToShort(object obj, short defaultValue)
        {
            if (obj == null)
            {
                return defaultValue;
            }
            try
            {
                return Convert.ToInt16(obj);
            }
            catch
            {
                return defaultValue;
            }
        }

        public static int StrToInt(object obj, int defaultValue)
        {
            if (obj == null)
            {
                return defaultValue;
            }
            try
            {
                return Convert.ToInt32(obj);
            }
            catch
            {
                return defaultValue;
            }
        }

        public static long StrToLong(object obj, long defaultValue)
        {
            if (obj == null)
            {
                return defaultValue;
            }
            try
            {
                return Convert.ToInt64(obj);
            }
            catch
            {
                return defaultValue;
            }
        }
        public static float StrToFloat(object obj, float defaultValue)
        {
            if (obj == null)
            {
                return defaultValue;
            }
            try
            {
                return float.Parse(obj.ToString());
            }
            catch
            {
                return defaultValue;
            }
        }
        public static decimal StrToDecimal(object obj, decimal defaultValue)
        {
            if (obj == null)
            {
                return defaultValue;
            }
            try
            {
                return decimal.Parse(obj.ToString());
            }
            catch
            {
                return defaultValue;
            }
        }
        public static DateTime? StrToNullableDateTime(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            try
            {
                return DateTime.Parse(obj.ToString());
            }
            catch
            {
                return null;
            }
        }

    }

}