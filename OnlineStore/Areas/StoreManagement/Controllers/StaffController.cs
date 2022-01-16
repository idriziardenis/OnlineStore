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
    public class StaffController : Controller
    {
        private IHttpContextAccessor _httpContextAccessor;
        private IStringLocalizer<SharedResource> _sharedLocalizer;
        private IUserRepository _userRepository;
        private IRolesRepository _rolesRepository;
        private IUserService _userService;
        private ISelectListService _selectListService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public StaffController(IHttpContextAccessor httpContextAccessor, IStringLocalizer<SharedResource> sharedLocalizer, IUserRepository userRepository, IRolesRepository rolesRepository, IUserService userService, ISelectListService selectListService, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _sharedLocalizer = sharedLocalizer;
            _userRepository = userRepository;
            _rolesRepository = rolesRepository;
            _userService = userService;
            _selectListService = selectListService;
            _userManager = userManager;
            _signInManager = signInManager;
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

        [HttpGet]
        public IActionResult Add()
        {
            try
            {
                var model = new StaffViewModel();
                model.Id = null;
                model.Genders = new SelectList(_selectListService.GetGendersKeysValues(_sharedLocalizer["Man"].Value, _sharedLocalizer["Woman"].Value), "BKey", "Value", selectedValue: model.Gender);
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
        public async Task<IActionResult> AddAsync(StaffViewModel model, IFormFile ProfilePicture)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    model.Id = null;
                    model.Genders = new SelectList(_selectListService.GetGendersKeysValues(_sharedLocalizer["Man"].Value, _sharedLocalizer["Woman"].Value), "BKey", "Value", selectedValue: model.Gender);
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
                            RoleId = "2"
                        };

                        _rolesRepository.AddUserRole(userRole);
                    }

                }
                else
                {

                }

                var resultMsg = new SuccessResult(true, _sharedLocalizer["ChangesSavedSuccessfully"]);

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
                var model = new StaffViewModel();
                if (user == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    model.Id = user.Id;
                    model.Name = user.Name;
                    model.Surname = user.Surname;
                    model.Email = user.Email;
                    model.Gender = user.Gender;
                    model.PhoneNumber = user.PhoneNumber;
                    model.Birthday = user.Birthday;
                    model.IsActive = user.IsActive.Value;

                    model.Genders = new SelectList(_selectListService.GetGendersKeysValues(_sharedLocalizer["Man"].Value, _sharedLocalizer["Woman"].Value), "BKey", "Value", selectedValue: model.Gender);
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
        public async Task<IActionResult> EditAsync(StaffViewModel model, IFormFile ProfilePicture)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    model.Id = null;
                    model.Genders = new SelectList(_selectListService.GetGendersKeysValues(_sharedLocalizer["Man"].Value, _sharedLocalizer["Woman"].Value), "BKey", "Value", selectedValue: model.Gender);
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

                var resultMsg = new SuccessResult(true, _sharedLocalizer["ChangesSavedSuccessfully"]);

                TempData["SuccessResultF"] = resultMsg.Success;
                TempData["SuccessResultM"] = resultMsg.Message;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public IActionResult GetStaffJson()
        {
            try
            {
                var staffs = _userRepository.GetAllStaff().OrderByDescending(x => x.Id).ToList();
                var result = staffs.Select(staff => new
                {
                    id = staff.Id,
                    name = staff.Name + " " + staff.Surname,
                    email = staff.Email,
                    role = _rolesRepository.GetByUserId(staff.Id).Name,
                    registereddate = (staff.CreateOnDate.HasValue ? staff.CreateOnDate.ToString() : null),
                    isactive = staff.IsActive.Value
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
                var staff = _userRepository.GetByStringId(id);
                if (staff != null)
                {
                    staff.IsDeleted = true;
                    staff.LastModifiedByUserId = _userService.GetUserId();
                    staff.LastModifiedOnDate = DateTime.Now;
                    _userRepository.Update(staff);
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
