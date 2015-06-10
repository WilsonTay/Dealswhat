using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DealsWhat_Admin.Models;

namespace DealsWhat_Admin.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (var context = new DealsContext())
            {
                var category = new DealCategory
                {
                    Id = Guid.NewGuid(),
                    Name = "CategoryAdmin"
                };

                var deal = new Deal
                {
                    CanonicalUrl = "canonical",
                    DateAdded = DateTime.Now,
                    FinePrint = "Fine",
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now,
                    ShortTitle = "Short",
                    ShortDescription = "Short",
                    LongTitle = "Long",
                    LongDescription = "Long"
                };

                var image = new DealImage
                {
                    Id = Guid.NewGuid(),
                    RelativeUrl = "url",
                    Order = 0
                };

                deal.DealImages.Add(image);

                category.Deals.Add(deal);

                context.DealCategories.Add(category);

                //context.SaveChanges();
            }
            return View();
        }
    }
}
