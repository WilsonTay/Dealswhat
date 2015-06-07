using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DealsWhat.Models
{
    public class Merchant
    {
        public Guid Id { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Website { get; set; }
        public string BusinessRegNumber { get; set; }
        public string Name { get; set; }
        public string About { get; set; }
        public string Address { get; set; }
        public double? AddressLat { get; set; }
        public double? AddressLng { get; set; }

        public List<Deal> Deals { get; set; } 
    }
}