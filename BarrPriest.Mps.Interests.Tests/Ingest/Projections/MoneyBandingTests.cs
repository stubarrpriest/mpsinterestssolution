using System;
using System.Collections.Generic;
using System.Text;
using BarrPriest.Mps.Interests.Ingest.Projections;
using NUnit.Framework;

namespace BarrPriest.Mps.Interests.Tests.Ingest.Projections
{
    [TestFixture]

    public class MoneyBandingTests
    {
        [TestCase(-1000, ExpectedResult = -1000)]
        [TestCase(-999, ExpectedResult = -500)]
        [TestCase(-1, ExpectedResult = -500)]
        [TestCase(0, ExpectedResult = 0)]
        [TestCase(1, ExpectedResult = 500)]
        [TestCase(999, ExpectedResult = 500)]
        [TestCase(1000, ExpectedResult = 1000)]
        [TestCase(9999, ExpectedResult = 1000)]
        [TestCase(10000, ExpectedResult = 10000)]
        [TestCase(24999, ExpectedResult = 10000)]
        [TestCase(25000, ExpectedResult = 25000)]
        [TestCase(49999, ExpectedResult = 25000)]
        [TestCase(50000, ExpectedResult = 50000)]
        [TestCase(74999, ExpectedResult = 50000)]
        [TestCase(75000, ExpectedResult = 75000)]
        [TestCase(99999, ExpectedResult = 75000)]
        [TestCase(100000, ExpectedResult = 100000)]
        [TestCase(249999, ExpectedResult = 100000)]
        [TestCase(250000, ExpectedResult = 250000)]
        [TestCase(499999, ExpectedResult = 250000)]
        [TestCase(500000, ExpectedResult = 500000)]
        [TestCase(749999, ExpectedResult = 500000)]
        [TestCase(750000, ExpectedResult = 750000)]
        [TestCase(999999, ExpectedResult = 750000)]
        [TestCase(1000000, ExpectedResult = 1000000)]
        [TestCase(1499999, ExpectedResult = 1000000)]
        [TestCase(1500000, ExpectedResult = 1500000)]
        [TestCase(1999999, ExpectedResult = 1500000)]
        [TestCase(2000000, ExpectedResult = 2000000)]
        [TestCase(2999999, ExpectedResult = 2000000)]
        [TestCase(3000000, ExpectedResult = 3000000)]
        [TestCase(9999999, ExpectedResult = 3000000)]
        [TestCase(10000000, ExpectedResult = 10000000)]
        public decimal TestMoneyBanding(decimal input)
        {
            return new MoneyBanding().Bucket(input);
        }
    }
}
