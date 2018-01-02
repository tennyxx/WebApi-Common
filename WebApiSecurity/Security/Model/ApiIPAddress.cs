using WebApi.Common.Security.Interface;
using WebApi.Common.Util;

namespace WebApi.Common.Security
{
    /// <summary>
    /// 签名校验类
    /// </summary>
    public class ApiIPAddress : IApiSecurity
    {
        protected SecurityConfiguration _security;
        public RequestDetail _detail;

        public ApiIPAddress(RequestDetail detail)
        {
            _detail = detail;
        }

        /// <summary>
        /// 验证签名是否合法
        /// </summary>
        /// <returns></returns>
        public ApiMessage IsValid()
        {
            var result = new ApiMessage(true);

            if (_detail.ApiAuthorize.CheckIPAddress == 0)
                return result;

            bool isPass = false;
            string ipaddress = IPAddressUtil.GetClientIP();
            if (!string.IsNullOrEmpty(ipaddress) && ipaddress.Split('.').Length == 4)
            {
                var longIp = IpHelper.IP2Long(ipaddress);
                foreach (var ip in _detail.AllowedIPAddress)
                {
                    if (ip.IpAddrStart <= longIp && longIp <= ip.IpAddrEnd)
                    {
                        isPass = true;
                        break;
                    }
                }

                if (isPass)
                    return result;
            }

            result.Success = false;
            result.ErrorMessage = string.Format("当前IP地址 {0} 被限制访问", ipaddress);
            result.ErrorCode = (int)ApiMemberError.ForbiddenIPAddress;

            return result;
        }
    }
}