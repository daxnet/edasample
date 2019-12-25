using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdaSample.Services.Orders.Sagas
{
    public class OrderDetailsItem
    {
        private OrderDetailsItem() { }

        public OrderDetailsItem(Guid productId, int quality)
        {
            ProductId = productId;
            Quality = quality;
        }

        public Guid ProductId { get; set; }

        public int Quality { get; set; }
    }
}
