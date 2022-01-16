using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using OnlineStore.Areas.StoreManagement.Models;
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
    public class CategoryController : Controller
    {
        private ISelectListService _selectListService;
        private IUserService _userService;
        private IStringLocalizer<SharedResource> _sharedLocalizer;
        public IWebHostEnvironment _env { get; }
        private IHttpContextAccessor _httpContextAccessor;
        private ICategoryRepository _categoryRepository;

        public CategoryController(ISelectListService selectListService, IUserService userService, IStringLocalizer<SharedResource> sharedLocalizer, IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor, ICategoryRepository categoryRepository)
        {
            _selectListService = selectListService;
            _userService = userService;
            _sharedLocalizer = sharedLocalizer;
            _env = env;
            _httpContextAccessor = httpContextAccessor;
            _categoryRepository = categoryRepository;
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
                var model = new CategoryViewModel();

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

                var category = _categoryRepository.GetById(id);

                var model = new CategoryViewModel();
                model.Id = category.Id;
                model.Name = category.Name;
                model.Description = category.Description;
                model.IsActive = category.IsActive.GetValueOrDefault();

                return View("Add", model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public IActionResult Add(CategoryViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("Add", model);
                }
                if (model.Id == 0)
                {
                    var category = new Category();
                    category.Name = model.Name;
                    category.Description = model.Description;
                    category.IsActive = model.IsActive;
                    category.IsDeleted = false;
                    category.Description = model.Description;
                    category.CreatedByUserId = _userService.GetUserId();
                    category.CreatedOnDate = DateTime.Now;
                    _categoryRepository.Add(category);

                }
                else
                {

                    var category = _categoryRepository.GetById(model.Id);
                    category.Name = model.Name;
                    category.Description = model.Description;
                    category.IsActive = model.IsActive;
                    category.LastModifiedByUserId = _userService.GetUserId();
                    category.LastModifiedOnDate = DateTime.Now;
                    _categoryRepository.Update(category);
                }
                var result = new SuccessResult(true, _sharedLocalizer["ChangesSavedSuccessfully"]);

                TempData["SuccessResultF"] = result.Success;
                TempData["SuccessResultM"] = result.Message;

                return RedirectToAction("Index", "Category");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IActionResult GetCategoriesJson()
        {
            try
            {
                var categories = _categoryRepository.GetAll().OrderByDescending(x => x.Id).ToList();

                var result = categories.Select(category => new
                {
                    id = category.Id,
                    name = category.Name,
                    isActive = category.IsActive
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
                var category = _categoryRepository.GetById(id);
                if (category != null)
                {
                    category.IsDeleted = true;
                    category.LastModifiedByUserId = _userService.GetUserId();
                    category.LastModifiedOnDate = DateTime.Now;
                    _categoryRepository.Update(category);
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
