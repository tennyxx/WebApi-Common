using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Common
{
    /// <summary>
    /// 异常类别
    /// </summary>
    public class ApiException : Exception
    {
        public int Code { get; set; }

        public ApiException()
            : base()
        {

        }
        public ApiException(int code, string message)
            : base(message)
        {
            Code = code;
        }
        public ApiException(int code, string message, Exception innerException)
            : base(message, innerException)
        {
            Code = code;
        }
    }
}