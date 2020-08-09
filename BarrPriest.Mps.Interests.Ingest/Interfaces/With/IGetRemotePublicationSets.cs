using System.Threading.Tasks;

namespace BarrPriest.Mps.Interests.Ingest.Interfaces.With
{
    public interface IGetRemotePublicationSets
    {
        Task<string[]> NextPublicationSetsAfterAsync(string currentPublicationSet);
    }
}
