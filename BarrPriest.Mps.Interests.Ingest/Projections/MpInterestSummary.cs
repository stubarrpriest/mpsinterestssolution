using System;
using System.Collections.Generic;
using System.Text;

namespace BarrPriest.Mps.Interests.Ingest.Projections
{
    public class MpInterestSummary
    {
        public MpInterestSummary(
            string identifier,
            string name,
            decimal currentValue,
            decimal historicalValue,
            DateTime latestEntryDate)
        {
            this.Identifier = identifier;

            this.Name = name;

            this.CurrentValue = currentValue;

            this.HistoricalValue = historicalValue;

            this.LatestEntryDate = latestEntryDate;
        }

        public string Identifier { get; }

        public string Name { get; }

        public decimal CurrentValue { get; }

        public decimal HistoricalValue { get; }

        public DateTime LatestEntryDate { get; }
    }
}
