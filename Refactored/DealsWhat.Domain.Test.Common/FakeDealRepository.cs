using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DealsWhat.Domain.Interfaces;
using DealsWhat.Domain.Model;

namespace DealsWhat.Domain.Test.Common
{
    public sealed class FakeDealRepository : IRepository<DealModel>
    {
        private IList<DealModel> deals;
         
        public FakeDealRepository(IList<DealModel> deals)
        {
            this.deals = deals;
        }

        public IEnumerable<DealModel> Get(Expression<Func<DealModel, bool>> query)
        {
            return deals.Where(query.Compile());
        }

        public IEnumerable<DealModel> GetAll()
        {
            return deals;
        }
    }
}
