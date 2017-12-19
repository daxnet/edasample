using EdaSample.Common.Events;
using EdaSample.Services.Customer.Events;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdaSample.Services.Customer.Controllers
{
    [Route("api/[controller]")]
    public class CustomersController : Controller
    {
        private readonly IEventBus eventBus;

        public CustomersController(IEventBus eventBus)
        {
            this.eventBus = eventBus;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] dynamic model)
        {
            var customerName = (string)model.Name;
            await this.eventBus.PublishAsync(new CustomerCreatedEvent(customerName));
            return Ok();
        }
    }
}
