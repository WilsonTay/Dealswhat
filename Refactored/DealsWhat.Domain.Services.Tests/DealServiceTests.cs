using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security;
using DealsWhat.Domain.Interfaces;
using DealsWhat.Domain.Model;
using DealsWhat.Domain.Test.Common;
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

            fixture.Register<IRepositoryFactory>(() => fixture.Create<FakeRepositoryFactory>());
        }

        [TestMethod]
        public void CategoryIdNotNull_ShouldCallToCategoryRepository()
        {
            var query = new DealSearchQuery
            {
                CategoryId = "a"
            };

            var mockedRepository = Mock.Get(fixture.Freeze<IRepository<DealCategoryModel>>());
            var dealService = fixture.Create<DealService>();

            dealService.SearchDeals(query);

            mockedRepository.Verify(m => m.GetAll(), Times.Once);
        }

        [TestMethod]
        public void Query_BySearchTerm_ShouldFindShortTitle()
        {
            var deals = Enumerable.Range(0, 10).Select(a => CreateDeal()).ToList();
            var validDeal = CreateDeal(shortTitle: "Alienware computer");

            deals.Add(validDeal);

            var fakeRepository = new FakeDealRepository(deals);
            fixture.Register<IRepository<DealModel>>(() => fakeRepository);

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
            fixture.Register<IRepository<DealModel>>(() => fakeRepository);

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
            fixture.Register<IRepository<DealModel>>(() => fakeRepository);

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
            fixture.Register<IRepository<DealModel>>(() => fakeRepository);

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
        public void Query_BySearchTerm_NotFoundShouldReturnEmptyList()
        {
            var service = fixture.Create<DealService>();
            var result = service.SearchDeals(new DealSearchQuery
            {
                SearchTerm = "a"
            });

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void Query_ByCategoryId_NotFoundShouldReturnEmptyList()
        {
            var dealCategories = new List<DealCategoryModel>();

            var mockedCategoryRepository = Mock.Get(fixture.Freeze<IRepository<DealCategoryModel>>());
            mockedCategoryRepository.Setup(m => m.GetAll()).Returns(dealCategories);

            var service = fixture.Create<DealService>();
            var result = service.SearchDeals(new DealSearchQuery
            {
                CategoryId = "a"
            });

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void Query_ByCategoryId_ShouldReturnProperDeals()
        {
            var dealCategories = new List<DealCategoryModel>();
            var dealService = GenerateDealCategoryAndCreateService(dealCategories);

            var categoryId = Guid.NewGuid();
            var validDeal = TestModelFactory.CreateDeal(shortTitle: "valid deal");
            var dealCategory = TestModelFactory.CreateDealCategory(name: "category", key: categoryId);

            dealCategory.AddDeal(validDeal);

            dealCategories.Add(dealCategory);

            var query = new DealSearchQuery
            {
                CategoryId = categoryId.ToString()
            };

            var deals = dealService.SearchDeals(query).ToList();

            deals.Count().ShouldBeEquivalentTo(1);
            deals[0].ShouldBeEquivalentTo(validDeal);
        }

        private DealService GenerateDealCategoryAndCreateService(List<DealCategoryModel> dealCategories)
        {
            var deals = Enumerable.Range(0, 10).Select(a => CreateDeal()).ToList();
            var otherDealCategory = TestModelFactory.CreateDealCategory("other category");

            foreach (var deal in deals)
            {
                otherDealCategory.AddDeal(deal);
            }

            dealCategories.Add(otherDealCategory);

            var fakeRepository = new FakeDealCategoryRepository(dealCategories);
            fixture.Register<IRepository<DealCategoryModel>>(() => fakeRepository);

            var fakeRepositoryFactory = fixture.Create<FakeRepositoryFactory>();
            fixture.Register<IRepositoryFactory>(() => fakeRepositoryFactory);

            var dealService = fixture.Create<DealService>();

            return dealService;
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


        private DealService GenerateDealsAndCreateDealService(DealModel validDeal)
        {
            var deals = Enumerable.Range(0, 10).Select(a => CreateDeal()).ToList();
            deals.Add(validDeal);

            var fakeRepository = new FakeDealRepository(deals);
            fixture.Register<IRepository<DealModel>>(() => fakeRepository);

            var fakeRepositoryFactory = fixture.Create<FakeRepositoryFactory>();
            fixture.Register<IRepositoryFactory>(() => fakeRepositoryFactory);

            var dealService = fixture.Create<DealService>();
            return dealService;
        }


        private static DealModel CreateDeal(
            string shortTitle = "short title",
            string shortDescription = "short description",
            string longTitle = "long title",
            string longDescription = "long description",
            string finePrint = "fineprint",
            string highlight = "highlight",
            string canonicalUrl = "url",
            object id = null)
        {
            var deal = TestModelFactory.CreateCompleteDeal(shortTitle, shortDescription, longTitle, longDescription, finePrint, highlight);

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
