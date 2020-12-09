using RicoCore.Services.ECommerce.Products.Dtos;

namespace RicoCore.Models
{
    public class ShoppingCartsViewModel
    {
        public ProductItemViewModel ProductItem { set; get; }
        
        public int Quantity { set; get; }

        public decimal Price { set; get; }

        //public ColorViewModel Color { get; set; }

        //public SizeViewModel Size { get; set; }
    }
}