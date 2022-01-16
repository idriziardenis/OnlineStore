using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineStore.Interfaces;
using OnlineStore.Entities;
using OnlineStore.Areas.Store.Models;
using Microsoft.AspNetCore.Authorization;

namespace OnlineStore.Areas.Store.Controllers
{
    [Area("Store")]
    [Authorize("Client")]
    public class ShoppingCartController : Controller
    {
        private IShoppingCartRepository _shoppingCartRepository;
        private IUserService _userService;
        private IFileHelper _fileHelper;
        public ShoppingCartController(IShoppingCartRepository shoppingCartRepository, IUserService userService, IFileHelper fileHelper)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _userService = userService;
            _fileHelper = fileHelper;
        }

        public IActionResult Index()
        {
            try
            {
                var model = new ShoppingCartViewModel();
                model.ShoppingCartItems = new List<ShoppingCartItemViewModel>();
                model.ClientId = _userService.GetUserId();
                var shoppingCartItems = _shoppingCartRepository.GetByClientId(model.ClientId);

                foreach (var item in shoppingCartItems)
                {
                    var shoppingCartItem = new ShoppingCartItemViewModel();
                    shoppingCartItem.Id = item.Id;
                    shoppingCartItem.ProductId = item.ProductId;
                    shoppingCartItem.Name = item.Product?.Name;
                    shoppingCartItem.Price = item.Product.Price.GetValueOrDefault();
                    shoppingCartItem.Quantity = item.Quantity;
                    var upload = item.Product?.ProductUpload?.FirstOrDefault(x => x.IsThumbnail);
                    shoppingCartItem.Path = upload == null ? "/uploads/notfound/notfound_75.png" : _fileHelper.GetProperFilePath(Enums.FileTypes.Image, Enums.Thumbnails.Grid, upload.Path).Replace("~","");
                    shoppingCartItem.Total = shoppingCartItem.Price * shoppingCartItem.Quantity;
                    model.ShoppingCartItems.Add(shoppingCartItem);
                }
                model.Total = model.ShoppingCartItems.Sum(x => x.Total);
                return View(model);
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity = 1)
        {
            try
            {
                var currentUserId = _userService.GetUserId();
                var exist = _shoppingCartRepository.GetByClientIdAndProductId(currentUserId, productId);
                if(exist == null)
                {
                    var shoppingCartItem = new ShoppingCart()
                    {
                        ClientId = currentUserId,
                        Quantity = quantity,
                        ProductId = productId
                    };

                    _shoppingCartRepository.Add(shoppingCartItem);
                    _shoppingCartRepository.SaveChanges();
                }
                else
                {
                    exist.Quantity += quantity;
                    _shoppingCartRepository.Update(exist);
                    _shoppingCartRepository.SaveChanges();
                }

                return Ok();
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public IActionResult DeleteItem(int productId)
        {
            try
            {
                var currentUserId = _userService.GetUserId();
                var exist = _shoppingCartRepository.GetByClientIdAndItemId(currentUserId, productId);
                if (exist != null)
                {
                    _shoppingCartRepository.Remove(exist);
                    _shoppingCartRepository.SaveChanges();
                }

                return Ok();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
