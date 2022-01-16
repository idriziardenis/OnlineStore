using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Areas.Store.Models;
using OnlineStore.Interfaces;
using OnlineStore.Entities;
namespace OnlineStore.Areas.Store.Controllers
{
    [Area("Store")]
    public class HomeController : Controller
    {
        private IProductRepository _productRepository;

        public HomeController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public IActionResult Index(int? categoryId, string search)
        {
            try
            {
                var model = new HomeViewModel();
                model.FeaturedProducts = new List<ProductItemViewModel>();
                model.Products = new List<ProductItemViewModel>();
                var allProducts = _productRepository.GetAllProducts().Where(x => x.IsActive.GetValueOrDefault() == true && x.IsDeleted.GetValueOrDefault() == false);
                List<Product> nonFeaturedProducts = new List<Product>();
                if (categoryId.HasValue == false && string.IsNullOrEmpty(search))
                {
                    var featuredProducts = allProducts.Where(x => x.IsFeatured);
                    foreach (var item in featuredProducts)
                    {
                        var product = new ProductItemViewModel();
                        product.Id = item.Id;
                        product.Name = item.Name;
                        product.Price = item.Price.GetValueOrDefault();
                        var upload = item.ProductUpload?.FirstOrDefault(x => x.IsThumbnail);
                        product.Path = upload == null ? "/uploads/notfound/notfound.png" : upload.Path.Replace("~", "");
                        model.FeaturedProducts.Add(product);
                    }
                }
                if (categoryId.HasValue)
                {
                    nonFeaturedProducts = allProducts.Where(x => x.CategoryId == categoryId.GetValueOrDefault()).ToList();
                }
                else if (!string.IsNullOrEmpty(search))
                {
                    nonFeaturedProducts = allProducts.Where(x => x.Name.ToLower().Contains(search.ToLower()) || x.Description.ToLower().Contains(search.ToLower()) || search.ToLower().Contains(x.Name.ToLower()) || search.ToLower().Contains(x.Description.ToLower())).ToList();
                }
                else
                {
                    nonFeaturedProducts = allProducts.Where(x => !x.IsFeatured).ToList();
                }

                foreach (var item in nonFeaturedProducts)
                {
                    var product = new ProductItemViewModel();
                    product.Id = item.Id;
                    product.Name = item.Name;
                    product.Price = item.Price.GetValueOrDefault();
                    var upload = item.ProductUpload?.FirstOrDefault(x => x.IsThumbnail);
                    product.Path = upload == null ? "/uploads/notfound/notfound.png" : upload.Path.Replace("~", "");
                    model.Products.Add(product);
                }

                return View(model);
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public IActionResult Search(string search)
        {
            try
            {
                return RedirectToAction("Index", new { search = search });
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }
    }
}
