using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Areas.StoreManagement.Controllers
{
    [Area("StoreManagement")]
    public class HomeController : Controller
    {
        private IOptions<RequestLocalizationOptions> _options;
        private IHttpContextAccessor _httpContextAccessor;
        public HomeController(IOptions<RequestLocalizationOptions> options, IHttpContextAccessor httpContextAccessor)
        {
            _options = options;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            return View();
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