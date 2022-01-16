using System;
using System.Collections.Generic;

namespace OnlineStore.Entities
{
    public partial class ShoppingCart
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ClientId { get; set; }
        public int Quantity { get; set; }

        public virtual AspNetUsers Client { get; set; }
        public virtual Product Product { get; set; }
    }
}
