﻿using EdaSample.Common.Commands;
using EdaSample.Common.DataAccess;
using EdaSample.Common.Events;
using EdaSample.Services.Common.Commands;
using EdaSample.Services.Common.Events;
using EdaSample.Services.Orders.Models;
using EdaSample.Services.Orders.Sagas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EdaSample.Services.Orders.CommandHandlers
{
    public class CreateOrderCommandHandler : BaseCommandHandler<CreateOrderCommand>
    {
        private readonly IDataAccessObject dao;
        // private ICommandBus commandBus;
        private IEventBus eventBus;

        public CreateOrderCommandHandler(IDataAccessObject dao, /*ICommandBus commandBus, */IEventBus eventBus)
        {
            this.dao = dao;
            // this.commandBus = commandBus;
            this.eventBus = eventBus;
        }

        public override async Task<bool> HandleAsync(CreateOrderCommand message, CancellationToken cancellationToken = default)
        {
            // Command should only initializes the corresponding Saga instead of doing any
            // additional business logic here. Sagas are the objects that manage the BASE transactions.

            var salesOrder = new SalesOrder
            {
                CustomerId = message.CustomerId,
                Status = OrderStatus.Pending // Set status to Pending, waiting for the confirm of the order.
            };

            foreach (var line in message.Lines)
            {
                salesOrder.Lines.Add(new SalesOrderLine
                {
                    Price = line.Price,
                    ProductId = line.ProductId,
                    Quantity = line.Quantity
                });
            }

            // Inserts the sales order to the backend database.
            await this.dao.AddAsync(salesOrder);
            await this.eventBus.PublishAsync(new OrderCreatedEvent
            {
                SalesOrderId = salesOrder.Id,
                CustomerId = message.CustomerId,
                TotalAmount = salesOrder.TotalAmount
            }, cancellationToken);

            return true;
        }
    }
}