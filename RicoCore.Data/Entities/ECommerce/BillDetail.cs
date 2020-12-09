using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities
{
    [Table("BillDetails")]
    public class BillDetail : DomainEntity<Guid>
    {
        public BillDetail() { }
        public BillDetail(Guid id, Guid billId, string code, int productItemId, string productItemName, int quantity, decimal price, string color)
        {
            Id = id;
            BillId = billId;
            ProductItemId = productItemId;
            Quantity = quantity;
            Price = price;
            Color = color;
            ProductItemName = productItemName;
            Code = code;          
            //SizeId = sizeId;
        }

        public BillDetail(Guid billId, string code, int productItemId, string productItemName, int quantity, decimal price, string color)
        {
            BillId = billId;
            ProductItemId = productItemId;
            Quantity = quantity;
            Price = price;
            Color = color;
            ProductItemName = productItemName;
            Code = code;
            
            //SizeId = sizeId;
        }

        public Guid BillId { set; get; }
        public string Code { set; get; }
        public int ProductItemId { set; get; }
        public string ProductItemName { set; get; }       
        public int Quantity { set; get; }

        public decimal Price { set; get; }
        
        public string Color { get; set; }

        //public Guid? SizeId { get; set; }

        //[ForeignKey("BillId")]
        //public virtual Bill Bill { set; get; }

        //[ForeignKey("ProductId")]
        //public virtual Product Product { set; get; }

        //[ForeignKey("ColorId")]
        //public virtual Color Color { set; get; }

        //[ForeignKey("SizeId")]
        //public virtual Size Size { set; get; }
    }
}
