using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DealsWhat.Domain.Interfaces;
using DealsWhat.Domain.Model;

namespace DealsWhat.Infrastructure.DataAccess
{
    public class EFDealRepository : IRepository<Deal>
    {
        private readonly Model1 dbContext;

        public EFDealRepository(Model1 dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<Deal> Get(Expression<Func<Deal, bool>> query)
        {
            return this.dbContext.Deals.Where(query.Compile());
        }

        public IEnumerable<Deal> GetAll()
        {
            return this.dbContext.Deals;
        }
    }
}
