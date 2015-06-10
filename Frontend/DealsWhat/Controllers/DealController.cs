using System.Web.Http;
using DealsWhat.Models;
using DealsWhat.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DealsWhat.Controllers.Aggregator;

namespace DealsWhat.Controllers
{
    public class DealController : Controller
    {
        //
        // GET: /Deal/
        public ActionResult Index(string id)
        {
            using (var context = new DealsContext())
            {
                var deal = context.Deals.First(d => d.CanonicalUrl == id);
                var popularDeals = DealsAggregator.SuggestPopularDeals(Request.RequestContext.HttpContext, context, 10);
                var viewModel = new DealSpecificProductViewModel(deal, popularDeals);

                return View(viewModel);
            }

        }

    }
}
