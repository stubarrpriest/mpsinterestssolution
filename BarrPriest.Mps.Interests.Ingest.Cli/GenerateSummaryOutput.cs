using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using BarrPriest.Mps.Interests.Ingest.Events;
using BarrPriest.Mps.Interests.Ingest.Interfaces.With.DirectoryStructure;
using BarrPriest.Mps.Interests.Ingest.Projections;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BarrPriest.Mps.Interests.Ingest.Cli
{
    public class GenerateSummaryOutput
    {
        private readonly ILogger<GenerateSummaryOutput> logger;

        private readonly DirectoryStructureRawHtml dataSource;

        private readonly AmountByPublicationSetForEachMpProjection projection;

        private readonly string localDataPath;

        private readonly string outputSummaryFileName;

        public GenerateSummaryOutput(
            ILogger<GenerateSummaryOutput> logger,
            DirectoryStructureRawHtml dataSource,
            IOptions<IngestOptions> options,
            AmountByPublicationSetForEachMpProjection projection)
        {
            this.logger = logger;

            this.dataSource = dataSource;

            this.projection = projection;

            this.localDataPath = options.Value.LocalDataPath;

            this.outputSummaryFileName = options.Value.OutputSummaryFileName;
        }

        public async Task MakeSummary()
        {
            foreach (var publicationSet in this.dataSource.PublicationSetsFrom(this.localDataPath).OrderBy(x => x))
            {
                this.logger.LogInformation($"Extracting monetary values from publication set {publicationSet}");

                foreach (var rawData in this.dataSource.MpDataFrom(this.localDataPath, publicationSet))
                {
                    this.projection.Handle(new RawHtmlDataAcquiredEvent(rawData.SourceUrl, rawData.Acquired, rawData.Html));
                }
            }

            await this.OutputFile();
        }

        private async Task OutputFile()
        {
            var fileContent = JsonSerializer.Serialize(this.projection.Result());

            var filePath = $"{this.localDataPath}\\{this.outputSummaryFileName}";

            var file = new FileInfo(filePath);

            file.Directory?.Create();

            await File.WriteAllTextAsync(filePath, fileContent);

            this.logger.LogInformation($"Json file outputted to {filePath}");
        }
    }
}
