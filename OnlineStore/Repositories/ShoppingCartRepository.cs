using Microsoft.EntityFrameworkCore;
using OnlineStore.Entities;
using OnlineStore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Repositories
{
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        protected readonly ApplicationDBContext _onlineStoreContext;
        public ShoppingCartRepository(ApplicationDBContext onlineStoreContext) : base(onlineStoreContext)
        {
            _onlineStoreContext = onlineStoreContext;
        }

        public List<ShoppingCart> GetByClientId(string clientId)
        {
            try
            {
                return _onlineStoreContext.ShoppingCart.Where(x => x.ClientId == clientId).Include(x => x.Product).ThenInclude(x=>x.ProductUpload).Include(x => x.Client).Where(x=>x.Product.IsActive == true && x.Product.IsDeleted == false).ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ShoppingCart GetByClientIdAndItemId(string clientId, int itemId)
        {
            try
            {
                return _onlineStoreContext.ShoppingCart.FirstOrDefault(x => x.ClientId == clientId && x.Id == itemId);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ShoppingCart GetByClientIdAndProductId(string clientId, int productId)
        {
            try
            {
                return _onlineStoreContext.ShoppingCart.Where(x => x.ClientId == clientId).Include(x => x.Product).ThenInclude(x => x.ProductUpload).Include(x => x.Client).FirstOrDefault(x => x.Product.IsActive == true && x.Product.IsDeleted == false && x.ProductId == productId);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
