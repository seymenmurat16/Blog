using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Data.Abstract;
using BlogApp.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlogApp.WebUI.Controllers
{
    public class CategoryController : Controller
    {
        private ICategoryRepository categoryRepository;
        private IBlogRepository blogRepository;
        public CategoryController(ICategoryRepository _categoryRepository, IBlogRepository _blogRepository)
        {
            categoryRepository = _categoryRepository;
            blogRepository = _blogRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult List()
        {
            return View(categoryRepository.GetAll());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category model)
        {
            if (ModelState.IsValid)
            {
                categoryRepository.AddCategory(model);
                return RedirectToAction("List");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult AddUpdate(int? id)
        {
            if(id == null) {
            return View(new Category());
            }
            else
            {
                var category = categoryRepository.GetById((int)id);
                return View(category);
            }
        }

        [HttpPost]
        public IActionResult AddUpdate(Category model)
        {
            if (ModelState.IsValid)
            {
                categoryRepository.SaveCategory(model);
                TempData["message"] = $"{model.Name} kayıt edildi.";
                return RedirectToAction("List");
            }
            return View(model);
        }

        public IActionResult Delete(int id)
        {
            TempData["message"] = $"{categoryRepository.GetById(id).Name} silindi.";
            categoryRepository.DeleteCategory(id);
            return RedirectToAction("List");
        }
    }
}