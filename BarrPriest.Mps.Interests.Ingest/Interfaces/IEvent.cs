using System;

namespace BarrPriest.Mps.Interests.Ingest.Interfaces
{
    public interface IEvent
    {
        Guid EventId { get; }
    }
}
