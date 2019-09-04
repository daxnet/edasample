using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EdaSample.Common.Messages
{
    public interface IMessageHandler
    {
        Task<bool> HandleAsync(IMessage message, CancellationToken cancellationToken = default);

        bool CanHandle(IMessage message);
    }

    public interface IMessageHandler<in T> : IMessageHandler
        where T : IMessage
    {
        Task<bool> HandleAsync(T message, CancellationToken cancellationToken = default);
    }
}
