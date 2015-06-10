using System;
using DealsWhat.Models;

namespace DealsWhat.ViewModels
{
    public class DealImageViewModel
    {
        public Guid Id { get; set; }
        public string RelativeUrl { get; set; }
        public int Order { get; set; }

        public DealImageViewModel(DealImages dealImage)
        {
            Id = dealImage.Id;
            RelativeUrl = dealImage.RelativeUrl;
            Order = dealImage.Order;
        }
    }
}