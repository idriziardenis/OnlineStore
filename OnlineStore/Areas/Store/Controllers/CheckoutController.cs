using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineStore.Entities;
using OnlineStore.Interfaces;
using OnlineStore.Areas.Store.Models;
using Microsoft.AspNetCore.Authorization;

namespace OnlineStore.Areas.Store.Controllers
{
    [Area("Store")]
    [Authorize("Client")]
    public class CheckoutController : Controller
    {
        private IShoppingCartRepository _shoppingCartRepository;
        private IUserService _userService;
        private IUserRepository _userRepository;
        private IOrderRepository _orderRepository;
        private IOrderItemRepository _orderItemRepository;

        public CheckoutController(IShoppingCartRepository shoppingCartRepository, IUserService userService, IUserRepository userRepository, IOrderRepository orderRepository, IOrderItemRepository orderItemRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _userService = userService;
            _userRepository = userRepository;
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
        }

        public IActionResult Index()
        {
            try
            {
                var model = new CheckoutViewModel();
                var currentUserId = _userService.GetUserId();
                var currentUser = _userRepository.GetByStringId(currentUserId);

                model.Name = currentUser.Name;
                model.Surname = currentUser.Surname;
                model.CompanyName = currentUser.CompanyName;
                model.Country = currentUser.Country;
                model.State = currentUser.State;
                model.City = currentUser.City;
                model.Street = currentUser.Street;
                model.Zip = currentUser.Zip;
                model.Phone = currentUser.Phone;
                model.Email = currentUser.Email;

                var shoppingCarItems = _shoppingCartRepository.GetByClientId(currentUserId);
                model.Total = shoppingCarItems.Sum(x => x.Product.Price.GetValueOrDefault() * x.Quantity);
                model.Subtotal = model.Total;
                return View(model);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public IActionResult Index(CheckoutViewModel model)
        {
            try
            {
                var currentUserId = _userService.GetUserId();
                var currentUser = _userRepository.GetByStringId(currentUserId);

                currentUser.Name = model.Name;
                currentUser.Surname = model.Surname;
                currentUser.CompanyName = model.CompanyName;
                currentUser.Country = model.Country;
                currentUser.State = model.State;
                currentUser.City = model.City;
                currentUser.Street = model.Street;
                currentUser.Zip = model.Zip;
                currentUser.Phone = model.Phone;
                currentUser.Email = model.Email;
                _userRepository.Update(currentUser);
                _userRepository.SaveChanges();

                var shoppingCartItems = _shoppingCartRepository.GetByClientId(currentUserId);

                var order = new Order()
                {
                    CreatedByUserId = currentUserId,
                    CreatedOnDate = DateTime.Now,
                    IsDeleted = false,
                    Message = model.Message,
                    PaymentMethodId = 1,
                    Total = shoppingCartItems.Sum(x => x.Product.Price.GetValueOrDefault() * x.Quantity)
                };

                _orderRepository.Add(order);
                _orderRepository.SaveChanges();

                List<OrderItem> orderItems = new List<OrderItem>();
                foreach (var item in shoppingCartItems)
                {
                    var orderItem = new OrderItem()
                    {
                        OrderId = order.Id,
                        Price = item.Product.Price.GetValueOrDefault(),
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Total = item.Product.Price.GetValueOrDefault() * item.Quantity
                    };
                    orderItems.Add(orderItem);
                }

                _orderItemRepository.AddRange(orderItems);
                _orderItemRepository.SaveChanges();

                _shoppingCartRepository.RemoveRange(shoppingCartItems);
                _shoppingCartRepository.SaveChanges();
                return RedirectToAction("Completed", "Order");
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
