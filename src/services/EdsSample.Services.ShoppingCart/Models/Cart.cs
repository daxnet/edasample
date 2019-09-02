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
using System.Collections.Generic;
using System.Linq;

namespace EdsSample.Services.ShoppingCart.Models
{
    /// <summary>
    /// Represents a shopping cart.
    /// </summary>
    public class Cart : IEntity
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Cart"/> class.
        /// </summary>
        public Cart()
        {
            this.CartItems = new List<CartItem>();
            this.Id = Guid.NewGuid();
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Gets or sets the cart items.
        /// </summary>
        /// <value>
        /// The cart items.
        /// </value>
        public List<CartItem> CartItems { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the customer who owns the shopping cart.
        /// </summary>
        /// <value>
        /// The customer identifier.
        /// </value>
        public Guid CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the shopping cart.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid Id { get; set; }

        #endregion Public Properties

        public void AddOrUpdateItem(CartItem item)
        {
            var existingItem = CartItems.FirstOrDefault(ci => ci.ProductId == item.ProductId);
            if (existingItem == null)
            {
                CartItems.Add(item);
            }
            else
            {
                existingItem.Quantity += item.Quantity;
            }
        }
    }
}