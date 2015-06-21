using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DealsWhat.Domain.Interfaces;
using DealsWhat.Domain.Model;

namespace DealsWhat.Infrastructure.DataAccess
{
    public class EFDealRepository : IRepository<Deal>
    {
        private readonly IUnitOfWork unitOfWork;

        public EFDealRepository(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }


        public IEnumerable<Deal> GetAll()
        {
            foreach (var deal in this.unitOfWork.Set<Models.Deal>())
            {
                yield return Convert(deal);
            }
        }

        private DealsWhat.Domain.Model.Deal Convert(Models.Deal source)
        {
            var mappedDeal = Mapper.Map<Models.Deal, DealsWhat.Domain.Model.Deal>(source);

             return mappedDeal;
        }
    }
}
