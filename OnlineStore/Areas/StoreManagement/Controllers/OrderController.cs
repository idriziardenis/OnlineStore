using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineStore.Entities;
using OnlineStore.Interfaces;
using OnlineStore.Areas.StoreManagement.Models;
using OnlineStore.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace OnlineStore.Areas.StoreManagement.Controllers
{
    [Area("StoreManagement")]
    [Authorize(Policy = "Admin")]
    public class OrderController : Controller
    {
        private IOrderRepository _orderRepository;
        private IUserService _userService;

        public OrderController(IOrderRepository orderRepository, IUserService userService)
        {
            _orderRepository = orderRepository;
            _userService = userService;
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

        public IActionResult GetOrdersJson()
        {
            try
            {
                var culture = GlobalFunctions.GetCulture(HttpContext);
                var orders = _orderRepository.GetAll().Where(x => x.IsDeleted.GetValueOrDefault() == false).OrderByDescending(x => x.Id).ToList();

                var result = orders.Select(order => new
                {
                    id = order.Id,
                    payment = "Cash on delivery",
                    total = order.Total,
                    message = order.Message
                });

                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                var order = _orderRepository.GetById(id);
                if (order != null)
                {
                    order.IsDeleted = true;
                    order.LastModifiedByUserId = _userService.GetUserId();
                    order.LastModifiedOnDate = DateTime.Now;
                    _orderRepository.Update(order);
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
