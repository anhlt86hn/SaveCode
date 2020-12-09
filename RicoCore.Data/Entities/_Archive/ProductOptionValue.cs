using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.ECommerce
{
    [Table("EComProductOptionValues")]
    public class ProductOptionValue : DomainEntity<Guid>
    {
        public ProductOptionValue()
        {
        }

        public ProductOptionValue(Guid productOptionId, Guid productId, Guid optionId, Guid optionValueId, int quantity, bool subtract, decimal price, string pricePrefix, int points, string pointsPrefix, decimal weight, string weightPrefix)
        {
            ProductOptionId = productOptionId;
            ProductId = productId;
            OptionId = optionId;
            OptionValueId = optionValueId;
            Quantity = quantity;
            Subtract = subtract;
            Price = price;
            PricePrefix = pricePrefix;
            Points = points;
            PointsPrefix = pointsPrefix;
            Weight = weight;
            WeightPrefix = weightPrefix;
        }

        [Required]
        public Guid ProductOptionId { set; get; }

        [Required]
        public Guid ProductId { set; get; }

        [Required]
        public Guid OptionId { set; get; }

        [Required]
        public Guid OptionValueId { set; get; }

        [Required]
        public int Quantity { set; get; }

        [Required]
        public bool Subtract { set; get; }

        [Required]
        public decimal Price { set; get; }

        [MaxLength(1)]
        [Required]
        public string PricePrefix { set; get; }

        [Required]
        public int Points { set; get; }

        [MaxLength(1)]
        [Required]
        public string PointsPrefix { set; get; }

        [Required]
        public decimal Weight { set; get; }

        [MaxLength(1)]
        [Required]
        public string WeightPrefix { set; get; }
    }
}