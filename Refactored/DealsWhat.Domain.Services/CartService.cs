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

        public void AddCartItem(string emailAddress, NewCartItemModel model)
        {
            var repository = this.repositoryFactory.CreateUserRepository();
            var dealRepository = this.repositoryFactory.CreateDealRepository();

            var user = repository.FindByEmailAddress(emailAddress);
            var deal = dealRepository.FindByKey(model.DealId);

            var dealOption = deal.Options.First(d => d.Key.ToString().Equals(model.DealOptionId));
            var attributes = dealOption.Attributes.Where(d => model.SelectedAttributes.Contains(d.Key.ToString())).ToList();

            var cartItem = CartItemModel.Create(dealOption, attributes);
             user.AddToCart(cartItem);

            repository.Save();
        }

        public IEnumerable<CartItemModel> GetCartItems(string emailAddress)
        {
            var repository = this.repositoryFactory.CreateUserRepository();

            var user = repository.FindByEmailAddress(emailAddress);

            return user.CartItems.ToList();
        }

        public void RemoveCartItem(string emailAddress, string cartItemId)
        {
            var repository = this.repositoryFactory.CreateUserRepository();

            var user = repository.FindByEmailAddress(emailAddress);
            var toRemove = user.CartItems.FirstOrDefault(c => c.Key.ToString().Equals(cartItemId));

            user.RemoveFromCart(toRemove);
        }
    }
}
