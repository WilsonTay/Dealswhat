using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DealsWhat.Domain.Model;
using DealsWhat.Domain.Test.Common;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace DealsWhat.Domain.Models.Tests
{
    [TestClass]
    public class DealCategoryTests
    {
        private IFixture fixture;

        [TestInitialize]
        public void TestInitialize()
        {
            fixture = new Fixture().Customize(new AutoMoqCustomization());
        }

        [TestMethod]
        public void CreateDealCategory_ShouldHaveAttributesMapped()
        {
            var categoryName = "Food & Drinks";

            var category = TestModelFactory.CreateDealCategory(name: categoryName);

            category.Name.ShouldBeEquivalentTo(categoryName);
        }

        [TestMethod]
        public void AddDeal_DealAddedShouldBeRetrievable()
        {
            var categoryName = "Food & Drinks";
            var deals = Enumerable.Range(0, 10).ToList().Select(a => TestModelFactory.CreateDeal()).ToList();

            var category = TestModelFactory.CreateDealCategory(name: categoryName);

            foreach (var deal in deals)
            {
                category.AddDeal(deal);
            }

            foreach (var deal in deals)
            {
                category.Deals.Should().Contain(deal);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddDeal_DuplicateDeal_ExceptionExpected()
        {
            var deal = TestModelFactory.CreateDeal();
            var category = TestModelFactory.CreateDealCategory();

            category.AddDeal(deal);
            category.AddDeal(deal);
        }
    }
}
