using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using OnlineStore.Areas.StoreManagement.Models;
using OnlineStore.Data;
using OnlineStore.Entities;
using OnlineStore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Areas.StoreManagement.Controllers
{
    [Area("StoreManagement")]
    [Authorize(Policy = "Admin")]
    public class ClientsController : Controller
    {
        private IHttpContextAccessor _httpContextAccessor;
        private IUserRepository _userRepository;
        private IRolesRepository _rolesRepository;
        private IUserService _userService;
        private ISelectListService _selectListService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private IStringLocalizer<SharedResource> _sharedLocalizer;

        public ClientsController(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository, IRolesRepository rolesRepository, IUserService userService, ISelectListService selectListService, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IStringLocalizer<SharedResource> sharedLocalizer)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
            _rolesRepository = rolesRepository;
            _userService = userService;
            _selectListService = selectListService;
            _userManager = userManager;
            _signInManager = signInManager;
            _sharedLocalizer = sharedLocalizer;
        }

        public IActionResult Index()
        {
            try
            {
                var b = _sharedLocalizer["Man"];
                var a = _sharedLocalizer["Man"].Value;
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

        [HttpGet]
        public IActionResult Add()
        {
            try
            {
                var model = new ClientViewModel();
                model.Id = null;
                model.Genders = new SelectList(_selectListService.GetGendersKeysValues(), "BKey", "Value", selectedValue: model.Gender);
                model.IsActive = true;
                return View(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAsync(ClientViewModel model, IFormFile ProfilePicture)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    model.Id = null;
                    model.Genders = new SelectList(_selectListService.GetGendersKeysValues(), "BKey", "Value", selectedValue: model.Gender);
                    model.IsActive = true;

                    return View("Add", model);
                }

                var user = new ApplicationUser();
                if (string.IsNullOrEmpty(model.Id))
                {
                    user.Name = model.Name;
                    user.Surname = model.Surname;
                    user.UserName = model.Email;
                    user.Email = model.Email;
                    user.Gender = model.Gender;
                    user.PhoneNumber = model.PhoneNumber;
                    user.Birthday = model.Birthday;
                    user.CreateByUserId = _userService.GetUserId();
                    user.CreateOnDate = DateTime.Now;
                    user.IsActive = model.IsActive;
                    user.IsDeleted = false;
                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {

                        var userRole = new AspNetUserRoles()
                        {
                            UserId = user.Id,
                            RoleId = "1",
                        };

                        _rolesRepository.AddUserRole(userRole);

                    }

                }

                var resultMsg = new SuccessResult(true, "Changes Saved Successfully!");

                TempData["SuccessResultF"] = resultMsg.Success;
                TempData["SuccessResultM"] = resultMsg.Message;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpGet]
        public async Task<IActionResult> EditAsync(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                var model = new ClientViewModel();
                if (user == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    var roleId = _rolesRepository.GetByUserId(id).Id;
                    if (roleId != "4")
                    {
                        return RedirectToAction("Index");
                    }
                    model.Id = user.Id;
                    model.Name = user.Name;
                    model.Surname = user.Surname;
                    model.Email = user.Email;

                    model.Gender = user.Gender;
                    model.PhoneNumber = user.PhoneNumber;
                    model.Birthday = user.Birthday;
                    model.IsActive = user.IsActive.Value;

                    model.Genders = new SelectList(_selectListService.GetGendersKeysValues(), "BKey", "Value", selectedValue: model.Gender);
                }

                return View(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(ClientViewModel model, IFormFile ProfilePicture)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    model.Id = null;
                    model.Genders = new SelectList(_selectListService.GetGendersKeysValues(), "BKey", "Value", selectedValue: model.Gender);
                    model.IsActive = true;

                    return View("Add", model);
                }

                var user = await _userManager.FindByIdAsync(model.Id);
                if (user == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    user.Name = model.Name;
                    user.Surname = model.Surname;
                    user.Gender = model.Gender;
                    user.PhoneNumber = model.PhoneNumber;
                    user.Birthday = model.Birthday;
                    user.LastModifiedByUserId = _userService.GetUserId();
                    user.LastModifiedOnDate = DateTime.Now;
                    user.IsActive = model.IsActive;

                    await _userManager.UpdateAsync(user);

                    if (model.IsPasswordChanged)
                    {
                        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                        var result = await _userManager.ResetPasswordAsync(user, token, model.Password);
                    }
                }

                var resultMsg = new SuccessResult(true, "Changes Saved Successfully!");

                TempData["SuccessResultF"] = resultMsg.Success;
                TempData["SuccessResultM"] = resultMsg.Message;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public IActionResult GetClientsJson()
        {
            try
            {
                var clients = _userRepository.GetAllClients().OrderByDescending(x => x.Id).ToList();

                var result = clients.Select(client => new
                {
                    id = client.Id,
                    name = client.Name + " " + client.Surname,
                    email = client.Email,
                    phone = client.PhoneNumber,
                    registereddate = (client.CreateOnDate.HasValue ? client.CreateOnDate.ToString() : null),
                    isactive = client.IsActive.Value
                });

                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpDelete]
        public IActionResult Delete(string id)
        {
            try
            {
                var client = _userRepository.GetByStringId(id);
                if (client != null)
                {
                    client.IsDeleted = true;
                    client.LastModifiedByUserId = _userService.GetUserId();
                    client.LastModifiedOnDate = DateTime.Now;
                    _userRepository.Update(client);
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
