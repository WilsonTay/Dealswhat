using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DealsWhat.Domain.Interfaces;
using DealsWhat.Domain.Model;

namespace DealsWhat.Domain.Services
{
    public class CartService
    {
        private readonly IRepositoryFactory repositoryFactory;

        public CartService(IRepositoryFactory repositoryFactory)
        {
            this.repositoryFactory = repositoryFactory;
        }

        public void AddCartItem(string userId, CartItemModel model)
        {
            var repository = this.repositoryFactory.CreateUserRepository();

            var user = repository.FindByKey(userId);

            user.AddToCart(model);

            repository.Save();
        }

        public IEnumerable<CartItemModel> GetCartItems(string userId)
        {
            var repository = this.repositoryFactory.CreateUserRepository();

            var user = repository.FindByKey(userId);

            return user.CartItems.ToList();
        }

        public void RemoveCartItem(string userId, string cartItemId)
        {
            var repository = this.repositoryFactory.CreateUserRepository();

            var user = repository.FindByKey(userId);
            var toRemove = user.CartItems.FirstOrDefault(c => c.Key.ToString().Equals(cartItemId));

            user.RemoveFromCart(toRemove);
        }
    }
}
