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
using System;

namespace EdaSample.Services.Inventory.Models
{
    public class Product : IEntity
    {
        #region Public Properties

        public string Description { get; set; }
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string PictureFileName { get; set; }
        public float Price { get; set; }
        public string Unit { get; set; }

        public int Inventory { get; set; }

        #endregion Public Properties
    }
}