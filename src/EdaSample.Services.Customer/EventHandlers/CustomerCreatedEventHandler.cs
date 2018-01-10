using EdaSample.Common.Events;
using EdaSample.Services.Customer.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EdaSample.Services.Customer.EventHandlers
{
    public class CustomerCreatedEventHandler : IEventHandler<CustomerCreatedEvent>
    {
        private readonly IEventStore eventStore;
        private readonly ILogger logger;

        public CustomerCreatedEventHandler(IEventStore eventStore,
            ILogger<CustomerCreatedEventHandler> logger)
        {
            this.eventStore = eventStore;
            this.logger = logger;
            this.logger.LogInformation($"CustomerCreatedEventHandler构造函数调用完成。Hash Code: {this.GetHashCode()}.");
        }

        public bool CanHandle(IEvent @event)
            => @event.GetType().Equals(typeof(CustomerCreatedEvent));

        public async Task<bool> HandleAsync(CustomerCreatedEvent @event, CancellationToken cancellationToken = default)
        {
            this.logger.LogInformation($"开始处理CustomerCreatedEvent事件，处理器Hash Code：{this.GetHashCode()}.");
            await this.eventStore.SaveEventAsync(@event);
            this.logger.LogInformation($"结束处理CustomerCreatedEvent事件，处理器Hash Code：{this.GetHashCode()}.");
            return true;
        }

        public Task<bool> HandleAsync(IEvent @event, CancellationToken cancellationToken = default)
            => CanHandle(@event) ? HandleAsync((CustomerCreatedEvent)@event, cancellationToken) : Task.FromResult(false);
    }
}
