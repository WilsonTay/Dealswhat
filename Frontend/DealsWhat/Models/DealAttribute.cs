using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace DealsWhat.Models
{
    public class DealAttribute
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        [JsonIgnore]
        public virtual DealOption DealOption { get; set; }

        [JsonIgnore]
        public virtual IList<Cart> Carts { get; set; }
    }
}