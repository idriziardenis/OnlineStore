using OnlineStore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Areas.Store.Models
{
    public class HomeViewModel
    {
        public List<ProductItemViewModel> Products { get; set; }
        public List<ProductItemViewModel> FeaturedProducts { get; set; }
    }
}
