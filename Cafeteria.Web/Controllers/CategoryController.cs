using Cafeteria.Web.Data;
using Cafeteria.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cafeteria.Web.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var categories = await _db.Category.ToListAsync();
                return View(categories);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category obj)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _db.Category.Add(obj);
                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    return StatusCode(500, "Internal server error. Please try again later.");
                }
            }
            return View(obj);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return BadRequest("Invalid category ID.");
            }

            var obj = await _db.Category.FindAsync(id);
            if (obj == null)
            {
                return NotFound($"Category with ID {id} not found.");
            }

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _db.Category.Update(obj);
                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    return StatusCode(500, "Internal server error. Please try again later.");
                }
            }
            return View(obj);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return BadRequest("Invalid category ID.");
            }
            var obj = await _db.Category.FindAsync(id);
            if (obj == null)
            {
                return NotFound($"Category with ID {id} not found.");
            }

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int? id)
        {
            if (id == null || id == 0)
            {
                return BadRequest("Invalid category ID.");
            }

            var obj = await _db.Category.FindAsync(id);
            if (obj == null)
            {
                return NotFound($"Category with ID {id} not found.");
            }

            try
            {
                _db.Category.Remove(obj);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }
    }
}
