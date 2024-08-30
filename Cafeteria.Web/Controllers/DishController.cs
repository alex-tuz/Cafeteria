using Cafeteria.Web.Configs;
using Cafeteria.Web.Data;
using Cafeteria.Web.Models;
using Cafeteria.Web.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Cafeteria.Web.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class DishController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly FilePathsConfig _filePathsConfig;

        public DishController(ApplicationDbContext db,
            IWebHostEnvironment webHostEnvironment,
            IOptions<FilePathsConfig> filePathsConfig)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
            _filePathsConfig = filePathsConfig.Value;

        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var dishes = await _db.Dishes
                    .Include(d => d.Category)
                    .Include(d => d.Currency)
                    .ToListAsync();

                return View(dishes);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        public async Task<IActionResult> Create()
        {
            var categories = await _db.Category.ToListAsync();
            var currencies = await _db.Currencies.ToListAsync();

            var dishVM = new DishVM
            {
                Dish = new Dish(),
                CategorySelectList = new SelectList(categories, "Id", "Name"),
                CurrencySelectList = new SelectList(currencies, "Id", "Name")
            };

            return View(dishVM);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var dish = await _db.Dishes.FindAsync(id);
            if (dish == null)
            {
                return NotFound($"Dish with ID {id} not found.");
            }

            var categories = await _db.Category.ToListAsync();
            var currencies = await _db.Currencies.ToListAsync();

            var dishVM = new DishVM
            {
                Dish = dish,
                CategorySelectList = new SelectList(categories, "Id", "Name"),
                CurrencySelectList = new SelectList(currencies, "Id", "Name"),
                ImagePath = Path.Combine("\\", _filePathsConfig.ImagePath ?? "",
                    dish.Image ?? "")
            };

            return View(dishVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(DishVM dishVM)
        {
            if (string.IsNullOrEmpty(_filePathsConfig.ImagePath))
                return StatusCode(500, "Internal server error. Please try again later.");

            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;

                string uploadPath = Path.Combine(webRootPath, _filePathsConfig.ImagePath);

                if (dishVM.Dish.Id == 0)
                {
                    // Creating new dish
                    if (files.Count > 0)
                    {
                        dishVM.Dish.Image = await SaveFileAsync(files[0], uploadPath);
                    }

                    _db.Dishes.Add(dishVM.Dish);
                }
                else
                {
                    // Updating existing dish
                    var objFromDb = await _db.Dishes.AsNoTracking().FirstOrDefaultAsync(u => u.Id == dishVM.Dish.Id);

                    if (objFromDb == null)
                    {
                        return NotFound();
                    }

                    if (files.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(objFromDb.Image))
                        {
                            string oldFilePath = Path.Combine(uploadPath, objFromDb.Image);
                            if (System.IO.File.Exists(oldFilePath))
                            {
                                System.IO.File.Delete(oldFilePath);
                            }
                        }

                        dishVM.Dish.Image = await SaveFileAsync(files[0], uploadPath);
                    }
                    else
                    {
                        dishVM.Dish.Image = objFromDb.Image;
                    }

                    _db.Dishes.Update(dishVM.Dish);
                }

                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            dishVM.CategorySelectList = await _db.Category
                .Select(i => new SelectListItem { Text = i.Name, Value = i.Id.ToString() })
                .ToListAsync();

            dishVM.CurrencySelectList = await _db.Currencies
                .Select(i => new SelectListItem { Text = i.Name, Value = i.Id.ToString() })
                .ToListAsync();

            return View(nameof(Create), dishVM);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == 0)
            {
                return BadRequest("Invalid dish ID.");
            }

            var dish = await _db.Dishes.Include(u => u.Category)
                                        .FirstOrDefaultAsync(u => u.Id == id);

            if (dish == null)
            {
                return NotFound($"Category with ID {id} not found.");
            }

            return View(dish);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult DeletePost(int? id)
        //{
        //    var obj = _db.Dishes.Find(id);
        //    if (obj == null)
        //    {
        //        return NotFound();
        //    }

        //    string webRootPath = _webHostEnvironment.WebRootPath;

        //    string upload = webRootPath + _filePathsConfig.ImagePath;

        //    if (obj.Image != null)
        //    {
        //        var oldFile = Path.Combine(upload, obj.Image);

        //        if (System.IO.File.Exists(oldFile))
        //        {
        //            System.IO.File.Delete(oldFile);
        //        }
        //    }

        //    _db.Dishes.Remove(obj);
        //    _db.SaveChanges();

        //    return RedirectToAction(nameof(Index));
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int? id)
        {
            try
            {
                var dish = await _db.Dishes.FindAsync(id);
                if (dish == null)
                {
                    return NotFound($"Dish with ID {id} not found.");
                }

                string webRootPath = _webHostEnvironment.WebRootPath;
                string upload = Path.Combine(webRootPath, _filePathsConfig.ImagePath ?? "");

                if (!string.IsNullOrEmpty(dish.Image))
                {
                    var oldFile = Path.Combine(upload, dish.Image);

                    if (System.IO.File.Exists(oldFile))
                    {
                        System.IO.File.Delete(oldFile);
                    }
                }

                _db.Dishes.Remove(dish);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception )
            {
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        private async Task<string> SaveFileAsync(IFormFile file, string uploadPath)
        {
            string fileName = Guid.NewGuid().ToString();
            string extension = Path.GetExtension(file.FileName);
            string filePath = Path.Combine(uploadPath, fileName + extension);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return fileName + extension;
        }
    }
}
