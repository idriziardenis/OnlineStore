using OnlineStore.Entities;
using OnlineStore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        protected readonly ApplicationDBContext _onlineStoreContext;
        public CategoryRepository(ApplicationDBContext onlineStoreContext) : base(onlineStoreContext)
        {
            _onlineStoreContext = onlineStoreContext;
        }
    }
}
