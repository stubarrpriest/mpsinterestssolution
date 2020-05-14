using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using AngleSharp.Html;
using AngleSharp.Html.Parser;
using BarrPriest.Mps.Interests.Ingest.Interfaces.With.DirectoryStructure;
using BarrPriest.Mps.Interests.Ingest.Interfaces.With.ParliamentWebsite;
using LibGit2Sharp;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace BarrPriest.Mps.Interests.Ingest.Cli
{
    class Program
    {
        private const string DataPath = @"c:\temp\mpsinterests\";

        private const string RepoPath = @"c:\temp\mps\";

        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();

            ConfigureServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            if (args[0].ToUpperInvariant() == "SCRAPE")
            {
                var scraper = serviceProvider.GetService<HtmlScreenScraper>();

                scraper.Scrape();
            }

            if (args[0].ToUpperInvariant() == "GITUPDATE")
            {
                GitUpdate();
            }
        }

        private static void GitUpdate()
        {
            var dataSource = new DirectoryStructureRawHtml();

            var lastPublicationSet = string.Empty;

            var lastPublicationDate = DateTime.MinValue;

            var commitMessage = string.Empty;

            foreach (var publicationSet in dataSource.PublicationSetsFrom(DataPath).OrderBy(x => x))
            {
                foreach (var rawData in dataSource.MpDataFrom(DataPath, publicationSet))
                {
                    if (lastPublicationSet == string.Empty)
                    {
                        lastPublicationSet = publicationSet;
                    }

                    if (lastPublicationDate == DateTime.MinValue)
                    {
                        lastPublicationDate = rawData.LikelyPublicationDate;
                    }

                    if (lastPublicationSet != publicationSet)
                    {
                        commitMessage = $"Add amendments to register made on {lastPublicationDate}";

                        CommitAllFiles(RepoPath, commitMessage, lastPublicationDate);

                        Console.WriteLine(commitMessage);

                        lastPublicationSet = publicationSet;

                        lastPublicationDate = rawData.LikelyPublicationDate;
                    }

                    File.WriteAllText($"{RepoPath}\\{rawData.MpKey}.html", FormatHtml(rawData.Html));
                }

                commitMessage = $"Add amendments to register made on {lastPublicationDate}";
            }

            CommitAllFiles(RepoPath, commitMessage, lastPublicationDate);
        }

        private static void CommitAllFiles(string directory, string message, DateTime date)
        {
            using var repo = new Repository(directory);

            Commands.Stage(repo, "*");

            var signature = new Signature("Stuart Barr", "stuart.b@barrpriestltd.co.uk", new DateTimeOffset(date));

            repo.Commit(message, signature, signature);
        }

        private static string FormatHtml(string htmlInput)
        {
            var parsedDocument = new HtmlParser().ParseDocument(htmlInput);

            var sw = new StringWriter();

            parsedDocument.ToHtml(sw, new PrettyMarkupFormatter());

            return sw.ToString();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services
                .AddLogging(configure => configure.AddConsole())
                .AddTransient<ParliamentWebsiteRawHtml>()
                .AddTransient<HtmlScreenScraper>();

        }
    }
}
