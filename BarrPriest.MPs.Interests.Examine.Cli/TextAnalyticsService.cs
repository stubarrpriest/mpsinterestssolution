using Azure.AI.TextAnalytics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MoreLinq;

namespace BarrPriest.MPs.Interests.Examine.Cli
{
    public class TextAnalyticsService
    {
        private readonly TextAnalyticsClient client;

        private readonly int maxDocSize;

        public TextAnalyticsService(TextAnalyticsClient client, int maxDocSize)
        {
            this.client = client;

            this.maxDocSize = maxDocSize;
        }

        public async Task<List<CategorizedEntity>> RecogniseEntitiesFrom(CompatibleDocument document, string mpKey)
        {
            var interestingEntities = new List<CategorizedEntity>();

            if (document.Length() > 0)
            {
                foreach (var chunk in document.DocumentChunks())
                {
                    Console.WriteLine($"Request | {mpKey}");

                    var response = await this.client.RecognizeEntitiesAsync(chunk);

                    foreach (var entity in response.Value)
                    {
                        if (entity.Category == EntityCategory.Person || entity.Category == EntityCategory.Organization)
                        {
                            interestingEntities.Add(entity);
                        }
                    }
                }
            }

            return interestingEntities;
        }

        private async Task<List<KeyValuePair<DocumentKey, CategorizedEntity>>> GetNamedEntitiesAsync(IEnumerable<TextDocumentInput> chunkOfDocuments)
        {
            var actions = new TextAnalyticsActions()
            {
                RecognizeEntitiesActions = new List<RecognizeEntitiesAction>() { new RecognizeEntitiesAction() }
            };

            var taskEntities = new List<KeyValuePair<DocumentKey, CategorizedEntity>>();

            var operation = await client.StartAnalyzeActionsAsync(chunkOfDocuments, actions);

            await operation.WaitForCompletionAsync();

            await foreach (var documentsInPage in operation.Value)
            {
                var entitiesResult = documentsInPage.RecognizeEntitiesResults.FirstOrDefault().DocumentsResults;
                
                foreach (var result in entitiesResult)
                {
                    Console.WriteLine(result.Id);

                    foreach (CategorizedEntity entity in result.Entities)
                    {
                        if (entity.Category == EntityCategory.Person || entity.Category == EntityCategory.Organization)
                        {
                            taskEntities.Add(new KeyValuePair<DocumentKey, CategorizedEntity>(new DocumentKey(result.Id), entity));
                        }
                    }
                }
            }

            return taskEntities;
        }

        public Task<List<KeyValuePair<DocumentKey, CategorizedEntity>>> DownloadNamedEntitiesAsync(IEnumerable<TextDocumentInput> chunkOfDocuments)
        {
            return Task.Run(async () =>
            {
                var entities = await GetNamedEntitiesAsync(chunkOfDocuments);

                return entities;
            });
        }

        public async Task<List<KeyValuePair<DocumentKey, CategorizedEntity>>> RecogniseEntitiesFrom(List<TextDocumentInput> documents)
        {
            var interestingEntities = new List<KeyValuePair<DocumentKey, CategorizedEntity>>();

            try
            {
                var downloads = documents.Batch(25).Select(DownloadNamedEntitiesAsync);

                var results = await Task.WhenAll(downloads);

                foreach (var list in results)
                {
                    interestingEntities.AddRange(list);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                Console.WriteLine(ex.StackTrace);
            }

            return interestingEntities;
        }

        public string MakeADocumentFrom(List<CategorizedEntity> entities)
        {
            var document = new StringBuilder();

            foreach (var entity in entities)
            {
                document.AppendLine(entity.Text);
            }

            return document.ToString();
        }

        public async Task<List<LinkedEntity>> GetLinkedEntitiesFrom(CompatibleDocument document)
        {
            var linkedEntities = new List<LinkedEntity>();

            if (document.Length() > 0)
            {
                foreach (var chunk in document.DocumentChunks())
                {
                    var response = await this.client.RecognizeLinkedEntitiesAsync(chunk);

                    foreach (var entity in response.Value)
                    {
                        linkedEntities.Add(entity);
                    }
                }
            }

            return linkedEntities;
        }
    }
}
