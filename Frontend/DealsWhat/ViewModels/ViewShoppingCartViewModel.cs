using System.Collections.Generic;
using System.Linq;
using DealsWhat.Models;

namespace DealsWhat.ViewModels
{
    public class ViewShoppingCartViewModel
    {
        public IList<CartViewModel> CartItems { get; set; }
        public double TotalPrice { get; set; }

        public ViewShoppingCartViewModel(IList<Cart> carts)
        {
            CartItems = carts.Select(c => new CartViewModel(c)).ToList();

            foreach (var item in CartItems)
            {
                TotalPrice += item.TotalPrice;
            }
        }
    }
}