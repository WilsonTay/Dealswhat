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
    public sealed class FakeDealCategoryRepository : IRepository<DealCategoryModel>
    {
        private readonly IList<DealCategoryModel> dealCategories;

        public FakeDealCategoryRepository(IList<DealCategoryModel> dealCategories)
        {
            this.dealCategories = dealCategories;
        }

        public IEnumerable<DealCategoryModel> Get(Expression<Func<DealCategoryModel, bool>> query)
        {
            return dealCategories.Where(query.Compile());
        }

        public IEnumerable<DealCategoryModel> GetAll()
        {
            return dealCategories;
        }

        public void Create(DealCategoryModel model)
        {
            throw new NotImplementedException();
        }

        public DealCategoryModel FindByKey(object key)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
