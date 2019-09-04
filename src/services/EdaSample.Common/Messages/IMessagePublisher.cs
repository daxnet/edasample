using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EdaSample.Common.Messages
{
    public interface IMessagePublisher : IDisposable
    {
        Task PublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default)
            where TMessage : IMessage;
    }
}
