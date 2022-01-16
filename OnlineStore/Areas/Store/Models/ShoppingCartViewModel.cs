using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Areas.Store.Models
{
    public class ShoppingCartViewModel
    {
        public List<ShoppingCartItemViewModel> ShoppingCartItems { get; set; }
        public string ClientId { get; set; }
        public decimal Total { get; set; }
    }

    public class ShoppingCartItemViewModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Path { get; set; }
        public decimal Total { get; set; }
    }
}
