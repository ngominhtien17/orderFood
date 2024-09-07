using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OrderFood.Models;
using PagedList.Core;

namespace OrderFood.Controllers
{
    public class ProductController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly DbOrderFoodContext _context;
        public ProductController(DbOrderFoodContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page = 1, int CateID = 0)
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
    }
}
