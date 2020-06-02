using System;
using System.Collections.Generic;
using System.Text;

namespace BarrPriest.Mps.Interests.Ingest.Projections
{
    public class MpInterestValue
    {
        public MpInterestValue(string name, decimal amount, DateTime asOf)
        {
            this.Name = name;

            this.Amount = amount;

            this.AsOf = asOf;
        }

        public string Name { get; }

        public decimal Amount { get; }

        public DateTime AsOf { get; }
    }
}
