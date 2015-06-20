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
        private readonly Model1 dbContext;

        public EFDealCategoryRepository(Model1 dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<DealCategory> GetAll()
        {
            foreach (var category in this.dbContext.DealCategories)
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
