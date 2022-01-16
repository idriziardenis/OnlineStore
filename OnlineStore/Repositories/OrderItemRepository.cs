using OnlineStore.Entities;
using OnlineStore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Repositories
{
    public class OrderItemRepository : Repository<OrderItem>, IOrderItemRepository
    {
        protected readonly ApplicationDBContext _onlineStoreContext;
        public OrderItemRepository(ApplicationDBContext onlineStoreContext) : base(onlineStoreContext)
        {
            _onlineStoreContext = onlineStoreContext;
        }
    }
}
