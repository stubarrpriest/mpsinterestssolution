using System;
using System.Linq;

namespace BarrPriest.Mps.Interests.Ingest.Interfaces.With
{
    public class RawHtmlData
    {
        private readonly Uri uri;

        public RawHtmlData(string sourceUrl, DateTimeOffset acquired, string html)
        {
            this.SourceUrl = sourceUrl;

            this.uri = new Uri(this.SourceUrl);

            this.Acquired = acquired;

            this.Html = html;
        }

        public string SourceUrl { get; }

        public string PublicationSet
        {
            get
            {
                return this.uri != null ? this.uri.Segments[^2].Replace("/", string.Empty) : string.Empty;
            }
        }

        public DateTime LikelyPublicationDate
        {
            get
            {
                var dateParts = new int[] { int.Parse(this.PublicationSet.Substring(0, 2)), int.Parse(this.PublicationSet.Substring(2, 2)), int.Parse(this.PublicationSet.Substring(4, 2)) };

                return new DateTime(this.AssumedYearFromTwoCharacters(dateParts[0]), dateParts[1], dateParts[2]);
            }
        }

        public string MpKey
        {
            get { return this.uri != null ? this.uri.Segments.Last().Replace(".htm", string.Empty) : string.Empty; }
        }

        public DateTimeOffset Acquired { get; }

        public string Html { get; }

        private int AssumedYearFromTwoCharacters(int datePart)
        {
            return 2000 + datePart;
        }
    }
}
