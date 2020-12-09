using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Data.Interfaces;
using RicoCore.Infrastructure.Enums;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.ECommerce
{
    [Table("EComProducts")]
    public class Product : DomainEntity<Guid>, ISortable, ISwitchable, IDateTracking
    {
        public Product()
        {
        }

        public Product(string model, string sku, string upc, string ean, string jan, string isbn, string mpn, string location, int quantity, Guid stockStatusId, string image, Guid manufacturerId, bool shipping, decimal price, int points, Guid taxClassId, DateTime dateAvailable, decimal weight, Guid weightClassId, decimal length, decimal width, decimal height, Guid lengthClassId, bool subtract, int minimum, int viewed, int sortOrder, Status status, DateTime dateCreated, DateTime? dateModified, DateTime? dateDeleted)
        {
            Model = model;
            Sku = sku;
            Upc = upc;
            Ean = ean;
            Jan = jan;
            Isbn = isbn;
            Mpn = mpn;
            Location = location;
            Quantity = quantity;
            StockStatusId = stockStatusId;
            Image = image;
            ManufacturerId = manufacturerId;
            Shipping = shipping;
            Price = price;
            Points = points;
            TaxClassId = taxClassId;
            DateAvailable = dateAvailable;
            Weight = weight;
            WeightClassId = weightClassId;
            Length = length;
            Width = width;
            Height = height;
            LengthClassId = lengthClassId;
            Subtract = subtract;
            Minimum = minimum;
            Viewed = viewed;
            SortOrder = sortOrder;
            Status = status;
            DateCreated = dateCreated;
            DateModified = dateModified;
            DateDeleted = dateDeleted;
        }

        [MaxLength(64)]
        public string Model { set; get; }

        [MaxLength(64)]
        public string Sku { set; get; }

        [MaxLength(12)]
        public string Upc { set; get; }

        [MaxLength(14)]
        public string Ean { set; get; }

        [MaxLength(13)]
        public string Jan { set; get; }

        [MaxLength(17)]
        public string Isbn { set; get; }

        [MaxLength(64)]
        public string Mpn { set; get; }

        [MaxLength(128)]
        public string Location { set; get; }
        public int Quantity { set; get; }

        public Guid StockStatusId { set; get; }
        [MaxLength(255)]
        public string Image { set; get; }
        public Guid ManufacturerId { set; get; }
        public bool Shipping { set; get; }
        public decimal Price { set; get; }
        public int Points { set; get; }
        public Guid TaxClassId { set; get; }
        public DateTime DateAvailable { set; get; }
        public decimal Weight { set; get; }
        public Guid WeightClassId { set; get; }
        public decimal Length { set; get; }
        public decimal Width { set; get; }
        public decimal Height { set; get; }
        public Guid LengthClassId { set; get; }
        public bool Subtract { set; get; }
        public int Minimum { set; get; }
        public int Viewed { set; get; }
        public int SortOrder { get; set; }
        public Status Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateDeleted { get; set; }
    }
}