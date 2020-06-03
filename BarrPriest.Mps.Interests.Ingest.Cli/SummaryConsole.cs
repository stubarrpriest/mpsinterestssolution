using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<string> ShowReport()
        {
            var dataExplorer = new AmountByPublicationSetForEachMpProjectionExplorer(await this.dataSource.GetProjectionData($"{this.localDataPath}\\{this.outputSummaryFileName}"));

            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("Financial interests since 14/12/2015, top twenty entries by value");

            stringBuilder.AppendLine(string.Format("|{0,20}|{1,20}|{2,20}", "Member's name", "Approximate value", "Most recent entry"));

            stringBuilder.Append(this.ShowEarners(() => dataExplorer.TopEarnersOverall(20)));

            stringBuilder.AppendLine("Current financial interests, top twenty entries by value");

            stringBuilder.AppendLine(string.Format("|{0,20}|{1,20}|{2,20}", "Member's name", "Approximate value", "Most recent entry"));

            stringBuilder.Append(this.ShowEarners(() => dataExplorer.TopCurrentEarners(20)));

            stringBuilder.AppendLine("Historic financial interests, top twenty entries by value");

            stringBuilder.AppendLine(string.Format("|{0,20}|{1,20}|{2,20}", "Member's name", "Approximate value", "Most recent entry"));

            stringBuilder.Append(this.ShowEarners(() => dataExplorer.TopHistoricalEarners(20)));

            return stringBuilder.ToString();
        }

        public string ShowEarners(Func<List<MpInterestValue>> query)
        {
            var stringBuilder = new StringBuilder();

            var list = query.Invoke();

            foreach (var item in list)
            {
                stringBuilder.AppendLine(string.Format("|{0,20}|{1,20}|{2,20}", item.Name.Replace("_", ",").ToUpper(), new RangeDisplay().Bucket(item.Amount), item.AsOf.ToShortDateString()));
            }

            var total = decimal.Round(list.Sum(x => x.Amount));

            stringBuilder.AppendLine(string.Format("|{0,20}| +/- {1,15}|", "TOTAL", total.ToString("C0")));

            return stringBuilder.ToString();
        }
    }
}
