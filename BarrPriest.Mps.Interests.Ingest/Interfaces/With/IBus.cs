using System.Threading.Tasks;

namespace BarrPriest.Mps.Interests.Ingest.Interfaces.With
{
    public interface IBus
    {
        Task PublishAsync(IEvent eventMessage);
    }
}
