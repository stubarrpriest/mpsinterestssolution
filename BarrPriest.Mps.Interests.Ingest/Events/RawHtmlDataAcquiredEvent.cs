using System;
using System.Collections.Generic;
using System.Text;

namespace BarrPriest.Mps.Interests.Ingest.Events
{
    public class RawHtmlDataAcquiredEvent
    {
        public RawHtmlDataAcquiredEvent(string sourceUrl, DateTimeOffset acquired, string html)
        {
            this.SourceUrl = sourceUrl;

            this.Acquired = acquired;

            this.Html = html;
        }

        public string SourceUrl { get; private set; }

        public DateTimeOffset Acquired { get; private set; }

        public string Html { get; private set; }
    }
}
