using EdaSample.Services.Orders.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdaSample.Services.Orders.Sagas
{
    public class OrderDetails
    {
        public OrderDetails() { }

        public OrderDetails(SalesOrder salesOrder)
        {
            CustomerId = salesOrder.CustomerId;
            SalesOrderId = salesOrder.Id;
            Items = new List<OrderDetailsItem>(salesOrder.Lines.Select(l => new OrderDetailsItem(l.ProductId, l.Quantity)));
            TotalAmount = salesOrder.TotalAmount;
        }

        public Guid CustomerId { get; set; }

        public Guid SalesOrderId { get; set; }

        public List<OrderDetailsItem> Items { get; set; }

        public float TotalAmount { get; set; }
    }
}
