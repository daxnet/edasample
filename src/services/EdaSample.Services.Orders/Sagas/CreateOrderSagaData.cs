using EdaSample.Common.Sagas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdaSample.Services.Orders.Sagas
{
    public class CreateOrderSagaData : SagaData
    {

        public OrderDetails SalesOrderDetails { get; set; }

        public bool CreditWithdrewSuccessful { get; set; }

        public bool InventoryReservedSuccessful { get; set; }
    }
}
