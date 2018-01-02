
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using WebApi.Common.Security.Interface;
using WebApi.Common.Util;

namespace WebApi.Common.Security
{
    public class SecurityActionFilter:ActionFilterAttribute
    {
        //验证签名
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var result = new ApiMessage();
            var requestParam = HttpContext.Current.Request;
            //获取url传递的appkey、签名、时间戳
            var appkey1 = RequestUtility.GetQueryString("appkey");
            var signature = RequestUtility.GetQueryString("signature");
            var timestamp = RequestUtility.GetQueryString("timestamp");
            //如果参数列表不存在 appkey,签名，时间戳，则add
            if (!actionContext.ActionArguments.ContainsKey("appkey"))
                actionContext.ActionArguments.Add("appkey", appkey1);
            if (!actionContext.ActionArguments.ContainsKey("signature"))
                actionContext.ActionArguments.Add("signature", signature);
            if (!actionContext.ActionArguments.ContainsKey("timestamp"))
                actionContext.ActionArguments.Add("timestamp", timestamp);

            #region 验证签名
            //判断参数列表不存在appkey，则直接result赋值错误信息
            if (!actionContext.ActionArguments.ContainsKey("appkey"))
            {
                result.Success = false;
                result.ErrorCode = (int)ApiMemberError.InvalidArgument;
                result.ErrorMessage = "Action激活失败";
            }else
            {
                RequestDetail detail = new RequestDetail();
                string appkey = RequestUtility.GetQueryString("appkey");
                detail.RouteArguments = new Dictionary<string, object>();
                detail.RequestArguments = new Dictionary<string, object>();
                detail.RequestArguments.Add("appkey", appkey);
                detail.RequestArguments.Add("timestamp", RequestUtility.GetQueryString("timestamp"));
                //把post携带的参数放进RequestArguments
                for (int i = 0; i < requestParam.Form.Count; i++)
                {
                    detail.RequestArguments.Add(requestParam.Form.Keys[i].ToLower(), requestParam.Form.Get(i));
                }
                //把post请求携带的json类参数放入RequestArguments
                if (requestParam.ContentType.Contains("application/json"))
                {
                    var sr = new StreamReader(requestParam.InputStream);
                    var stream = sr.ReadToEnd();
                    var dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(stream);
                    if (dic != null)
                    {
                        foreach (var item in dic)
                        {
                            detail.RequestArguments.Add(item.Key, item.Value);
                        }
                    }

                }
                //url携带的get参数
                for (int i = 0; i < requestParam.QueryString.Count; i++)
                {
                    detail.RouteArguments.Add(requestParam.QueryString.Keys[i].ToLower(), requestParam.QueryString.Get(i));
                }
                //省略从库里取授权信息、密钥信息和允许访问的ip
                //detail.ApiAuthorize = ApiAuthorizeBll.GetInstance().GetApiAuthorizeByKey(appkey);
                //detail.ApiRsaSecret = ApiRsaSecretBll.GetInstance().GetApiRsaSecretByKey(appkey);
                //detail.AllowedIPAddress = ApiWhiteListBll.GetInstance().GetApiWhiteListByAppkey(appkey);
                if (detail.ApiAuthorize != null)
                {
                    if (detail.ApiAuthorize.CheckIPAddress != 0)
                    {
                        IApiSecurity ipAddress = new ApiIPAddress(detail);
                        result = ipAddress.IsValid();
                    }

                    if (result.Success && detail.ApiAuthorize.Algorithm != 0)
                    {
                        IApiSecurity security = new CommonEncryptApiSignature(detail);
                        result = security.IsValid();
                    }
                }
                else
                {
                    result.Success = false;
                    result.ErrorCode = (int)ApiMemberError.ArgumentError;
                    result.ErrorMessage = "签名校验失败!";
                }
            }
            #endregion

            if (!result.Success)
            {
                var response = new HttpResponseMessage();
                response.StatusCode = HttpStatusCode.OK;
                response.Content = new StringContent(JsonHelper.ObjectToJson(result));
                //在这里为了不继续走流程，要throw出来，才会立马返回到客户端
                throw new HttpResponseException(response);
            }
        }
    }
}