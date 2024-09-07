using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderFood.Extension;
using OrderFood.Helpper;
using OrderFood.Models;
using OrderFood.ModelViews;
using System.Security.Claims;

namespace OrderFood.Controllers
{
    [Authorize]
    public class AccountController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly DbOrderFoodContext _context;
        public INotyfService _notifyService { get; }
        public AccountController(DbOrderFoodContext context, INotyfService notyfService)
        {
            _context = context;
            _notifyService = notyfService;
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ValidatePhone(string Phone)
        {
            try
            {
                var khachhang = _context.Users.AsNoTracking().SingleOrDefault(x => x.Mobile.ToLower() == Phone);
                if (khachhang != null)
                {
                    return Json(data: "Số điện thoại :" + Phone + "Đã được sử dụng");
                }
                return Json(data: true);
            }
            catch
            {
                return Json(data: true);
            }
        }

        public IActionResult ValidateEmail(string Email)
        {
            try
            {
                var khachhang = _context.Users.AsNoTracking().SingleOrDefault(x => x.Email.ToLower() == Email);
                if (khachhang != null)
                {
                    return Json(data: "Số điện thoại :" + Email + "Đã được sử dụng");
                }
                return Json(data: true);
            }
            catch
            {
                return Json(data: true);
            }
        }

        [Route("Tai-khoan-cua-toi.html", Name = "Dashboard")]
        public IActionResult Dashboard()
        {
            var taikhoanID = HttpContext.Session.GetString("UserId");
            if (taikhoanID != null)
            {
               var khachhang = _context.Users.AsNoTracking().SingleOrDefault(x=>x.UserId==Convert.ToInt32(taikhoanID));
               if(khachhang != null)
                {
                    var lSDonHang = _context.Orders
                        .AsNoTracking()
                        .Where(x => x.UserId == khachhang.UserId)
                        .Include(o => o.Payment)
                        .Include(o => o.Product)
                        .Include(o => o.User)
                        .OrderByDescending(x => x.OrderDate)
                        .ToList();
                    ViewBag.lSDonHang = lSDonHang;
                    return View(khachhang);
                }
            }
            return RedirectToAction("Login");
        }
        [HttpGet]
        [AllowAnonymous]
        [Route("Dang-ky.html", Name = "DangKy")]
        public IActionResult DangKyTaiKhoan()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("Dang-ky.html", Name = "DangKy")]
        public async Task<IActionResult> DangKyTaiKhoan(RegisterVM taikhoan)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string salt = Utilities.GetRandomKey();
                    User khachhang = new User
                    {
                        Name = taikhoan.FullName,
                        Username = taikhoan.FullName,
                        Mobile = taikhoan.Phone.Trim().ToLower(),
                        Email = taikhoan.Email.Trim().ToLower(),
                        Password = (taikhoan.Password + salt.Trim()).TOMD5(),
                        PostCode = salt,
                        CreateDate = DateTime.Now

                    };
                    try
                    {
                        _context.Add(khachhang);
                        await _context.SaveChangesAsync();
                        HttpContext.Session.SetString("UserId", khachhang.UserId.ToString());
                        //Lưu Session Makh
                        var taikhoanID = HttpContext.Session.GetString("UserId");
                        //Identitv
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, khachhang.Username),
                            new Claim("UserId",khachhang.UserId.ToString())
                        };
                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
                        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        await HttpContext.SignInAsync(claimsPrincipal);
                        return RedirectToAction("Login", "Account");
                    }
                    catch
                    {
                        return RedirectToAction("DangKyTaiKhoan", "Account");
                    }
                }
                else
                {
                    return View(taikhoan);
                }
            }
            catch
            {
                return View(taikhoan);
            }
        }

        [AllowAnonymous]
        [Route("Dang-nhap.html", Name = "DangNhap")]
        public IActionResult Login()
        {
            var taikhoanID = HttpContext.Session.GetString("UserId");
            if (taikhoanID != null)
            {
                _notifyService.Success("Thành công");
                return RedirectToAction("Dashboard", "Account");
            }
            return View();

        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Dang-nhap.html", Name = "DangNhap")]
        public async Task<IActionResult> Login(LoginViewModel user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isEmail = Utilities.IsValidEmail(user.UserName);
                    if (isEmail == false)
                    {
                        return View(user);
                    }
                    var khachhang = _context.Users.AsNoTracking().SingleOrDefault(x => x.Email.Trim() == user.UserName);
                    if (khachhang == null)
                    {
                        _notifyService.Success("Mail khong ton tai");
                        return RedirectToAction("DangKyTaiKhoan");
                    }
                    string pass = (user.Password + khachhang.PostCode.Trim()).TOMD5();
                    if (khachhang.Password != pass)
                    {
                        _notifyService.Success("Sai thông tin đăng nhập");
                        return View(user);
                    }

                    HttpContext.Session.SetString("UserId", khachhang.UserId.ToString());
                    var taikhoanID = HttpContext.Session.GetString("UserId");

                    var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, khachhang.Username),
                            new Claim("UserId",khachhang.UserId.ToString())
                        };
                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    await HttpContext.SignInAsync(claimsPrincipal);
                    _notifyService.Success("Đăng nhập thành công");
                    return RedirectToAction("Dashboard", "Account");
                }
                else
                {
                    return View(user);
                }
            }
            catch
            {
                _notifyService.Success("Đăng nhập thất bại");
                return RedirectToAction("DangKyTaiKhoan", "Account");
            }
            
        }
        [HttpGet]
        [Route("Dang-xuat.html", Name ="Logout")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            HttpContext.Session.Remove("UserId");
            return RedirectToAction("Index", "Home");
        }

    }
}

