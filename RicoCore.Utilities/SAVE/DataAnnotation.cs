using System;
using System.Collections.Generic;
using System.Text;

namespace RicoCore.Utilities.Code
{
    class DataAnnotation
    {
        /// [DefaultValue(0)]
        /// [StringLength(255)]
        /// [MaxLength(255, ErrorMessage = "Số điện thoại không vượt quá 50 ký tự")]
        /// [EmailAddress]
        /// [Required(ErrorMessage = "Tên phải nhập")]
        /// [ForeignKey("")]
        /// [Table("Posts")]
        /// [Column(TypeName = "varchar")]
        /// [Column(Order = 1)]
        ///    [DefaultValue(Status.Actived)]
        //        public Status Status { set; get; } = Status.Actived;
        //        [DefaultValue(0)]
        //        [StringLength(255)]
        //        [MaxLength(255, ErrorMessage = "Số điện thoại không vượt quá 50 ký tự")]
        //        [Required]
        //        [EmailAddress]
        //        [Required(ErrorMessage = "Tên phải nhập")]
        //        [Column(TypeName="varchar")]
        //        [Column(Order = 1)]
        //        [ForeignKey("CategoryId")]
        //        public virtual ProductCategory ProductCategory { set; get; }
        //        public virtual ICollection<Product> Products { set; get; }
    }
}
