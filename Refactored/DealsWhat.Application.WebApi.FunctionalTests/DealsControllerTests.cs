using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
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
                var deal = TestModelFactory.CreateDeal();

                deals.Add(deal);
            }

            return deals;
        }

        [TestMethod]
        public void GetSingleDealById_AllFieldMatches()
        {
            var deals = CreateSampleDeals();
            var matchingDeal = TestModelFactory.CreateCompleteDeal();
            deals.Add(matchingDeal);

            var baseEndpoints = new List<string>();
            baseEndpoints.Add("http://localhost:9000/api/deal?id=");
            baseEndpoints.Add("http://localhost:9000/Api/dEal?iD=");
            baseEndpoints.Add("http://Localhost:9000/api/DEAL?Id=");
            baseEndpoints.Add("http://LOCALHOST:9000/API/DEAL/?ID=");
            baseEndpoints.Add("http://LOCALHOST:9000/API/deal/?ID=");

            foreach (var baseEndpoint in baseEndpoints)
            {
                var endpoint = baseEndpoint + matchingDeal.Key.ToString();

                var response = CreateWebApiServiceAndGetResponse(
                    deals,
                    new List<DealCategoryModel>(),
                    endpoint);

                var actualDeal = JsonConvert.DeserializeObject<FrontEndSpecificDeal>(response);

                AssertFrontEndSpecificDealEquality(actualDeal, matchingDeal);
            }
        }

        [TestMethod]
        public void GetSingleDealByCanonicalUrl_AllFieldMatches()
        {
            var deals = CreateSampleDeals();
            var matchingDeal = TestModelFactory.CreateCompleteDeal();
            deals.Add(matchingDeal);

            var baseEndpoints = new List<string>();
            baseEndpoints.Add("http://localhost:9000/api/deal?url=");
            baseEndpoints.Add("http://localhost:9000/Api/dEal?url=");
            baseEndpoints.Add("http://Localhost:9000/api/DEAL?urL=");
            baseEndpoints.Add("http://LOCALHOST:9000/API/DEAL?URL=");
            baseEndpoints.Add("http://localhost:9000/Api/dEal/?url=");

            foreach (var baseEndpoint in baseEndpoints)
            {
                var endpoint = baseEndpoint + matchingDeal.CanonicalUrl;

                var response = CreateWebApiServiceAndGetResponse(
                    deals,
                    new List<DealCategoryModel>(),
                    endpoint);

                var actualDeal = JsonConvert.DeserializeObject<FrontEndSpecificDeal>(response);

                AssertFrontEndSpecificDealEquality(actualDeal, matchingDeal);
            }
        }

        [TestMethod]
        public void GetSingleDeal_DealOptionMatches()
        {
            var deals = CreateSampleDeals();
            var matchingDeal = TestModelFactory.CreateCompleteDeal();

            for (int i = 0; i < 10; i++)
            {
                var option = TestModelFactory.CreateDealOptionWithAttributes();
                matchingDeal.AddOption(option);
            }

            deals.Add(matchingDeal);

            var endpoint = "http://localhost:9000/api/deal?url=" + matchingDeal.CanonicalUrl;

            var response = CreateWebApiServiceAndGetResponse(
                deals,
                new List<DealCategoryModel>(),
                endpoint);

            var actualDeal = JsonConvert.DeserializeObject<FrontEndSpecificDeal>(response);

            AssertFrontEndSpecificDealEquality(actualDeal, matchingDeal);
        }

        [TestMethod]
        public void GetSingleDeal_ImageUrlCorrectlyConstructed()
        {
            var baseImageUrl = "http://www.dealswhat.com/dealimages/";

            ConfigurationManager.AppSettings["DealImageBaseUrl"] = baseImageUrl;

            var deals = CreateSampleDeals();
            var matchingDeal = TestModelFactory.CreateCompleteDeal();

            for (int i = 0; i < 10; i++)
            {
                var option = TestModelFactory.CreateDealOptionWithAttributes();
                matchingDeal.AddOption(option);
            }

            deals.Add(matchingDeal);

            var endpoint = "http://localhost:9000/api/deal?url=" + matchingDeal.CanonicalUrl;

            var response = CreateWebApiServiceAndGetResponse(
                deals,
                new List<DealCategoryModel>(),
                endpoint);

            var actualDeal = JsonConvert.DeserializeObject<FrontEndSpecificDeal>(response);

            var images = actualDeal.ImageUrls;

            images.Should().NotBeEmpty();

            foreach (var image in images)
            {
                Assert.IsTrue(image.StartsWith(baseImageUrl));
            }
        }

        [TestMethod]
        public void GetDeals_ThumbnailsCorrectlyLoaded()
        {
            var sampleDeals = CreateSampleDeals();

            foreach (var deal in sampleDeals)
            {
                var sampleImages =
                    Enumerable.Range(0, 10).ToList().Select(a => TestModelFactory.CreateDealImage(order: a)).ToList();

                foreach (var img in sampleImages)
                {
                    deal.AddImage(img);
                }
            }

            var endpoint = "http://localhost:9000/api/deals";

            var response = CreateWebApiServiceAndGetResponse(
                sampleDeals,
                new List<DealCategoryModel>(),
                endpoint);

            var deals = JsonConvert.DeserializeObject<IEnumerable<FrontEndDeal>>(response).ToList();

            foreach (var deal in deals)
            {
                var matchingDeal = sampleDeals.First(d => d.Key.ToString().Equals(deal.Id));
                AssertFrontEndDealEquality(deal, matchingDeal);
            }

        }

        [TestMethod]
        public void GetDeals_ThumbnailUrlsCorrectlyConstructed()
        {
            var baseImageUrl = "http://www.dealswhat.com/dealimages/";

            ConfigurationManager.AppSettings["DealImageBaseUrl"] = baseImageUrl;

            var sampleDeals = CreateSampleDeals();

            foreach (var deal in sampleDeals)
            {
                var sampleImages =
                    Enumerable.Range(0, 10).ToList().Select(a => TestModelFactory.CreateDealImage(order: a)).ToList();

                foreach (var img in sampleImages)
                {
                    deal.AddImage(img);
                }
            }

            var endpoint = "http://localhost:9000/api/deals";

            var response = CreateWebApiServiceAndGetResponse(
                sampleDeals,
                new List<DealCategoryModel>(),
                endpoint);

            var deals = JsonConvert.DeserializeObject<IEnumerable<FrontEndDeal>>(response).ToList();

            foreach (var deal in deals)
            {
                var matchingDeal = sampleDeals.First(d => d.Key.ToString().Equals(deal.Id));
                AssertFrontEndDealEquality(deal, matchingDeal);

                foreach (var url in deal.ThumbnailUrls)
                {
                    Assert.IsTrue(url.StartsWith(baseImageUrl));
                }
            }
        }

        [TestMethod]
        public void GetDealByCategory_AllFieldMatches()
        {
            var categoryName = "category1";
            var sampleDeals = CreateSampleDeals();
            var category = TestModelFactory.CreateDealCategory(name: categoryName);

            sampleDeals.ToList().ForEach(d =>
            {
                category.AddDeal(d);
            });

            var categoryName2 = "category2";
            var sampleDeals2 = CreateSampleDeals();
            var category2 = TestModelFactory.CreateDealCategory(name: categoryName2);

            sampleDeals2.ToList().ForEach(d =>
            {
                category2.AddDeal(d);
            });

            var categories = new List<DealCategoryModel>();
            categories.Add(category);
            categories.Add(category2);

            var response = "";
            var baseEndpoints = new List<string>();
            baseEndpoints.Add("http://localhost:9000/api/deals?categoryid=");
            baseEndpoints.Add("http://localhost:9000/aPi/Deals?caTegoryid=");
            baseEndpoints.Add("http://localhost:9000/api/Deals?categoryId=");
            baseEndpoints.Add("http://localhost:9000/api/DEALS?CATEGORYID=");
            baseEndpoints.Add("http://localhost:9000/API/DEALS?CATEGORYID=");

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
                         AssertFrontEndDealEquality(deal, matchingDeal);
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
            baseEndpoints.Add("http://localhost:9000/api/deals");
            baseEndpoints.Add("http://localhost:9000/aPi/deals/");
            baseEndpoints.Add("http://localhost:9000/api/Deals");
            baseEndpoints.Add("http://localhost:9000/api/DEALS/");
            baseEndpoints.Add("http://localhost:9000/API/DEALS");

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
                    AssertFrontEndDealEquality(deal, matchingDeal);
                }
            }
        }

        [TestMethod]
        public void GetDealsBySearchTerm_AllFieldMatches()
        {
            var query = "PuChOng BbQ";
            var sampleDeals = CreateSampleDeals().ToList();
            var validDeals = new List<DealModel>();
            var matchingDeal1 = TestModelFactory.CreateDeal("10% discount for puchong bbq", "description", "a", "b", "c", "d");
            var matchingDeal2 = TestModelFactory.CreateDeal("a", "10% discount for puchong bbq", "a", "b", "c", "d");
            var matchingDeal3 = TestModelFactory.CreateDeal("a", "description", "a", "10% discount for puchong bbq", "c", "d");
            var matchingDeal4 = TestModelFactory.CreateDeal("a", "c", "a", "10% discount for puchong bbq", "c", "d");

            validDeals.Add(matchingDeal1);
            validDeals.Add(matchingDeal2);
            validDeals.Add(matchingDeal3);
            validDeals.Add(matchingDeal4);

            sampleDeals.AddRange(validDeals);

            var response = "";
            var baseEndpoints = new List<string>();
            baseEndpoints.Add("http://localhost:9000/api/deals?search=");
            baseEndpoints.Add("http://localhost:9000/aPi/deals?SeArch=");
            baseEndpoints.Add("http://localhost:9000/api/Deals?Search=");
            baseEndpoints.Add("http://localhost:9000/api/DEALS?SEARCH=");
            baseEndpoints.Add("http://localhost:9000/API/DEALS?SEARCH=");

            foreach (var baseEndpoint in baseEndpoints)
            {
                var endpoint = baseEndpoint + query;

                response = CreateWebApiServiceAndGetResponse(
                    sampleDeals,
                    new List<DealCategoryModel>(),
                    endpoint);

                // To View Model
                var deals = JsonConvert.DeserializeObject<IEnumerable<FrontEndDeal>>(response).ToList();

                var expected = validDeals;

                deals.Count.ShouldBeEquivalentTo(expected.Count);

                foreach (var deal in deals)
                {
                    var matchingDeal = expected.First(d => d.Key.ToString().Equals(deal.Id));
                    AssertFrontEndDealEquality(deal, matchingDeal);
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

        private static void AssertFrontEndDealEquality(FrontEndDeal deal, DealModel matchingDeal)
        {
            deal.EndTime.Should().Be(matchingDeal.EndTime);
            deal.ShortTitle.Should().BeEquivalentTo(matchingDeal.ShortTitle);
            deal.ShortDescription.Should().BeEquivalentTo(matchingDeal.ShortDescription);
            deal.RegularPrice.Should().Be(matchingDeal.RegularPrice);
            deal.SpecialPrice.Should().Be(matchingDeal.SpecialPrice);
            deal.CanonicalUrl.Should().BeEquivalentTo(matchingDeal.CanonicalUrl);

            foreach (var image in matchingDeal.Images)
            {
                var extension = Path.GetExtension(image.RelativeUrl);
                var thumbPostfix = "_thumb" + extension;
                var thumbUrl = image.RelativeUrl.Replace(extension, thumbPostfix);

                Assert.IsTrue(deal.ThumbnailUrls.Any(d => d.EndsWith(thumbUrl)));
            }
        }

        private static void AssertFrontEndSpecificDealEquality(FrontEndSpecificDeal deal, DealModel matchingDeal)
        {
            deal.Id.Should().BeEquivalentTo(matchingDeal.Key.ToString());
            deal.Fineprint.Should().BeEquivalentTo(matchingDeal.FinePrint);
            deal.Highlight.Should().BeEquivalentTo(matchingDeal.Highlight);

            deal.ShortTitle.Should().BeEquivalentTo(matchingDeal.ShortTitle);
            deal.ShortDescription.Should().BeEquivalentTo(matchingDeal.ShortDescription);
            deal.LongTitle.Should().BeEquivalentTo(matchingDeal.LongTitle);
            deal.LongDescription.Should().BeEquivalentTo(matchingDeal.LongDescription);

            deal.RegularPrice.Should().Be(matchingDeal.RegularPrice);
            deal.SpecialPrice.Should().Be(matchingDeal.SpecialPrice);
            deal.StartTime.Should().Be(matchingDeal.StartTime);
            deal.EndTime.Should().Be(matchingDeal.EndTime);

            matchingDeal.Images.Count().ShouldBeEquivalentTo(deal.ImageUrls.Count);
            foreach (var image in matchingDeal.Images)
            {
                Assert.IsTrue(deal.ImageUrls.Any(a => a.Contains(image.RelativeUrl)), string.Format("Image {0} is not found.", image.RelativeUrl));
            }

            foreach (var option in matchingDeal.Options)
            {
                var matchingOption =
                    deal.DealOptions.First(
                        d =>
                            d.ShortTitle.Equals(option.ShortTitle) && d.RegularPrice.Equals(option.RegularPrice) &&
                            d.SpecialPrice.Equals(option.SpecialPrice));

                foreach (var attribute in option.Attributes)
                {
                    Assert.IsTrue(matchingOption.DealAttributes.Any(att => attribute.Name.Equals(att.Name) && attribute.Value.Equals(att.Value)));
                }
            }
        }
    }
}
