using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EdaSample.Common.DataAccess;
using EdaSample.Common.Events;
using EdaSample.DataAccess.MongoDB;
using EdaSample.EventBus.RabbitMQ;
using EdaSample.EventBus.Simple;
using EdaSample.EventStores.Dapper;
using EdaSample.EventStores.MongoDB;
using EdaSample.Integration.AspNetCore;
using EdaSample.Services.Common.Events;
using EdaSample.Services.Customer.EventHandlers;
using EdaSample.Services.Customer.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization;
using RabbitMQ.Client;

namespace EdaSample.Services.Customer
{
    public class Startup
    {
        private const string RMQ_EXCHANGE = "EdaSample.Exchange";
        private const string RMQ_QUEUE = "EdaSample.CustomerServiceQueue";

        private readonly ILogger logger;

        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            Configuration = configuration;
            this.logger = loggerFactory.CreateLogger<Startup>();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            this.logger.LogInformation("正在对服务进行配置...");

            services.AddMvc();
            services.AddOptions();

            // Configure event store.
            services.Configure<PostgreSqlConfig>(Configuration.GetSection("postgresql"));
            services.AddTransient<IEventStore>(serviceProvider =>
                new DapperEventStore(Configuration["postgresql:connectionString"],
                    serviceProvider.GetRequiredService<ILogger<DapperEventStore>>()));

            // Configure data access component.
            var mongoServer = Configuration["mongo:server"];
            var mongoDatabase = Configuration["mongo:database"];
            var mongoPort = Convert.ToInt32(Configuration["mongo:port"]);
            services.AddSingleton<IDataAccessObject>(serviceProvider => new MongoDataAccessObject(mongoDatabase, mongoServer, mongoPort));

            // Configure event handlers.
            var eventHandlerExecutionContext = new EventHandlerExecutionContext(services, 
                sc => sc.BuildServiceProvider());
            services.AddSingleton<IEventHandlerExecutionContext>(eventHandlerExecutionContext);

            // Configure RabbitMQ.
            var rabbitServer = Configuration["rabbit:server"];
            var connectionFactory = new ConnectionFactory { HostName = rabbitServer };
            services.AddSingleton<IEventBus>(sp => new RabbitMQEventBus(connectionFactory,
                sp.GetRequiredService<ILogger<RabbitMQEventBus>>(),
                sp.GetRequiredService<IEventHandlerExecutionContext>(),
                RMQ_EXCHANGE,
                queueName: RMQ_QUEUE));

            this.logger.LogInformation("服务配置完成，已注册到IoC容器！");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<CustomerCreatedEvent, CustomerCreatedEventHandler>();
            eventBus.Subscribe<CustomerCreatedEvent, AddNewCustomerEventHandler>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseMvc();
        }
    }
}
