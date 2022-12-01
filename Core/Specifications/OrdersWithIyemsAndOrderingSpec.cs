using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Entities.OrderAggregates;

namespace Core.Specifications
{
    public class OrdersWithIyemsAndOrderingSpec : BaseSpecification<Order>
    {
        public OrdersWithIyemsAndOrderingSpec(string email) : base(x => x.BuyerEmail == email)
        {
            AddIncludes(x => x.DeliveryMethod);
            AddIncludes(x => x.OrderItems);
            AddOrderByDescending(x => x.OrderDate);
        }

        public OrdersWithIyemsAndOrderingSpec(int id, string email) : 
        base(x => x.Id == id && x.BuyerEmail == email)
        {
            AddIncludes(x => x.DeliveryMethod);
            AddIncludes(x => x.OrderItems);
        }
    }
}