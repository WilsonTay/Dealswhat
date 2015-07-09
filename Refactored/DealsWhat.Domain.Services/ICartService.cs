using System.Collections.Generic;
using DealsWhat.Domain.Model;

namespace DealsWhat.Domain.Services
{
    public interface ICartService
    {
        void AddCartItem(string userId, CartItemModel model);
        IEnumerable<CartItemModel> GetCartItems(string userId);
        void RemoveCartItem(string userId, string cartItemId);
    }
}