using OnlineStore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Interfaces
{
    public interface IShoppingCartRepository : IRepository<ShoppingCart>
    {
        List<ShoppingCart> GetByClientId(string clientId);
        ShoppingCart GetByClientIdAndProductId(string clientId, int productId);
        ShoppingCart GetByClientIdAndItemId(string clientId, int itemId);
    }
}
