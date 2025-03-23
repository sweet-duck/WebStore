using Microsoft.AspNetCore.Mvc;
using WebStore.Models;
using WebStore.Database;
using WebStore.Helpers;
using WebStore.Models.Cart;
using WebStore.Entities;

namespace WebStore.Controllers
{
    public class OrderController : BaseController
    {
        private readonly DatabaseContext _db;

        public OrderController(DatabaseContext db) : base(db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Checkout()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart")
                       ?? new List<CartItem>();

            cart = cart.Where(x =>
            {
                var query = _db.Items.Where(item => item.ProductID == x.ProductId).AsEnumerable().Where(item => item.IsInStock());

                if (query == null || !query.Any()) return false;

                x.Quantity = Math.Min(x.Quantity, query.Count());
                return true;
            }).ToList();

            HttpContext.Session.SetObjectAsJson("Cart", cart);

            if (cart.Count == 0) return RedirectToAction("Index", "Home");

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

            /* if (!ModelState.IsValid)
             {
                 Console.WriteLine("Někde nastala chyba?!");
                 model.CartItems = cart;
                 return View(model);
             }*/

            Console.WriteLine(model);

            if (!cart.Any()) return RedirectToAction("Index", "Home");

            foreach (var x in cart)
            {
                var items = _db.Items.Where(item => item.ProductID == x.ProductId).AsEnumerable().Where(item => item.IsInStock()).ToList();

                foreach (var item in items)
                {
                    item.Status = "Na cestě";

                    int userId = int.Parse(ViewBag.UserID);

                    var order = new Order(userId, item.ID, model.DeliveryMethod, model.PaymentMethod);
                    _db.Orders.Add(order);
                }
            }

            _db.SaveChanges();

            HttpContext.Session.Remove("Cart");
            return RedirectToAction("Index", "Home");
        }
    }
}
