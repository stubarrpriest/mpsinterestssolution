using Azure;
using Azure.AI.TextAnalytics;
using BarrPriest.Mps.Interests.Ingest.Interfaces.With;
using BarrPriest.Mps.Interests.Ingest.Interfaces.With.DirectoryStructure;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarrPriest.MPs.Interests.Examine.Cli
{
    public static class Program
    {
        private static readonly AzureKeyCredential credentials = new AzureKeyCredential("");
        
        private static readonly Uri endpoint = new Uri("");

        private static readonly string connectionString = "";

        private static int MaxDocSize = 5000;

        private static string MPDataPath = @"C:\temp\mpsinterestsnew";

        public static async Task Main(string[] args)
        {
            var client = new TextAnalyticsClient(endpoint, credentials);

            var databaseService = new SqlResultStore(connectionString);

            var textAnalyticsService = new TextAnalyticsService(client, MaxDocSize);

            await BatchAnalysis(client, databaseService, textAnalyticsService);
        }

        static async Task SerialAnalysis(TextAnalyticsClient client, SqlResultStore databaseService, TextAnalyticsService textAnalyticsService)
        {
            var dataSource = new DirectoryStructureRawHtml();

            var mps = GetMpData(GetPublicationSets(dataSource).First(), dataSource);

            var count = 1;

            var skipValue = 0;

            foreach (var mp in mps.Skip(skipValue))
            {
                var source = mp.FilteredHtml;

                var interestingEntities = await textAnalyticsService.RecogniseEntitiesFrom(new CompatibleDocument(source, MaxDocSize), mp.MpKey);

                await databaseService.SaveEntities(mp, interestingEntities);

                // Linked entities are not very useful.

                // var extractedEntities = textAnalyticsService.MakeADocumentFrom(interestingEntities);
                
                // var linkedEntities = await GetLinkedEntitiesFrom(client, new CompatibleDocument(extractedEntities, MaxDocSize));

                // await SaveLinkedEntities(mp, linkedEntities);

                Console.WriteLine($"Completed {count + skipValue} {mp.MpKey}");

                count++;
            }
        }

        static List<RawHtmlData> GetMpData(string publicationSet, DirectoryStructureRawHtml dataSource)
        {
            return dataSource.MpDataFrom(MPDataPath, publicationSet).ToList();
        }

        static List<string> GetPublicationSets(DirectoryStructureRawHtml dataSource)
        {
            return dataSource.PublicationSetsFrom(MPDataPath).OrderByDescending(x => x).Take(118).Skip(102).ToList();
        }

        static async Task BatchAnalysis(TextAnalyticsClient client, SqlResultStore databaseService, TextAnalyticsService textAnalyticsService)
        {
            var dataSource = new DirectoryStructureRawHtml();

            foreach (var publicationSet in GetPublicationSets(dataSource))
            {
                var stopwatch = Stopwatch.StartNew();

                Console.WriteLine($"Publication set starts: {publicationSet}");

                var mps = GetMpData(publicationSet, dataSource);

                Console.WriteLine($"Count of MPs: {mps.Count}");

                var count = 1;

                var skipValue = 0;

                var batchDocuments = new List<TextDocumentInput>();

                foreach (var mp in mps.Skip(skipValue))
                {
                    var index = 1;

                    foreach (var doc in new CompatibleDocument(mp.FilteredHtml, MaxDocSize).DocumentChunks())
                    {
                        batchDocuments.Add(new TextDocumentInput(new DocumentKey(mp, index).ToString(), doc));

                        index++;
                    }
                }

                Console.WriteLine($"Beginning analysis after {stopwatch.ElapsedMilliseconds}ms");

                var interestingEntities = await textAnalyticsService.RecogniseEntitiesFrom(batchDocuments);

                Console.WriteLine($"Analysis completed after {stopwatch.Elapsed.TotalSeconds} seconds");

                await databaseService.SaveEntities(interestingEntities);

                Console.WriteLine($"Publication set completes: {publicationSet} | {stopwatch.Elapsed.TotalSeconds} seconds");
            }
        }
    }
}
