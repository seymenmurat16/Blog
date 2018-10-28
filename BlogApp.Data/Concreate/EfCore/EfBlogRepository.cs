using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlogApp.Data.Abstract;
using BlogApp.Entity;

namespace BlogApp.Data.Concreate.EfCore
{
    public class EfBlogRepository : IBlogRepository
    {
        BlogContext context;

        public EfBlogRepository(BlogContext _context)
        {
            context = _context;
        }

        public void AddBlog(Blog entity)
        {
            context.Blogs.Add(entity);
            context.SaveChanges();
        }

        public void DeleteBlog(int blogId)
        {
            var blog = context.Blogs.FirstOrDefault(p => p.BlogId == blogId);
            if (blog != null)
            {
                context.Blogs.Remove(blog);
                context.SaveChanges();
            }
        }

        public IQueryable<Blog> GetAll()
        {
            return context.Blogs;
        }

        public Blog GetById(int blogId)
        {
            return context.Blogs.FirstOrDefault(p => p.BlogId == blogId);
        }

        public void UpdateBlog(Blog entity)
        {
            var blog = GetById(entity.BlogId);
            if(blog != null)
            {
                blog.Title = entity.Title;
                blog.Description = entity.Description;
                blog.CategoryId = entity.CategoryId;
                blog.Image = entity.Image;
                blog.Body = entity.Body;
                blog.isApproved = entity.isApproved;
                blog.isHome = entity.isHome;
                context.SaveChanges();
            }
            context.SaveChanges();
        }

    }
}
