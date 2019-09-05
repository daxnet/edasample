using EdaSample.Common.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdaSample.Common.Commands
{
    public interface ICommandHandler : IMessageHandler
    {
    }

    public interface ICommandHandler<in TCommand> : IMessageHandler<TCommand>, ICommandHandler
        where TCommand : ICommand
    { }
}
