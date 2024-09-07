using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OrderFood.Models;
using PagedList.Core;
using OrderFood.Helpper;

namespace OrderFood.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminProductsController : Controller
    {
        private readonly DbOrderFoodContext _context;
        public INotyfService _notifyService { get; }

        public AdminProductsController(DbOrderFoodContext context, INotyfService notifyService)
        {
            _context = context;
            _notifyService = notifyService;
        }

        // GET: Admin/AdminProducts
        public async Task<IActionResult> Index(int page = 1, int CateID = 0)
        {
            var pageNumber = page;
            var pageSize = 10;

            List<Product> IsProducts = new List<Product>();
            if (CateID != 0)
            {
                IsProducts = _context.Products
                .AsNoTracking()
                .Where(x => x.CategoryId == CateID)
                .Include(x => x.Category)
                .OrderByDescending(x => x.ProductId).ToList();
            }
            else
            {
                IsProducts = _context.Products
                .AsNoTracking()
                .Include(x => x.Category)
                .OrderByDescending(x => x.ProductId).ToList();
            }

            PagedList<Product> models = new PagedList<Product>(IsProducts.AsQueryable(), pageNumber, pageSize);
            ViewBag.CurrentCateID = CateID;
            ViewBag.CurrentPage = pageNumber;

            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CategoryId", "Name", CateID);
            return View(models);
        }
        public IActionResult Filtter(int CateID = 0)
        {
            var url = $"/Admin/AdminProducts?CateID={CateID}";
            if (CateID == 0)
            {
                url = $"/Admin/AdminProducts";
            }

            return Json(new { status = "success", redirectUrl = url });

        }


        // GET: Admin/AdminProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Admin/AdminProducts/Create
        public IActionResult Create()
        {
            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CategoryId", "Name");
            return View();
        }

        // POST: Admin/AdminProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,Name,Description,Price,Quantity,ImageUrl,CategoryId,CreateDate")] Product product, Microsoft.AspNetCore.Http.IFormFile filename)
        {
            if (ModelState.IsValid)
            {
                product.Name = Utilities.ToTitleCase(product.Name);
                if (filename != null)
                {
                    string extension = Path.GetExtension(filename.FileName);
                    string image = Utilities.SEOUrl(product.Name) + extension;
                    product.ImageUrl = await Utilities.UploadFile(filename, @"products", image.ToLower());
                }
                if (string.IsNullOrEmpty(product.ImageUrl)) product.ImageUrl = "default.jpg";
                product.CreateDate = DateTime.Now;

                _context.Add(product);
                await _context.SaveChangesAsync();
                _notifyService.Success("Thêm món thành công");
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", product.CategoryId);
            return View(product);
        }

        // GET: Admin/AdminProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CategoryId", "Name", product.CategoryId);
            return View(product);
        }

        // POST: Admin/AdminProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Name,Description,Price,Quantity,ImageUrl,CategoryId,IsActive,CreateDate")] Product product, Microsoft.AspNetCore.Http.IFormFile filename)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    product.Name = Utilities.ToTitleCase(product.Name);
                    if (filename != null)
                    {
                        string extension = Path.GetExtension(filename.FileName);
                        string image = Utilities.SEOUrl(product.Name) + extension;
                        product.ImageUrl = await Utilities.UploadFile(filename, @"products", image.ToLower());
                    }
                    if (string.IsNullOrEmpty(product.ImageUrl)) product.ImageUrl = "default.jpg";
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                    _notifyService.Success("Chỉnh sửa thành công");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
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
            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CategoryId", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Admin/AdminProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Admin/AdminProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'DbOrderFoodContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            _notifyService.Success("Xóa món thành công");
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
