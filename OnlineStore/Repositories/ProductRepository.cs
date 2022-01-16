using Microsoft.EntityFrameworkCore;
using OnlineStore.Areas.StoreManagement.Models;
using OnlineStore.Entities;
using OnlineStore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        protected readonly ApplicationDBContext _onlineStoreContext;
        public ProductRepository(ApplicationDBContext onlineStoreContext) : base(onlineStoreContext)
        {
            _onlineStoreContext = onlineStoreContext;
        }

        public Product GetProductById(int id)
        {
            try
            {
                return _onlineStoreContext.Product.Include(x => x.ProductUpload).Include(x=>x.Category).Where(x => x.Id == id).FirstOrDefault();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ProductUpload GetProductThumbnail(int id)
        {
            try
            {
                return _onlineStoreContext.ProductUpload.Where(a => a.ProductId == id && a.IsThumbnail == true).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteThumbnail(ProductUpload entity)
        {
            try
            {
                _onlineStoreContext.ProductUpload.Remove(entity);
                _onlineStoreContext.SaveChanges();
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public void AddProductImage(ProductUpload image)
        {
            try
            {
                _onlineStoreContext.ProductUpload.Add(image);
                _onlineStoreContext.SaveChanges();
            }
            catch (Exception)
            {

                throw;

            }
        }
        public string GetThumbnailPath(int productId, int size)
        {
            try
            {
                var upload = _onlineStoreContext.ProductUpload.Where(x => x.ProductId == productId && x.IsThumbnail).FirstOrDefault();
                var path = "";
                if (upload != null)
                {
                    path = upload.Path;
                }

                var final = "";

                if (!string.IsNullOrEmpty(path))
                {
                    // remove ~
                    var pathwithoutsymbol = path.Substring(1, path.Length - 1);
                    //add_75
                    var splitted = pathwithoutsymbol.Split('.');
                    final = splitted[0] + $"_{size}." + splitted[1];
                }
                else
                {
                    final = $"/uploads/notfound/notfound_{size}.png";
                }
                return final;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public SuccessResult DeleteImage(int id)
        {
            try
            {
                var productVariantImage = _onlineStoreContext.ProductUpload.FirstOrDefault(p => p.Id == id);
                if (productVariantImage == null) return new SuccessResult(false, $"Could not find Product image with id: {id}");

                _onlineStoreContext.ProductUpload.Remove(productVariantImage);
                _onlineStoreContext.SaveChanges();

                return new SuccessResult(true, "Product image deleted successfully!");
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<Product> GetAllProducts()
        {
            try
            {
                return _onlineStoreContext.Product.Include(x => x.ProductUpload).ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}