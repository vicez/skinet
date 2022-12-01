using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities.OrderAggregates
{
    public class OrderItem : BaseEntity
    {
        public OrderItem()
        {
        }

        public OrderItem(ProductItemOrdered productItemOrdered, int quantity, decimal price)
        {
            ProductItemOrdered = productItemOrdered;
            Quantity = quantity;
            Price = price;
        }

        public ProductItemOrdered ProductItemOrdered { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}