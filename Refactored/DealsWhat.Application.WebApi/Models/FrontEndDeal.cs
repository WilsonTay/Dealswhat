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
        public string RegularPrice { get; set; }
        public string SpecialPrice { get; set; }

        public string CanonicalUrl { get; set; }

        public string Id { get; set; }
    }
}