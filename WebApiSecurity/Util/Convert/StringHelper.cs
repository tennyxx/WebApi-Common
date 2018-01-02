using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using System.Linq;

namespace WebApi.Common.Util
{
    public class StringUtility
    {
        #region 过滤SQL注入
        public static string SqlReplace(string source)
        {
            source.Replace("'", "''");
            return source;
        }
        #endregion

        #region 获取指定分隔符的字符串数据

        /// <summary>
        /// 获取去掉指定分隔符的字符串数组
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <param name="split">分隔符</param>
        /// <returns>String[]</returns>
        public static string[] Split(string source, char split)
        {
            if (source != null && source != "")
            {
                if (source.Substring(source.Length - 1, 1) == split.ToString())
                    source = source.Substring(0, source.Length - 1);
                if (source.Substring(0, 1) == split.ToString())
                {
                    source = source.Substring(1);
                }
            }
            string[] result = null;
            result = source.Split(split);
            return result;
        }

        #endregion

        #region 将文本转义为Html文本
        /// <summary>
        /// 将文本转义为Html文本
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string Conver2Html(string source)
        {
            source = HttpContext.Current.Server.HtmlEncode(source);
            source = source.Replace("\r\n", "<br/>");
            source = source.Replace("\n", "<br/>");
            return source;
        }
        #endregion

        #region 将Html文本取消转义

        /// <summary>
        /// 将Html文本取消转义
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string DisConverHtml(string source)
        {
            if (source != null)
            {
                source = HttpContext.Current.Server.HtmlDecode(source);
                source = source.Replace("<br/>", "\r\n");
                source = source.Replace("<br/>", "\n");
            }
            return source;
        }

        #endregion

        #region 取小数点精度为2的字符串
        /// <summary>
        /// 取小数点精度为2的字符串
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string Cut2Bit(string source)
        {
            try
            {
                double value = double.Parse(source);
                source = Round(value, 2).ToString();
                if (source.IndexOf(".") == -1)
                {
                    source += ".00";
                }
                return source;
            }
            catch { }
            return string.Empty;
        }
        #endregion

        #region 取出指定精度的数值(四舍五入)
        public static double Round(double value, int decimals)
        {
            if (value < 0)
            {
                return Math.Round(value + 5 / Math.Pow(10, decimals + 1), decimals, MidpointRounding.AwayFromZero);
            }
            else { return Math.Round(value, decimals, MidpointRounding.AwayFromZero); }
        }
        #endregion

        #region 获取指定长度的字符串
        public static string CutString(string source, int len, bool isAddDot)
        {
            if (string.IsNullOrEmpty(source))
                return string.Empty;

            if (source.Length <= len)
            {
                return source;
            }
            string strTemp = "";
            int intTemp = 0;
            int intPos = 0;
            while ((intTemp < len * 2) && (intPos < source.Length))
            {
                if (((int)source[intPos]) < 128)
                {
                    //单字节
                    strTemp += source[intPos].ToString();
                    intTemp++;
                }
                else if (intTemp == (len * 2 - 1))
                {
                    //双字节
                    break;
                }
                else
                {
                    strTemp += source[intPos].ToString();
                    intTemp += 2;
                }

                intPos++;
            }
            if (isAddDot)
            {
                strTemp += "...";
            }
            return strTemp;
        }
        #endregion

