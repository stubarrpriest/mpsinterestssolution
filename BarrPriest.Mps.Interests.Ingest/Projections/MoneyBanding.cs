using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BarrPriest.Mps.Interests.Ingest.Projections
{
    public class MoneyBanding
    {
        private readonly Dictionary<decimal, Tuple<decimal, decimal>> range = new Dictionary<decimal, Tuple<decimal, decimal>>()
        {
            { 500, new Tuple<decimal, decimal>(1m, 999m) },
            { 1000, new Tuple<decimal, decimal>(1000m, 9999m) },
            { 10000, new Tuple<decimal, decimal>(10000m, 24999m) },
            { 25000, new Tuple<decimal, decimal>(25000m, 49999m) },
            { 50000, new Tuple<decimal, decimal>(50000m, 74999m) },
            { 75000, new Tuple<decimal, decimal>(75000m, 99999m) },
            { 100000, new Tuple<decimal, decimal>(100000m, 249999m) },
            { 250000, new Tuple<decimal, decimal>(250000m, 499999m) },
            { 500000, new Tuple<decimal, decimal>(500000m, 749999m) },
            { 750000, new Tuple<decimal, decimal>(750000m, 999999m) },
            { 1000000, new Tuple<decimal, decimal>(1000000m, 1499999m) },
            { 1500000, new Tuple<decimal, decimal>(1500000m, 1999999m) },
            { 2000000, new Tuple<decimal, decimal>(2000000m, 2999999m) },
            { 3000000, new Tuple<decimal, decimal>(3000000m, 9999999m) },
            { 10000000, new Tuple<decimal, decimal>(10000000m, decimal.MaxValue) },
        };

        public decimal Bucket(decimal input)
        {
            var isNegative = false;

            if (input == 0)
            {
                return 0;
            }

            if (input < 0)
            {
                input = input * -1;

                isNegative = true;
            }

            foreach (var key in this.range.Keys)
            {
                if (decimal.Round(input) >= this.range[key].Item1 && decimal.Round(input) <= this.range[key].Item2)
                {
                    if (isNegative)
                    {
                        return key * -1;
                    }
                    else
                    {
                        return key;
                    }
                }
            }

            if (isNegative)
            {
                return this.range.Keys.Last() * -1;
            }
            else
            {
                return this.range.Keys.Last();
            }
        }
    }
}
