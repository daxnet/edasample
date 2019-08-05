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

using EdaSample.Common.Events;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace EdaSample.EventStores.MongoDB
{
    public class MongoEventStore : IEventStore
    {
        #region Private Fields

        private readonly IMongoClient client;
        private readonly IMongoDatabase database;
        private readonly ILogger logger;
        private bool disposedValue = false;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoEventStore"/> class.
        /// </summary>
        /// <param name="databaseName">Name of the database.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="server">The server.</param>
        /// <param name="port">The port.</param>
        public MongoEventStore(string databaseName, ILogger<MongoEventStore> logger, string server = "localhost", int port = 27017)
        {
            this.client = new MongoClient($"mongodb://{server}:{port}/{databaseName}");
            this.database = this.client.GetDatabase(databaseName);
            this.logger = logger;
            logger.LogInformation($"MongoEventStore构造函数调用完成。Hash Code：{this.GetHashCode()}.");
        }

        #endregion Public Constructors

        #region Public Methods

        public void Dispose() => Dispose(true);

        public async Task SaveEventAsync<TEvent>(TEvent @event) where TEvent : IEvent
        {
            // Group events by event type.
            var collection = this.database.GetCollection<TEvent>(typeof(TEvent).Name);
            await collection.InsertOneAsync(@event);
        }

        #endregion Public Methods

        #region Protected Methods

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.logger.LogInformation($"MongoEventStore已经被Dispose。Hash Code:{this.GetHashCode()}.");
                }

                disposedValue = true;
            }
        }

        #endregion Protected Methods
    }
}