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
        private readonly IRepository<Deal> dealRepository;
        private readonly IRepository<DealCategory> dealCategoryRepository; 
         
        public FakeRepositoryFactory(IRepository<Deal> dealRepository,
            IRepository<DealCategory> dealCategoryRepository)
        {
            this.dealRepository = dealRepository;
            this.dealCategoryRepository = dealCategoryRepository;
        }

        public IRepository<Deal> CreateDealRepository()
        {
            return dealRepository;
        }

        public IRepository<DealCategory> CreateDealCategoryRepository()
        {
            return dealCategoryRepository;
        }
    }
}
