using System;
using System.Collections.Generic;
using System.Text;

namespace BarrPriest.Mps.Interests.Ingest.Projections
{
    public class PublicationSet
    {
        public PublicationSet(
            string publicationSetName,
            decimal amount,
            DateTime publicationDate,
            decimal incomeBand,
            decimal change,
            decimal changeBand)
        {
            this.PublicationSetName = publicationSetName;

            this.Amount = amount;

            this.PublicationDate = publicationDate;

            this.IncomeBand = incomeBand;

            this.Change = change;

            this.ChangeBand = changeBand;
        }

        public string PublicationSetName { get; private set; }

        public decimal Amount { get; private set; }

        public DateTime PublicationDate { get; private set; }

        public decimal IncomeBand { get; private set; }

        public decimal Change { get; private set; }

        public decimal ChangeBand { get; private set; }
    }
}
