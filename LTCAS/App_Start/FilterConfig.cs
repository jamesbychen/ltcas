using System.Web;
using System.Web.Mvc;
using Elmah;

namespace LTCAS
{
    //加入ELMAH自訂錯誤訊息
    public class ElmahHandledErrorLoggerFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.ExceptionHandled)
                ErrorSignal.FromCurrentContext().Raise(context.Exception);
        }
    }
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            // Elmah 必須置於第一順位
            filters.Add(new ElmahHandledErrorLoggerFilter());
            // 預設的 Exception Action Filter
            filters.Add(new HandleErrorAttribute());
        }
    }
}
