using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Cafeteria.Web.Data;
using Cafeteria.Web.Models;
using Cafeteria.Web.Utillity;

using Cafeteria.Web;
using Microsoft.EntityFrameworkCore;
using Cafeteria.Web.Models.ViewModels;
using System.Security.Claims;

namespace Rocky_Test.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _db;
        [BindProperty]
        public ProductUserVM ProductUserVM { get; set; }
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
            var prodList = _db.Dishes
                .Include(u => u.Category)
                .Include(u => u.Currency).Where(u => prodInCart.Contains(u.Id)).ToList();

            return View(prodList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName(nameof(Index))]
        public IActionResult IndexPost()
        {
            return RedirectToAction(nameof(Summary));
        }

        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext?.Session?.Get<IEnumerable<ShoppingCart>>(WC.SessionCart)?.Count() > 0)
            {
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }
            List<int> prodInCart = shoppingCartList.Select(i => i.DishId).ToList();
            IEnumerable<Dish> prodList = _db.Dishes.Where(u => prodInCart.Contains(u.Id));

            ProductUserVM = new ProductUserVM()
            {
                ApplicationUser = _db.ApplicationUser.FirstOrDefault(u => u.Id == claim.Value),
                DishList = prodList.ToList(),
            };

            return View(ProductUserVM);
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
