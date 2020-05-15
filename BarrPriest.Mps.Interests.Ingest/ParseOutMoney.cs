using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace BarrPriest.Mps.Interests.Ingest
{
    public static class ParseOutMoney
    {
        [FunctionName("ParseOutMoney")]
        public static void Run([QueueTrigger("interests", Connection = "")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
