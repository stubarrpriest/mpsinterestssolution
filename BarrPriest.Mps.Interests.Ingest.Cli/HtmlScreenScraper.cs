using System.IO;
using System.Text.Json;
using BarrPriest.Mps.Interests.Ingest.Interfaces.With.ParliamentWebsite;
using Microsoft.Extensions.Logging;

namespace BarrPriest.Mps.Interests.Ingest.Cli
{
    public class HtmlScreenScraper
    {
        private const string DataPath = @"c:\temp\mpsinterests\";

        private const string Root = "https://publications.parliament.uk/pa/cm/cmregmem";

        private readonly string[] sessionPages = new string[]
        {
            "contents1617.htm",
            "contents1719.htm",
            "contents1920.htm",
            "contents1921.htm"
        };

        private readonly ILogger<HtmlScreenScraper> logger;

        private readonly ParliamentWebsiteRawHtml dataAcquirer;

        public HtmlScreenScraper(ILogger<HtmlScreenScraper> logger, ParliamentWebsiteRawHtml dataAcquirer)
        {
            this.logger = logger;

            this.dataAcquirer = dataAcquirer;
        }

        public void Scrape()
        {
            foreach (var sessionPage in this.sessionPages)
            {
                var publicationSets = dataAcquirer.PublicationSetsInSessionListedAt($"{Root}/{sessionPage}");

                var localFolder = DataPath;

                this.logger.LogInformation($"Scraping {publicationSets.Length} publication sets to {localFolder}");

                foreach (var publicationSet in publicationSets)
                {
                    this.logger.LogInformation($"Scraping {publicationSet}");

                    var publicationSetRoot = $"{Root}/{publicationSet}";

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
