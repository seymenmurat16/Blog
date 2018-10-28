using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Data.Abstract;
using BlogApp.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlogApp.WebUI.Controllers
{
    public class BlogController : Controller
    {
        private IBlogRepository blogRepository;
        private ICategoryRepository categoryRepository;
        public BlogController(IBlogRepository _blogRepository, ICategoryRepository _categoryRepository)
        {
            blogRepository = _blogRepository;
            categoryRepository = _categoryRepository;
        }

        public IActionResult Index(int? id,string q)
        {
            if (id == null)
            {
                if (!string.IsNullOrEmpty(q))
                {
                    return View(blogRepository.GetAll().Where(i=>i.isApproved).Where(i => i.Title.Contains(q) || i.Description.Contains(q) || i.Body.Contains(q)).OrderByDescending(i => i.Date));
                }
                return View(blogRepository.GetAll().Where(i => i.isApproved).OrderByDescending(i => i.Date));
            }
            else
            {
                if (!string.IsNullOrEmpty(q))
                {
                    return View(blogRepository.GetAll().Where(i => i.isApproved).Where(i => i.Title.Contains(q) || i.Description.Contains(q) || i.Body.Contains(q)).OrderByDescending(i => i.Date));
                }
                return View(blogRepository.GetAll().Where(i => i.isApproved && i.CategoryId == id).OrderByDescending(i => i.Date));
            }
            
            
        }

        public IActionResult List()
        {
            return View(blogRepository.GetAll());
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(categoryRepository.GetAll(), "CategoryId", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Blog model,IFormFile file)
        {
            if(file != null)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", file.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                model.Image = file.FileName;
            }
            
            model.Date = DateTime.Now;
            if (ModelState.IsValid)
            {
                blogRepository.AddBlog(model);
                return RedirectToAction("List");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            var blog = blogRepository.GetById(id);
            ViewBag.Categories = new SelectList(categoryRepository.GetAll(), "CategoryId", "Name");
            return View(blog);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Blog model,IFormFile file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", file.FileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    model.Image = file.FileName;
                }
                
                blogRepository.UpdateBlog(model);
                TempData["message"] = $"{model.Title} güncellendi.";
                return RedirectToAction("List");
            }
            ViewBag.Categories = new SelectList(categoryRepository.GetAll(), "CategoryId", "Name");
            return View(model);
        }

        public IActionResult Delete(int id)
        {
            TempData["message"] = $"{blogRepository.GetById(id).Title} silindi.";
            blogRepository.DeleteBlog(id);
            return RedirectToAction("List");
        }

        public IActionResult Detail(int id)
        {
            return View(blogRepository.GetById(id));
        }
    }
}