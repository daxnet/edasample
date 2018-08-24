using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EdaSample.Common.DataAccess;
using EdaSample.Common.Events;
using EdaSample.DataAccess.MongoDB;
using EdaSample.EventBus.RabbitMQ;
using EdaSample.Integration.AspNetCore;
using EdaSample.Services.Common.Events;
using EdaSample.Services.Notification.EventHandlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace EdaSample.Services.Notification
{
    public class Startup
    {
        private const string RMQ_EXCHANGE = "EdaSample.Exchange";
        private const string RMQ_QUEUE = "EdaSample.NotificationServiceQueue";

        private readonly ILogger logger;

        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            Configuration = configuration;
            this.logger = loggerFactory.CreateLogger<Startup>();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            var eventHandlerExecutionContext = new EventHandlerExecutionContext(services,
                sc => sc.BuildServiceProvider());
            services.AddSingleton<IEventHandlerExecutionContext>(eventHandlerExecutionContext);

            var connectionFactory = new ConnectionFactory { HostName = "localhost" };
            services.AddSingleton<IEventBus>(sp => new RabbitMQEventBus(connectionFactory,
                sp.GetRequiredService<ILogger<RabbitMQEventBus>>(),
                sp.GetRequiredService<IEventHandlerExecutionContext>(),
                RMQ_EXCHANGE,
                queueName: RMQ_QUEUE));

            var mongoServer = Configuration["mongo:server"];
            var mongoDatabase = Configuration["mongo:database"];
            var mongoPort = Convert.ToInt32(Configuration["mongo:port"]);
            services.AddSingleton<IDataAccessObject>(serviceProvider => new MongoDataAccessObject(mongoDatabase, mongoServer, mongoPort));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<CustomerCreatedEvent, CustomerCreatedEventHandler>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
