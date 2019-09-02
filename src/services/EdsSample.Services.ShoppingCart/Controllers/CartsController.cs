using EdaSample.Common.DataAccess;
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

        private readonly IDataAccessObject dataAccessObject;
        private readonly ILogger logger;

        #endregion Private Fields

        #region Public Constructors

        public CartsController(IDataAccessObject dataAccessObject, ILogger<CartsController> logger)
        {
            this.logger = logger;
            this.dataAccessObject = dataAccessObject;
        }

        #endregion Public Constructors

        #region Public Methods

        [HttpPost("add-item/{customerId}")]
        public async Task<IActionResult> AddItemAsync(Guid customerId, [FromBody] CartItem cartItem)
        {
            var cart = (await this.dataAccessObject.FindBySpecificationAsync<Cart>(c => c.CustomerId == customerId))
                .FirstOrDefault();
            if (cart == null)
            {
                cart = await this.CreateCartAsync(customerId);
            }

            cart.AddOrUpdateItem(cartItem);
            await this.dataAccessObject.UpdateByIdAsync(cart.Id, cart);
            return Ok(cart);
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
