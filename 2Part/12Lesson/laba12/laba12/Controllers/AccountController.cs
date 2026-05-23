using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using laba12.Data;
using laba12.Models;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;

namespace laba12.Controllers
{
    public class AccountController : Controller
    {
        private readonly UsersDbContext _context;
        public AccountController(UsersDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(string login, string password, string confirmPassword)
        {
            if (password != confirmPassword)
            {
                ViewBag.Error = "Пароль и подтверждение не совпадают";
                return View();
            }

            if (!Regex.IsMatch(login, @"^[a-zA-Z0-9]{6,}$"))
            {
                ViewBag.Error = "Логин должен быть не менее 6 символов и содержать только английские буквы и цифры";
                return View();
            }
            if (!Regex.IsMatch(password, @"^[a-zA-Z0-9]{6,}$"))
            {
                ViewBag.Error = "Пароль должен быть не менее 6 символов и содержать только английские буквы и цифры";
                return View();
            }

            if (_context.Users.Any(u => u.Login == login))
            {
                ViewBag.Error = "Пользователь с таким логином уже существует";
                return View();
            }

            var userRole = _context.Roles.FirstOrDefault(r => r.Name == "user");
            if (userRole == null)
            {
                ViewBag.Error = "Роль пользователя не найдена";
                return View();
            }

            var user = new User
            {
                Login = login,
                Password = password,
                Balance = 0,
                RoleId = userRole.Id
            };
            _context.Users.Add(user);
            _context.SaveChanges();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string login, string password)
        {
            var user = _context.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Login == login && u.Password == password);
            if (user == null)
            {
                ViewBag.Error = "Неверный логин или пароль";
                return View();
            }
            HttpContext.Session.SetString("UserId", user.Id.ToString());
            HttpContext.Session.SetString("UserLogin", user.Login);
            HttpContext.Session.SetString("UserRole", user.Role?.Name ?? "user");
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}