using System;
using System.Collections.Generic;
using System.Text;

namespace BarrPriest.Mps.Interests.Ingest.Projections
{
    public class MpInterestDetail
    {
        public MpInterestDetail(
            string identifier,
            string name,
            decimal currentValue,
            decimal historicalValue,
            DateTime latestEntryDate,
            PublicationSet[] publicationSets,
            string gitHubPathHash)
        {
            this.Identifier = identifier;

            this.Name = name;

            this.CurrentValue = currentValue;

            this.HistoricalValue = historicalValue;

            this.LatestEntryDate = latestEntryDate;

            this.PublicationSets = publicationSets;

            this.GitHubPathHash = gitHubPathHash;
        }

        public string Identifier { get; }

        public string Name { get; }

        public decimal CurrentValue { get; }

        public decimal HistoricalValue { get; }

        public DateTime LatestEntryDate { get; }

        public PublicationSet[] PublicationSets { get; }

        public string GitHubPathHash { get; }
    }
}
