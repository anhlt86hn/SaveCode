using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RicoCore.Services.Content.Posts.Dtos
{
    public class PostImageViewModel
    {
        public Guid PostId { get; set; }

        [StringLength(255)]
        public string Path { get; set; }

        [StringLength(255)]
        public string Caption { get; set; }

        public int SortOrder { get; set; }
    }
}
