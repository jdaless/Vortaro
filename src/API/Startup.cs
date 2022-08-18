using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

[assembly: FunctionsStartup(typeof(API.Startup))]

namespace API
{
    class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var Configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: true)
                    .AddEnvironmentVariables()
                    .Build();

            builder.Services.AddDbContext<VortaroContext>(options => options.UseMySql(
                Configuration.GetConnectionString("UVD"), 
                new MySqlServerVersion(new System.Version(5, 7, 36))));
        }
    }
}