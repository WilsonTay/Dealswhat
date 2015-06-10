using DealsWhat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DealsWhat.ViewModels
{
    public class CartViewModel
    {
        public int Quantity { get; set; }
        public DealViewModel Deal { get; set; }
        public double TotalPrice { get; set; }

        public CartViewModel(Cart cart)
        {
            Quantity = cart.Quantity;
            //Deal = new DealViewModel(cart.Deal);
            //TotalPrice = cart.Deal.SpecialPrice * Quantity;
        }
    }
}