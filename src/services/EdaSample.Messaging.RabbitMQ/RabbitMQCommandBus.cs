using EdaSample.Common.Commands;
using EdaSample.Common.Messages;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdaSample.Messaging.RabbitMQ
{
    public sealed class RabbitMQCommandBus : RabbitMQMessageBus<ICommand, ICommandHandler>, ICommandBus
    {
        public RabbitMQCommandBus(IConnectionFactory connectionFactory, 
            ILogger<RabbitMQCommandBus> logger, 
            IMessageHandlerContext messageHandlerContext, 
            string exchangeName, 
            string exchangeType = "fanout", 
            string queueName = null, bool autoAck = false) 
            : base(connectionFactory, logger, messageHandlerContext, exchangeName, exchangeType, queueName, autoAck)
        {
        }
    }
}
