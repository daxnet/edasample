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
    /// <summary>
    /// Represents a product in the EDA Shop.
    /// </summary>
    /// <seealso cref="EdaSample.Common.IEntity" />
    public class Product : IEntity
    {

        #region Public Properties

        /// <summary>
        /// Gets or sets the description of the product.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the product.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the total number of items in the inventory for the current product.
        /// </summary>
        /// <value>
        /// The inventory.
        /// </value>
        public int Inventory { get; set; }

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the file name of the picture which represents
        /// the image of the product.
        /// </summary>
        /// <value>
        /// The name of the picture file.
        /// </value>
        public string PictureFileName { get; set; }

        /// <summary>
        /// Gets or sets the price of the product.
        /// </summary>
        /// <value>
        /// The price.
        /// </value>
        public float Price { get; set; }

        /// <summary>
        /// Gets or sets the unit name of the product.
        /// </summary>
        /// <value>
        /// The unit.
        /// </value>
        public string Unit { get; set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() => Name;

        #endregion Public Methods

    }
}