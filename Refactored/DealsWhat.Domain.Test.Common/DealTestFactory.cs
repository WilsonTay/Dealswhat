using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DealsWhat.Domain.Model;

namespace DealsWhat.Domain.Test.Common
{
    public static class DealTestFactory
    {
        public static Deal CreateDeal(
            string shortTitle = "short title",
            string shortDescription = "short description",
            string longTitle = "long title",
            string longDescription = "long description",
            string finePrint = "fineprint",
            string highlight = "highlight",
            string canonicalUrl = "url",
            object id = null)
        {
            var deal = Deal.Create(shortTitle, shortDescription, longTitle, longDescription, finePrint, highlight);

            if (id != null)
            {
                deal.Key = id;
            }

            return deal;
        }

        public static DealCategory CreateDealCategory(object key = null, string name = "category1")
        {
            var category = DealCategory.Create(name);

            if (key != null)
            {
                category.Key = key;
            }

            return category;
        }
    }
}
