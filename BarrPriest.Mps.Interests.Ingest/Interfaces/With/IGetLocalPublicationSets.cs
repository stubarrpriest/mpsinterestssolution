using System.Threading.Tasks;

namespace BarrPriest.Mps.Interests.Ingest.Interfaces.With
{
    public interface IGetLocalPublicationSets
    {
        Task<string> MostRecentPublicationSetAsync();
    }
}
