using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Http.SelfHost;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.WebApi;
using DealsWhat.Domain.Interfaces;
using DealsWhat.Infrastructure.DataAccess;
using Microsoft.Practices.Unity;
using Owin;
using Unity.WebApi;
using WebApplication1.Controllers;

namespace WebApplication1
{
    public class Startup
    {
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {
            HttpConfiguration config = new HttpConfiguration();

            var builder = new ContainerBuilder();
            builder.RegisterInstance<IRepositoryFactory>(new EFRepositoryFactory(new Model1()));
            //builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterApiControllers(typeof(DealsController).Assembly);



            var container = builder.Build();



            var resolver = new AutofacWebApiDependencyResolver(container);
            config.DependencyResolver = resolver;

            config.Routes.MapHttpRoute(
      name: "DefaultApi",
      routeTemplate: "api/{controller}/{id}",
       defaults: new { id = RouteParameter.Optional });

            appBuilder.UseWebApi(config);
        }
    }
}