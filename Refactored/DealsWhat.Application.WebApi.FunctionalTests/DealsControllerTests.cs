using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using DealsWhat.Models;
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
        private IFixture fixture;

        [TestInitialize]
        public void TestInitialize()
        {
            fixture = new Fixture().Customize(new AutoMoqCustomization());
        }


        private IList<DealModel> CreateSampleDeals()
        {
            var deals = new List<DealModel>();

            for (int i = 0; i < 100; i++)
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

        [TestMethod]
        public void GetDealByCategory_CaseInsensitive()
        {

        }

        [TestMethod]
        public void GetDealByCategory_AllFieldMatches()
        {
            var categoryName = "category1";
            var sampleDeals = CreateSampleDeals();
            var category = DealCategoryModel.Create(categoryName);

            sampleDeals.ToList().ForEach(d =>
            {
                category.AddDeal(d);
            });

            var categoryName2 = "category2";
            var sampleDeals2 = CreateSampleDeals();
            var category2 = DealCategoryModel.Create(categoryName);

            sampleDeals2.ToList().ForEach(d =>
            {
                category2.AddDeal(d);
            });

            var categories = new List<DealCategoryModel>();
            categories.Add(category);
            categories.Add(category2);

            var response = "";
            var baseEndpoints = new List<string>();
            baseEndpoints.Add("http://localhost:9000/api/frontenddeals?categoryid=");
            baseEndpoints.Add("http://localhost:9000/aPi/frontenddeals?caTegoryid=");
            baseEndpoints.Add("http://localhost:9000/api/FrontendDeals?categoryId=");
            baseEndpoints.Add("http://localhost:9000/api/FRONTENDDEALS?CATEGORYID=");
            baseEndpoints.Add("http://localhost:9000/API/FRONTENDDEALS?CATEGORYID=");

            // Loop thru each endpoint with different casing and
            // both categories to ensure deals are returned correctly.
            foreach (var baseEndpoint in baseEndpoints)
            {
                Action<DealCategoryModel, IList<DealModel>> assertByCategoryAction = (expectedCategory, expectedDeals) =>
                 {
                     var endpoint = baseEndpoint + expectedCategory.Key.ToString();

                     response = CreateWebApiServiceAndGetResponse(
                         new List<DealModel>(),
                         categories,
                         endpoint);

                     // To View Model
                     var deals = JsonConvert.DeserializeObject<IEnumerable<FrontEndDeal>>(response).ToList();

                     var expected = expectedDeals;

                     deals.Count.ShouldBeEquivalentTo(expected.Count);

                     foreach (var deal in deals)
                     {
                         var matchingDeal = expected.First(d => d.Key.ToString().Equals(deal.Id));
                         AssertDealEquality(deal, matchingDeal);
                     }
                 };

                assertByCategoryAction(category, sampleDeals);
                assertByCategoryAction(category2, sampleDeals2);
            }
        }


        [TestMethod]
        public void GetAllDeals_AllFieldMatches()
        {
            var sampleDeals = CreateSampleDeals();
            var sampleDealCategories = new List<DealCategoryModel>();

            var response = "";
            var baseEndpoints = new List<string>();
            baseEndpoints.Add("http://localhost:9000/api/frontenddeals");
            baseEndpoints.Add("http://localhost:9000/aPi/frontenddeals/");
            baseEndpoints.Add("http://localhost:9000/api/FrontendDeals");
            baseEndpoints.Add("http://localhost:9000/api/FRONTENDDEALS/");
            baseEndpoints.Add("http://localhost:9000/API/FRONTENDDEALS");

            foreach (var endpoint in baseEndpoints)
            {
                response = CreateWebApiServiceAndGetResponse(
                    sampleDeals,
                    sampleDealCategories,
                    endpoint);

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

        private static IDisposable CreateWebApiService(
           IList<DealModel> deals,
           IList<DealCategoryModel> dealCategories)
        {
            var dealRepository = new FakeDealRepository(deals);
            var dealCategoryRepository = new FakeDealCategoryRepository(dealCategories);

            var builder = new ContainerBuilder();

            builder.RegisterInstance<IRepositoryFactory>(new FakeRepositoryFactory(dealRepository, dealCategoryRepository));
            builder.RegisterApiControllers(typeof(FrontEndDealsController).Assembly);

            builder.RegisterType<DealService>().As<IDealService>();

            var container = builder.Build();

            var resolver = new AutofacWebApiDependencyResolver(container);
            WebApiContext.DefaultResolver = resolver;

            var baseAddress = "http://localhost:9000/";

            return WebApp.Start<Startup>(url: baseAddress);
        }

        private static string CreateWebApiServiceAndGetResponse(
            IList<DealModel> sampleDeals,
            IList<DealCategoryModel> sampleDealCategories,
            string endpoint)
        {
            var response = "";

            using (var service = CreateWebApiService(sampleDeals, sampleDealCategories))
            {
                HttpClient client = new HttpClient();

                var result = client.GetAsync(endpoint).Result;

                using (var reader = new StreamReader(result.Content.ReadAsStreamAsync().Result))
                {
                    response = reader.ReadToEnd();
                }
            }

            return response;
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
