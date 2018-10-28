using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Data.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.WebUI.ViewComponents
{
    public class CategoryMenuViewComponent : ViewComponent
    {
        public ICategoryRepository categoryRepository;

        public CategoryMenuViewComponent(ICategoryRepository _categoryRepository)
        {
            categoryRepository = _categoryRepository;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedCategory = RouteData?.Values["id"];
            return View(categoryRepository.GetAll());
        }
    }
}
