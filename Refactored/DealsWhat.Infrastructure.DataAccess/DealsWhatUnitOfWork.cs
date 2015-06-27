using DealsWhat.Models;

namespace DealsWhat.Infrastructure.DataAccess
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class DealsWhatUnitOfWork : DbContext, IUnitOfWork
    {
        // Your context has been configured to use a 'Model1' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'DealsWhat.Infrastructure.DataAccess.Model1' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'Model1' 
        // connection string in the application configuration file.
        public DealsWhatUnitOfWork()
            : base("name=Model1")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public DbSet<User> Users { get; set; }
        public DbSet<Deal> Deals { get; set; }
        public DbSet<DealCategory> DealCategories { get; set; }
        public DbSet<Merchant> Merchants { get; set; }
        public DbSet<DealImages> DealImages { get; set; }
        public DbSet<DealComment> DealComments { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<DealOption> DealOptions { get; set; }
        public DbSet<DealAttribute> DealAttributes { get; set; }

        public IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        public void Commit()
        {
             this.SaveChanges();
        }
    }
}