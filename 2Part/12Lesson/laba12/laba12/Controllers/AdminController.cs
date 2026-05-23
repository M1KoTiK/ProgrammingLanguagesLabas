using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using laba12.Data;
using laba12.Models;
using System.Linq;

namespace laba12.Controllers
{
    public class AdminController : Controller
    {
        private readonly UsersDbContext _context;
        public AdminController(UsersDbContext context)
        {
            _context = context;
        }

        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("UserRole") == "admin";
        }

        public IActionResult Users()
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            var users = _context.Users.ToList();
            return View(users);
        }

        [HttpPost]
        public IActionResult UpdateBalance(int userId, decimal newBalance)
        {
            if (!IsAdmin())
                return Json(new { success = false, message = "Доступ запрещён" });

            var user = _context.Users.Find(userId);
            if (user == null)
                return Json(new { success = false, message = "Пользователь не найден" });

            user.Balance = newBalance;
            _context.SaveChanges();
            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult DeleteUser(int userId)
        {
            if (!IsAdmin())
                return Json(new { success = false, message = "Доступ запрещён" });

            var user = _context.Users.Find(userId);
            if (user == null)
                return Json(new { success = false, message = "Пользователь не найден" });

            _context.Users.Remove(user);
            _context.SaveChanges();
            return Json(new { success = true });
        }
    }
}