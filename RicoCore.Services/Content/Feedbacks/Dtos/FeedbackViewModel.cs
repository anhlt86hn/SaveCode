using System;
using System.ComponentModel.DataAnnotations;
using RicoCore.Services.Content.Contacts.Dtos;
using RicoCore.Infrastructure.Enums;

namespace RicoCore.Services.Content.Feedbacks.Dtos
{
    public class FeedbackViewModel
    {
        public Guid Id { set; get; }

        [MaxLength(100, ErrorMessage = "Tên không được quá 100 ký tự")]
        [Required(ErrorMessage = "Tên phải nhập")]
        public string Name { set; get; }

        [MaxLength(100, ErrorMessage = "Email không được quá 100 ký tự")]
        [Required(ErrorMessage = "Email phải nhập")]
        public string Email { set; get; }

        [MaxLength(500, ErrorMessage = "Tin nhắn không được quá 500 ký tự")]
        public string Message { set; get; }       

        [Required(ErrorMessage = "Phải nhập trạng thái")]
        public Status Status { set; get; }

        public DateTime DateCreated { set; get; }
        public DateTime? DateModified { set; get; }
    }
}