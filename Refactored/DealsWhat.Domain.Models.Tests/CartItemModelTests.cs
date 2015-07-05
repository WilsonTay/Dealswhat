using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DealsWhat.Domain.Model;
using DealsWhat.Domain.Test.Common;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DealsWhat.Domain.Models.Tests
{
    [TestClass]
    public class CartItemModelTests
    {
        [TestMethod]
        public void Create_IdGenerated()
        {
            var dealOption = TestModelFactory.CreateDealOptionWithAttributes();
            var attrValues = new List<DealAttributeModel>();
            attrValues.Add(dealOption.Attributes.First());

            var cartItem = CartItemModel.Create(dealOption, attrValues);

            cartItem.Key.Should().NotBeNull();
        }

        [TestMethod]
        public void Create_AllFieldMatches()
        {
            var dealOption = TestModelFactory.CreateDealOptionWithAttributes();
            var attrValues = new List<DealAttributeModel>();
            attrValues.Add(dealOption.Attributes.First());

            var cartItem = CartItemModel.Create(dealOption, attrValues);

            cartItem.DealOption.ShouldBeEquivalentTo(dealOption);
            cartItem.AttributeValues.Should().Contain(attrValues[0]);
        }

    }
}
