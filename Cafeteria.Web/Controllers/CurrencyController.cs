using Cafeteria.Web.Data;
using Cafeteria.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cafeteria.Web.Controllers
{
    public class CurrencyController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CurrencyController(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            var currencies = await _db.Currencies.ToListAsync();
            return View(currencies);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Currency obj)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _db.Currencies.Add(obj);
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
            var obj = await _db.Currencies.FindAsync(id);
            if (obj == null)
            {
                return NotFound($"Currency with ID {id} not found.");
            }

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Currency obj)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _db.Currencies.Update(obj);
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
            var obj = await _db.Currencies.FindAsync(id);
            if (obj == null)
            {
                return NotFound($"Currency with ID {id} not found.");
            }

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int? id)
        {
            var obj = await _db.Currencies.FindAsync(id);
            if (obj == null)
            {
                return NotFound($"Currency with ID {id} not found.");
            }

            try
            {
                _db.Currencies.Remove(obj);
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
