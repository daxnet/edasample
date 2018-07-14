using EdaSample.Common.Events;
using EdaSample.Services.Common.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EdaSample.Services.Notification.EventHandlers
{
    public class CustomerCreatedEventHandler : EdaSample.Common.Events.EventHandler<CustomerCreatedEvent>
    {
        private readonly ILogger logger;

        public CustomerCreatedEventHandler(ILogger<CustomerCreatedEventHandler> logger)
        {
            this.logger = logger;
        }

        public override Task<bool> HandleAsync(CustomerCreatedEvent @event, CancellationToken cancellationToken = default(CancellationToken))
        {
            this.logger.LogInformation("已经成功发送通知消息。");
            return Task.FromResult(true);
        }
    }
}
