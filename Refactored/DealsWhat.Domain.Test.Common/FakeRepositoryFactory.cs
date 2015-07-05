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
        private readonly IRepository<UserModel> userRepository;  

        public FakeRepositoryFactory(
            IRepository<DealModel> dealRepository = null,
            IRepository<DealCategoryModel> dealCategoryRepository = null,
            IRepository<UserModel> userRepository = null)
        {
            this.dealRepository = dealRepository;
            this.dealCategoryRepository = dealCategoryRepository;
            this.userRepository = userRepository;
        }

        public IRepository<DealModel> CreateDealRepository()
        {
            return dealRepository;
        }

        public IRepository<DealCategoryModel> CreateDealCategoryRepository()
        {
            return dealCategoryRepository;
        }

        public IRepository<UserModel> CreateUserRepository()
        {
            return userRepository;
        }
    }
}
