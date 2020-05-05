using System;
using System.Collections.Generic;
using System.Text;

namespace BarrPriest.Mps.Interests.Ingest
{
    public class MoneyParseResult
    {
        public decimal Amount { get; }

        public MoneyParseResult(decimal amount)
        {
            Amount = amount;
        }
    }
}
