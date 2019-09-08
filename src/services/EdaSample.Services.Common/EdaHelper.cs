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

using EdaSample.Common.Events;

namespace EdaSample.Services.Common
{
    /// <summary>
    /// Represents the helpers for the EdaSample application.
    /// </summary>
    public static class EdaHelper
    {
        public const string RMQ_EVENT_EXCHANGE = "EdaSample.EventExchange";
        public const string RMQ_COMMAND_EXCHANGE = "EdaSample.CommandExchange";
    }
}