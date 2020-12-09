using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Data.Enums;
using RicoCore.Data.Interfaces;
using RicoCore.Infrastructure.Enums;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.Content
{
    [Table("Slides")]
    public class Slide : DomainEntity<int>, ISwitchable, ISortable
    {
        public Slide()
        {
        }

        public Slide(string name, string image, string url, int sortOrder, Status status, string content, SlideGroup group, 
            string mainCaption, string subCaption, string smallCaption, string actionCaption)
        {            
            Name = name;           
            Image = image;
            Url = url;
            SortOrder = sortOrder;
            Status = status;
            Content = content;
            GroupAlias = group;
            MainCaption = mainCaption;
            SubCaption = subCaption;
            SmallCaption = smallCaption;
            ActionCaption = actionCaption;
        }

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