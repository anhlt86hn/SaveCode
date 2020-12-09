using System.ComponentModel.DataAnnotations;
using RicoCore.Infrastructure.Enums;

namespace RicoCore.Services.Content.Contacts.Dtos
{
    public class ContactDetailViewModel
    {
        public int Id { set; get; }

        [Required(ErrorMessage = "Tên không được trống")]
        [MaxLength(100, ErrorMessage = "Tên không vượt quá 100 ký tự")]
        public string Name { set; get; }

        [MaxLength(50, ErrorMessage = "Số điện thoại không vượt quá 50 ký tự")]
        public string Phone { set; get; }

        [MaxLength(100, ErrorMessage = "Email không vượt quá 100 ký tự")]
        public string Email { set; get; }

        [MaxLength(100, ErrorMessage = "Website không vượt quá 100 ký tự")]
        public string Website { set; get; }

        [MaxLength(500, ErrorMessage = "Địa chỉ không vượt quá 500 ký tự")]
        public string Address { set; get; }

        public string Other { set; get; }

        public double? Lat { set; get; }

        public double? Lng { set; get; }

        public Status Status { set; get; }
    }
}