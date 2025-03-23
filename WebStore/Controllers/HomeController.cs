using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebStore.Database;
using WebStore.Entities;
using WebStore.Models.Cart;

namespace WebStore.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(DatabaseContext context) : base(context) { }

        public IActionResult Index()
        {

            var featuredProducts = DatabaseContext.Products
                .Where(p => p.Sale != 0)
                .Take(4)
                .ToList();

            ViewBag.FeaturedProducts = featuredProducts;

            return View();
        }

        [HttpPost]
        public IActionResult Search(string search)
        {
            if (string.IsNullOrEmpty(search))
                return RedirectToAction("Index");

            return RedirectToAction("SearchList", "Category", new { query = search });

        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
