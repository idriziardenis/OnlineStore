using System;
using System.Collections.Generic;

namespace OnlineStore.Entities
{
    public partial class ProductUpload
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public bool IsThumbnail { get; set; }

        public virtual Product Product { get; set; }
    }
}
