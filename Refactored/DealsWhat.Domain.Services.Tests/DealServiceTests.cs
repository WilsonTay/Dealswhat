using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security;
using DealsWhat.Domain.Interfaces;
using DealsWhat.Domain.Model;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace DealsWhat.Domain.Services.Tests
{
    [TestClass]
    public class DealServiceTests
    {
        private IFixture fixture;

        [TestInitialize]
        public void Initialize()
        {
            fixture = new Fixture().Customize(new AutoMoqCustomization());
        }

        [TestMethod]
        public void ShouldCallToRepository()
        {
            var mockedRepository = Mock.Get(fixture.Freeze<IRepository<Deal>>());
            var dealService = fixture.Create<DealService>();

            dealService.SearchDeals(fixture.Create<DealSearchQuery>());

            mockedRepository.Verify(m => m.GetAll(), Times.Once);
        }

        [TestMethod]
        public void Query_BySearchTerm_ShouldFindShortTitle()
        {
            var deals = Enumerable.Range(0, 10).Select(a => CreateDeal()).ToList();
            var validDeal = CreateDeal(shortTitle: "Alienware computer");

            deals.Add(validDeal);

            var fakeRepository = new FakeDealRepository(deals);
            fixture.Register<IRepository<Deal>>(() => fakeRepository);

            var dealService = fixture.Create<DealService>();
            var searchTerm = "alienware";

            var query = new DealSearchQuery
            {
                SearchTerm = searchTerm
            };

            var results = dealService.SearchDeals(query);

            results.Should().NotBeEmpty();

            foreach (var result in results)
            {
                AssertContainsIgnoreCase(result.ShortTitle, searchTerm);
            }
        }

        private static void AssertContainsIgnoreCase(string compared, string searchTerm)
        {
            Assert.IsTrue(compared.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) > -1);
        }

        [TestMethod]
        public void Query_BySearchTerm_ShouldFindShortDescription()
        {
            var deals = Enumerable.Range(0, 10).Select(a => CreateDeal()).ToList();
            var validDeal = CreateDeal(shortDescription: "This is Alienware computer description");

            deals.Add(validDeal);

            var fakeRepository = new FakeDealRepository(deals);
            fixture.Register<IRepository<Deal>>(() => fakeRepository);

            var dealService = fixture.Create<DealService>();
            var searchTerm = "alienware";

            var query = new DealSearchQuery
            {
                SearchTerm = searchTerm
            };

            var results = dealService.SearchDeals(query);

            results.Should().NotBeEmpty();

            foreach (var result in results)
            {
                AssertContainsIgnoreCase(result.ShortDescription, searchTerm);
            }
        }

        [TestMethod]
        public void Query_BySearchTerm_ShouldFindLongTitle()
        {
            var deals = Enumerable.Range(0, 10).Select(a => CreateDeal()).ToList();
            var validDeal = CreateDeal(longTitle: "Korean BBQ in Puchong");

            deals.Add(validDeal);

            var fakeRepository = new FakeDealRepository(deals);
            fixture.Register<IRepository<Deal>>(() => fakeRepository);

            var dealService = fixture.Create<DealService>();
            var searchTerm = "puchong";

            var query = new DealSearchQuery
            {
                SearchTerm = searchTerm
            };

            var results = dealService.SearchDeals(query);

            results.Should().NotBeEmpty();

            foreach (var result in results)
            {
                AssertContainsIgnoreCase(result.LongTitle, searchTerm);
            }
        }

        [TestMethod]
        public void Query_BySearchTerm_ShouldFindLongDescription()
        {
            var deals = Enumerable.Range(0, 10).Select(a => CreateDeal()).ToList();
            var validDeal = CreateDeal(longDescription: "Description of Korean BBQ in Puchong");

            deals.Add(validDeal);

            var fakeRepository = new FakeDealRepository(deals);
            fixture.Register<IRepository<Deal>>(() => fakeRepository);

            var dealService = fixture.Create<DealService>();
            var searchTerm = "kOrean bbq";

            var query = new DealSearchQuery
            {
                SearchTerm = searchTerm
            };

            var results = dealService.SearchDeals(query);

            results.Should().NotBeEmpty();

            foreach (var result in results)
            {
                AssertContainsIgnoreCase(result.LongDescription, searchTerm);
            }
        }

        [TestMethod]
        public void Query_ByCategoryId_ShouldReturnProperDeals()
        {      
        }

        [TestMethod]
        public void Query_ByDealId_ShouldReturnProperDeal()
        {
            var guid = Guid.NewGuid();
            var query = new SingleDealSearchQuery
            {
                Id = guid.ToString()
            };

            var validDeal = CreateDeal(id: guid);

            var dealService = GenerateDealsAndCreateDealService(validDeal);
            var result = dealService.SearchSingleDeal(query);

            result.Should().NotBe(null);
            result.Should().Be(validDeal);
        }

        [TestMethod]
        public void Query_ByDealId_InvalidIdShouldReturnNoDeal()
        {
            var guid = Guid.NewGuid();
            var query = new SingleDealSearchQuery
            {
                Id = Guid.NewGuid().ToString()
            };

            var validDeal = CreateDeal(id: guid);

            var dealService = GenerateDealsAndCreateDealService(validDeal);
            var result = dealService.SearchSingleDeal(query);

            result.Should().Be(null);
        }

        [TestMethod]
        public void Query_ByCanonicalUrl_ShouldReturnProperDeal()
        {
            var url = "puchong-bbq";
            var query = new SingleDealSearchQuery
            {
                CanonicalUrl = url
            };

            var validDeal = CreateDeal(canonicalUrl: url);

            var dealService = GenerateDealsAndCreateDealService(validDeal);
            var result = dealService.SearchSingleDeal(query);

            result.Should().NotBe(null);
            result.Should().Be(validDeal);
        }

        [TestMethod]
        public void Query_ByCanonicalUrl_ShouldOnlyReturnExactMatch()
        {
            var url = "puchong-bbq";
            var query = new SingleDealSearchQuery
            {
                CanonicalUrl = url
            };

            var validDeal = CreateDeal(canonicalUrl: url + "-more-url-here");

            var dealService = GenerateDealsAndCreateDealService(validDeal);
            var result = dealService.SearchSingleDeal(query);

            result.Should().Be(null);
        }


        private DealService GenerateDealsAndCreateDealService(Deal validDeal)
        {
            var deals = Enumerable.Range(0, 10).Select(a => CreateDeal()).ToList();
            deals.Add(validDeal);

            var fakeRepository = new FakeDealRepository(deals);
            fixture.Register<IRepository<Deal>>(() => fakeRepository);

            var dealService = fixture.Create<DealService>();
            return dealService;
        }


        private static Deal CreateDeal(
            string shortTitle = "short title",
            string shortDescription = "short description",
            string longTitle = "long title",
            string longDescription = "long description",
            string finePrint = "fineprint",
            string highlight = "highlight",
            string canonicalUrl = "url",
            object id = null)
        {
            var deal = Deal.Create(shortTitle, shortDescription, longTitle, longDescription, finePrint, highlight);

            if (id != null)
            {
                deal.Key = id;
            }

            if (!string.IsNullOrEmpty(canonicalUrl))
            {
                deal.CanonicalUrl = canonicalUrl;
            }

            return deal;
        }
    }
}
