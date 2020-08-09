using System;
using System.Collections.Generic;
using System.Text;
using BarrPriest.Mps.Interests.Ingest.Interfaces;

namespace BarrPriest.Mps.Interests.Ingest.Events
{
    public class NewPublicationSetDiscoveredEvent : IEvent
    {
        public NewPublicationSetDiscoveredEvent(string publicationSetName)
        {
            this.PublicationSetName = publicationSetName;

            this.EventId = Guid.NewGuid();

            this.EventTime = DateTimeOffset.UtcNow;
        }

        public Guid EventId { get; }

        public DateTimeOffset EventTime { get; }

        public string PublicationSetName { get; }
    }
}