        #region 清除HTML格式
        public static string CutHtmlFormat(string strhtml)
        {
            #region
            //删除脚本

            strhtml = System.Text.RegularExpressions.Regex.Replace(strhtml, @"<script[^>]*?>.*?</script>", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            //删除HTML

            strhtml = System.Text.RegularExpressions.Regex.Replace(strhtml, @"<(.[^>]*)>", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            strhtml = System.Text.RegularExpressions.Regex.Replace(strhtml, @"([\r\n])[\s]+", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            strhtml = System.Text.RegularExpressions.Regex.Replace(strhtml, @"-->", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            strhtml = System.Text.RegularExpressions.Regex.Replace(strhtml, @"<!--.*", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            //strhtml =System.Text.RegularExpressions. Regex.Replace(strhtml,@"<A>.*</A>","");

            //strhtml =System.Text.RegularExpressions. Regex.Replace(strhtml,@"<[a-zA-Z]*=\.[a-zA-Z]*\?[a-zA-Z]+=\d&\w=%[a-zA-Z]*|[A-Z0-9]","");



            strhtml = System.Text.RegularExpressions.Regex.Replace(strhtml, @"&(quot|#34);", "\"", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            strhtml = System.Text.RegularExpressions.Regex.Replace(strhtml, @"&(amp|#38);", "&", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            strhtml = System.Text.RegularExpressions.Regex.Replace(strhtml, @"&(lt|#60);", "<", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            strhtml = System.Text.RegularExpressions.Regex.Replace(strhtml, @"&(gt|#62);", ">", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            strhtml = System.Text.RegularExpressions.Regex.Replace(strhtml, @"&(nbsp|#160);", " ", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            strhtml = System.Text.RegularExpressions.Regex.Replace(strhtml, @"&(iexcl|#161);", "\xa1", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            strhtml = System.Text.RegularExpressions.Regex.Replace(strhtml, @"&(cent|#162);", "\xa2", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            strhtml = System.Text.RegularExpressions.Regex.Replace(strhtml, @"&(pound|#163);", "\xa3", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            strhtml = System.Text.RegularExpressions.Regex.Replace(strhtml, @"&(copy|#169);", "\xa9", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            strhtml = System.Text.RegularExpressions.Regex.Replace(strhtml, @"&#(\d+);", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);


            strhtml.Replace("<", "");

            strhtml.Replace(">", "");

            strhtml.Replace("\r\n", "");
            strhtml.Replace(" ", "");
            strhtml.Trim();

            //strhtml=HttpContext.Current.Server.HtmlEncode(strhtml).Trim();
            #endregion
            return strhtml;

        }


        public static string CutHref(string strHtml, string href, string text)
        {
            if (href != "")
            {
                Regex reg = new Regex(@"(?is)<a(?:(?!href=).)*href=(['""]?)(?<url>[^""\s>]*)\1[^>]*>(?<text>(?:(?!</?a\b).)*)</a>");
                MatchCollection mc = reg.Matches(strHtml);
                string strUrl = "";
                string strTitle = "";
                foreach (Match m in mc)
                {
                    strUrl = m.Groups["url"].Value;
                    strTitle = m.Groups["text"].Value;
                    if (href != "")
                        strHtml = strHtml.Replace(strUrl, href);

                    if (text != "")
                        strHtml = strHtml.Replace(strTitle, text);
                }

            }
            else
            {
                strHtml = System.Text.RegularExpressions.Regex.Replace(strHtml, @"(?is)<a(?:(?!href=).)*href=(['""]?)(?<url>[^""\s>]*)\1[^>]*>(?<text>(?:(?!</?a\b).)*)</a>", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            }
            return strHtml;
        }

        /// <summary>
        /// 过滤html
        /// </summary>
        /// <param name="strConnect">要过滤的内容</param>
        /// <param name="flg">【1：过滤脚本】</param>
        /// <returns>过滤后的值</returns>
        public static string strFormatHrefAndJavaScript(string strConnect, int flg)
        {
            string str = "";
            //string strRegex = "href=\'[^\"]*\'";
            string strRegex = "href=\"[^\"]*\"";
            string strRegexT = "target=\"[^\"]*\"";
            //string strRegexC = "onclick=\"[^\"]*\"";
            //string strRegexMV = "onmouseover=\"[^\"]*\"";
            //string strRegexMU = "onmouseout=\"[^\"]*\"";
            string strRegexLK = "<link .*?[^>]*>";

            Regex reg = new Regex(strRegex);
            Regex reg1 = new Regex(strRegexT);
            //Regex reg2 = new Regex(strRegexC);
            //Regex reg3 = new Regex(strRegexMV);
            //Regex reg4 = new Regex(strRegexMU);
            Regex reg5 = new Regex(strRegexLK);
            strConnect = reg1.Replace(strConnect, "");
            //strConnect = reg2.Replace(strConnect, "");
            //strConnect = reg3.Replace(strConnect, "");
            //strConnect = reg4.Replace(strConnect, "");
            //strConnect = reg5.Replace(strConnect, "");
            //strConnect = strConnect.Replace("'", "\"");
            str = reg.Replace(strConnect, "href=\"###\"");
            return str;
        }

        public static string RemoveSpecifyHtml(string ctx)
        {

            string[] holdTags = { "img", "br", "strong", "b", "span", "li", "p" };//保留的 tag  

            // <(?!((/?\s?li\b)|(/?\s?ul\b)|(/?\s?a\b)|(/?\s?img\b)|(/?\s?br\b)|(/?\s?span\b)|(/?\s?b\b)))[^>]+>  

            string regStr = string.Format(@"<(?!((/?\s?{0})))[^>]+>", string.Join(@"\b)|(/?\s?", holdTags));

            Regex reg = new Regex(regStr, RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase);

            return reg.Replace(ctx, "");

        }

        #endregion


        #region 去掉空格、回车、指定的符号
        public static string strReplace(string source, string[] flag)
        {

            source = source.Trim();
            source = source.Replace("\n", "");
            source = source.Replace("\r", "");
            for (int i = 0; i < flag.Length; i++)
            {
                source = source.Replace(flag[i], "");

            }

            return source;
        }
        #endregion

        #region 截掉字符串最后指定的字符
        public static string CutLastFlag(string source, string flag)
        {

            if (source.LastIndexOf(flag) == source.Length - flag.Length)
            {
                source = source.Substring(0, source.Length - flag.Length);
            }
            return source;
        }
        #endregion

        #region 判断字符串是否为数字
        public static bool IsNumeric(string value)
        {
            return Regex.IsMatch(value, @"^[+-]?\d*[.]?\d*$");
        }
        #endregion

        #region 判断字符串是否为整型数字
        public static bool IsInt(string value)
        {
            return Regex.IsMatch(value, @"^[+-]?\d*$");
        }
        #endregion

        #region 判断字符串是否为无符号数字
        public static bool IsUnsign(string value)
        {
            return Regex.IsMatch(value, @"^\d*[.]?\d*$");
        }
        #endregion

        #region 生成随机数
        //  private static char[] constant =   
        //{   
        //  '2','3','4','5','6','7','8','9',  
        //  'a','b','c','d','e','f','g','h','i','j','k','m','n','p','q','r','s','t','u','v','w','x','y','z',   
        //  'A','B','C','D','E','F','G','H','J','K','M','N','P','R','S','T','U','V','W','X','Y','Z'   
        //};
        //  public static string GenerateRandomNumber(int Length)
        //  {
        //      int intlength = constant.Length-1;
        //      System.Text.StringBuilder newRandom = new System.Text.StringBuilder(intlength);
        //      Random rd = new Random();
        //      for (int i = 0; i < Length; i++)
        //      {
        //          newRandom.Append(constant[rd.Next(intlength)]);
        //      }
        //      return newRandom.ToString();
        //  }

        private static char[] constant =
      {
        '0','1','2','3','4','5','6','7','8','9'
      };
        public static string GenerateRandomNumber(int Length)
        {
            int intlength = constant.Length - 1;
            System.Text.StringBuilder newRandom = new System.Text.StringBuilder(intlength);
            Random rd = new Random();
            for (int i = 0; i < Length; i++)
            {
                newRandom.Append(constant[rd.Next(intlength)]);
            }
            return newRandom.ToString();
        }
        #endregion

        #region 分割字符串
        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="value"></param>
        /// <param name="splitStr"></param>
        /// <returns></returns>
        public static IList<String> SplistString(string value, string splitStr, StringSplitOptions splitOpt = StringSplitOptions.RemoveEmptyEntries)
        {
            var list = new List<string>();
            if (!string.IsNullOrEmpty(value))
            {
                return value.Split(new[] { splitStr }, StringSplitOptions.RemoveEmptyEntries).ToList();
            }

            return list;
        }
        #endregion


    }
}
