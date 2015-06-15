using System.Linq;
using System.Web;
using System.Web.Mvc;
using DealsWhat.Controllers.Aggregator;
using DealsWhat.Helpers;
using DealsWhat.Models;
using DealsWhat.ViewModels;

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
                var popularDeals = DealsAggregator.SuggestPopularDeals(Request.RequestContext.HttpContext, context, 10).ToList();

                foreach (var image in popularDeals.SelectMany(a => a.Pictures))
                {
                    image.RelativeUrl =
                       VirtualPathUtility.ToAbsolute(PathHelper.ConvertRelativeToAbsoluteDealImagePath(image.RelativeUrl));
                }


                var viewModel = new DealSpecificProductViewModel(deal, popularDeals);

                return View(viewModel);
            }

        }

    }
}
