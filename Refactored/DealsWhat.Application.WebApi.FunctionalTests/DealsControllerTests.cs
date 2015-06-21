using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using Autofac;
using Autofac.Integration.WebApi;
using DealsWhat.Application.WebApi.Controllers;
using DealsWhat.Application.WebApi.Models;
using DealsWhat.Domain.Interfaces;
using DealsWhat.Domain.Model;
using DealsWhat.Domain.Services;
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

        private static IList<DealModel> sampleDeals;

        private static IList<DealCategoryModel> sampleDealCategories;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            fixture = new Fixture().Customize(new AutoMoqCustomization());

            // TODO: Make this immutable so tests cannot modify it.
            sampleDeals = GetSampleDeals();
            sampleDealCategories = GetSampleDealCategories();

            var builder = new ContainerBuilder();

            var dealRepository = new FakeDealRepository(sampleDeals);
            var dealCategoryRepository = new FakeDealCategoryRepository(sampleDealCategories);

            builder.RegisterInstance<IRepositoryFactory>(new FakeRepositoryFactory(dealRepository, dealCategoryRepository));
            builder.RegisterApiControllers(typeof(FrontEndDealsController).Assembly);

            builder.RegisterType<DealService>().As<IDealService>();

            var container = builder.Build();

            var resolver = new AutofacWebApiDependencyResolver(container);
            WebApiContext.DefaultResolver = resolver;

            var baseAddress = "http://localhost:9000/";

            webApp = WebApp.Start<Startup>(url: baseAddress); 
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            webApp.Dispose();
        }

        private static IList<DealModel> GetSampleDeals()
        {
            var deals = new List<DealModel>();

            for (int i = 0; i < 1; i++)
            {
                var deal = DealModel.Create(
                    fixture.Create<string>(),
                    fixture.Create<string>(),
                    fixture.Create<string>(),
                    fixture.Create<string>(),
                    fixture.Create<string>(),
                    fixture.Create<string>());

                deals.Add(deal);
            }

            return deals;
        }


        private static IList<DealCategoryModel> GetSampleDealCategories()
        {
            var dealCategories = new List<DealCategoryModel>();

            return dealCategories;
        }


        [TestMethod]
        public void GetAllDeals_AllFieldMatches()
        {
            HttpClient client = new HttpClient();

            var result = client.GetAsync("http://localhost:9000/api/frontenddeals/").Result;

            using (var reader = new StreamReader(result.Content.ReadAsStreamAsync().Result))
            {
                var response = reader.ReadToEnd();

                // To View Model
                var deals = JsonConvert.DeserializeObject<IEnumerable<FrontEndDeal>>(response).ToList();

                var expected = sampleDeals;

                deals.Count.ShouldBeEquivalentTo(expected.Count);

                foreach (var deal in deals)
                {
                    var matchingDeal = expected.First(d => d.Key.ToString().Equals(deal.Id));
                    AssertDealEquality(deal, matchingDeal);
                }
            }
        }

        private static void AssertDealEquality(FrontEndDeal deal, DealModel matchingDeal)
        {
            deal.ShortTitle.Should().BeEquivalentTo(matchingDeal.ShortTitle);
            deal.ShortDescription.Should().BeEquivalentTo(matchingDeal.ShortDescription);
            deal.RegularPrice.Should().BeEquivalentTo(matchingDeal.RegularPrice.ToString());
            deal.SpecialPrice.Should().BeEquivalentTo(matchingDeal.SpecialPrice.ToString());
            deal.CanonicalUrl.Should().BeEquivalentTo(matchingDeal.CanonicalUrl);
        }
    }
}
