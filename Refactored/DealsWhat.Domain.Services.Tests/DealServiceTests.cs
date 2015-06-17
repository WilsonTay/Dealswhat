using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            var deals = fixture.CreateMany<Deal>(10).ToList();
            deals[0].ShortTitle = "Alienware computer";

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
            var deals = fixture.CreateMany<Deal>(10).ToList();
            deals[0].ShortDescription = "This is Alienware computer description";

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
            var deals = fixture.CreateMany<Deal>(10).ToList();
            deals[0].LongTitle = "Korean BBQ in Puchong";

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
            var deals = fixture.CreateMany<Deal>(10).ToList();
            deals[0].LongDescription = "Description of Korean BBQ in Puchong";

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

        private IList<Deal> GetFakeDeals()
        {
            var deals = fixture.CreateMany<Deal>(10).ToList();

            deals[0].ShortTitle = "Alienware computer";
            deals[1].ShortDescription = "This is alienware computer";
            deals[2].LongTitle = "Korean BBQ";
            deals[3].LongDescription = "This is description for korean BBQ";

            return deals;
        }
    }
}
