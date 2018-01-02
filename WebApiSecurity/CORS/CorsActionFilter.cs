using System.Web.Http.Filters;
namespace WebApi.Common
{
    public class CorsActionFilter:ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            //响应头添加Access-Control-Allow-Origin  *是全部允许，如果允许某个域名，则直接写域名
            actionExecutedContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            base.OnActionExecuted(actionExecutedContext);
        }
    }
}