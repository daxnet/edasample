using Dapper;
using EdaSample.Common.DataAccess;
using EdaSample.Common.Events;
using EdaSample.Services.Common.Events;
using EdaSample.Services.Customer.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EdaSample.Services.Customer.EventHandlers
{
    public class AddNewCustomerEventHandler : BaseEventHandler<CustomerCreatedEvent>
    {
        private readonly ILogger logger;
        private readonly IDataAccessObject dao;

        public AddNewCustomerEventHandler(ILogger<AddNewCustomerEventHandler> logger, IDataAccessObject dao)
        {
            this.logger = logger;
            this.dao = dao;
        }

        public override async Task<bool> HandleAsync(CustomerCreatedEvent @event, CancellationToken cancellationToken = default)
        {
            var customer = new Model.Customer(@event.CustomerId, @event.CustomerName, @event.Email);
            await dao.AddAsync(customer);
            this.logger.LogInformation($"客户信息创建成功。");

            return true;
        }
    }
}
