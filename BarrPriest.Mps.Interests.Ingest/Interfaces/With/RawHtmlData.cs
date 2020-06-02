using System;
using System.Linq;
using HtmlAgilityPack;
using ScrapySharp.Extensions;

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
                var publicationSetDate = new PublicationSetDate(this.PublicationSet);

                return publicationSetDate.LikelyPublicationDate;
            }
        }

        public string MpKey
        {
            get { return this.uri != null ? this.uri.Segments.Last().Replace(".htm", string.Empty) : string.Empty; }
        }

        public DateTimeOffset Acquired { get; }

        public string Html { get; }

        public string FilteredHtml
        {
            get
            {
                var allNodes = HtmlNode.CreateNode(this.Html);

                var interestNodes = allNodes.CssSelect("p.indent");

                interestNodes = interestNodes.Union(allNodes.CssSelect("p.indent2"));

                return interestNodes.MergeInParentNode("div").OuterHtml;
            }
        }
    }
}
