using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineStore.Interfaces;
using OnlineStore.Areas.StoreManagement.Models;
using Microsoft.AspNetCore.Authorization;

namespace OnlineStore.Areas.StoreManagement.Controllers
{
    [Area("StoreManagement")]
    [Authorize(Policy = "Admin")]
    public class HomeController : Controller
    {
        private IProductRepository _productRepository;
        private ICategoryRepository _categoryRepository;
        private IOrderRepository _orderRepository;
        private IUserRepository _userRepository;
        private IOptions<RequestLocalizationOptions> _options;
        private IHttpContextAccessor _httpContextAccessor;
        public HomeController(IOptions<RequestLocalizationOptions> options, IHttpContextAccessor httpContextAccessor, IProductRepository productRepository, ICategoryRepository categoryRepository, IOrderRepository orderRepository, IUserRepository userRepository)
        {
            _options = options;
            _httpContextAccessor = httpContextAccessor;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _orderRepository = orderRepository;
            _userRepository = userRepository;
        }

        public IActionResult Index()
        {
            try
            {
                var model = new DashboardViewModel();
                model.NoProducts = _productRepository.GetAll().Where(x => x.IsDeleted.GetValueOrDefault() == false).Count();
                model.NoCategories = _categoryRepository.GetAll().Where(x => x.IsDeleted.GetValueOrDefault() == false).Count();
                model.NoOrders = _orderRepository.GetAll().Where(x => x.IsDeleted.GetValueOrDefault() == false).Count();
                model.NoClients = _userRepository.GetAllClients().Where(x => x.IsDeleted.GetValueOrDefault() == false).Count();

                return View(model);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            try
            {
                var cultureItems = _options.Value.SupportedUICultures.Select(x => x.Name).ToArray();
                if (cultureItems.Any(x => x.Equals(culture)))
                {
                    Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)), new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1), HttpOnly = true, Secure = _httpContextAccessor.HttpContext.Request.IsHttps });
                }
                return LocalRedirect(returnUrl);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}