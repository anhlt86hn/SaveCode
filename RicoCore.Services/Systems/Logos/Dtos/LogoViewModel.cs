using System;
using System.ComponentModel.DataAnnotations;
using RicoCore.Data.Enums;
using RicoCore.Infrastructure.Enums;

namespace RicoCore.Services.Systems.Logos.Dtos
{
    public class LogoViewModel
    {
        public int Id { set; get; }
        public string Image { set; get; }
        public string ImageAlt { set; get; }
        public string Size { set; get; }
        public string Favicon { set; get; }
        public bool Status { set; get; }
        public int SortOrder { set; get; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateModified { get; set; }

        public DateTime? DateDeleted { get; set; }
    }
}