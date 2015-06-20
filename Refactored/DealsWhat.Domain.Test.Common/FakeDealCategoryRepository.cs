using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DealsWhat.Domain.Interfaces;
using DealsWhat.Domain.Model;

namespace DealsWhat.Domain.Test.Common
{
    public sealed class FakeDealCategoryRepository : IRepository<DealCategory>
    {
        private readonly IList<DealCategory> dealCategories;

        public FakeDealCategoryRepository(IList<DealCategory> dealCategories)
        {
            this.dealCategories = dealCategories;
        }

        public IEnumerable<DealCategory> Get(Expression<Func<DealCategory, bool>> query)
        {
            return dealCategories.Where(query.Compile());
        }

        public IEnumerable<DealCategory> GetAll()
        {
            return dealCategories;
        }
    }
}
