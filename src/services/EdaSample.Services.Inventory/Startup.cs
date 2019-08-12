using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EdaSample.Common.DataAccess;
using EdaSample.DataAccess.MongoDB;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Swagger;

namespace EdaSample.Services.Inventory
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

            // Configure Swagger
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info
                {
                    Title = "EdaSample Inventory Service",
                    Version = "v1",
                    Description = "The RESTful API for inventories."
                });

                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xml = Path.Combine(basePath, "EdaSample.Services.Inventory.xml");
                if (File.Exists(xml))
                {
                    options.IncludeXmlComments(xml);
                }
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "EdaSample Inventory Service");
            });

            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseMvc();
        }
    }
}
