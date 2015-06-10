using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using DealsWhat_Admin.Controllers;
using log4net;

namespace DealsWhat_Admin
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            log4net.Config.XmlConfigurator.Configure();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            //uncomment in order to bypass logging when running locally.
            //if (!Request.IsLocal)
            //{
            Exception ex = Server.GetLastError();
            if (ex is HttpUnhandledException && ex.InnerException != null)
            {
                ex = ex.InnerException;
            }

            if (ex != null)
            {
                var logger = LogManager.GetLogger(typeof(HomeController));

                logger.Error("error", ex);
                //Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.
                //   ExceptionPolicy.HandleException(ex, "AllExceptionsPolicy");
                //Server.ClearError();

                //Response.Redirect("~/Utility/ErrorPage.htm");
            }
            //}
        }
    }
}