using Elmah;
using LTCAS.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace LTCAS
{

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }    /// <summary>

        /// Handles the Error event of the Application control.

        /// </summary>

        /// <param name="sender">The source of the event.</param>

        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>

        protected void Application_Error(object sender, EventArgs e)

        {

            var app = (MvcApplication)sender;

            var context = app.Context;

            var ex = app.Server.GetLastError();

            context.Response.Clear();

            context.ClearError();

            var httpException = ex as HttpException;

            var routeData = new RouteData();

            routeData.Values["controller"] = "Error";

            routeData.Values["exception"] = ex;

            routeData.Values["action"] = "Index";

            if (httpException != null)

            {

                switch (httpException.GetHttpCode())

                {

                    case 403:

                        routeData.Values["action"] = "Forbidden";

                        break;

                    case 404:

                        routeData.Values["action"] = "PageNotFound";

                        break;

                    case 500:

                        routeData.Values["action"] = "InternalError";

                        break;

                    default:

                        routeData.Values["action"] = "GenericError";

                        break;

                }

            }

            // Pass exception details to the target error View.

            routeData.Values.Add("Error", ex.Message);

            // Avoid IIS7 getting in the middle

            context.Response.TrySkipIisCustomErrors = true;

            IController controller = new ErrorController();

            controller.Execute(new RequestContext(new HttpContextWrapper(context), routeData));

        }

        //發送郵件篩選條件
        public void ErrorMail_Filtering(object sender, ExceptionFilterEventArgs e)

        {

            var httpException = e.Exception as HttpException;
            //如果404錯誤（找不到網頁）則忽略
            if (httpException != null && httpException.GetHttpCode() == 404)
            {
                e.Dismiss();
            }

        }
    }
}
