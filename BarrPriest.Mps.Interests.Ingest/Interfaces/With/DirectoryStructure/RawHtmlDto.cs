using System;

namespace BarrPriest.Mps.Interests.Ingest.Interfaces.With.DirectoryStructure
{
    public class RawHtmlDto
    {
        public string SourceUrl { get; set; }

        public DateTimeOffset Acquired { get; set; }

        public string Html { get; set; }
    }
}
