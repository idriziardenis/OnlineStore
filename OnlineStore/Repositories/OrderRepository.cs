using OnlineStore.Entities;
using OnlineStore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        protected readonly ApplicationDBContext _onlineStoreContext;
        public OrderRepository(ApplicationDBContext onlineStoreContext) : base(onlineStoreContext)
        {
            _onlineStoreContext = onlineStoreContext;
        }
    }
}
