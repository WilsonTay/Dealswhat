using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace DealsWhat.Models
{
    public class DealOption
    {
        public Guid Id { get; set; }
        public string ShortTitle { get; set; }
        public double RegularPrice { get; set; }
        public double SpecialPrice { get; set; }

        public virtual IList<DealAttribute> Attributes { get; set; }

        [JsonIgnore]
        public virtual Deal Deal { get; set; }
    }
}