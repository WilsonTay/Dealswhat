using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DealsWhat.Domain.Interfaces;
using DealsWhat.Domain.Model;

namespace DealsWhat.Infrastructure.DataAccess
{
    public class EFDealCategoryRepository : IRepository<DealCategory>
    {
        private readonly IUnitOfWork unitOfWork;

        public EFDealCategoryRepository(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<DealCategory> GetAll()
        {
            foreach (var category in this.unitOfWork.Set<Models.DealCategory>())
            {
                yield return Convert(category);
            }
        }

        private DealsWhat.Domain.Model.DealCategory Convert(Models.DealCategory source)
        {
            var mappedDeal = Mapper.Map<Models.DealCategory, DealsWhat.Domain.Model.DealCategory>(source);

            return mappedDeal;
        }
    }
}
