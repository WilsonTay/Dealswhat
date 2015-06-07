using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DealsWhat.Models;

namespace DealsWhat.ViewModels
{
    public class DealAttributeViewModel
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Id { get; set; }

        public DealAttributeViewModel(DealAttribute dealAttribute)
        {
            Id = dealAttribute.Id.ToString();
            Name = dealAttribute.Name;
            Value = dealAttribute.Value;
        }
    }
}