using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace OrderFood.ModelViews
{
    public class LoginViewModel
    { 
        [Key]
        
        [Required(ErrorMessage = "Vui lòng nhập Email")]
        [MaxLength(150)]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Địa chỉ Email")]
        //[EmailAddress]
        public string UserName { get; set; }

        [Display(Name = "Mật khẩu:")]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [MinLength(5, ErrorMessage="Bạn cần đặt mật khẩu tối thiểu 5 ký tự")]
        public string Password { get; set; }
    }
}
