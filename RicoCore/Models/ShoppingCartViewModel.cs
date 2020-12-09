using RicoCore.Services.ECommerce.Products.Dtos;

namespace RicoCore.Models
{
    public class ShoppingCartViewModel
    {
        //public ProductViewModel Product { set; get; }
        public string ProductUrl { set; get; }
        public ProductItemViewModel ProductItem { set; get; }                    
        public int Quantity { set; get; }
        public decimal Price { set; get; }
        public string Color { set; get; }
        //public ColorViewModel Color { get; set; }

        //public SizeViewModel Size { get; set; }
    }
}