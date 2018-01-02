using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using WebApi.Common;

namespace WebApi.Common.Security
{
    [DataContract]
    [Serializable]
    public class ApiMessage
    {
        /// <summary>
        /// 
        /// </summary>
        public ApiMessage()
            : this(true)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="success"></param>
        public ApiMessage(bool success)
        {
            Success = success;
            ErrorCode = success ? 0 : -1;
        }
        public ApiMessage(Exception ex)
        {
            int code = (int)ApiMemberError.UnkownError;
            string message = ex.Message;
            string detail = string.Empty;
            bool foreseenFounded = false;

            int count = 0;

            while (ex != null && count < 5)
            {
                if (foreseenFounded)
                {
                    detail += ex.Message + "\r\n" + ex.StackTrace + "\r\n";
                }
                else
                {
                    if (ex is ApiException)
                    {
                        var exception = (ex as ApiException);
                        code = exception.Code;
                        message = ex.Message;
                        detail = ex.Message + "\r\n" + ex.StackTrace + "\r\n";

                        foreseenFounded = true;
                    }
                    else
                    {
                        detail += ex.Message + "\r\n" + ex.StackTrace + "\r\n";
                    }
                }

                ex = ex.InnerException;
                count++;
            }

            ErrorCode = code;
            ErrorMessage = message;
            ErrorDetail = detail;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="ex"></param>
        public ApiMessage(int code, string message)
        {
            ErrorCode = code;
            ErrorMessage = message;
        }

        /// <summary>
        /// 执行是否成功
        /// </summary>
        [DataMember]
        public bool Success { get; set; }
        /// <summary>
        /// 返回值泛型对象
        /// </summary>
        [DataMember]
        public dynamic Data { get; set; }
        /// <summary>
        /// 错误码:-1系统错误，0无错误，其他值表示自定义错误
        /// </summary>
        [DataMember]
        public int ErrorCode { get; set; }
        /// <summary>
        /// 错误描述信息
        /// </summary>
        [DataMember]
        public string ErrorMessage { get; set; }
        /// <summary>
        /// 错误堆栈信息
        /// </summary>
        [DataMember]
        public string ErrorDetail { get; set; }
        [JsonIgnore]
        public bool ErrorCodeFormated = false;
    }

    public enum ApiMemberError
    {
        [Description("系统错误")]
        SystemError = -1,
        [Description("无效的IP地址")]
        ForbiddenIPAddress = -101,
        [Description("无效的签名")]
        InvalidSignature = -102,
        [Description("参数错误")]
        ArgumentError = -203,
        [Description("无效的参数")]
        InvalidArgument = -204,
        [Description("未知的参数")]
        UnkownError = -999
    }
}