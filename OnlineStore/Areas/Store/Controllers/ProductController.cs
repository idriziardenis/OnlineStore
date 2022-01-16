using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineStore.Areas.Store.Models;
using OnlineStore.Interfaces;
using OnlineStore.Entities;
namespace OnlineStore.Areas.Store.Controllers
{
    [Area("Store")]
    public class ProductController : Controller
    {
        private IProductRepository _productRepository;
        private IFileHelper _fileHelper;

        public ProductController(IProductRepository productRepository, IFileHelper fileHelper)
        {
            _productRepository = productRepository;
            _fileHelper = fileHelper;
        }

        public IActionResult Details(int id)
        {
            try
            {
                if(id == 0)
                {
                    return RedirectToAction("Index", "Home");
                }

                var product = _productRepository.GetProductById(id);
                if (product == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                var upload = product.ProductUpload?.ToList();

                var main = upload.FirstOrDefault(x => x.IsThumbnail);
                var others = upload.Where(x => x.IsThumbnail == false);
                var model = new ProductDetailsViewModel()
                {
                    Id = id,
                    CategoryId = product.CategoryId,
                    CategoryName = product.Category?.Name,
                    Name = product.Name,
                    Descrption = product.Description,
                    Price = product.Price.GetValueOrDefault(),
                    Stock = product.Stock,
                    MainPath = main == null ? "/uploads/notfound/notfound.png" : main.Path.Replace("~", ""),
                    Paths = others.Select(x => x.Path.Replace("~", "")).ToList()
                };
                return View(model);
            }
            catch (Exception)
            {

                throw;
            }
            
        }
    }
}
