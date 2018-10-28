using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Data.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private IBlogRepository blogRepository;

        public HomeController(IBlogRepository _blogRepository)
        {
            blogRepository = _blogRepository;
        }

        public IActionResult Index()
        {
            return View(blogRepository.GetAll().Where(i=>i.isApproved && i.isHome));
        }

        public IActionResult List()
        {
            return View();
        }
        
        public IActionResult Detail()
        {
            return View();
        }
    }
}