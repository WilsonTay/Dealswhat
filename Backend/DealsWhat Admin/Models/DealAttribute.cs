//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;

namespace DealsWhat_Admin.Models
{
    public partial class DealAttribute
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public Nullable<Guid> DealOption_Id { get; set; }
    
        public virtual DealOption DealOption { get; set; }
    }
}
