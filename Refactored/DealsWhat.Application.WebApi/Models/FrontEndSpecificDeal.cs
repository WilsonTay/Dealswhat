using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DealsWhat.Application.WebApi.Models
{
    public class FrontEndSpecificDeal
    {
        public string Id { get; set; }
        public string Fineprint { get; set; }
        public string Highlight { get; set; }

        public IList<string> ImageUrls { get; set; }
        public double RegularPrice { get; set; }
        public double SpecialPrice { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime StartTime { get; set; }

        public string ShortTitle { get; set; }
        public string LongTitle { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }

        // TODO: Deal Options.
    }
}