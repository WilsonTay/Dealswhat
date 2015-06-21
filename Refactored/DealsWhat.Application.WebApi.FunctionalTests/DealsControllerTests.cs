using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using Autofac;
using Autofac.Integration.WebApi;
using DealsWhat.Application.WebApi.Controllers;
using DealsWhat.Domain.Interfaces;
using DealsWhat.Domain.Model;
using DealsWhat.Domain.Test.Common;
using FluentAssertions;
using Microsoft.Owin.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace DealsWhat.Application.WebApi.FunctionalTests
{
    [TestClass]
    public class DealsControllerTests : TestBase
    {
        private static IFixture fixture;

        private static IDisposable webApp;

        private static IList<Deal> sampleDeals;

        private static IList<DealCategory> sampleDealCategories;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            fixture = new Fixture().Customize(new AutoMoqCustomization());

            sampleDeals = GetSampleDeals();
            sampleDealCategories = GetSampleDealCategories();

            var builder = new ContainerBuilder();

            var dealRepository = new FakeDealRepository(sampleDeals);
            var dealCategoryRepository = new FakeDealCategoryRepository(sampleDealCategories);

            builder.RegisterInstance<IRepositoryFactory>(new FakeRepositoryFactory(dealRepository, dealCategoryRepository));
            builder.RegisterApiControllers(typeof(DealsController).Assembly);

            var container = builder.Build();

            var resolver = new AutofacWebApiDependencyResolver(container);
            WebApiContext.DefaultResolver = resolver;

            var baseAddress = "http://localhost:9000/";

            webApp = WebApp.Start<Startup>(url: baseAddress); 
        }

        [ClassCleanup]
        public static void AssemblyCleanup()
        {
            webApp.Dispose();
        }

        private static IList<Deal> GetSampleDeals()
        {
            var deals = new List<Deal>();

            var mockedDeals = fixture.CreateMany<Deal>(100);
            mockedDeals.ToList().ForEach(d =>
            {
                deals.Add(d);
            });

            return deals;
        }


        private static IList<DealCategory> GetSampleDealCategories()
        {
            var dealCategories = new List<DealCategory>();

            return dealCategories;
        }


        [TestMethod]
        public void GetAllDeals_AllFieldMatches()
        {
            HttpClient client = new HttpClient();

            var result = client.GetAsync("http://localhost:9000/api/deals/").Result;

            using (var reader = new StreamReader(result.Content.ReadAsStreamAsync().Result))
            {
                var response = reader.ReadToEnd();
                var deals = JsonConvert.DeserializeObject<IList<Deal>>(response);

                var actual = GetSampleDeals();

                deals.Count.ShouldBeEquivalentTo(actual.Count);

                foreach (var sampleDeal in sampleDeals)
                {
                    deals.Should().Contain(sampleDeal);
                }
            }
        }
    }
}
