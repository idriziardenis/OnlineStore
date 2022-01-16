using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using OnlineStore.Areas.StoreManagement.Models;
using OnlineStore.Helpers;
using OnlineStore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Areas.StoreManagement.Controllers
{
    [Area("StoreManagement")]
    [Authorize(Policy = "Admin")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private IHttpContextAccessor _httpContextAccessor;
        private IStringLocalizer<SharedResource> _sharedLocalizer;
        private IRolesRepository _rolesRepository;
        private IUserService _userService;

        public RolesController(RoleManager<IdentityRole> roleManager, IHttpContextAccessor httpContextAccessor, IStringLocalizer<SharedResource> sharedLocalizer, IRolesRepository rolesRepository, IUserService userService)
        {
            _roleManager = roleManager;
            _httpContextAccessor = httpContextAccessor;
            _sharedLocalizer = sharedLocalizer;
            _rolesRepository = rolesRepository;
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

        public IActionResult Add()
        {
            return View("Add");
        }

        public IActionResult GetRolesJson()
        {
            try
            {
                var roles = _rolesRepository.GetAllRoles().ToList();

                var culture = GlobalFunctions.GetCulture(HttpContext);

                var result = roles.Select(role => new
                {
                    id = role.Id,
                    name = (culture == "sq-Al" ? role.Name : role.Name),
                    isstaffrole = role.Id == "2",
                });

                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
