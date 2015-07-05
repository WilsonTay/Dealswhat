using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DealsWhat.Domain.Interfaces;
using DealsWhat.Domain.Model;

namespace DealsWhat.Domain.Test.Common
{
    public class FakeUserRepository : IRepository<UserModel>
    {
        private List<UserModel> users;

        public FakeUserRepository(List<UserModel> users)
        {
            this.users = users;
        }

        public IEnumerable<UserModel> GetAll()
        {
            return this.users;
        }

        public void Create(UserModel model)
        {
            this.users.Add(model);
        }

        public UserModel FindByKey(object key)
        {
            return users.First(a => a.Key.ToString().Equals(key.ToString()));
        }

        public void Save()
        {
        }
    }
}
