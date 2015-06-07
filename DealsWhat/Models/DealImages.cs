using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace DealsWhat.Models
{
    public class DealImages
    {
        public Guid Id { get; set; }
        public string RelativeUrl { get; set; }
        public int Order { get; set; }

        [JsonIgnore]
        public virtual Deal Deal { get; set; }
    }
}