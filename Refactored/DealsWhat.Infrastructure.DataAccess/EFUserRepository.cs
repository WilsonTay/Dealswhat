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
    public class EFUserRepository : IUserRepository
    {
        private readonly IUnitOfWork unitOfWork;

        public EFUserRepository(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }


        public IEnumerable<UserModel> GetAll()
        {
            foreach (var user in this.unitOfWork.Set<Models.User>())
            {
                yield return Convert(user);
            }
        }

        public void Create(UserModel model)
        {
            throw new NotImplementedException();
        }

        public UserModel FindByKey(object key)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            this.unitOfWork.Commit();
        }

        public UserModel FindByEmailAddress(string emailAddress)
        {
            // TODO: Null cart.
            var entity = this.unitOfWork.Set<Models.User>().FirstOrDefault(u => u.Email.Equals(emailAddress));

            if (entity != null)
            {
                return Convert(entity);
            }

            return null;
        }

        private DealsWhat.Domain.Model.UserModel Convert(Models.User source)
        {
            var mappedDeal = Mapper.Map<Models.User, DealsWhat.Domain.Model.UserModel>(source);

            return mappedDeal;
        }
    }
}
