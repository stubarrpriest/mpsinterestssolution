using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
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

        public async Task Scrape()
        {
            foreach (var sessionPage in this.parliamentWebsiteSessionPageNames)
            {
                var publicationSets = await this.dataAcquirer.PublicationSetsInSessionListedAtAsync($"{this.parliamentWebsiteRootDirectory}/{sessionPage}");

                var localFolder = this.localDataPath;

                this.logger.LogInformation($"Scraping {publicationSets.Length} publication sets to {localFolder}");

                foreach (var publicationSet in publicationSets)
                {
                    this.logger.LogInformation($"Scraping {publicationSet}");

                    var publicationSetRoot = $"{this.parliamentWebsiteRootDirectory}/{publicationSet}";

                    var links = await this.dataAcquirer.LinksToIndividualMpPagesAsync($"{publicationSetRoot}/contents.htm");

                    var result = await this.dataAcquirer.MpDataFromAsync(publicationSetRoot, links);

                    foreach (var mpData in result)
                    {
                        var fileContent = JsonSerializer.Serialize(mpData);

                        var filePath = $"{localFolder}\\{mpData.PublicationSet}\\{mpData.MpKey}.json";

                        var file = new FileInfo(filePath);

                        file.Directory?.Create();

                        await File.WriteAllTextAsync(filePath, fileContent);
                    }
                }
            }
        }
    }
}
