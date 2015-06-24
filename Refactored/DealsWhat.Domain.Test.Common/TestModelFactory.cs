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
    public static class TestModelFactory
    {
        private static readonly IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());

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

            for (int i = 1; i < 5; i++)
            {
                deal.AddOption(CreateDealOptionWithAttributes());
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

            var regularPrice = fixture.Create<double>();
            var specialPrice = fixture.Create<double>(regularPrice);

            deal.SetPrice(regularPrice, specialPrice);

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

        public static DealOptionModel CreateDealOption(
            string shortTitle = "",
            double regularPrice = 0,
            double specialPrice = 0)
        {
            if (regularPrice == 0 && specialPrice == 0)
            {
                regularPrice = fixture.Create<double>();
                specialPrice = fixture.Create<double>(regularPrice);
            }

            var dealOption = DealOptionModel.Create(
                shortTitle.Equals("") ? fixture.Create<string>() : shortTitle,
                regularPrice,
                specialPrice);

            return dealOption;
        }

        public static DealOptionModel CreateDealOptionWithAttributes(
            string shortTitle = "",
            double regularPrice = 0,
            double specialPrice = 0,
            int optionsToCreate = 3)
        {
            var dealOption = CreateDealOption(shortTitle, regularPrice, specialPrice);

            for (int i = 0; i < optionsToCreate; i++)
            {
                var attribute = CreateDealAttribute();

                dealOption.AddAttribute(attribute);
            }

            return dealOption;
        }

        public static DealAttributeModel CreateDealAttribute(string name = "", string value = "")
        {
            return DealAttributeModel.Create(
                name.Equals("") ? fixture.Create<string>() : name,
                value.Equals("") ? fixture.Create<string>() : value);
        }
    }
}
