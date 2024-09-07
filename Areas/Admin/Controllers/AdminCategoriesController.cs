using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OrderFood.Helpper;
using OrderFood.Models;
using PagedList.Core;

namespace OrderFood.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminCategoriesController : Controller
    {
        private readonly DbOrderFoodContext _context;
        public INotyfService _notifyService { get; }
        public AdminCategoriesController(DbOrderFoodContext context, INotyfService notifyService)
        {
            _context = context;
            _notifyService = notifyService;
        }

        // GET: Admin/AdminCategories
        //public async Task<IActionResult> Index()
        //{
        //    return _context.Categories != null ?
        //                View(await _context.Categories.ToListAsync()) :
        //                Problem("Entity set 'DbOrderFoodContext.Categories'  is null.");
        //}
        public async Task<IActionResult> Index(int page = 1)
        {
            var pageNumber = page;
            var pageSize = 10;

            PagedList<Category> models = new(_context.Categories, pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;

            //ViewData["DanhMuc"] = new SelectList(_context.Categories, "CategoryId", "Name");
            return View(models);
        }

        // GET: Admin/AdminCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Admin/AdminCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,Name,ImageUrl,IsActive,CreatedDate")] Category category, Microsoft.AspNetCore.Http.IFormFile filename)
        {
            if (ModelState.IsValid)
            {
                category.Name = Utilities.ToTitleCase(category.Name);
                if (filename != null)
                {
                    string extension = Path.GetExtension(filename.FileName);
                    string image = Utilities.SEOUrl(category.Name) + extension;
                    category.ImageUrl = await Utilities.UploadFile(filename, @"categories", image.ToLower());
                }
                if (string.IsNullOrEmpty(category.ImageUrl)) category.ImageUrl = "default.jpg";
                category.CreatedDate = DateTime.Now;

                _context.Add(category);
                await _context.SaveChangesAsync();
                _notifyService.Success("Thêm danh mục thành công");
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Admin/AdminCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Admin/AdminCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,Name,ImageUrl,IsActive,CreatedDate")] Category category, Microsoft.AspNetCore.Http.IFormFile filename)
        {
            if (id != category.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    category.Name = Utilities.ToTitleCase(category.Name);
                    if (filename != null)
                    {
                        string extension = Path.GetExtension(filename.FileName);
                        string image = Utilities.SEOUrl(category.Name) + extension;
                        category.ImageUrl = await Utilities.UploadFile(filename, @"categories", image.ToLower());
                    }
                    if (string.IsNullOrEmpty(category.ImageUrl)) category.ImageUrl = "default.jpg";

                    _context.Update(category);
                    await _context.SaveChangesAsync();
                    _notifyService.Success("Chỉnh sửa danh mục thành công");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.CategoryId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Admin/AdminCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Admin/AdminCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Categories == null)
            {
                return Problem("Entity set 'DbOrderFoodContext.Categories'  is null.");
            }
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }

            await _context.SaveChangesAsync();
            _notifyService.Success("Xóa danh mục thành công");
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return (_context.Categories?.Any(e => e.CategoryId == id)).GetValueOrDefault();
        }
    }
}
