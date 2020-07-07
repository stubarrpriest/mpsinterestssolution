using System.Reflection;
using BarrPriest.Mps.Interests.Ingest;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

[assembly: FunctionsStartup(typeof(Startup))]

namespace BarrPriest.Mps.Interests.Ingest
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var config = new ConfigurationBuilder()
                .AddUserSecrets(Assembly.GetExecutingAssembly(), false)
                .AddEnvironmentVariables()
                .Build();
        }
    }
}
