using AdminBlog.Filter;
using AdminBlog.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AdminBlog.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BlogContext _context;

        public HomeController(ILogger<HomeController> logger, BlogContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Login(string Email, string Password)
        {
            var author =_context.Author.FirstOrDefault(x => x.Email == Email && x.Password == Password);
            if (author == null)
                return RedirectToAction(nameof(Index));

            HttpContext.Session.SetInt32("id",author.Id);
            return RedirectToAction(nameof(Category));
        }


        public IActionResult Category()
        {
            List<Category> categories = _context.Categories.ToList();
            return View(categories);
        }

        public async Task<IActionResult> AddCategory(Category category)
        {
            if(category.Id == 0)
            {
                await _context.AddAsync(category);
            }
            else
            {
                _context.Update(category);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Category));
        }

        public async Task<IActionResult> CategoryDetails(int Id)
        {
            var category = await _context.Categories.FindAsync(Id);
            return Json(category);
        }

        public async Task<IActionResult> DeleteCategory(int? Id)
        {
            Category category = await _context.Categories.FindAsync(Id);
            _context.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Category));
        }

        public IActionResult Author()
        {
            List<Author> authors = _context.Author.ToList();
            return View(authors);
        }

        public async Task<IActionResult> AddAuthor(Author author)
        {
            if (author.Id == 0)
            {
                await _context.AddAsync(author);
            }
            else
            {
                _context.Update(author);
            }
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Author));
        }


        public async Task<IActionResult> AuthorDetails(int Id)
        {
            var author = await _context.Author.FindAsync(Id);
            return Json(author);
        }

        public async Task<IActionResult> DeleteAuthor(int? Id)
        {
            Author author = await _context.Author.FindAsync(Id);
            _context.Remove(author);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Author));
        }

        

        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}