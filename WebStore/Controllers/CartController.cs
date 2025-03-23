using Microsoft.AspNetCore.Mvc;
using WebStore.Controllers;
using WebStore.Database;
using WebStore.Models.Cart;
using WebStore.Entities;
using WebStore.Helpers;

namespace WebStore.Controllers
{
    public class CartController : BaseController
    {
        private const string CART_SESSION_KEY = "Cart";

        public CartController(DatabaseContext context) : base(context)
        {
        }

        // GET: /Cart/Index
        public IActionResult Index()
        {
            var cart = GetCartFromSession();
            return View(cart);
        }

        // POST: /Cart/Add
        [HttpPost]
        public IActionResult Add(int productId, int quantity = 1)
        {
            var product = DatabaseContext.Products.Find(productId);
            if (product == null)
            {
                // Produkt neexistuje
                return NotFound();
            }

            var cart = GetCartFromSession();

            // Hledáme, zda už položka v košíku je
            var existingItem = cart.FirstOrDefault(c => c.ProductId == productId);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductId = product.ID,
                    ProductName = product.Name,
                    Price = product.Price,
                    Quantity = quantity
                });
            }

            SaveCartToSession(cart);

            // Můžeme přesměrovat na košík
            return RedirectToAction("Index");
        }

        // POST: /Cart/Remove
        [HttpPost]
        public IActionResult Remove(int productId)
        {
            var cart = GetCartFromSession();
            var item = cart.FirstOrDefault(x => x.ProductId == productId);
            if (item != null)
            {
                cart.Remove(item);
                SaveCartToSession(cart);
            }
            return RedirectToAction("Index");
        }

        // POST: /Cart/Update
        [HttpPost]
        public IActionResult Update(int productId, int quantity)
        {
            var cart = GetCartFromSession();
            var item = cart.FirstOrDefault(x => x.ProductId == productId);
            if (item != null)
            {
                if (quantity > 0)
                {
                    item.Quantity = quantity;
                }
                else
                {
                    cart.Remove(item);
                }
                SaveCartToSession(cart);
            }
            return RedirectToAction("Index");
        }

        // Případně akce pro vyprázdnění košíku
        [HttpPost]
        public IActionResult Clear()
        {
            SaveCartToSession(new List<CartItem>());
            return RedirectToAction("Index");
        }

        // Pomocné metody pro čtení/uložení do session
        private List<CartItem> GetCartFromSession()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>(CART_SESSION_KEY);
            if (cart == null)
            {
                cart = new List<CartItem>();
            }
            return cart;
        }

        private void SaveCartToSession(List<CartItem> cart)
        {
            HttpContext.Session.SetObjectAsJson(CART_SESSION_KEY, cart);
        }
    }
}
