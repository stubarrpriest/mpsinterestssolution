using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace BarrPriest.Mps.Interests.Ingest.Interfaces.With.DirectoryStructure
{
    public class DirectoryStructureRawHtml
    {
        public RawHtmlData[] MpDataFrom(string path, string publicationSet)
        {
            var list = new List<RawHtmlData>();

            var dirInfo = new DirectoryInfo($"{path}\\{publicationSet}");

            foreach (var file in dirInfo.GetFiles())
            {
                var content = File.ReadAllText(file.FullName);

                var item = JsonSerializer.Deserialize<RawHtmlDto>(content);

                list.Add(new RawHtmlData(item.SourceUrl, item.Acquired, item.Html));
            }

            return list.ToArray();
        }

        public string[] PublicationSetsFrom(string path)
        {
            var list = new List<string>();

            var dirInfo = new DirectoryInfo(path);

            foreach (var directory in dirInfo.GetDirectories())
            {
                list.Add(directory.Name);
            }

            return list.ToArray();
        }
    }
}
