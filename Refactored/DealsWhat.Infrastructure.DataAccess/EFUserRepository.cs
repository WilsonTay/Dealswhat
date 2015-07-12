using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DealsWhat.Domain.Interfaces;
using DealsWhat.Domain.Model;
using DealsWhat.Models;

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

        public void Update(UserModel model)
        {
            var entity = Mapper.Map<DealsWhat.Domain.Model.UserModel, Models.User>(model);

            this.unitOfWork.Update(entity);
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

        public void AddToCart(string emailAddress, CartItemModel cart)
        {
            var attrIds = cart.AttributeValues.Select(d => Guid.Parse(d.Key.ToString()));
            var dealOptionGuid = Guid.Parse(cart.DealOption.Key.ToString());

            var dealOption = this.unitOfWork.Set<Models.DealOption>().First(d => d.Id == dealOptionGuid);
            var attributes = this.unitOfWork.Set<Models.DealAttribute>().Where(d => attrIds.Contains(d.Id)).ToList();
            var entity = new Cart
            {
                DealAttributes = attributes,
                DealOption = dealOption,
                Quantity = 1,
                Id = Guid.Parse(cart.Key.ToString())
            };

            //var mappedCart = Mapper.Map<DealsWhat.Domain.Model.CartItemModel, Models.Cart>(cart);
            var user = this.unitOfWork.Set<Models.User>().FirstOrDefault(u => u.Email.Equals(emailAddress));
            user.Carts.Add(entity);
        }

        private DealsWhat.Domain.Model.UserModel Convert(Models.User source)
        {
            var mappedDeal = Mapper.Map<Models.User, DealsWhat.Domain.Model.UserModel>(source);

            return mappedDeal;
        }
    }
}
