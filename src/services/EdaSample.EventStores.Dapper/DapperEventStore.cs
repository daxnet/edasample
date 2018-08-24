using EdaSample.Common.Events;
using System;
using Dapper;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace EdaSample.EventStores.Dapper
{
    public class DapperEventStore : IEventStore
    {
        private readonly string connectionString;
        private readonly ILogger logger;

        public DapperEventStore(string connectionString,
            ILogger<DapperEventStore> logger)
        {
            this.connectionString = connectionString;
            this.logger = logger;
            logger.LogInformation($"DapperEventStore构造函数调用完成。Hash Code：{this.GetHashCode()}.");
        }

        public async Task SaveEventAsync<TEvent>(TEvent @event) where TEvent : IEvent
        {
            logger.LogInformation($"DapperEventStore正在更新数据库。Hash Code: {this.GetHashCode()}.");
            const string sql = @"INSERT INTO [dbo].[Events] 
([EventId], [EventPayload], [EventTimestamp]) 
VALUES 
(@eventId, @eventPayload, @eventTimestamp)";
            using (var connection = new SqlConnection(this.connectionString))
            {
                await connection.ExecuteAsync(sql, new
                {
                    eventId = @event.Id,
                    eventPayload = JsonConvert.SerializeObject(@event),
                    eventTimestamp = @event.Timestamp
                });
            }
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.logger.LogInformation($"DapperEventStore已经被Dispose。Hash Code:{this.GetHashCode()}.");
                }

                disposedValue = true;
            }
        }

        public void Dispose() => Dispose(true);
        #endregion
    }
}
