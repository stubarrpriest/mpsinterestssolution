using System;
using System.Collections.Generic;
using System.Text;

namespace BarrPriest.Mps.Interests.Ingest.Projections
{
    public class PublicationSetTotal
    {
        public PublicationSetTotal(string publicationSet, decimal amount)
        {
            this.PublicationSet = publicationSet;

            this.Amount = amount;
        }

        public string PublicationSet { get; }

        public decimal Amount { get; }
    }
}
