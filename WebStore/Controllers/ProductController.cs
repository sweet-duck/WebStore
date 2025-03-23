using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebStore.Database;
using WebStore.Entities;
using System.IO;

namespace WebStore.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IWebHostEnvironment _env;
        public ProductController(DatabaseContext context, IWebHostEnvironment env) : base(context)
        {
            _env = env;
        }

        public IActionResult Index(int? categoryId)
        {
            var query = DatabaseContext.Products.Include(p => p.Category).AsQueryable();
            if (categoryId.HasValue)
                query = query.Where(p => p.CategoryID == categoryId.Value);
            var products = query.ToList();
            return View(products);
        }

        public IActionResult Details(int id)
        {
            var product = DatabaseContext.Products.Include(p => p.Category).FirstOrDefault(p => p.ID == id);
            if (product == null)
                return NotFound();

            string imagesFolder = Path.Combine(_env.WebRootPath, "images", id.ToString());
            List<string> imageFiles = new List<string>();
            if (Directory.Exists(imagesFolder))
            {
                var files = Directory.GetFiles(imagesFolder);
                foreach (var file in files)
                {
                    string fileName = Path.GetFileName(file);
                    imageFiles.Add($"/images/{id}/{fileName}");
                }
            }
            ViewBag.ImageFiles = imageFiles;

            var reviews = DatabaseContext.Reviews
                .Where(r => r.ProductID == id)  
                .OrderByDescending(r => r.CreatedAt)
                .ToList();

            ViewBag.Reviews = reviews;

            return View(product);
        }

        [HttpPost]
        public IActionResult AddReview(int productId, int rating, string comment, bool anonymous = false)
        {
            if (!ViewBag.IsAuthenticated)
                return RedirectToAction("Login", "Auth");

            Console.WriteLine(anonymous);
            

            var product = DatabaseContext.Products.Find(productId);
            if (product == null)
                return NotFound();

            int userID = int.Parse(HttpContext.Session.GetString("UserID") ?? "-1");

            var review = new Review
            {
                ProductID = productId,
                AccountID = userID,
                Rating = rating,
                Comment = comment,
                Anonymous = anonymous
            };

            DatabaseContext.Reviews.Add(review);
            DatabaseContext.SaveChanges();

            return RedirectToAction("Details", new { id = productId });
        }

        [HttpPost]
        public IActionResult DeleteReview(int commentId)
        {
            if (!ViewBag.IsAuthenticated)
                return RedirectToAction("Login", "Auth");


            var review = DatabaseContext.Reviews.Find(commentId);
            if (review == null)
                return NotFound();

            int userID = int.Parse(HttpContext.Session.GetString("UserID") ?? "-1");

  
            review.DeletedAt = DateTime.Now;
            DatabaseContext.SaveChanges();

            return RedirectToAction("Details", new { id = review.ProductID });
        }

    }
}
