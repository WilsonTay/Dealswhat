//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace DealsWhat_Admin.Models
{
    public partial class DealCategory
    {
        public DealCategory()
        {
            this.Deals = new HashSet<Deal>();
        }
    
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
    
        public virtual ICollection<Deal> Deals { get; set; }
    }
}
