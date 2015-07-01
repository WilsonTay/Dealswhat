using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DealsWhat.Application.WebApi.Models
{
    public class FrontEndDeal
    {
        public string ShortTitle { get; set; }
        public string ShortDescription { get; set; }
        public double RegularPrice { get; set; }
        public double SpecialPrice { get; set; }

        public string CanonicalUrl { get; set; }

        public DateTime EndTime { get; set; }

        public string Id { get; set; }

        public IList<string> ThumbnailUrls { get; set; }
    }
}