using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace DealsWhat.Models
{
    public class DealComment
    {
        public Guid Id { get; set; }
        public DateTime DatePosted { get; set; }
        public string Message { get; set; }

        [JsonIgnore]
        public virtual User Poster { get; set; }
        [JsonIgnore]
        public virtual Deal Deal { get; set; }
    }
}