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

using EdaSample.Common;
using EdaSample.Common.Events;
using EdaSample.Common.Messages;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EdaSample.Integration.AspNetCore
{
    public sealed class MessageHandlerContext : IMessageHandlerContext
    {
        #region Private Fields

        private readonly ConcurrentDictionary<Type, List<Type>> registrations = new ConcurrentDictionary<Type, List<Type>>();
        private readonly IServiceCollection registry;
        private readonly Func<IServiceCollection, IServiceProvider> serviceProviderFactory;

        #endregion Private Fields

        #region Public Constructors

        public MessageHandlerContext(IServiceCollection registry,
            Func<IServiceCollection, IServiceProvider> serviceProviderFactory = null)
        {
            this.registry = registry;
            this.serviceProviderFactory = serviceProviderFactory ?? (sc => registry.BuildServiceProvider());
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task HandleMessageAsync(IMessage message, CancellationToken cancellationToken = default)
        {
            var eventType = message.GetType();
            if (this.registrations.TryGetValue(eventType, out List<Type> handlerTypes) &&
                handlerTypes?.Count > 0)
            {
                var serviceProvider = this.serviceProviderFactory(this.registry);
                using (var childScope = serviceProvider.CreateScope())
                {
                    foreach (var handlerType in handlerTypes)
                    {
                        var handler = (IMessageHandler)childScope.ServiceProvider.GetService(handlerType);
                        if (handler.CanHandle(message))
                        {
                            await handler.HandleAsync(message, cancellationToken);
                        }
                    }
                }
            }
        }

        public bool HandlerRegistered<TMessage, TMessageHandler>()
            where TMessage : IMessage
            where TMessageHandler : IMessageHandler
            => this.HandlerRegistered(typeof(TMessage), typeof(TMessageHandler));

        public bool HandlerRegistered(Type eventType, Type handlerType)
        {
            if (this.registrations.TryGetValue(eventType, out List<Type> handlerTypeList))
            {
                return handlerTypeList != null && handlerTypeList.Contains(handlerType);
            }

            return false;
        }

        public void RegisterHandler<TMessage, TMessageHandler>()
            where TMessage : IMessage
            where TMessageHandler : IMessageHandler
            => this.RegisterHandler(typeof(TMessage), typeof(TMessageHandler));

        public void RegisterHandler(Type eventType, Type handlerType)
        {
            Utils.ConcurrentDictionarySafeRegister(eventType, handlerType, this.registrations);
            this.registry.AddTransient(handlerType);
        }

        #endregion Public Methods
    }
}