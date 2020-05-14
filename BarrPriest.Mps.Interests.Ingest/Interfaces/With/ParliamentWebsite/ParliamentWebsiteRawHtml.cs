using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ScrapySharp.Extensions;
using ScrapySharp.Network;

namespace BarrPriest.Mps.Interests.Ingest.Interfaces.With.ParliamentWebsite
{
    public class ParliamentWebsiteRawHtml
    {
        private readonly ILogger<ParliamentWebsiteRawHtml> logger;

        private readonly ScrapingBrowser browser = new ScrapingBrowser() { Encoding = Encoding.UTF8 };

        public ParliamentWebsiteRawHtml(ILogger<ParliamentWebsiteRawHtml> logger)
        {
            this.logger = logger;
        }

        public async Task<RawHtmlData> MpDataFromAsync(string url)
        {
            var mpInterestPage = await this.browser.NavigateToPageAsync(new Uri(url));

            var nodes = mpInterestPage.Html.CssSelect("div#mainTextBlock > p");

            return new RawHtmlData(url, DateTimeOffset.Now, nodes.MergeInParentNode("div").OuterHtml);
        }

        public async Task<RawHtmlData[]> MpDataFromAsync(string rootDirUrl, string[] mpUrls)
        {
            var rawData = new List<RawHtmlData>();

            var stopWatch = new Stopwatch();

            long totalElapsed = 0;

            foreach (var mpUrl in mpUrls)
            {
                stopWatch.Start();

                this.logger.LogInformation($"Fetching {mpUrl}");

                rawData.Add(await this.MpDataFromAsync($"{rootDirUrl}/{mpUrl}"));

                stopWatch.Stop();

                var elapsed = stopWatch.ElapsedMilliseconds;

                totalElapsed += elapsed;

                stopWatch.Reset();

                this.logger.LogInformation($"Fetched {mpUrl} in {elapsed}ms from {totalElapsed}ms");
            }

            return rawData.ToArray();
        }

        public async Task<string[]> LinksToIndividualMpPagesAsync(string urlContentsPage)
        {
            var links = new List<string>();

            var contentsPage = await this.browser.NavigateToPageAsync(new Uri(urlContentsPage));

            var nodes = contentsPage.Html.CssSelect("div#mainTextBlock > p > a");

            foreach (var anchor in nodes)
            {
                foreach (var attribute in anchor.Attributes.Where(x => x.Name == "href"))
                {
                    if (!attribute.Value.StartsWith("#"))
                    {
                        links.Add(attribute.Value);
                    }
                }
            }

            return links.ToArray();
        }

        public async Task<string[]> PublicationSetsInSessionListedAtAsync(string urlSessionPage)
        {
            var links = new List<string>();

            var contentsPage = await this.browser.NavigateToPageAsync(new Uri(urlSessionPage));

            var nodes = contentsPage.Html.CssSelect("div#maincontent a");

            foreach (var anchor in nodes)
            {
                if (anchor.InnerText.ToUpperInvariant().StartsWith("HTML"))
                {
                    foreach (var attribute in anchor.Attributes.Where(x => x.Name == "href"))
                    {
                        if (!attribute.Value.StartsWith("introduction"))
                        {
                            links.Add(attribute.Value.Split('/').FirstOrDefault());
                        }
                    }
                }
            }

            return links.ToArray();
        }
    }
}
