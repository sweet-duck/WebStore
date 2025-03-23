using Microsoft.AspNetCore.Mvc;
using WebStore.Database;
using WebStore.Entities;
using WebStore.Helpers;
using WebStore.Models.Auth;

namespace WebStore.Controllers
{
    public class AuthController : BaseController
    {

        public AuthController(DatabaseContext context) : base(context) { }

        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }

            Account? account = DatabaseContext.Accounts.SingleOrDefault(a => a.Username == loginViewModel.Username);
            if (account == null || account.Password.ToLower() != SHA256Helper.HashPassword(loginViewModel.Password).ToLower())
            {
                TempData["Message"] = "Wrong username or password!";
                TempData["MessageType"] = "danger";
                return View(loginViewModel);
            }

            HttpContext.Session.SetString("User", account.Username);
            HttpContext.Session.SetString("UserID", account.ID + "");
            HttpContext.Session.SetString("Role", account.Role);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(registerViewModel);
            }

            Console.WriteLine("Registrace..1");

            Account? account = DatabaseContext.Accounts.SingleOrDefault(a => a.Username == registerViewModel.Username);
            if (account != null)
            {
                TempData["Message"] = "Toto jméno je již zabrané!";
                TempData["MessageType"] = "danger";
                return View(registerViewModel);
            }

            
            if (registerViewModel.Password != registerViewModel.PasswordAgain)
            {
                TempData["Message"] = "Hesla se neshodují!";
                TempData["MessageType"] = "danger";
                return View(registerViewModel);
            }

            string passwordHash = SHA256Helper.HashPassword(registerViewModel.Password);

            Account newAccount = new Account(registerViewModel.Username, passwordHash, "user");
            DatabaseContext.Accounts.Add(newAccount);
            DatabaseContext.SaveChanges();

            HttpContext.Session.SetString("User", newAccount.Username);
            HttpContext.Session.SetString("UserID", newAccount.ID + "");
            HttpContext.Session.SetString("Role", newAccount.Role);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("User");
            HttpContext.Session.Remove("UserID");
            HttpContext.Session.Remove("Role");

            return RedirectToAction("Index", "Home");
        }
    }
}
