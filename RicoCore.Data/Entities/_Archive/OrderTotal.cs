using System;
using System.ComponentModel.DataAnnotations.Schema;
using RicoCore.Data.Interfaces;
using RicoCore.Infrastructure.SharedKernel;

namespace RicoCore.Data.Entities.ECommerce
{
    [Table("EComOrderTotals")]
    public class OrderTotal : DomainEntity<Guid>, ISortable
    {
        public OrderTotal()
        {
        }

        public OrderTotal(Guid orderId, string code, string title, decimal value, int sortOrder)
        {
            OrderId = orderId;
            Code = code;
            Title = title;
            Value = value;
            SortOrder = sortOrder;
        }

        public Guid OrderId { set; get; }
        public string Code { set; get; }
        public string Title { set; get; }
        public decimal Value { set; get; }
        public int SortOrder { get; set; }
    }
}