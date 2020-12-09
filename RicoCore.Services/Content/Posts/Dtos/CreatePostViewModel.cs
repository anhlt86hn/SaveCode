using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using RicoCore.Infrastructure.Enums;

namespace RicoCore.Services.Content.Posts.Dtos
{
    public class CreatePostViewModel : PostViewModel
    {
        public CreatePostViewModel()
        {
            Categories = new List<SelectListItem>();
        }
        public IList<SelectListItem> Categories { set; get; }     
    }
}