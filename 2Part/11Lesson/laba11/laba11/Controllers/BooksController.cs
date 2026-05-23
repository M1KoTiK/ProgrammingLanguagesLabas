using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using laba11.Data;
using laba11.Models;
using System.Linq;
using System.Threading.Tasks;

namespace laba11.Controllers
{
    public class BooksController : Controller
    {
        private readonly BooksDbContext _context;
        public BooksController(BooksDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var books = await _context.Books.ToListAsync();
            return View(books);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Book book)
        {
            if (string.IsNullOrWhiteSpace(book.Title) ||
                string.IsNullOrWhiteSpace(book.Author) ||
                book.Pages <= 0 ||
                book.Year <= 0)
            {
                ViewBag.Error = "Все поля обязательны. Страницы и год > 0.";
                return View(book);
            }

            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> SearchByAuthor(string author)
        {
            if (string.IsNullOrWhiteSpace(author))
            {
                var authors = await _context.Books
                    .Select(b => b.Author)
                    .Distinct()
                    .ToListAsync();
                return View("AuthorsList", authors);
            }
            else
            {
                var books = await _context.Books
                    .Where(b => b.Author.Contains(author))
                    .ToListAsync();
                return View("BooksByAuthor", books);
            }
        }
    }
}