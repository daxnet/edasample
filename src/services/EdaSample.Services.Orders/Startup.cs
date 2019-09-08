using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EdaSample.Common.Commands;
using EdaSample.Common.Messages;
using EdaSample.Integration.AspNetCore;
using EdaSample.Messaging.RabbitMQ;
using EdaSample.Services.Common;
using EdaSample.Services.Common.Commands;
using EdaSample.Services.Orders.CommandHandlers;
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
            var messageHandlerExecutionContext = new MessageHandlerContext(services,
                sc => sc.BuildServiceProvider());
            services.AddSingleton<IMessageHandlerContext>(messageHandlerExecutionContext);

            // Configure RabbitMQ.
            var rabbitServer = Configuration["rabbit:server"];
            var connectionFactory = new ConnectionFactory { HostName = rabbitServer };
            services.AddSingleton<ICommandBus>(sp => new RabbitMQCommandBus(connectionFactory,
                sp.GetRequiredService<ILogger<RabbitMQCommandBus>>(),
                sp.GetRequiredService<IMessageHandlerContext>(),
                EdaHelper.RMQ_COMMAND_EXCHANGE,
                queueName: typeof(Startup).Namespace));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var commandBus = app.ApplicationServices.GetRequiredService<ICommandBus>();
            commandBus.Subscribe<CreateOrderCommand, CreateOrderCommandHandler>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
