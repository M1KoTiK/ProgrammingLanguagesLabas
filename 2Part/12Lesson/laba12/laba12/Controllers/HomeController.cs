using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using laba12.Data;
using laba12.Models;
using System.Linq;
using System.Text.RegularExpressions;

namespace laba12.Controllers
{
    public class HomeController : Controller
    {
        private readonly UsersDbContext _context;
        public HomeController(UsersDbContext context)
        {
            _context = context;
        }

        private bool IsAuthenticated()
        {
            return HttpContext.Session.GetString("UserId") != null;
        }

        private int GetCurrentUserId()
        {
            return int.Parse(HttpContext.Session.GetString("UserId"));
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (!IsAuthenticated())
                return RedirectToAction("Login", "Account");

            var userId = GetCurrentUserId();
            var user = _context.Users.Find(userId);
            if (user == null) return RedirectToAction("Login", "Account");

            ViewBag.UserLogin = user.Login;
            ViewBag.Balance = user.Balance;
            ViewBag.Role = HttpContext.Session.GetString("UserRole");
            return View();
        }

        [HttpPost]
        public IActionResult Transfer(string toLogin, decimal amount)
        {
            if (!IsAuthenticated())
                return Json(new { success = false, message = "Не авторизован" });

            if (amount <= 0)
                return Json(new { success = false, message = "Сумма перевода должна быть положительной" });

            var senderId = GetCurrentUserId();
            var sender = _context.Users.Find(senderId);
            if (sender == null)
                return Json(new { success = false, message = "Отправитель не найден" });

            if (sender.Balance < amount)
                return Json(new { success = false, message = "Недостаточно средств" });

            var receiver = _context.Users.FirstOrDefault(u => u.Login == toLogin);
            if (receiver == null)
                return Json(new { success = false, message = "Получатель не найден" });

            sender.Balance -= amount;
            receiver.Balance += amount;
            _context.SaveChanges();
            return Json(new { success = true, newBalance = sender.Balance });
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            if (!IsAuthenticated())
                return RedirectToAction("Login", "Account");
            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(string oldPassword, string newPassword, string confirmPassword)
        {
            if (!IsAuthenticated())
                return RedirectToAction("Login", "Account");

            if (newPassword != confirmPassword)
            {
                ViewBag.Error = "Новый пароль и подтверждение не совпадают";
                return View();
            }
            if (!Regex.IsMatch(newPassword, @"^[a-zA-Z0-9]{6,}$"))
            {
                ViewBag.Error = "Пароль должен быть не менее 6 символов и содержать только английские буквы и цифры";
                return View();
            }

            var userId = GetCurrentUserId();
            var user = _context.Users.Find(userId);
            if (user.Password != oldPassword)
            {
                ViewBag.Error = "Неверный старый пароль";
                return View();
            }

            user.Password = newPassword;
            _context.SaveChanges();
            ViewBag.Message = "Пароль успешно изменён";
            return View();
        }
    }
}