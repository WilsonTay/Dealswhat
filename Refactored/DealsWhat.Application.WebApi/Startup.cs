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
using DealsWhat.Domain.Interfaces;
using DealsWhat.Infrastructure.DataAccess;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(DealsWhat.Application.WebApi.Startup))]

namespace DealsWhat.Application.WebApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            config.DependencyResolver = WebApiContext.DefaultResolver;

            config.Routes.MapHttpRoute(
                          name: "DefaultApi",
                          routeTemplate: "api/{controller}/{id}",
                          defaults: new { id = RouteParameter.Optional });

            app.UseWebApi(config);
        }

    }
}
