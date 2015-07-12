﻿using System;
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
            return deals.FirstOrDefault(d => d.Key.Equals(key));
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
