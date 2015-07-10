using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DealsWhat.Domain.Interfaces;
using DealsWhat.Domain.Model;

namespace DealsWhat.Domain.Services
{
    public class CartService : ICartService
    {
        private readonly IRepositoryFactory repositoryFactory;

        public CartService(IRepositoryFactory repositoryFactory)
        {
            this.repositoryFactory = repositoryFactory;
        }

        public void AddCartItem(string emailAddress, CartItemModel model)
        {
            var repository = this.repositoryFactory.CreateUserRepository();

            var user = repository.FindByEmailAddress(emailAddress);

            user.AddToCart(model);

            repository.Save();
        }

        public IEnumerable<CartItemModel> GetCartItems(string emailAddress)
        {
            var repository = this.repositoryFactory.CreateUserRepository();

            var user = repository.FindByKey(emailAddress);

            return user.CartItems.ToList();
        }

        public void RemoveCartItem(string emailAddress, string cartItemId)
        {
            var repository = this.repositoryFactory.CreateUserRepository();

            var user = repository.FindByKey(emailAddress);
            var toRemove = user.CartItems.FirstOrDefault(c => c.Key.ToString().Equals(cartItemId));

            user.RemoveFromCart(toRemove);
        }
    }
}
