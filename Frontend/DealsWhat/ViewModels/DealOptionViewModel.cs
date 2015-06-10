using System.Collections.Generic;
using System.Linq;
using DealsWhat.Models;

namespace DealsWhat.ViewModels
{
    public class DealOptionViewModel
    {
        public string Id { get; set; }
        public string ShortTitle { get; set; }
        public double SpecialPrice { get; set; }
        public double RegularPrice { get; set; }
        public IList<DealAttributeViewModel> DealAttributes { get; set; }

        public DealOptionViewModel(DealOption dealOption)
        {
            ShortTitle = dealOption.ShortTitle;
            SpecialPrice = dealOption.SpecialPrice;
            RegularPrice = dealOption.RegularPrice;
            Id = dealOption.Id.ToString();

            DealAttributes = dealOption.Attributes.Select(a => new DealAttributeViewModel(a)).ToList();
        }
    }
}