using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DealsWhat.Models
{
    public class Cart
    {
        public Guid Id { get; set; }

        public string User_UserId { get; set; }

        [ForeignKey("User_UserId")]
        public User User { get; set; }

        public int Quantity { get; set; }

        public DealOption DealOption { get; set; }
        public IList<DealAttribute> DealAttributes { get; set; }
        //public virtual IList<DealOptionAttribute> SelectedAttributes { get; set; }
    }
}