using System;
using System.Collections.Generic;
using System.Text;
using BarrPriest.Mps.Interests.Ingest;
using NUnit.Framework;

namespace BarrPriest.Mps.Interests.Tests.Ingest
{
    [TestFixture]
    public class MoneyParserTests
    {
        [Test]
        public void CanGetMoneyFromHtml()
        {
            var parser = new MoneyParser();

            Assert.AreEqual(string.Empty, parser.Result());
        }
    }
}
