using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BarrPriest.Mps.Interests.Ingest.Cli
{
    public class RangeDisplay
    {
        private readonly Dictionary<string, Tuple<decimal, decimal>> range = new Dictionary<string, Tuple<decimal, decimal>>()
        {
            { "< £1,000", new Tuple<decimal, decimal>(0m, 999m) },
            { "> £1,000", new Tuple<decimal, decimal>(1000m, 9999m) },
            { "> £10,000", new Tuple<decimal, decimal>(10000m, 24999m) },
            { "> £25,000", new Tuple<decimal, decimal>(25000m, 49999m) },
            { "> £50,000", new Tuple<decimal, decimal>(50000m, 74999m) },
            { "> £75,000", new Tuple<decimal, decimal>(75000m, 99999m) },
            { "> £100,000", new Tuple<decimal, decimal>(100000m, 249999m) },
            { "> £250,000", new Tuple<decimal, decimal>(250000m, 499999m) },
            { "> £500,000", new Tuple<decimal, decimal>(500000m, 749999m) },
            { "> £750,000", new Tuple<decimal, decimal>(750000m, 999999m) },
            { "> £1,000,000", new Tuple<decimal, decimal>(1000000m, 1499999m) },
            { "> £1,500,000", new Tuple<decimal, decimal>(1500000m, 1999999m) },
            { "> £2,000,000", new Tuple<decimal, decimal>(2000000m, 2999999m) },
            { "> £3,000,000", new Tuple<decimal, decimal>(3000000m, 9999999m) },
            { "> £10,000,000", new Tuple<decimal, decimal>(10000000m, decimal.MaxValue) },
        };

        public string Bucket(decimal input)
        {
            if (input < 0)
            {
                return "0";
            }

            foreach (var key in this.range.Keys)
            {
                if (decimal.Round(input) >= this.range[key].Item1 && decimal.Round(input) <= this.range[key].Item2)
                {
                    return key;
                }
            }

            return this.range.Keys.Last();
        }
    }
}
