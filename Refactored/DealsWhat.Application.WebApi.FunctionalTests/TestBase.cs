using System;
using System.Text;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Web.Configuration;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using DealsWhat.Application.WebApi.Controllers;
using DealsWhat.Domain.Interfaces;
using DealsWhat.Domain.Model;
using DealsWhat.Domain.Test.Common;
using DealsWhat.Infrastructure.DataAccess;
using Microsoft.Owin.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DealsWhat.Application.WebApi.FunctionalTests
{
    /// <summary>
    /// Summary description for TestSetup
    /// </summary>
    [TestClass]
    public class TestBase
    {
        private static IDisposable webApp;

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            var builder = new ContainerBuilder();

            var dealRepository = new FakeDealRepository(new List<Deal>());
            var dealCategoryRepository = new FakeDealCategoryRepository(new List<DealCategory>());

            builder.RegisterInstance<IRepositoryFactory>(new FakeRepositoryFactory(dealRepository, dealCategoryRepository));
            builder.RegisterApiControllers(typeof(DealsController).Assembly);

            var container = builder.Build();

            var resolver = new AutofacWebApiDependencyResolver(container);
            WebApiContext.DefaultResolver = resolver;

            var baseAddress = "http://localhost:9000/";

            webApp = WebApp.Start<Startup>(url: baseAddress);

        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            webApp.Dispose();
        }
    }
}
