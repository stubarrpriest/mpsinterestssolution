using System.IO;
using System.Text.Json;
using BarrPriest.Mps.Interests.Ingest.Interfaces.With.ParliamentWebsite;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BarrPriest.Mps.Interests.Ingest.Cli
{
    public class HtmlScreenScraper
    {
        private readonly string localDataPath;

        private readonly string parliamentWebsiteRootDirectory;

        private readonly string[] parliamentWebsiteSessionPageNames;

        private readonly ILogger<HtmlScreenScraper> logger;

        private readonly ParliamentWebsiteRawHtml dataAcquirer;

        public HtmlScreenScraper(ILogger<HtmlScreenScraper> logger, ParliamentWebsiteRawHtml dataAcquirer, IOptions<IngestOptions> options)
        {
            this.logger = logger;

            this.dataAcquirer = dataAcquirer;

            this.localDataPath = options.Value.LocalDataPath;

            this.parliamentWebsiteRootDirectory = options.Value.ParliamentWebsiteRootDirectory;

            this.parliamentWebsiteSessionPageNames = options.Value.ParliamentWebsiteSessionPageNames;
        }

        public void Scrape()
        {
            foreach (var sessionPage in this.parliamentWebsiteSessionPageNames)
            {
                var publicationSets = dataAcquirer.PublicationSetsInSessionListedAt($"{parliamentWebsiteRootDirectory}/{sessionPage}");

                var localFolder = localDataPath;

                this.logger.LogInformation($"Scraping {publicationSets.Length} publication sets to {localFolder}");

                foreach (var publicationSet in publicationSets)
                {
                    this.logger.LogInformation($"Scraping {publicationSet}");

                    var publicationSetRoot = $"{parliamentWebsiteRootDirectory}/{publicationSet}";

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
    }
}
