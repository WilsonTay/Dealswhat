using System.Data.Entity;

namespace DealsWhat_Admin.Models
{
    public class DealsContext : DbContext
    {
        public DealsContext()
            : base("DealsWhatEntities")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Deal> Deals { get; set; }
        public DbSet<DealCategory> DealCategories { get; set; }
        public DbSet<Merchant> Merchants { get; set; }
        public DbSet<DealImage> DealImages { get; set; }
        public DbSet<DealComment> DealComments { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<DealAttribute> DealAttributes { get; set; }
        public DbSet<DealOption> DealOptions { get; set; }
    }
}