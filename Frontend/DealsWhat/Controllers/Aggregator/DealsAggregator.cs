using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DealsWhat.Models;

namespace DealsWhat.Controllers.Aggregator
{
    public class DealsAggregator
    {
        public static IEnumerable<Deal> SuggestPopularDeals(HttpContextBase httpContext, DealsContext dealsContext, int max = 10)
        {
            return dealsContext.Deals.Include("Pictures").Take(max);
        }
    }
}