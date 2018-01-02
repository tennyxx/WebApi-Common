using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Common.Util;
using WebApi.Common.Security.Interface;

namespace WebApi.Common.Security
{
    public class CommonEncryptApiSignature : ApiSignature
    {

        private Dictionary<string, object> _requestArguments;
        private Dictionary<string, object> _routeArguments;
        public CommonEncryptApiSignature(RequestDetail detail)
            : base(detail)
        {
            _requestArguments = detail.RequestArguments;
            _routeArguments = detail.RouteArguments;
        }

        public override ApiMessage IsValid()
        {
            ApiMessage message = new ApiMessage(true);
            if (
                string.IsNullOrEmpty(_requestArguments["timestamp"].ToString()) ||
                string.IsNullOrEmpty(_requestArguments["appkey"].ToString()))
            {
                message.Success = false;
                message.ErrorCode = (int)ApiMemberError.ArgumentError;
                message.ErrorMessage = "传入的参数有错误";
            }
            else
            {
                string standardSignature = "";
                #region CreateSignature
                List<string> paramKeys = _requestArguments.Select(item => item.Key).ToList();
                if (!_requestArguments.Keys.Contains("appsecret", StringComparer.OrdinalIgnoreCase))
                    paramKeys.Add("appsecret");

                paramKeys = paramKeys.Select(item => new
                {
                    Name = item,
                    Alias = _requestArguments.Keys.Contains(item, StringComparer.OrdinalIgnoreCase)
                }).OrderBy(item => item.Name).Select(item => item.Name).ToList();
                string hashString = string.Empty;
                for (int i = 0; i < paramKeys.Count; i++)
                {
                    string key = paramKeys[i];
                    dynamic currentArgValue = null;
                    if (key == "appsecret")
                    {
                        currentArgValue = _detail.ApiAuthorize.Signature;
                    }
                    else if ((_requestArguments.Keys as IEnumerable<string>).Contains(key, StringComparer.OrdinalIgnoreCase))
                    {
                        currentArgValue = _requestArguments[key];
                    }
                    hashString += currentArgValue;
                }

                switch ((Algorithm)_detail.ApiAuthorize.Algorithm)
                {
                    case Algorithm.MD5:
                        {
                            standardSignature = new ApiMD5().GetSignature(hashString);
                        }
                        break;
                    case Algorithm.RSA:
                        {
                            standardSignature = new ApiRSA(_detail.ApiRsaSecret.PrivateKey).GetSignature(hashString);
                        }
                        break;
                    case Algorithm.SHA1:
                        {
                            standardSignature = new ApiSHA1().GetSignature(hashString);
                        }
                        break;
                }
                #endregion


                if (!(_routeArguments.Keys as IEnumerable<string>).Contains("signature", StringComparer.OrdinalIgnoreCase) ||
                    _routeArguments["signature"].ToString() != standardSignature)
                {
                    message.Success = false;
                    message.ErrorCode = (int)ApiMemberError.InvalidSignature;
                    message.ErrorMessage = "签名校验失败";
                }
                //省略插入日志 
                //ApiAccessLogEntity entity = new ApiAccessLogEntity();
                //entity.AppKey = _requestArguments["appkey"].ToString();
                //entity.RequestParam = "传入签名：" + _routeArguments["signature"].ToString() + " 生成签名:" + standardSignature;
                //entity.ResponseParam = JsonHelper.ObjectToJson(_requestArguments);
                //entity.IpAddr = IPAddressUtil.GetClientIP();
                //entity.AccessUrl = _requestArguments["appkey"].ToString() + ":" + _requestArguments["timestamp"].ToString() + ":" + _routeArguments["signature"].ToString();
                //entity.CreateTime = DateTime.Now;
                //entity.IsActive = 1;
                //ApiAccessLogBll.GetInstance().Insert(entity);
            }

            return message;
        }
    }
}