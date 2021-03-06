﻿// ============================================================================
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
using EdaSample.Services.Inventory.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdaSample.Services.Inventory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        #region Private Fields

        private readonly IDataAccessObject dataAccessObject;
        private readonly ILogger logger;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductsController"/> class.
        /// </summary>
        /// <param name="dataAccessObject">The data access object.</param>
        /// <param name="logger">The logger.</param>
        public ProductsController(IDataAccessObject dataAccessObject, ILogger<ProductsController> logger)
        {
            this.dataAccessObject = dataAccessObject;
            this.logger = logger;
        }

        #endregion Public Constructors

        #region Public Methods

        [HttpPost]
        public async Task<IActionResult> CreateAsync(Product product)
        {
            if (string.IsNullOrEmpty(product.Name))
            {
                return BadRequest("Product name was not specified.");
            }

            var productsWithSameName = await this.dataAccessObject.FindBySpecificationAsync<Product>(p => string.Equals(p.Name, product.Name));
            if (productsWithSameName?.Count() > 0)
            {
                return Conflict($"The product '{product.Name}' already exists.");
            }

            var id = Guid.NewGuid();
            product.Id = id;
            await this.dataAccessObject.AddAsync(product);
            logger.LogInformation("产品信息注册成功。");
            return Created(Url.Action("GetByKeyAsync", new { id }), id);
        }

        [HttpPost("create-many")]
        public async Task<IActionResult> CreateManyAsync(IEnumerable<Product> products)
        {
            foreach (var product in products)
            {
                await CreateAsync(product);
            }

            return new NoContentResult();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAllAsync()
        {
            var products = await this.dataAccessObject.GetAllAsync<Product>();
            foreach (var product in products)
            {
                await this.dataAccessObject.DeleteByIdAsync<Product>(product.Id);
            }

            return new NoContentResult();
        }

        [HttpGet("{id}")]
        public async Task<Product> GetByKeyAsync(Guid id)
        {
            return await this.dataAccessObject.GetByIdAsync<Product>(id);
        }

        [HttpGet]
        public async Task<IEnumerable<Product>> GetProducts() => await this.dataAccessObject.GetAllAsync<Product>();

        #endregion Public Methods
    }
}