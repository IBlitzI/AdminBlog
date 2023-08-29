﻿using AdminBlog.Filter;
using AdminBlog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace AdminBlog.Controllers
{
    
    public class BlogController : Controller
    {
        private readonly ILogger<BlogController> _logger;
        private readonly BlogContext _context;

        public BlogController(ILogger<BlogController> logger, BlogContext context)
        {
            _logger = logger;
            _context = context;
        }



        public IActionResult Index()
        {
            var list = _context.Blogs.ToList();

            return View(list);
        }

        public IActionResult Blog()
        {
            ViewBag.Categories = _context.Categories.Select(w =>
                new SelectListItem
                {
                    Text = w.Name,
                    Value = w.Id.ToString()
                }
            ).ToList();
            return View();
        }

        public IActionResult Publish(int Id)
        {
            var blog = _context.Blogs.Find(Id);
            blog.IsPublish = true;
            _context.Update(blog);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Save(Blog model)
        {
            if(model != null)
            {
                var file = Request.Form.Files.First();
                //Blog için eklenen resimlerin kaydedilmesi istedinilen yer.
                string savePath = Path.Combine("C:","Users","cls","Desktop","BlogDeneme","MyBlog","MyBlog","wwwroot","img");
                var fileName = $"{DateTime.Now:MMddHHmmss}.{file.FileName.Split(".").Last()}";
                var fileUrl = Path.Combine(savePath,fileName);
                using(var fileStream = new FileStream(fileUrl, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                model.ImagePath = fileName;
                model.AuthorId =(int) HttpContext.Session.GetInt32("id");
                await _context.AddAsync(model);
                await _context.SaveChangesAsync();
                return Json(true);
            }
            return Json(false);
        }

        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}