using Microsoft.AspNetCore.Mvc;
using WebStore.Models;
using WebStore.Database;
using WebStore.Helpers;
using WebStore.Models.Cart;

namespace WebStore.Controllers
{
    public class OrderController : Controller
    {
        private readonly DatabaseContext _db;

        public OrderController(DatabaseContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Checkout()
        {
            // Načíst položky košíku ze session
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart")
                       ?? new List<CartItem>();

            var model = new CheckoutViewModel
            {
                CartItems = cart
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Checkout(CheckoutViewModel model)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart")
                       ?? new List<CartItem>();

            if (!ModelState.IsValid)
            {
                model.CartItems = cart;
                return View(model);
            }

            if (!cart.Any()) return RedirectToAction("Index", "Home");
            

            // Tady vytvoříte entitu Order, uložíte do DB atd.
            // Např.:
            // var order = new Order { FullName = model.FullName, Email = model.Email, ... };
            // _db.Orders.Add(order);
            // _db.SaveChanges();

            // Vyčistit košík
            HttpContext.Session.Remove("Cart");

            // Přejít na stránku s poděkováním / potvrzením
            return RedirectToAction("Confirmation", new { /* orderId = order.Id */ });
        }

        public IActionResult Confirmation(int orderId)
        {
            // Můžete načíst objednávku z DB a zobrazit
            // var order = _db.Orders.Find(orderId);
            // if(order == null) ...
            return View();
        }
    }
}
