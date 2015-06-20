using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DealsWhat.Domain.Model;
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

            var category = DealTestHelper.CreateDealCategory(name: categoryName);

            category.Name.ShouldBeEquivalentTo(categoryName);
        }

        [TestMethod]
        public void AddDeal_DealAddedShouldBeRetrievable()
        {
            var categoryName = "Food & Drinks";
            var deals = Enumerable.Range(0, 10).ToList().Select(a => DealTestHelper.CreateDeal()).ToList();

            var category = DealTestHelper.CreateDealCategory(name: categoryName);

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
            var deal = DealTestHelper.CreateDeal();
            var category = DealTestHelper.CreateDealCategory();

            category.AddDeal(deal);
            category.AddDeal(deal);
        }
    }
}
