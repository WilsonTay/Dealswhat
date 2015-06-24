using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DealsWhat.Domain.Test.Common;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DealsWhat.Domain.Models.Tests
{
    [TestClass]
    public class DealOptionTests
    {
        [TestMethod]
        public void Create_AllFieldsSet()
        {
            var shortTitle = "short title";
            var regularPrice = 10.5;
            var specialPrice = 5.5;

            var dealOption = TestModelFactory.CreateDealOption(shortTitle, regularPrice, specialPrice);

            dealOption.ShortTitle.ShouldBeEquivalentTo(shortTitle);
            dealOption.RegularPrice.ShouldBeEquivalentTo(regularPrice);
            dealOption.SpecialPrice.ShouldBeEquivalentTo(specialPrice);
        }

        [TestMethod]
        public void AddAttributes_AttributesValid()
        {
            var dealOption = TestModelFactory.CreateDealOption();

            var attributes = Enumerable.Range(0, 10).Select(a => TestModelFactory.CreateDealAttribute()).ToList();

            foreach (var attribute in attributes)
            {
                dealOption.AddAttribute(attribute);
            }

            foreach (var attribute in attributes)
            {
                dealOption.Attributes.Should().Contain(attribute);
            }
        }
    }
}
