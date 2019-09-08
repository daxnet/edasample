using EdaSample.Common.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdaSample.Services.Common.Commands
{
    /// <summary>
    /// Represents the command which informs the system that a sales order should be created.
    /// </summary>
    /// <seealso cref="EdaSample.Common.Commands.ICommand" />
    public class CreateOrderCommand : ICommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrderCommand"/> class.
        /// </summary>
        public CreateOrderCommand()
        {
            Id = Guid.NewGuid();
            Timestamp = DateTime.UtcNow;
            Lines = new List<CreateOrderLine>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrderCommand"/> class.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="shoppingCartId">The shopping cart identifier.</param>
        /// <param name="lines">The lines.</param>
        public CreateOrderCommand(Guid customerId, Guid shoppingCartId, IEnumerable<CreateOrderLine> lines)
            : this()
        {
            CustomerId = customerId;
            ShoppingCartId = shoppingCartId;
            Lines.AddRange(lines);
        }

        public Guid Id { get; }

        public DateTime Timestamp { get; }

        public Guid CustomerId { get; set; }

        public Guid ShoppingCartId { get; set; }

        public List<CreateOrderLine> Lines { get; set; }
    }

    public class CreateOrderLine
    {
        public Guid ProductId { get; set; }

        public int Quantity { get; set; }

        public float Price { get; set; }
    }
}
