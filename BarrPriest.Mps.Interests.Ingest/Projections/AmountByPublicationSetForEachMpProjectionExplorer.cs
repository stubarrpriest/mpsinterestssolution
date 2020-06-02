using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BarrPriest.Mps.Interests.Ingest.Interfaces.With;

namespace BarrPriest.Mps.Interests.Ingest.Projections
{
    public class AmountByPublicationSetForEachMpProjectionExplorer
    {
        private readonly Dictionary<string, Dictionary<string, PublicationSetTotal>> input;

        public AmountByPublicationSetForEachMpProjectionExplorer(Dictionary<string, Dictionary<string, PublicationSetTotal>> input)
        {
            this.input = input;
        }

        public List<MpInterestValue> TopFiftyEarners()
        {
            var currentValueByMp = new List<MpInterestValue>();

            foreach (var key in this.input.Keys)
            {
                var mostRecentEntryKey = this.input[key].Max(x => x.Key);

                var likelyPublicationDate = new PublicationSetDate(mostRecentEntryKey);

                currentValueByMp.Add(new MpInterestValue(key, this.input[key][mostRecentEntryKey].Amount, likelyPublicationDate.LikelyPublicationDate));
            }

            return currentValueByMp.OrderByDescending(x => x.Amount).Take(50).ToList();
        }
    }
}
