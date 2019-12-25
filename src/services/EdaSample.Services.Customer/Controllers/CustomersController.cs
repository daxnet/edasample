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

using EdaSample.Common.DataAccess;
using EdaSample.Common.Events;
using EdaSample.Services.Common.Events;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EdaSample.Services.Customer.Controllers
{
    [Route("api/[controller]")]
    public class CustomersController : Controller
    {
        #region Private Fields

        private readonly IConfiguration configuration;
        private readonly string connectionString;
        private readonly IDataAccessObject dao;
        private readonly IEventBus eventBus;
        private readonly ILogger logger;
        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomersController"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="eventBus">The event bus.</param>
        /// <param name="dao">The DAO.</param>
        /// <param name="logger">The logger.</param>
        public CustomersController(IConfiguration configuration,
            IEventBus eventBus,
            IDataAccessObject dao,
            ILogger<CustomersController> logger)
        {
            this.configuration = configuration;
            this.connectionString = configuration["postgresql:connectionString"];
            this.eventBus = eventBus;
            this.dao = dao;
            this.logger = logger;
        }

        #endregion Public Constructors

        #region Public Methods

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

            return Created(Url.Action("Get", new { name }), customerId);
        }

        [HttpDelete("{name}")]
        public async Task<IActionResult> DeleteByNameAsync(string name)
        {
            var customers = await dao.FindBySpecificationAsync<Model.Customer>(p => p.Name == name);
            if (customers?.Count() == 0)
            {
                return NotFound();
            }

            await dao.DeleteByIdAsync<Model.Customer>(customers.First().Id);
            return NoContent();
        }

        // 获取指定ID的客户信息
        [HttpGet("{name}")]
        public async Task<IActionResult> Get(string name)
        {
            var customers = await dao.FindBySpecificationAsync<Model.Customer>(p => p.Name == name);
            if (customers?.Count() == 0)
            {
                return NotFound();
            }

            return Ok(customers.First());
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCustomersAsync()
            => Ok(await dao.GetAllAsync<Model.Customer>());

        #endregion Public Methods
    }
}