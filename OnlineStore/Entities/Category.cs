using System;
using System.Collections.Generic;

namespace OnlineStore.Entities
{
    public partial class Category
    {
        public Category()
        {
            Product = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime? CreatedOnDate { get; set; }
        public string LastModifiedByUserId { get; set; }
        public DateTime? LastModifiedOnDate { get; set; }

        public virtual ICollection<Product> Product { get; set; }
    }
}
