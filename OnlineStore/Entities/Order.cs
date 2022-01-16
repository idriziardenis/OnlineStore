using System;
using System.Collections.Generic;

namespace OnlineStore.Entities
{
    public partial class Order
    {
        public Order()
        {
            OrderItem = new HashSet<OrderItem>();
        }

        public int Id { get; set; }
        public int PaymentMethodId { get; set; }
        public decimal Total { get; set; }
        public string Message { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime? CreatedOnDate { get; set; }
        public string LastModifiedByUserId { get; set; }
        public DateTime? LastModifiedOnDate { get; set; }

        public virtual ICollection<OrderItem> OrderItem { get; set; }
    }
}
