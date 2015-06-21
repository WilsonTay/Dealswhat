using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DealsWhat.Domain.Interfaces;
using DealsWhat.Domain.Model;

namespace DealsWhat.Domain.Test.Common
{
    public class FakeRepositoryFactory : IRepositoryFactory
    {
        private readonly IRepository<DealModel> dealRepository;
        private readonly IRepository<DealCategoryModel> dealCategoryRepository; 
         
        public FakeRepositoryFactory(IRepository<DealModel> dealRepository,
            IRepository<DealCategoryModel> dealCategoryRepository)
        {
            this.dealRepository = dealRepository;
            this.dealCategoryRepository = dealCategoryRepository;
        }

        public IRepository<DealModel> CreateDealRepository()
        {
            return dealRepository;
        }

        public IRepository<DealCategoryModel> CreateDealCategoryRepository()
        {
            return dealCategoryRepository;
        }
    }
}
