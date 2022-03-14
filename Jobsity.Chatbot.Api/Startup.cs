using FluentValidation.AspNetCore;
using Jobsity.Chatbot.Api.Common.ApiFilters;
using Jobsity.Chatbot.Api.Persistence.Configuration;
using Jobsity.Chatbot.Api.Persistence.Extensions;
using Jobsity.Chatbot.Api.ServiceBus;
using Jobsity.Chatbot.Api.ServiceBus.Configuration;
using Jobsity.Chatbot.Api.ServiceBus.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Jobsity.Chatbot.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.SetCosmosDbConfiguration(configuration);
            this.SetServiceBusConfiguration(configuration);
        }

        public ICosmosDbConfiguration CosmosDbConfiguration { get; set; }
        public IServiceBusConfiguration ServiceBusConfiguration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddCors();

            services.AddMvc(o =>
            {
                o.Filters.Add<ValidationFilter>();
            })
                .AddFluentValidation(c => { c.RegisterValidatorsFromAssemblyContaining<Startup>(); })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddMediatR(typeof(Startup).Assembly);
            services.AddAutoMapper(typeof(Startup).Assembly);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo() { Title = "Jobsity Chatbot API V1", Version = "v1" });
                c.CustomSchemaIds((type) => type.FullName);
                c.DescribeAllParametersInCamelCase();
            });

            services.AddScoped<StockBotService>();

            services.AddStockService(this.ServiceBusConfiguration);
            services.AddCosmosDb(this.CosmosDbConfiguration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            }

            app.UseRouting();
            app.UseHttpsRedirection();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI();
        }

        public void SetCosmosDbConfiguration(IConfiguration configuration)
        {
            this.CosmosDbConfiguration = new CosmosDbConfiguration();
            configuration.GetSection(nameof(this.CosmosDbConfiguration)).Bind(this.CosmosDbConfiguration);
        }

        public void SetServiceBusConfiguration(IConfiguration configuration)
        {
            this.ServiceBusConfiguration = new ServiceBusConfiguration();
            configuration.GetSection(nameof(this.ServiceBusConfiguration)).Bind(this.ServiceBusConfiguration);
        }
    }
}