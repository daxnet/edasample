using EdaSample.Common.Events;
using EdaSample.Common.Messages;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdaSample.Messaging.RabbitMQ
{
    public sealed class RabbitMQEventBus : RabbitMQMessageBus<IEvent, IEventHandler>, IEventBus
    {
        public RabbitMQEventBus(IConnectionFactory connectionFactory, 
            ILogger<RabbitMQEventBus> logger, 
            IMessageHandlerContext messageHandlerContext, 
            string exchangeName, 
            string exchangeType = "fanout", 
            string queueName = null, 
            bool autoAck = false) : base(connectionFactory, logger, messageHandlerContext, exchangeName, exchangeType, queueName, autoAck)
        {
        }
    }
}
