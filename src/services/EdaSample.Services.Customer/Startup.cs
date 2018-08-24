using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EdaSample.Common.Events;
using EdaSample.EventBus.RabbitMQ;
using EdaSample.EventBus.Simple;
using EdaSample.EventStores.Dapper;
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
            services.Configure<MssqlConfig>(Configuration.GetSection("mssql"));

            services.AddTransient<IEventStore>(serviceProvider => 
                new DapperEventStore(Configuration["mssql:connectionString"], 
                    serviceProvider.GetRequiredService<ILogger<DapperEventStore>>()));

            var eventHandlerExecutionContext = new EventHandlerExecutionContext(services, 
                sc => sc.BuildServiceProvider());
            services.AddSingleton<IEventHandlerExecutionContext>(eventHandlerExecutionContext);
            // services.AddSingleton<IEventBus, PassThroughEventBus>();

            var connectionFactory = new ConnectionFactory { HostName = "localhost" };
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

            app.UseMvc();
        }
    }
}
