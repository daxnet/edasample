using EdaSample.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdaSample.Services.Orders.Models
{
    public class SalesOrder : IAggregateRoot
    {
        public SalesOrder()
        {
            Id = Guid.NewGuid();
            Lines = new List<SalesOrderLine>();
            Status = OrderStatus.Created;
        }

        public Guid Id { get; }

        public Guid CustomerId { get; set; }

        public List<SalesOrderLine> Lines { get; set; }

        public OrderStatus Status { get; set; }
    }
}
