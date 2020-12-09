using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using RicoCore.Infrastructure.Enums;

namespace RicoCore.Services.Content.Pages.Dtos
{
    public class PageViewModel
    {
        public Guid Id { get; set; }
        [Required]
        [MaxLength(256)]
        public string Name { set; get; }

        [Required]
        [MaxLength(256)]
        public string Url { get; set; }

        [Required]
        [MaxLength(70)]
        public string MetaTitle { get; set; }

        [Required]
        [MaxLength(256)]
        public string MetaKeywords { get; set; }

        [Required]
        [MaxLength(256)]
        public string MetaDescription { get; set; }

        public string Content { set; get; }

        public Status Status { set; get; }

        public string UniqueCode { set; get; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateModified { get; set; }

        public DateTime? DateDeleted { get; set; }
    }
}
