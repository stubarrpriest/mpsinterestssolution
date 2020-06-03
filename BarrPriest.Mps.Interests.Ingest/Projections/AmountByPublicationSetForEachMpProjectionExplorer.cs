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

        public List<MpInterestValue> TopEarnersOverall(int take)
        {
            var memberCount = this.input.Keys.Count;

            var allEarnings = this.TopCurrentEarners(memberCount).Union(this.TopHistoricalEarners(memberCount));

            return allEarnings
                .GroupBy(x => x.Name)
                .Select(g => new MpInterestValue(g.Key, g.Sum(x => x.Amount), g.Max(x => x.AsOf)))
                .OrderByDescending(x => x.Amount)
                .Take(take)
                .ToList();
        }

        public List<MpInterestValue> TopCurrentEarners(int take)
        {
            var currentValueByMp = new List<MpInterestValue>();

            foreach (var key in this.input.Keys)
            {
                var mostRecentEntryKey = this.input[key].Max(x => x.Key);

                var likelyPublicationDate = new PublicationSetDate(mostRecentEntryKey);

                currentValueByMp.Add(new MpInterestValue(key, this.input[key][mostRecentEntryKey].Amount, likelyPublicationDate.LikelyPublicationDate));
            }

            return currentValueByMp.OrderByDescending(x => x.Amount).Take(take).ToList();
        }

        public List<MpInterestValue> TopHistoricalEarners(int take)
        {
            var moneyWhichHasLeftTheRegister = new List<MpInterestValue>();

            foreach (var key in this.input.Keys)
            {
                var totalForThisMp = 0m;

                var lastValue = 0m;

                var lastPublicationSet = string.Empty;

                foreach (var publicationSet in this.input[key].Keys.OrderBy(x => x))
                {
                    var value = this.input[key][publicationSet].Amount;

                    if (value < lastValue)
                    {
                        totalForThisMp += lastValue - value;
                    }

                    lastValue = value;

                    lastPublicationSet = publicationSet;
                }

                moneyWhichHasLeftTheRegister.Add(new MpInterestValue(key, totalForThisMp, new PublicationSetDate(lastPublicationSet).LikelyPublicationDate));
            }

            return moneyWhichHasLeftTheRegister.OrderByDescending(x => x.Amount).Take(take).ToList();
        }
    }
}
