using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RicoCore.Models.AccountViewModels
{
    public class LoginCustomerViewModel
    {
        [Required(ErrorMessage = "Không được trống")]
        [Display(Name = "Tài khoản")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Không được trống")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [Display(Name = "Ghi nhớ mật khẩu?")]
        public bool RememberMe { get; set; }
    }
}
