using System.Collections.Generic;
using WebApi.Common.Security;

namespace WebApi.Common.Security
{
    public class RequestDetail
    {
        /// <summary>
        /// 授权信息
        /// </summary>
        public ApiAuthorizeEntity ApiAuthorize { get; set; }
        /// <summary>
        /// RSA密钥配置信息
        /// </summary>
        public ApiRsaSecretEntity ApiRsaSecret { get; set; }
        /// <summary>
        /// 允许访问的IP地址
        /// </summary>
        public IList<ApiWhiteListEntity> AllowedIPAddress { get; set; }
        /// <summary>
        /// 请求中携带的参数
        /// </summary>
        public Dictionary<string, object> RequestArguments { get; set; }
        /// <summary>
        /// 路由方法的参数
        /// </summary>
        public Dictionary<string, object> RouteArguments { get; set; }
    }

    /// <summary>
    /// 加密方式
    /// </summary>
    public enum Algorithm
    {
        MD5 = 1,
        RSA = 2,
        SHA1 = 3
    }
}