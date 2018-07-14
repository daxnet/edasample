using Dapper;
using EdaSample.Common.Events;
using EdaSample.Services.Common.Events;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace EdaSample.Services.Customer.Controllers
{
    [Route("api/[controller]")]
    public class CustomersController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly string connectionString;
        private readonly IEventBus eventBus;
        private readonly ILogger logger;

        public CustomersController(IConfiguration configuration,
            IEventBus eventBus,
            ILogger<CustomersController> logger)
        {
            this.configuration = configuration;
            this.connectionString = configuration["mssql:connectionString"];
            this.eventBus = eventBus;
            this.logger = logger;
        }


        // 获取指定ID的客户信息
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            const string sql = "SELECT [CustomerId] AS Id, [CustomerName] AS Name FROM [dbo].[Customers] WHERE [CustomerId]=@id";
            using (var connection = new SqlConnection(connectionString))
            {
                var customer = await connection.QueryFirstOrDefaultAsync<Model.Customer>(sql, new { id });
                if (customer == null)
                {
                    return NotFound();
                }

                return Ok(customer);
            }
        }

        // 创建新的客户信息
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] dynamic model)
        {
            this.logger.LogInformation($"开始创建客户信息。");
            var name = (string)model.Name;
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("请指定客户名称。");
            }

            var email = (string)model.Email;
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("电子邮件地址不能为空。");
            }
            
            // 由于数据库更新需要通过事件处理器进行异步更新，因此无法在Controller中得到
            // 数据库更新后的Customer ID。此处通过Guid.NewGuid获得，实际中可以使用独立
            // 的Identity Service产生。
            var customerId = Guid.NewGuid();

            await this.eventBus.PublishAsync(new CustomerCreatedEvent(customerId, name, email));

            return Created(Url.Action("Get", new { id = customerId }), customerId);
        }
    }
}
