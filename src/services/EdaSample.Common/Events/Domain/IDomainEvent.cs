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
// Copyright (c) 2017-2018 Sunny Chen (daxnet)
//
// ============================================================================

using System;

namespace EdaSample.Common.Events.Domain
{
    public interface IDomainEvent : IEvent
    {
        Guid AggregateRootId { get; set; }
        string AggregateRootType { get; set; }
        long Sequence { get; set; }
    }
}