using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EdaSample.Common.Commands;
using EdaSample.Common.DataAccess;
using EdaSample.Common.Events;
using EdaSample.Common.Messages;
using EdaSample.Common.Sagas;
using EdaSample.DataAccess.MongoDB;
using EdaSample.Integration.AspNetCore;
using EdaSample.Messaging.RabbitMQ;
using EdaSample.Services.Common;
using EdaSample.Services.Common.Commands;
using EdaSample.Services.Common.Events;
using EdaSample.Services.Orders.CommandHandlers;
using EdaSample.Services.Orders.Sagas;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace EdaSample.Services.Orders
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Configure data access component.
            var mongoServer = Configuration["mongo:server"];
            var mongoDatabase = Configuration["mongo:database"];
            var mongoPort = Convert.ToInt32(Configuration["mongo:port"]);
            services.AddSingleton<IDataAccessObject>(serviceProvider => new MongoDataAccessObject(mongoDatabase, mongoServer, mongoPort));

            var messageHandlerExecutionContext = new MessageHandlerContext(services,
                sc => sc.BuildServiceProvider());
            services.AddSingleton<IMessageHandlerContext>(messageHandlerExecutionContext);

            // Configure RabbitMQ.
            var rabbitServer = Configuration["rabbit:server"];
            var connectionFactory = new ConnectionFactory { HostName = rabbitServer };
            services.AddSingleton((Func<IServiceProvider, ICommandBus>)(sp => new RabbitMQCommandBus(connectionFactory,
                sp.GetRequiredService<ILogger<RabbitMQCommandBus>>(),
                sp.GetRequiredService<IMessageHandlerContext>(),
                EdaHelper.RMQ_COMMAND_EXCHANGE,
                queueName: typeof(Startup).Namespace)));
            services.AddSingleton((Func<IServiceProvider, IEventBus>)(sp => new RabbitMQEventBus(connectionFactory,
                sp.GetRequiredService<ILogger<RabbitMQEventBus>>(),
                sp.GetRequiredService<IMessageHandlerContext>(),
                EdaHelper.RMQ_EVENT_EXCHANGE,
                queueName: typeof(Startup).Namespace)));

            // Configure Saga manager
            services.AddSingleton<ISagaManager>(sp => new SagaManager(sp.GetRequiredService<IEventBus>()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime appLifetime)
        {
            var commandBus = app.ApplicationServices.GetRequiredService<ICommandBus>();
            commandBus.Subscribe<CreateOrderCommand, CreateOrderCommandHandler>();

            var sagaManager = app.ApplicationServices.GetRequiredService<ISagaManager>();
            sagaManager.Register<CreateOrderSagaState, OrderCreatedEvent, CreateOrderSaga>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
