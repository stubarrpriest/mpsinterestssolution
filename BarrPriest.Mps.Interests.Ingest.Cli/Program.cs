using System.Threading.Tasks;
using BarrPriest.Mps.Interests.Ingest.Interfaces.With.DirectoryStructure;
using BarrPriest.Mps.Interests.Ingest.Interfaces.With.ParliamentWebsite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BarrPriest.Mps.Interests.Ingest.Cli
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var serviceCollection = new ServiceCollection();

            ConfigureServices(serviceCollection, configuration);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            if (args[0].ToUpperInvariant() == "SCRAPE")
            {
                var scraper = serviceProvider.GetService<HtmlScreenScraper>();

                await scraper.Scrape();
            }

            if (args[0].ToUpperInvariant() == "GITUPDATE")
            {
                var committer = serviceProvider.GetService<GitCommitter>();

                await committer.AddAndCommitAllFiles();
            }
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services
                .Configure<IngestOptions>(configuration.GetSection("IngestOptions"))
                .AddLogging(configure => configure.AddConsole())
                .AddTransient<ParliamentWebsiteRawHtml>()
                .AddTransient<HtmlScreenScraper>()
                .AddTransient<DirectoryStructureRawHtml>()
                .AddTransient<GitCommitter>();
        }
    }
}
