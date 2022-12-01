using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Core.Entities.OrderAggregates
{
    public enum OrderStatus
    {
        [EnumMember(Value = "Pending")]
        Pending,
        [EnumMember(Value = "Received")]
        PaymentReceived,
        [EnumMember(Value = "Failed")]
        PaymentFailed,
    }
}