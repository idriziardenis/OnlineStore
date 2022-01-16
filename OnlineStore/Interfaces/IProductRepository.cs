using OnlineStore.Areas.StoreManagement.Models;
using OnlineStore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Product GetProductById(int id);
        List<Product> GetAllProducts();
        ProductUpload GetProductThumbnail(int id);
        void DeleteThumbnail(ProductUpload entity);
        void AddProductImage(ProductUpload image);
        string GetThumbnailPath(int productId, int size);
        SuccessResult DeleteImage(int id);
    }
}
