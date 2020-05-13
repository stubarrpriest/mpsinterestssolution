using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using BarrPriest.Mps.Interests.Ingest.Interfaces.With.DirectoryStructure;
using BarrPriest.Mps.Interests.Ingest.Interfaces.With.ParliamentWebsite;
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
            if (args[0].ToUpperInvariant() == "SCRAPE")
            {
                ScrapeWebsiteData();
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

            foreach (var publicationSet in dataSource.PublicationSetsFrom(DataPath))
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

                        lastPublicationSet = publicationSet;

                        lastPublicationDate = rawData.LikelyPublicationDate;
                    }

                    File.WriteAllText($"{RepoPath}\\{rawData.MpKey}.html", rawData.Html);
                }
            }
        }



        private static void ScrapeWebsiteData()
        {
            var serviceCollection = new ServiceCollection();

            ConfigureServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var dataAcquirer = serviceProvider.GetService<ParliamentWebsiteRawHtml>();

            var root = "https://publications.parliament.uk/pa/cm/cmregmem";

            var sessionPages = new string[]
            {
                "contents1617.htm",
                "contents1719.htm",
                "contents1920.htm",
                "contents1921.htm"
            };

            foreach (var sessionPage in sessionPages)
            {
                var publicationSets = dataAcquirer.PublicationSetsInSessionListedAt($"{root}/{sessionPage}");

                var localFolder = DataPath;

                foreach (var publicationSet in publicationSets)
                {
                    var publicationSetRoot = $"{root}/{publicationSet}";

                    var links = dataAcquirer.LinksToIndividualMpPages($"{publicationSetRoot}/contents.htm");

                    var result = dataAcquirer.MpDataFrom(publicationSetRoot, links);

                    foreach (var mpData in result)
                    {
                        var fileContent = JsonSerializer.Serialize(mpData);

                        var filePath = $"{localFolder}\\{mpData.PublicationSet}\\{mpData.MpKey}.json";

                        var file = new FileInfo(filePath);

                        file.Directory?.Create();

                        File.WriteAllText(filePath, fileContent);
                    }
                }
            }
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(configure => configure.AddConsole()).AddTransient<ParliamentWebsiteRawHtml>();
        }
    }
}
