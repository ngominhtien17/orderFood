using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderFood.Models;

namespace OrderFood.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var dbContext = new DbOrderFoodContext(); // Thay thế YourDbContext bằng DbContext của bạn
            var sum = dbContext.Products.Sum(item => item.Quantity);
            var sum_acc = dbContext.Users.Count();
            var sum_cate = dbContext.Categories.Count();
            var sum_order = dbContext.Orders.Count();
            ViewBag.Cate = sum_cate;
            ViewBag.User = sum_acc;
            ViewBag.Order = sum_order;
            ViewBag.MyValue = sum;
            var money = dbContext.Orders
                       .AsNoTracking()
                       .Include(o => o.Payment)
                       .Include(o => o.Product)
                       .Include(o => o.User)
                       .Where(o => o.Status == true)
                       .OrderByDescending(x => x.OrderDate).ToList();
            //var money = dbContext.Orders.Select(item => item.Product.Price * item.Quantity);
            decimal doanhthu = 0;
            foreach(var item in money)
            {
                var a = item.Product.Price.Value;
                var b = item.Quantity.Value;
                doanhthu = a * b + doanhthu;
            }
            ViewBag.DoanhThu = doanhthu;
            return View();
        }
    }
}
