using EdaSample.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdaSample.Services.Orders.Models
{
    public class SalesOrderLine : IEntity
    {
        public SalesOrderLine()
            => Id = Guid.NewGuid();

        public Guid Id { get; }

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
    }
}
