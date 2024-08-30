using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Cafeteria.Web.Data;
using Cafeteria.Web.Models;
using Cafeteria.Web.Utillity;

using Cafeteria.Web;

namespace Rocky_Test.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CartController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var sessionCart = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            if (sessionCart == null || !sessionCart.Any())
            {
                return View(Enumerable.Empty<Dish>());
            }

            var prodInCart = sessionCart.Select(i => i.DishId).ToList();
            var prodList = _db.Dishes.Where(u => prodInCart.Contains(u.Id)).ToList();

            return View(prodList);
        }

        public IActionResult Remove(int id)
        {
            var sessionCart = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart) ?? new List<ShoppingCart>();

            var itemToRemove = sessionCart.FirstOrDefault(u => u.DishId == id);
            if (itemToRemove != null)
            {
                sessionCart.Remove(itemToRemove);
                HttpContext.Session.Set(WC.SessionCart, sessionCart);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
