using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DealsWhat.Models
{
    [DbConfigurationType(typeof(CachedDbConfiguration))]
    public class DealsContext : DbContext
    {
        public DealsContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Deal> Deals { get; set; }
        public DbSet<DealCategory> DealCategories { get; set; }
        public DbSet<Merchant> Merchants { get; set; }
        public DbSet<DealImages> DealImages { get; set; }
        public DbSet<DealComment> DealComments { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<DealOption> DealOptions { get; set; }
        public DbSet<DealAttribute> DealAttributes { get; set; }
    }
}