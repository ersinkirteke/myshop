using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MyShop.AddToCart
{
    public interface IShoppingCart
    {
        void Add(CartItem item);
        void Remove(CartItem item);
        Task<ReadOnlyCollection<CartItem>> GetCartItems();
    }
}