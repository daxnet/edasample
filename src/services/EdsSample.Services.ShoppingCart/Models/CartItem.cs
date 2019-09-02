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

using System;

namespace EdsSample.Services.ShoppingCart.Models
{
    /// <summary>
    /// Represents an item in the cart.
    /// </summary>
    public class CartItem
    {

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="CartItem"/> class.
        /// </summary>
        public CartItem() => Id = Guid.NewGuid();

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Gets or sets the identifier of the item.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the picture file.
        /// </summary>
        /// <value>
        /// The name of the picture file.
        /// </value>
        public string PictureFileName { get; set; }

        /// <summary>
        /// Gets or sets the price.
        /// </summary>
        /// <value>
        /// The price.
        /// </value>
        public float Price { get; set; }

        /// <summary>
        /// Gets or sets the product identifier of the item.
        /// </summary>
        /// <value>
        /// The product identifier.
        /// </value>
        public Guid ProductId { get; set; }

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        /// <value>
        /// The name of the product.
        /// </value>
        public string ProductName { get; set; }
        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        /// <value>
        /// The quantity.
        /// </value>
        public int Quantity { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override string ToString() => ProductName;

        #endregion Public Methods

    }
}