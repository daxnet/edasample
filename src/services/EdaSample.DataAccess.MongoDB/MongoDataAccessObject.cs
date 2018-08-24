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
// Copyright (c) 2017-2018 Sunny Chen (daxnet)
//
// ============================================================================

using EdaSample.Common;
using EdaSample.Common.DataAccess;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EdaSample.DataAccess.MongoDB
{
    /// <summary>
    /// Represents the data access object which manipulates the MongoDB database.
    /// </summary>
    /// <seealso cref="WeShare.Service.DataAccess.IDataAccessObject" />
    public sealed class MongoDataAccessObject : IDataAccessObject
    {
        #region Private Fields

        private readonly IMongoClient client;
        private readonly IMongoDatabase database;

        private bool disposedValue = false;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoDataAccessObject"/> class.
        /// </summary>
        /// <param name="databaseName">Name of the database.</param>
        /// <param name="server">The server on which the MongoDB database has deployed.</param>
        /// <param name="port">The port number to which the MongoDB database is listening.</param>
        public MongoDataAccessObject(string databaseName, string server = "localhost", int port = 27017)
        {
            this.client = new MongoClient($"mongodb://{server}:{port}/{databaseName}");
            this.database = this.client.GetDatabase(databaseName);
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Adds the given entity asynchronously.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity to be added.</param>
        /// <returns>
        /// The task which performs the adding operation.
        /// </returns>
        public async Task AddAsync<TEntity>(TEntity entity) where TEntity : IEntity
        {
            var collection = GetCollection<TEntity>();
            var options = new InsertOneOptions { BypassDocumentValidation = true };
            await collection.InsertOneAsync(entity, options);
        }

        /// <summary>
        /// Deletes the entity by specified identifier asynchronously.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="id">The identifier which represents the entity that is going to be deleted.</param>
        /// <returns>
        /// The task which performs the deletion operation.
        /// </returns>
        public async Task DeleteByIdAsync<TEntity>(Guid id) where TEntity : IEntity
        {
            var filterDefinition = Builders<TEntity>.Filter.Eq(x => x.Id, id);
            await GetCollection<TEntity>().DeleteOneAsync(filterDefinition);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Finds the entities which match the specified criteria that is defined by the given specification asynchronously.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="expr">The expression which defines the matching criteria.</param>
        /// <returns>
        /// The task which performs the data retrieval operation, and after the operation
        /// has completed, would return a list of entities that match the specified criteria.
        /// </returns>
        public async Task<IEnumerable<TEntity>> FindBySpecificationAsync<TEntity>(Expression<Func<TEntity, bool>> expr) where TEntity : IEntity
            => await (await GetCollection<TEntity>().FindAsync(expr)).ToListAsync();

        /// <summary>
        /// Gets all of the entities asynchronously.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>
        /// The task which performs the data retrieval operation, and after
        /// the operation has completed, would return a list of retrieved entities.
        /// </returns>
        public async Task<IEnumerable<TEntity>> GetAllAsync<TEntity>() where TEntity : IEntity =>
            await FindBySpecificationAsync<TEntity>(_ => true);

        /// <summary>
        /// Gets the entity by specified identifier asynchronously.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="id">The identifier of the entity.</param>
        /// <returns>
        /// The task which performs the data retrieval operation.
        /// </returns>
        public async Task<TEntity> GetByIdAsync<TEntity>(Guid id) where TEntity : IEntity =>
            (await FindBySpecificationAsync<TEntity>(x => x.Id.Equals(id))).FirstOrDefault();

        /// <summary>
        /// Updates the entity by the specified identifier asynchronously.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="id">The identifier of the entity to be updated.</param>
        /// <param name="entity">The entity which contains the updated value.</param>
        /// <returns>
        /// The task which performs the updating operation.
        /// </returns>
        public async Task UpdateByIdAsync<TEntity>(Guid id, TEntity entity) where TEntity : IEntity
        {
            var filterDefinition = Builders<TEntity>.Filter.Eq(x => x.Id, id);
            await GetCollection<TEntity>().ReplaceOneAsync(filterDefinition, entity);
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        private IMongoCollection<TEntity> GetCollection<TEntity>() where TEntity : IEntity =>
            this.database.GetCollection<TEntity>(typeof(TEntity).Name);

        #endregion Private Methods

        // To detect redundant calls
        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~MongoDataAccessObject() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }
    }
}