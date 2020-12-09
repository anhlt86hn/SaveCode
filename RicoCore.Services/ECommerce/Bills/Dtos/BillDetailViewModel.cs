using RicoCore.Services.ECommerce.Products.Dtos;
using System;

namespace RicoCore.Services.ECommerce.Bills.Dtos
{
    public class BillDetailViewModel
    {
        public Guid Id { get; set; }

        public Guid BillId { set; get; }

        public string Code { set; get; }
        public int ProductItemId { set; get; }        
        public string ProductItemName { set; get; }       
        public int Quantity { set; get; }

        public decimal Price { set; get; }
        public string Color { set; get; }

        //public int ColorId { get; set; }

        //public Guid? SizeId { get; set; }

        public BillViewModel Bill { set; get; }
        public ProductItemViewModel ProductItem { set; get; }
        //public ColorViewModel Color { set; get; }
        //public SizeViewModel Size { set; get; }
    }
}