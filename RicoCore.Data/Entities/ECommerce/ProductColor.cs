using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using RicoCore.Infrastructure.Enums;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities
{
    [Table("ProductColors")]
    public class ProductColor : DomainEntity<int>
    {
        public Guid ProductId { set; get; }
        public int ColorId { set; get; }
        public int ProductCategoryId { set; get; }
        public string ProductCategoryName { set; get; }
        public string ColorImage { get; set; }
        public string ProductName { set; get; }
        public string Name { set; get; }
        public string Url { set; get; }
        public int SortOrder { set; get; }
        public Status Status { set; get; }
       
    }
}
