using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineStore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Areas.StoreManagement.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
        public int CategoryId { get; set; }
        public int Stock { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public SelectList Categories { get; set; }
        public List<ProductUpload> Paths { get; set; }
        public ProductUpload MainPath { get; set; }
    }
}
