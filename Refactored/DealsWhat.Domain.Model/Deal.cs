using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealsWhat.Domain.Model
{
    public class Deal : IEntity
    {
        public string ShortTitle { get; set; }
        public string LongTitle { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public double RegularPrice { get; set; }
        public double SpecialPrice { get; set; }
        public string SKU { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string FinePrint { get; set; }
        public string Highlight { get; set; }
        public bool IsFeatured { get; set; }
        public string CanonicalUrl { get; set; }
        public DealStatus Status { get; set; }

        public object Key { get; }
    }
}
