﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealsWhat.Infrastructure.DataAccess
{
    public interface IUnitOfWork
    {
        IDbSet<TEntity> Set<TEntity>()
            where TEntity : class;

        void Commit();

        void Update<TEntity>(TEntity entity)
            where TEntity : class;
    }
}
