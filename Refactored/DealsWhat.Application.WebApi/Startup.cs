using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.WebApi;
using DealsWhat.Application.WebApi.Controllers;
using DealsWhat.Application.WebApi.Models;
using DealsWhat.Application.WebApi.Providers;
using DealsWhat.Domain.Interfaces;
using DealsWhat.Infrastructure.DataAccess;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;

[assembly: OwinStartup(typeof(DealsWhat.Application.WebApi.Startup))]

namespace DealsWhat.Application.WebApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            HttpConfiguration config = new HttpConfiguration();

            config.DependencyResolver = WebApiContext.DefaultResolver;
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                          name: "DefaultApi",
                          routeTemplate: "api/{controller}/{id}",
                          defaults: new { id = RouteParameter.Optional });

            app.UseWebApi(config);
        }

    }
}
