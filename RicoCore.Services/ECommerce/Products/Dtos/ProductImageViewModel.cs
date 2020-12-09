using System;
using System.ComponentModel.DataAnnotations;

namespace RicoCore.Services.ECommerce.Products.Dtos
{
    public class ProductImageViewModel
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        [StringLength(250)]
        public string Path { get; set; }

        [StringLength(250)]
        public string Caption { get; set; }
        public int SortOrder { get; set; }
    }
}