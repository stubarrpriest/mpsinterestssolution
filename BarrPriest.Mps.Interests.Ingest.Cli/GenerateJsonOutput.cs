using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using BarrPriest.Mps.Interests.Ingest.Interfaces.With.DirectoryStructure;
using BarrPriest.Mps.Interests.Ingest.Projections;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BarrPriest.Mps.Interests.Ingest.Cli
{
    public class GenerateJsonOutput
    {
        private readonly ILogger<GenerateJsonOutput> logger;

        private readonly DirectoryStructureOutputSummary dataSource;

        private readonly AmountByPublicationSetForEachMpProjection projection;

        private readonly string localDataPath;

        private readonly string outputSummaryFileName;

        private readonly string outputJsonPath;

        public GenerateJsonOutput(
            ILogger<GenerateJsonOutput> logger,
            DirectoryStructureOutputSummary dataSource,
            IOptions<IngestOptions> options,
            AmountByPublicationSetForEachMpProjection projection)
        {
            this.logger = logger;

            this.dataSource = dataSource;

            this.projection = projection;

            this.localDataPath = options.Value.LocalDataPath;

            this.outputSummaryFileName = options.Value.OutputSummaryFileName;

            this.outputJsonPath = options.Value.OutputJsonPath;
        }

        public async Task MakeJsonOutput()
        {
            var summaries = new List<MpInterestSummary>();

            var dataExplorer = new AmountByPublicationSetForEachMpProjectionExplorer(await this.dataSource.GetProjectionData($"{this.localDataPath}\\{this.outputSummaryFileName}"));

            foreach (var detail in dataExplorer.DetailsByMp())
            {
                this.logger.LogInformation($"Creating file for {detail.Name}");

                var filePath = $"{this.outputJsonPath}\\{detail.Identifier}.json";

                var fileContent = JsonSerializer.Serialize(detail, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true,
                });

                summaries.Add(new MpInterestSummary(detail.Identifier, detail.Name, detail.CurrentValue, detail.HistoricalValue, detail.LatestEntryDate));

                await this.OutputFile(filePath, fileContent);
            }

            await this.OutputFile($"{this.outputJsonPath}\\summary.json", JsonSerializer.Serialize(summaries.OrderByDescending(x => x.HistoricalValue).ToArray(), new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
            }));
        }

        private async Task OutputFile(string filePath, string fileContent)
        {
            var file = new FileInfo(filePath);

            file.Directory?.Create();

            await File.WriteAllTextAsync(filePath, fileContent);

            this.logger.LogInformation($"Json file outputted to {filePath}");
        }
    }
}
