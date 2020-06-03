using System;
using System.Collections.Generic;
using System.Text;
using BarrPriest.Mps.Interests.Ingest.Cli;
using NUnit.Framework;

namespace BarrPriest.Mps.Interests.Tests
{
    [TestFixture]
    public class RangeDisplayTests
    {
        [TestCase(-1, ExpectedResult = "0")]
        [TestCase(0, ExpectedResult = "< £1,000")]
        [TestCase(999, ExpectedResult = "< £1,000")]
        [TestCase(1000, ExpectedResult = "> £1,000")]
        [TestCase(9999, ExpectedResult = "> £1,000")]
        [TestCase(10000, ExpectedResult = "> £10,000")]
        [TestCase(24999, ExpectedResult = "> £10,000")]
        [TestCase(25000, ExpectedResult = "> £25,000")]
        [TestCase(49999, ExpectedResult = "> £25,000")]
        [TestCase(50000, ExpectedResult = "> £50,000")]
        [TestCase(74999, ExpectedResult = "> £50,000")]
        [TestCase(75000, ExpectedResult = "> £75,000")]
        [TestCase(99999, ExpectedResult = "> £75,000")]
        [TestCase(100000, ExpectedResult = "> £100,000")]
        [TestCase(249999, ExpectedResult = "> £100,000")]
        [TestCase(250000, ExpectedResult = "> £250,000")]
        [TestCase(499999, ExpectedResult = "> £250,000")]
        [TestCase(500000, ExpectedResult = "> £500,000")]
        [TestCase(749999, ExpectedResult = "> £500,000")]
        [TestCase(750000, ExpectedResult = "> £750,000")]
        [TestCase(999999, ExpectedResult = "> £750,000")]
        [TestCase(1000000, ExpectedResult = "> £1,000,000")]
        [TestCase(1499999, ExpectedResult = "> £1,000,000")]
        [TestCase(1500000, ExpectedResult = "> £1,500,000")]
        [TestCase(1999999, ExpectedResult = "> £1,500,000")]
        [TestCase(2000000, ExpectedResult = "> £2,000,000")]
        [TestCase(2999999, ExpectedResult = "> £2,000,000")]
        [TestCase(3000000, ExpectedResult = "> £3,000,000")]
        [TestCase(9999999, ExpectedResult = "> £3,000,000")]
        [TestCase(10000000, ExpectedResult = "> £10,000,000")]
        public string TestRanges(decimal input)
        {
            return new RangeDisplay().Bucket(input);
        }
    }
}
