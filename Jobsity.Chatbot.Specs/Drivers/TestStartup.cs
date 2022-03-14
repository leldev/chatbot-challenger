using Jobsity.Chatbot.Api;
using Jobsity.Chatbot.Api.Persistence.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobsity.Chatbot.Specs.Drivers
{
    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration, IWebHostEnvironment env) : base(configuration)
        {
            env.EnvironmentName = "Specs";
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).AddEnvironmentVariables();
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(this.CosmosDbConfiguration);
            base.ConfigureServices(services);

            services.AddControllers().AddApplicationPart(typeof(Startup).Assembly);
        }
    }
}
