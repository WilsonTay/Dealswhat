using System;
using System.Collections.Generic;

namespace DealsWhat.Models
{
    public class Cart
    {
        public Guid Id { get; set; }
        public User User { get; set; }

        public int Quantity { get; set; }

        public DealOption DealOption { get; set; }
        public IList<DealAttribute> DealAttributes { get; set; }
        //public virtual IList<DealOptionAttribute> SelectedAttributes { get; set; }
    }
}