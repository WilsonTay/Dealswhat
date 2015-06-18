using System;
using DealsWhat.Domain.Model;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;

namespace DealsWhat.Domain.Models.Tests
{
    [TestClass]
    public class DealTests
    {
        private string shortTitle = "short title";
        private string longTitle = "long title";
        private string shortDescription = "short description";
        private string longDescription = "long description";
        private string finePrint = "fine print";
        private string highlight = "highlight";
        private double regularPrice = 15.00;
        private double specialPrice = 7.50;
        private DateTime startTime;
        private DateTime endTime;

        [TestInitialize]
        public void TestInitialize()
        {
            startTime = DateTime.Now;
            endTime = DateTime.Now.AddDays(7);
        }

        [TestMethod]
        public void CreateDeal_AllAttributesSet()
        {
            var deal = Deal.Create(
                shortTitle,
                shortDescription,
                longTitle,
                longDescription,
                finePrint,
                highlight);

            deal.ShortTitle.Should().Be(shortTitle);
            deal.ShortDescription.Should().Be(shortDescription);
            deal.LongTitle.Should().Be(longTitle);
            deal.LongDescription.Should().Be(longDescription);
            deal.FinePrint.Should().Be(finePrint);
            deal.Highlight.Should().Be(highlight);

            deal.RegularPrice.Should().Be(0.0);
            deal.SpecialPrice.Should().Be(0.0);

            deal.DateAdded.Should().BeCloseTo(DateTime.Now, 20);
            deal.StartTime.Should().BeCloseTo(DateTime.Now, 20);
            deal.EndTime.Should().BeCloseTo(DateTime.Now.AddDays(7), 20);

            deal.IsFeatured.Should().BeFalse();
        }
    }
}
