using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DealsWhat.Domain.Model;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace DealsWhat.Domain.Test.Common
{
    public static class DealTestFactory
    {
        private static IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());

        public static DealModel CreateCompleteDeal(
            string shortTitle = "",
            string shortDescription = "",
            string longTitle = "",
            string longDescription = "",
            string finePrint = "",
            string highlight = "",
            string canonicalUrl = "",
            object id = null)
        {
            var deal = CreateDeal(shortTitle, shortDescription, longTitle, longDescription, finePrint, highlight, canonicalUrl, id);

            for (int i = 1; i < 5; i++)
            {
                deal.AddImage(CreateDealImage());
            }

            return deal;
        }

        public static DealModel CreateDeal(
            string shortTitle = "",
            string shortDescription = "",
            string longTitle = "",
            string longDescription = "",
            string finePrint = "",
            string highlight = "",
            string canonicalUrl = "",
            object id = null)
        {
            var deal = DealModel.Create(
                shortTitle.Equals("") ? fixture.Create<string>() : shortTitle,
                shortDescription.Equals("") ? fixture.Create<string>() : shortDescription,
                longTitle.Equals("") ? fixture.Create<string>() : longTitle,
                longDescription.Equals("") ? fixture.Create<string>() : longDescription,
                finePrint.Equals("") ? fixture.Create<string>() : finePrint,
                highlight.Equals("") ? fixture.Create<string>() : highlight);

            deal.Key = id ?? fixture.Create<string>();

            if (!string.IsNullOrEmpty(canonicalUrl))
            {
                deal.CanonicalUrl = canonicalUrl;
            }

            return deal;
        }

        public static DealCategoryModel CreateDealCategory(object key = null, string name = "")
        {
            var category = DealCategoryModel.Create(name.Equals("") ? fixture.Create<string>() : name);

            category.Key = key ?? fixture.Create<string>();

            return category;
        }

        public static DealImageModel CreateDealImage(string relativeUrl = "", int order = -1)
        {
            if (string.IsNullOrEmpty(relativeUrl))
            {
                relativeUrl = fixture.Create<string>();
            }

            if (order == -1)
            {
                order = fixture.Create<int>();
            }

            return DealImageModel.Create(relativeUrl, order);
        }
    }
}
