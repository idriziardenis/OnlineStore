using System;
using System.Collections.Generic;

namespace OnlineStore.Entities
{
    public partial class Product
    {
        public Product()
        {
            OrderItem = new HashSet<OrderItem>();
            ProductUpload = new HashSet<ProductUpload>();
            ShoppingCart = new HashSet<ShoppingCart>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
        public int CategoryId { get; set; }
        public int Stock { get; set; }
        public bool IsFeatured { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime? CreatedOnDate { get; set; }
        public string LastModifiedByUserId { get; set; }
        public DateTime? LastModifiedOnDate { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<OrderItem> OrderItem { get; set; }
        public virtual ICollection<ProductUpload> ProductUpload { get; set; }
        public virtual ICollection<ShoppingCart> ShoppingCart { get; set; }
    }
}
