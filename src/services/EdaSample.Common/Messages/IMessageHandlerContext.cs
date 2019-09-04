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
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EdaSample.Common.Messages
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMessageHandlerContext
    {
        #region Public Methods

        Task HandleMessageAsync(IMessage message, CancellationToken cancellationToken = default);

        bool HandlerRegistered<TMessage, TMessageHandler>()
            where TMessage : IMessage
            where TMessageHandler : IMessageHandler<TMessage>;

        bool HandlerRegistered(Type messageType, Type messageHandlerType);

        void RegisterHandler<TMessage, TMessageHandler>()
            where TMessage : IMessage
            where TMessageHandler : IMessageHandler<TMessage>;

        void RegisterHandler(Type messageType, Type messageHandlerType);

        #endregion Public Methods
    }
}