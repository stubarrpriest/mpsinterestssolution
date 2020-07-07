using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace BarrPriest.Mps.Interests.Ingest
{
    public static class IngestData
    {
        [FunctionName("CheckForNewData")]
        public static void CheckForNewData([TimerTrigger("0 0 10 * * *")] TimerInfo timer, ILogger log)
        {
            var correlationId = Guid.NewGuid();

            using (log.BeginScope("Executing CheckForNewData CorrelationId: {correlationId}", correlationId))
            {
                log.LogInformation("Timer is past due: {timerIsPastDue}", timer.IsPastDue);
            }
        }
    }
}
