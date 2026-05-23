using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading.Tasks;

namespace laba10.Controllers
{
    public class AccountController : Controller
    {
        private readonly IWebHostEnvironment _env;
        public AccountController(IWebHostEnvironment env)
        {
            _env = env;
        }

        private string GetUsersFolder()
        {
            return Path.Combine(_env.ContentRootPath, "App_Data", "users");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string login, string password)
        {
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Логин и пароль обязательны";
                return View();
            }

            var usersFolder = GetUsersFolder();
            if (!Directory.Exists(usersFolder))
                Directory.CreateDirectory(usersFolder);

            var filePath = Path.Combine(usersFolder, $"{login}.txt");
            if (System.IO.File.Exists(filePath))
            {
                ViewBag.Error = "Пользователь уже существует";
                return View();
            }

            await System.IO.File.WriteAllTextAsync(filePath, password);
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string login, string password)
        {
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Введите логин и пароль";
                return View();
            }

            var filePath = Path.Combine(GetUsersFolder(), $"{login}.txt");
            if (!System.IO.File.Exists(filePath))
            {
                ViewBag.Error = "Неверный логин или пароль";
                return View();
            }

            var savedPassword = await System.IO.File.ReadAllTextAsync(filePath);
            if (savedPassword == password)
            {
                TempData["Message"] = $"Добро пожаловать, {login}!";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Error = "Неверный логин или пароль";
                return View();
            }
        }
    }
}