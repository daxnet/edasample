using EdaSample.Common.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdaSample.Common.Commands
{
    public abstract class BaseCommandHandler<TCommand> : MessageHandler<TCommand>, ICommandHandler<TCommand>
        where TCommand : ICommand
    {
    }
}
