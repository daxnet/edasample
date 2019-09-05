using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EdaSample.Common.Messages
{
    public abstract class MessageHandler<TMessage> : IMessageHandler<TMessage>
        where TMessage : IMessage
    {
        public bool CanHandle(IMessage message) => typeof(TMessage).Equals(message.GetType());

        public abstract Task<bool> HandleAsync(TMessage message, CancellationToken cancellationToken = default);

        public Task<bool> HandleAsync(IMessage message, CancellationToken cancellationToken = default)
            => CanHandle(message) ? HandleAsync((TMessage)message, cancellationToken) : Task.FromResult(false);
    }
}
