using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using OnlineStore.Areas.StoreManagement.Models;
using OnlineStore.Entities;
using OnlineStore.Enums;
using OnlineStore.Helpers;
using OnlineStore.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Areas.StoreManagement.Controllers
{
    [Area("StoreManagement")]
    [Authorize(Policy = "Admin")]
    public class ProductController : Controller
    {
        private IProductRepository _productRepository;
        private ISelectListService _selectListService;
        private IUserService _userService;
        private IStringLocalizer<SharedResource> _sharedLocalizer;
        private IWebHostEnvironment _env;
        private IHttpContextAccessor _httpContextAccessor;
        private IFileHelper _fileHelper;

        public ProductController(IProductRepository productRepository, ISelectListService selectListService, IUserService userService, IStringLocalizer<SharedResource> sharedLocalizer, IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor, IFileHelper fileHelper)
        {
            _productRepository = productRepository;
            _selectListService = selectListService;
            _userService = userService;
            _sharedLocalizer = sharedLocalizer;
            _env = env;
            _httpContextAccessor = httpContextAccessor;
            _fileHelper = fileHelper;
        }

        public IActionResult Index()
        {
            try
            {
                if (TempData["SuccessResultF"] != null)
                {
                    var flag = (bool)TempData["SuccessResultF"];
                    var message = TempData["SuccessResultM"].ToString();
                    var SuccessResult = new SuccessResult(flag, message);
                    ViewBag.Notification = SuccessResult;
                }
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IActionResult Add()
        {
            try
            {
                var model = new ProductViewModel();
                var culture = GlobalFunctions.GetCulture(HttpContext);
                model.Categories = new SelectList(_selectListService.GetCategoriesKeysValues(), "Key", "Value");

                return View(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IActionResult Edit(int id)
        {
            try
            {
                if (id == 0)
                    return RedirectToAction("Index");

                var product = _productRepository.GetProductById(id);

                var vm = new ProductViewModel()
                {
                    Id = product.Id,
                    CategoryId = product.Id,
                    Description = product.Description,
                    IsFeatured = product.IsFeatured,
                    IsActive = product.IsActive.GetValueOrDefault(),
                    IsDeleted = product.IsDeleted.GetValueOrDefault(),
                    Name = product.Name,
                    Price = product.Price,
                    Stock = product.Stock
                };

                vm.Paths = product.ProductUpload.Where(x => x.IsThumbnail != true).ToList();
                vm.MainPath = product.ProductUpload.Where(x => x.IsThumbnail == true).FirstOrDefault();
                vm.Categories = new SelectList(_selectListService.GetCategoriesKeysValues(), "Key", "Value", vm.CategoryId);

                return View("Add", vm);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(ProductViewModel model, IFormFile MainImage, List<IFormFile> OtherImages)
        {
            try
            {
                int productId = 0;
                if (!ModelState.IsValid)
                {
                    var culture = GlobalFunctions.GetCulture(HttpContext);
                    model.Categories = new SelectList(_selectListService.GetCategoriesKeysValues(), "Key", "Value", model.CategoryId);

                    return View("Add", model);
                }
                if (model.Id == 0)
                {
                    var product = new Product();
                    product.Name = model.Name;
                    product.Description = model.Description;
                    product.Stock = model.Stock;
                    product.CategoryId = model.CategoryId;
                    product.Price = model.Price;
                    product.IsActive = model.IsActive;
                    product.IsFeatured = model.IsFeatured;
                    product.CreatedByUserId = _userService.GetUserId();
                    product.CreatedOnDate = DateTime.Now;
                    product.IsDeleted = false;
                    _productRepository.Add(product);
                    productId = product.Id;
                }
                else
                {
                    productId = model.Id;
                    var product = _productRepository.GetById(model.Id);
                    product.Name = model.Name;
                    product.Price = model.Price;
                    product.Description = model.Description;
                    product.IsActive = model.IsActive;
                    product.Stock = model.Stock;
                    product.Price = model.Price;
                    product.CategoryId = model.CategoryId;
                    product.IsActive = model.IsActive;
                    product.IsDeleted = false;
                    product.LastModifiedByUserId = _userService.GetUserId();
                    product.LastModifiedOnDate = DateTime.Now;
                    _productRepository.Update(product);

                }


                //For both Add and Edit
                if (MainImage != null)
                {
                    var fileName = Path.GetFileName(MainImage.FileName);
                    var uploadPath = "~/uploads/products/" + productId.ToString() + "/Image/" + fileName;

                    _fileHelper.SaveFile(FileTypes.Image, MainImage, "products", productId.ToString(), (int)Thumbnails.Grid, (int)Thumbnails.Catalog);

                    var findExisting = _productRepository.GetProductThumbnail(productId);
                    if (findExisting != null)
                    {
                        _productRepository.DeleteThumbnail(findExisting);
                    }
                    var variantUpload = new ProductUpload
                    {
                        ProductId = productId,
                        Name = fileName,
                        Path = uploadPath,
                        IsThumbnail = true
                    };
                    _productRepository.AddProductImage(variantUpload);

                }
                if (OtherImages.Count > 0)
                {
                    foreach (var item in OtherImages)
                    {
                        try
                        {
                            var fileName = Path.GetFileName(item.FileName);
                            var uploadPath = "~/uploads/products/" + productId.ToString() + "/Image/" + fileName;

                            _fileHelper.SaveFile(FileTypes.Image, item, "products", productId.ToString(), (int)Thumbnails.Grid, (int)Thumbnails.Catalog);

                            var variantUpload = new ProductUpload
                            {
                                ProductId = productId,
                                Name = fileName,
                                Path = uploadPath,
                                IsThumbnail = false
                            };
                            _productRepository.AddProductImage(variantUpload);
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }



                var result = new SuccessResult(true, _sharedLocalizer["ChangesSavedSuccessfully"]);

                TempData["SuccessResultF"] = result.Success;
                TempData["SuccessResultM"] = result.Message;
                return RedirectToAction("Index", "Product");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IActionResult GetProductsJson()
        {
            try
            {
                var culture = GlobalFunctions.GetCulture(HttpContext);
                var products = _productRepository.GetAll().Where(x=>x.IsDeleted.GetValueOrDefault() == false).OrderByDescending(x => x.Id).ToList();
                products.ForEach(x => x.CreatedByUserId = _productRepository.GetThumbnailPath(x.Id, (int)Thumbnails.Grid));

                var result = products.Select(product => new
                {
                    id = product.Id,
                    imagePath = product.CreatedByUserId,
                    name = product.Name,
                    description = product.Description,
                    price = product.Price,
                    isActive = product.IsActive,
                    isFeatured = product.IsFeatured,
                    stock = product.Stock
                });

                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IActionResult DeleteImage(int id)
        {
            try
            {
                var result = _productRepository.DeleteImage(id);

                if (result.Success)
                {
                    return Ok(true);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                var product = _productRepository.GetById(id);
                if (product != null)
                {
                    product.IsDeleted = true;
                    product.LastModifiedByUserId = _userService.GetUserId();
                    product.LastModifiedOnDate = DateTime.Now;
                    _productRepository.Update(product);
                }
                return Ok(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
