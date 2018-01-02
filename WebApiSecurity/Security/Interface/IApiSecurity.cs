using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Common.Security.Interface
{
    /// <summary>
    /// api安全校验接口
    /// </summary>
    public interface IApiSecurity
    {
        /// <summary>
        /// 验证签名是否合法
        /// </summary>
        /// <returns></returns>
        ApiMessage IsValid();
    }
}