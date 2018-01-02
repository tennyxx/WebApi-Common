using System.Net;
using System.Web;

namespace WebApi.Common.Security
{
    public class IPAddressUtil
    {
        public static string GetIpAddress()
        {
            string hostname = Dns.GetHostName();
            IPHostEntry localhost = Dns.GetHostEntry(hostname);
            IPAddress localaddr = localhost.AddressList[0];
            return localaddr.ToString();
        }

        /// <summary>
        /// 获取客户机的ＩＰ
        /// </summary>
        /// <returns></returns>
        public static string GetClientIP()
        {
            string ip = "";
            if (HttpContext.Current != null)
            {
                HttpRequest Request = HttpContext.Current.Request;
                HttpServerUtility Server = HttpContext.Current.Server;

                string addr = "";
                try
                {
                    /*穿过代理服务器取远程用户真实IP地址：*/
                    if (Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                    {
                        addr = Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                    }
                    else
                    {
                        addr = Request.ServerVariables["REMOTE_ADDR"].ToString();
                    }
                }
                catch
                {
                    addr = Request.UserHostAddress;
                }

                if (addr == null)
                {
                    addr = "";
                }

                ip = addr;//.Split(',')[0];
            }
            return ip;
        }
    }
}