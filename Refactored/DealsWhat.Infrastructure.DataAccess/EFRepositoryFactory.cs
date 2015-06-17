using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DealsWhat.Domain.Interfaces;
using DealsWhat.Domain.Model;

namespace DealsWhat.Infrastructure.DataAccess
{
    public class EFRepositoryFactory : IRepositoryFactory
    {
        private readonly Model1 dbContext;

        public EFRepositoryFactory(Model1 dbContext)
        {
            this.dbContext = dbContext;
        }

        public IRepository<Deal> CreateDealRepository()
        {
            return new EFDealRepository(this.dbContext);
        }
    }
}
