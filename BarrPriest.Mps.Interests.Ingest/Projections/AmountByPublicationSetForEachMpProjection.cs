using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BarrPriest.Mps.Interests.Ingest.Events;
using BarrPriest.Mps.Interests.Ingest.Interfaces.With;

namespace BarrPriest.Mps.Interests.Ingest.Projections
{
    public class AmountByPublicationSetForEachMpProjection
    {
        private readonly IParseMoneyFromHtml moneyParser;

        private readonly Dictionary<string, Dictionary<string, PublicationSetTotal>> mpTotalsByPublicationSet = new Dictionary<string, Dictionary<string, PublicationSetTotal>>();

        public AmountByPublicationSetForEachMpProjection(IParseMoneyFromHtml moneyParser)
        {
            this.moneyParser = moneyParser;
        }

        public void Handle(RawHtmlDataAcquiredEvent rawHtmlDataAcquiredEvent)
        {
            var rawHtmlData = new RawHtmlData(rawHtmlDataAcquiredEvent.SourceUrl, rawHtmlDataAcquiredEvent.Acquired, rawHtmlDataAcquiredEvent.Html);

            if (!this.mpTotalsByPublicationSet.ContainsKey(rawHtmlData.MpKey))
            {
                this.mpTotalsByPublicationSet.Add(rawHtmlData.MpKey, new Dictionary<string, PublicationSetTotal>());
            }

            var amount = this.moneyParser.Parse(rawHtmlData.FilteredHtml).Sum(x => x.Amount);

            this.mpTotalsByPublicationSet[rawHtmlData.MpKey][rawHtmlData.PublicationSet] = new PublicationSetTotal(rawHtmlData.PublicationSet, amount);
        }

        public Dictionary<string, Dictionary<string, PublicationSetTotal>> Result()
        {
            return new Dictionary<string, Dictionary<string, PublicationSetTotal>>(this.mpTotalsByPublicationSet);
        }
    }
}
