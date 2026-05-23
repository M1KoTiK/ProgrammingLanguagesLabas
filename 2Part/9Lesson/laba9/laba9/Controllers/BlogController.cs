using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using laba9.Data;
using laba9.Models;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace laba9.Controllers
{
    public class BlogController : Controller
    {
        private readonly BlogDBContext _context;
        public BlogController(BlogDBContext context) => _context = context;

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetRecords()
        {
            var records = await _context.BlogRecords
                .OrderByDescending(r => r.Date)
                .ToListAsync();
            return Json(records);
        }

        [HttpPost]
        public async Task<IActionResult> AddRecord([FromBody] string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                return BadRequest("Message cannot be empty");

            var record = new BlogRecord
            {
                Date = DateTime.Now,
                Message = message.Trim()
            };
            _context.BlogRecords.Add(record);
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }
    }
}