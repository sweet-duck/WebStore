using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebStore.Database;
using WebStore.Entities;

namespace WebStore.Controllers
{
    public class BaseController : Controller
    {
        public DatabaseContext DatabaseContext { get; set; }
        public BaseController(DatabaseContext databaseContext) { 
            DatabaseContext = databaseContext;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            RefreshCategories();

            ViewBag.IsAuthenticated = context.HttpContext.Session.GetString("User") != null;
            ViewBag.Username = context.HttpContext.Session.GetString("User");
            ViewBag.UserID = context.HttpContext.Session.GetString("UserID");
            ViewBag.Role = context.HttpContext.Session.GetString("Role");
            base.OnActionExecuting(context);
        }

        public void RefreshCategories()
        {         
            ViewBag.Categories = DatabaseContext.Categories.ToArray().ToList();

        }
    }
}
