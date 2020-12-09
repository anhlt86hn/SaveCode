using System;
using System.ComponentModel.DataAnnotations;
using RicoCore.Data.Enums;
using RicoCore.Infrastructure.Enums;

namespace RicoCore.Services.Content.Slides.Dtos
{
    public class SlideViewModel
    {
        public int Id { set; get; }
        [StringLength(250)]
        [Required]
        public string Name { set; get; }        
        [StringLength(250)]
        [Required]
        public string Image { set; get; }
        [StringLength(250)]
        public string Url { set; get; }
        public string Content { set; get; }
        public string MainCaption { set; get; }
        public string SubCaption { set; get; }
        public string SmallCaption { set; get; }
        public string ActionCaption { set; get; }
        [Required]
        public SlideGroup GroupAlias { get; set; }
        public int SortOrder { set; get; }
        public Status Status { set; get; }
    }
}