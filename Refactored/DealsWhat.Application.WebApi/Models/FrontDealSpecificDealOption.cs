using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DealsWhat.Application.WebApi.Models
{
    public class FrontDealSpecificDealOption
    {
        public string Id { get; set; }
        public string ShortTitle { get; set; }
        public double SpecialPrice { get; set; }
        public double RegularPrice { get; set; }
        public IList<FrontEndSpecificDealAttribute> DealAttributes { get; set; }

        public FrontDealSpecificDealOption()
        {
            DealAttributes = new List<FrontEndSpecificDealAttribute>();
        }
    }
}