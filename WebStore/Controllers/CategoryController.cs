using Microsoft.AspNetCore.Mvc;
using WebStore.Database;
using WebStore.Entities;

namespace WebStore.Controllers
{
    public class CategoryController : BaseController
    {
        public CategoryController(DatabaseContext context) : base(context) { }

        public IActionResult List(int id, int? minPrice, int? maxPrice, int? minRating, bool inStock = false, string? brand = null)
        {
            var category = DatabaseContext.Categories.FirstOrDefault(c => c.ID == id);
            if (category == null)
                return NotFound();

            SaveFilters(minPrice, maxPrice, minRating, inStock, brand);

            var query = ApplyFilters(DatabaseContext.Products
                .Where(p => p.CategoryID == id)
                .AsQueryable(), minPrice, maxPrice, minRating, inStock, brand);


            var products = query.ToList();
            ViewBag.Products = products;

            return View(category);
        }

        public IActionResult SearchList(string query, int? minPrice, int? maxPrice, int? minRating, bool inStock = false, string? brand = null)
        {
            var productsId = ApplyFilters(DatabaseContext.Products
                .Where(p => p.Name.ToLower().Contains(query.ToLower()) || p.Description.ToLower().Contains(query.ToLower()))
                .AsQueryable(), minPrice, maxPrice, minRating, inStock, brand).Select(p => p.ID).ToList();

            Category category = new Category();
            category.Name = "Vyhledávání...";

            var products = DatabaseContext.Products
                .Where(p => productsId.Contains(p.ID))
                .ToList();

            int count = products.Count;

            if (count == 0) category.Description = "Žádný produkt nenalezen.";
            else if (count == 1) category.Description = "Nalezen " + products.Count + " produkt.";
            else if(count == 2 || count == 3 || count == 4) category.Description = "Nalezeny " + products.Count + " produkty.";
            else category.Description = "Nalezeno " + products.Count + " produktů";

            if (category == null)
                return NotFound();

            ViewBag.Search = query;

            SaveFilters(minPrice, maxPrice, minRating, inStock, brand);

            ViewBag.Products = products;
            return View(category);
        }

        private void SaveFilters(int? minPrice, int? maxPrice, int? minRating, bool inStock = false, string? brand = null)
        {
            var avgRatings = DatabaseContext.Reviews
                .Where(r => r.DeletedAt == null)
                .GroupBy(r => r.ProductID)
                .ToDictionary(
                    g => g.Key,
                    g => Math.Floor(g.Average(r => r.Rating)*10)/10
                );
            ViewBag.AvgRatings = avgRatings;

            var stockCounts = DatabaseContext.Items
                .Where(i => i.Status.ToLower().Contains("sklad"))
                .GroupBy(i => i.ProductID)
                .ToDictionary(
                    g => g.Key,
                    g => g.Count()
                );

            ViewBag.StockCounts = stockCounts;
            ViewBag.Brand = (brand == null || brand == "") ? null : brand;

            if (minPrice.HasValue)
                ViewBag.MinPrice = minPrice;
            if (maxPrice.HasValue)
                ViewBag.MaxPrice = maxPrice;
            if (minRating.HasValue)
                ViewBag.MinRating = minRating;
            ViewBag.InStock = inStock;
        }

        private IQueryable<Product> ApplyFilters(IQueryable<Product> query, int? minPrice, int? maxPrice, int? minRating, bool inStock = false, string? brand = null)
        {
            if (minPrice.HasValue)
                query = query.Where(p => p.Price >= minPrice.Value);
            if (maxPrice.HasValue)
                query = query.Where(p => p.Price <= maxPrice.Value);
            if (minRating.HasValue)
                query = query.Where(p => DatabaseContext.Reviews.Where(r => r.ProductID == p.ID && r.DeletedAt == null).Average(r => r.Rating) >= minRating.Value);
            if (inStock)
                query = query.Where(p => DatabaseContext.Items.Where(i => i.ProductID == p.ID && i.Status.ToLower().Contains("sklad")).Count() > 0);
            if (brand != null)
                query = query.Where(p => p.Brand.ToLower().Contains(brand.ToLower()));

            return query;
        }
    }
}
