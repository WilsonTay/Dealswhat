using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DealsWhat.Domain.Interfaces;
using DealsWhat.Domain.Model;

namespace DealsWhat.Domain.Test.Common
{
    public sealed class FakeDealRepository : IRepository<Deal>
    {
        private IList<Deal> deals;
         
        public FakeDealRepository(IList<Deal> deals)
        {
            this.deals = deals;
        }

        public IEnumerable<Deal> Get(Expression<Func<Deal, bool>> query)
        {
            return deals.Where(query.Compile());
        }

        public IEnumerable<Deal> GetAll()
        {
            return deals;
        }
    }
}
