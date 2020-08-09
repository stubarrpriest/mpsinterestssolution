using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarrPriest.Mps.Interests.Ingest.Events;
using BarrPriest.Mps.Interests.Ingest.Interfaces.With;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace BarrPriest.Mps.Interests.Ingest
{
    public class IngestData
    {
        private readonly IBus bus;

        private readonly IGetLocalPublicationSets local;

        private readonly IGetRemotePublicationSets remote;

        public IngestData(IBus bus, IGetLocalPublicationSets local, IGetRemotePublicationSets remote)
        {
            this.bus = bus;

            this.local = local;

            this.remote = remote;
        }

        [FunctionName("CheckForNewData")]
        public async Task CheckForNewData([TimerTrigger("0 0 10 * * *")] TimerInfo timer, ILogger log)
        {
            var correlationId = Guid.NewGuid();

            using (log.BeginScope("Executing CheckForNewData CorrelationId: {correlationId}", correlationId))
            {
                var lastCompletedPublicationSet = await this.local.MostRecentPublicationSetAsync();

                log.LogInformation("Last completed PublicationSet {lastCompletedPublicationSet}", lastCompletedPublicationSet);

                var newPublicationSets = await this.remote.NextPublicationSetsAfterAsync(lastCompletedPublicationSet);

                log.LogInformation("New PublicationSets {newPublicationSets}", newPublicationSets.Length > 0 ? string.Join(',', newPublicationSets) : "None");

                if (newPublicationSets.Length > 0)
                {
                    var nextPublicationSet = newPublicationSets.OrderBy(x => x).First();

                    if (new PublicationSetDate(nextPublicationSet).LikelyPublicationDate > new PublicationSetDate(lastCompletedPublicationSet).LikelyPublicationDate)
                    {
                        var discoveredEvent = new NewPublicationSetDiscoveredEvent(nextPublicationSet);

                        await this.bus.PublishAsync(discoveredEvent);
                    }
                }
            }
        }
    }
}
