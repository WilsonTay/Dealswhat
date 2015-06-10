using System.Collections.Generic;
using DealsWhat.Models;

namespace DealsWhat.ViewModels
{
    public class IndexViewModel
    {
        public IList<Deal> FeaturedDeals { get; set; }
        public IList<Deal> PopularDeals { get; set; }
        public IList<Deal> NewDeals { get; set; }
        public IList<DealCategory> Categories { get; set; }
    }
}