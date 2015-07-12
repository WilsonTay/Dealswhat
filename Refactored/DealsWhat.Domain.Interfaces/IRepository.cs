using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DealsWhat.Domain.Model;

namespace DealsWhat.Domain.Interfaces
{
    public interface IRepository<T>
        where T : IAggregateRoot
    {
        IEnumerable<T> GetAll();

        void Update(T model);
        void Create(T model);

        T FindByKey(object key);

        void Save();
    }
}
