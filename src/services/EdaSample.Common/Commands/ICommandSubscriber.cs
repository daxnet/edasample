using EdaSample.Common.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdaSample.Common.Commands
{
    public interface ICommandSubscriber : IMessageSubscriber<ICommand, ICommandHandler>
    {
    }
}
