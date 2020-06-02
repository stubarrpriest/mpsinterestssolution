using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BarrPriest.Mps.Interests.Ingest.Projections;

namespace BarrPriest.Mps.Interests.Ingest.Interfaces.With.DirectoryStructure
{
    public class DirectoryStructureOutputSummary
    {
        public async Task<Dictionary<string, Dictionary<string, PublicationSetTotal>>> GetProjectionData(string path)
        {
            var content = await File.ReadAllTextAsync(path);

            var item = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, PublicationSetTotal>>>(content);

            return item;
        }
    }
}
