using System;
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