using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealsWhat.Domain.Model
{
    public class UserModel : IAggregateRoot,IEntity
    {
        public string EmailAddress { get; private set; }

        public IEnumerable<CartItemModel> CartItems
        {
            get { return cartItems; }
        }

        private readonly IList<CartItemModel> cartItems;

        private UserModel()
        {
            cartItems = new List<CartItemModel>();
        }

        public static UserModel Create(string emailAddress)
        {
            return new UserModel
            {
                EmailAddress = emailAddress
            };
        }

        public void AddToCart(CartItemModel cartItem)
        {
            this.cartItems.Add(cartItem);
        }

        public void RemoveFromCart(CartItemModel cartItem)
        {
            this.cartItems.Remove(cartItem);
        }

        public object Key { get; internal set; }
    }
}
