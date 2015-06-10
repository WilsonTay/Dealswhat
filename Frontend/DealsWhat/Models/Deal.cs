using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DealsWhat.Models
{
    public enum DealStatus
    {
        Published = 0,
        Draft = 1,
        Deleted = 2
    }

    public class Deal
    {
        public Guid Id { get; set; }
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


        [JsonIgnore]
        public virtual DealCategory Category { get; set; }
        [JsonIgnore]
        public virtual Merchant Merchant { get; set; }

        public virtual IList<DealComment> Comments { get; set; }

        public virtual IList<DealImages> Pictures { get; set; }

        public virtual IList<DealOption> DealOptions { get; set; }
    }
}