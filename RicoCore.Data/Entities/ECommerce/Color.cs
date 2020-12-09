using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using RicoCore.Infrastructure.Enums;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities
{
    [Table("Colors")]
    public class Color : DomainEntity<int>
    {
        public Color()
        {

        }

        public Color(string name, string englishName, string code, string url, int sortOrder, Status status)
        {
            Name = name;
            EnglishName = englishName;
            Code = code;
            Url = url;
            SortOrder = sortOrder;
            Status = status;                
        }
        [StringLength(250)]
        public string Name{ get; set; }

        [StringLength(250)]
        public string EnglishName { get; set; }

        [StringLength(250)]
        public string Code { get; set; }
        public string Url { set; get; }
        public int SortOrder { set; get; }
        public Status Status { set; get; }
        [StringLength(70)]
        public string MetaTitle { set; get; }

        [StringLength(500)]
        public string MetaKeywords { set; get; }

        [StringLength(255)]
        public string MetaDescription { set; get; }
    }
}
