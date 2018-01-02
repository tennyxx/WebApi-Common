using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Common.Security.Interface
{
    /// <summary>
    /// 签名校验基类
    /// </summary>
    public abstract class ApiSignature : IApiSecurity
    {
        protected RequestDetail _detail;
        public ApiSignature(RequestDetail detail)
        {
            _detail = detail;
        }

        /// <summary>
        /// 验证签名是否合法
        /// </summary>
        /// <returns></returns>
        public abstract ApiMessage IsValid();
    }
}