using System.Linq;
using System.Web.Mvc;
using DealsWhat.Models;
using DealsWhat.ViewModels;

namespace DealsWhat.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            using (var context = new DealsContext())
            {
                var model = new IndexViewModel();
                //model.FeaturedDeals = context.Deals.Include("Merchant").Include("Pictures").Where(d => d.IsFeatured).Take(3).ToList();
                model.NewDeals = context.Deals.Include("Merchant").Include("Pictures").OrderByDescending(d => d.DateAdded).Take(16).ToList();
                model.PopularDeals = new PopularDealsViewModel(context.Deals.Include("Pictures").OrderByDescending(d => d.DateAdded).Take(16).ToList());
                model.Categories = context.DealCategories.ToList();

                return View(model);
            }

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
