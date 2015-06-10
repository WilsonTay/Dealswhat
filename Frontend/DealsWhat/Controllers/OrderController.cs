using System.Linq;
using System.Web.Mvc;
using DealsWhat.Models;
using DealsWhat.ViewModels;

namespace DealsWhat.Controllers
{
    public class OrderController : Controller
    {
        public ActionResult CheckOut()
        {
            using (var context = new DealsContext())
            {
                var emailAddress = User.Identity.Name;
                var user = context.Users.First(u => u.EmailAddress == emailAddress);
                var carts = context.Carts.Where(c => c.User.UserId == user.UserId).ToList();
                var viewModel = new OrderCheckoutViewModel(carts);

                return View(viewModel);
            }
        }

    }
}
