using EdaSample.Common.Commands;
using EdaSample.Common.DataAccess;
using EdaSample.Services.Common.Commands;
using EdsSample.Services.ShoppingCart.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdsSample.Services.ShoppingCart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {

        #region Private Fields

        private readonly ICommandBus commandBus;
        private readonly IDataAccessObject dataAccessObject;
        private readonly ILogger logger;

        #endregion Private Fields

        #region Public Constructors

        public CartsController(ICommandBus commandBus, IDataAccessObject dataAccessObject, ILogger<CartsController> logger)
        {
            this.commandBus = commandBus;
            this.logger = logger;
            this.dataAccessObject = dataAccessObject;
        }

        #endregion Public Constructors

        #region Public Methods

        [HttpPost("add-item/{customerId}")]
        public async Task<IActionResult> AddItemAsync(Guid customerId, [FromBody] CartItem cartItem)
        {
            var cart = (await this.dataAccessObject.FindBySpecificationAsync<Cart>(c => c.CustomerId == customerId && c.Status == CartStatus.Normal))
                .FirstOrDefault();
            if (cart == null)
            {
                cart = await this.CreateCartAsync(customerId);
            }

            cart.AddOrUpdateItem(cartItem);
            await this.dataAccessObject.UpdateByIdAsync(cart.Id, cart);
            return Ok(cart);
        }

        [HttpPost("add-items/{customerId}")]
        public async Task<IActionResult> AddItemsAsync(Guid customerId, [FromBody] IEnumerable<CartItem> cartItems)
        {
            var cart = (await this.dataAccessObject.FindBySpecificationAsync<Cart>(c => c.CustomerId == customerId && c.Status == CartStatus.Normal))
                .FirstOrDefault();
            if (cart == null)
            {
                cart = await this.CreateCartAsync(customerId);
            }

            foreach(var cartItem in cartItems)
            {
                cart.AddOrUpdateItem(cartItem);
            }
            await this.dataAccessObject.UpdateByIdAsync(cart.Id, cart);
            return Ok(cart);
        }

        [HttpGet("get-by-customer/{customerId}")]
        public async Task<IActionResult> GetCartByCustomerIdAsync(Guid customerId)
        {
            var cart = (await this.dataAccessObject.FindBySpecificationAsync<Cart>(c => c.CustomerId == customerId && c.Status == CartStatus.Normal))
                .FirstOrDefault();
            if (cart == null)
            {
                return NotFound();
            }

            return Ok(cart);
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout([FromBody] dynamic body)
        {
            var cartId = (Guid)body.CartId;

            // Search for a cart that has the given ID and the status is 'Normal'.
            var cart = (await this.dataAccessObject.FindBySpecificationAsync<Cart>(c => c.Id == cartId && c.Status == CartStatus.Normal))
                .FirstOrDefault();

            if (cart == null)
            {
                return NotFound("No available shopping cart found.");
            }

            // Update the found cart's status to Checkout.
            cart.Status = CartStatus.Checkout;
            await this.dataAccessObject.UpdateByIdAsync(cartId, cart);

            // Send the CreateOrder command to create the sales order.
            var createOrderCommand = new CreateOrderCommand(cart.CustomerId, cartId, 
                cart.CartItems.Select(item => new CreateOrderLine
                {
                    ProductId = item.ProductId,
                    Price = item.Price,
                    Quantity = item.Quantity
                }));
            await this.commandBus.PublishAsync(createOrderCommand);
            return Ok();
        }

        #endregion Public Methods

        #region Private Methods

        private async Task<Cart> CreateCartAsync(Guid customerId)
        {
            var cart = new Cart { CustomerId = customerId };
            await this.dataAccessObject.AddAsync(cart);
            return cart;
        }

        #endregion Private Methods

    }
}
