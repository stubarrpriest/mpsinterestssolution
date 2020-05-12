using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text.Json;
using BarrPriest.Mps.Interests.Ingest.Interfaces.With.ParliamentWebsite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BarrPriest.Mps.Interests.Ingest.Cli
{
    class Program
    {
        static void Main(string[] args)
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

                var localFolder = @"c:\temp\mpsinterests\";

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
