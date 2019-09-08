using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdaSample.Services.Orders.Models
{
    public enum OrderStatus
    {
        Created,
        Pending,
        Accepted,
        Rejected
    }
}
