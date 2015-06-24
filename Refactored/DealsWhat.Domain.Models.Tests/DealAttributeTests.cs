using System;
using DealsWhat.Domain.Test.Common;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DealsWhat.Domain.Models.Tests
{
    [TestClass]
    public class DealAttributeTests
    {
        [TestMethod]
        public void Create_AllFieldMatches()
        {
            var name = "Size";
            var value = "M";

            var attribute = TestModelFactory.CreateDealAttribute(name, value);

            attribute.Name.ShouldBeEquivalentTo(name);
            attribute.Value.ShouldBeEquivalentTo(value);
        }
    }
}
