using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RicoCore.Models.AccountViewModels
{
    public class ExternalLoginViewModel
    {
        [Required(ErrorMessage = "Không được trống")]
        [Display(Name = "Tên đầy đủ")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Không được trống")]
        [Display(Name = "Sinh nhật")]
        public string DOB { get; set; }

        [Required(ErrorMessage = "Không được trống")]
        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }
    }
}
