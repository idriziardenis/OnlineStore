using OnlineStore.Entities;
using OnlineStore.Enums;
using OnlineStore.Interfaces;
using OnlineStore.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace OnlineStore.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly IRolesRepository _rolesRepository;
        private HttpContext _httpContext { get { return _contextAccessor.HttpContext; } }

        public UserService(IHttpContextAccessor contextAccessor, UserManager<ApplicationUser> userManager, IUserRepository userRepository, IRolesRepository rolesRepository)
        {
            _contextAccessor = contextAccessor;
            _userManager = userManager;
            _userRepository = userRepository;
            _rolesRepository = rolesRepository;
            if (_httpContext.User.Identity.IsAuthenticated)
            {
                var id = userManager.GetUserId(_httpContext.User);
                CurrentUser = userRepository.GetByStringId(id);
                CurrentRole = _rolesRepository.GetByUserId(id);
            }
        }

        private AspNetUsers CurrentUser { get; set; }
        private AspNetRoles CurrentRole{ get; set; }
        public string GetUserEmail()
        {
            if (CurrentUser != null)
            {
                return CurrentUser.Email;
            }
            else
            {
                return "";
            }
        }

        public string GetUserId()
        {
            try
            {


                if (CurrentUser != null)
                {
                    return CurrentUser.Id;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string GetUserName()
        {
            if (CurrentUser != null)
            {
                return CurrentUser.UserName;
            }
            else
            {
                return "";
            }
        }

        public string GetUserPhoneNumber()
        {
            if (CurrentUser != null)
            {
                return CurrentUser.PhoneNumber;
            }
            else
            {
                return "";
            }
        }

        public string GetUserRole()
        {
            if (CurrentRole != null)
            {
                return CurrentRole.Name;
            }
            else
            {
                return "";
            }
        }

        public string GetFullName()
        {
            if (CurrentUser != null)
            {
                return CurrentUser.Name + " " + CurrentUser.Surname;
            }
            else
            {
                return "";
            }
        }

        public string FormatCookie(string cookie)
        {
            // c=sq-AL|uic=sq-AL
            var list = cookie.Split('=', StringSplitOptions.RemoveEmptyEntries).ToList();

            return list[list.Count - 1];
        }


        public string GetCulture(HttpContext httpContext)
        {
            var cookie = httpContext.Request.Cookies[CookieRequestCultureProvider.DefaultCookieName];

            if (cookie != null)
            {
                var formatedCookie = FormatCookie(cookie);

                return formatedCookie;
            }
            else
            {
                // Fallback
                return "sq-AL";
            }
        }
    }
}
