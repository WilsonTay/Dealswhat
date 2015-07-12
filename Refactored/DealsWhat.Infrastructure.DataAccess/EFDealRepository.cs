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
    public class EFDealRepository : IRepository<DealModel>
    {
        private readonly IUnitOfWork unitOfWork;

        public EFDealRepository(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }


        public IEnumerable<DealModel> GetAll()
        {
            foreach (var deal in this.unitOfWork.Set<Models.Deal>())
            {
                yield return Convert(deal);
            }
        }

        public void Update(DealModel model)
        {
            throw new NotImplementedException();
        }

        public void Create(DealModel model)
        {
            throw new NotImplementedException();
        }

        public DealModel FindByKey(object key)
        {
            // HACK: Optimize this.
            var entity = this.unitOfWork.Set<Models.Deal>()
                .ToList()
                .FirstOrDefault(u => u.Id.ToString() == key.ToString());

            if (entity != null)
            {
                return Convert(entity);
            }

            return null;
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        private DealsWhat.Domain.Model.DealModel Convert(Models.Deal source)
        {
            var mappedDeal = Mapper.Map<Models.Deal, DealsWhat.Domain.Model.DealModel>(source);

             return mappedDeal;
        }
    }
}
