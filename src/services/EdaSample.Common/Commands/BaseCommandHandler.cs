// ============================================================================
//   ______    _        _____                       _
//  |  ____|  | |      / ____|                     | |
//  | |__   __| | __ _| (___   __ _ _ __ ___  _ __ | | ___
//  |  __| / _` |/ _` |\___ \ / _` | '_ ` _ \| '_ \| |/ _ \
//  | |___| (_| | (_| |____) | (_| | | | | | | |_) | |  __/
//  |______\__,_|\__,_|_____/ \__,_|_| |_| |_| .__/|_|\___|
//                                           | |
//                                           |_|
// MIT License
//
// Copyright (c) 2017-2019 Sunny Chen (daxnet)
//
// ============================================================================

using EdaSample.Common.Messages;

namespace EdaSample.Common.Commands
{
    /// <summary>
    /// Represents the base class for the command handlers.
    /// </summary>
    /// <typeparam name="TCommand">The type of the command.</typeparam>
    /// <seealso cref="EdaSample.Common.Messages.MessageHandler{TCommand}" />
    /// <seealso cref="EdaSample.Common.Commands.ICommandHandler{TCommand}" />
    public abstract class BaseCommandHandler<TCommand> : MessageHandler<TCommand>, ICommandHandler<TCommand>
        where TCommand : ICommand
    {
    }
}