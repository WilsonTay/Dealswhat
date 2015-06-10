using DealsWhat.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using DealsWhat.Controllers;
using log4net;
using WebMatrix.WebData;

namespace DealsWhat
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {

        public MvcApplication()
        {
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

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            Database.SetInitializer<DealsContext>(null);

            try
            {
                using (var context = new DealsContext())
                {
                    if (!context.Database.Exists())
                    {
                        // Create the SimpleMembership database without Entity Framework migration schema
                        ((IObjectContextAdapter)context).ObjectContext.CreateDatabase();
                        SampleData.Seed(context);
                    }
                }

                WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new InvalidOperationException("The ASP.NET Simple Membership database could not be initialized. For more information, please see http://go.microsoft.com/fwlink/?LinkId=256588", ex);
            }
        }
    }
}