using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BarrPriest.Mps.Interests.Ingest.Interfaces.With.DirectoryStructure;
using BarrPriest.Mps.Interests.Ingest.Projections;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BarrPriest.Mps.Interests.Ingest.Cli
{
    public class SummaryConsole
    {
        private readonly ILogger<SummaryConsole> logger;

        private readonly DirectoryStructureOutputSummary dataSource;

        private readonly string localDataPath;

        private readonly string outputSummaryFileName;

        public SummaryConsole(
            ILogger<SummaryConsole> logger,
            DirectoryStructureOutputSummary dataSource,
            IOptions<IngestOptions> options)
        {
            this.logger = logger;

            this.dataSource = dataSource;

            this.localDataPath = options.Value.LocalDataPath;

            this.outputSummaryFileName = options.Value.OutputSummaryFileName;
        }

        public async Task<string> ShowTopTwentyEarners()
        {
            var dataExplorer = new AmountByPublicationSetForEachMpProjectionExplorer(await this.dataSource.GetProjectionData($"{this.localDataPath}\\{this.outputSummaryFileName}"));

            var stringBuilder = new StringBuilder();

            foreach (var item in dataExplorer.TopFiftyEarners())
            {
                stringBuilder.AppendLine(string.Format("|{0,20}|{1,20}|{2,20}", item.Name, item.Amount.ToString("C"), item.AsOf.ToShortDateString()));
            }

            return stringBuilder.ToString();
        }
    }
}
