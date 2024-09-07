using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderFood.Extension;
using OrderFood.Models;
using OrderFood.ModelViews;

namespace OrderFood.Controllers
{
    public class ShoppingCartController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly DbOrderFoodContext _context;
        public INotyfService _notifyService { get; }
        public ShoppingCartController(DbOrderFoodContext context, INotyfService notyfService)
        {
            _context = context;
            _notifyService = notyfService;
        }
        public List<CartItem> GioHang
        {
            get
            {
                var gh = HttpContext.Session.Get<List<CartItem>>("GioHang");
                if(gh == default(List<CartItem>))
                {
                    gh = new List<CartItem>();
                }
                return gh;
            }
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
