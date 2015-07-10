using System.Collections.Generic;
using DealsWhat.Domain.Model;

namespace DealsWhat.Domain.Services
{
    public interface ICartService
    {
        void AddCartItem(string emailAddress, CartItemModel model);
        IEnumerable<CartItemModel> GetCartItems(string emailAddress);
        void RemoveCartItem(string emailAddress, string cartItemId);
    }
}