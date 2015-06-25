using System.Collections.Generic;
using System.Linq;
using DealsWhat.Models;

namespace DealsWhat.ViewModels
{
    public class PopularDealsViewModel
    {
        public IList<DealViewModel> Deals { get; set; }

        public PopularDealsViewModel(IEnumerable<Deal> popularDeals)
        {
            Deals = popularDeals.Select(d => new DealViewModel(d)).ToList();
        }
    }
}