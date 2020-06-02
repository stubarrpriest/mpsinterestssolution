using System;
using System.Collections.Generic;
using System.Text;
using BarrPriest.Mps.Interests.Ingest;
using BarrPriest.Mps.Interests.Ingest.Events;
using BarrPriest.Mps.Interests.Ingest.Projections;
using FakeItEasy;
using NUnit.Framework;

namespace BarrPriest.Mps.Interests.Tests.Ingest.Projections
{
    [TestFixture]
    public class AmountBySessionForEachMpProjectionTests
    {
        [Test]
        public void HandlesRawHtmlAcquiredEvent()
        {
            // Arrange
            var projection = new AmountByPublicationSetForEachMpProjection(A.Fake<IParseMoneyFromHtml>());

            var htmlEvent = new RawHtmlDataAcquiredEvent("https://somewebsite/160111/some_mp.htm", DateTimeOffset.MinValue, "foo");

            // Act
            projection.Handle(htmlEvent);

            // Assert
            Assert.AreEqual(1, projection.Result().Count);
        }

        [Test]
        public void SumsResultOfMoneyParsing()
        {
            // Arrange
            var moneyParser = A.Fake<IParseMoneyFromHtml>();

            A.CallTo(() => moneyParser.Parse(A<string>.Ignored)).Returns(new List<MoneyParseResult>()
            {
                new MoneyParseResult(10m),
                new MoneyParseResult(20m),
            });

            var projection = new AmountByPublicationSetForEachMpProjection(moneyParser);

            var htmlEvent = new RawHtmlDataAcquiredEvent("https://somewebsite/160111/john_smith.htm", DateTimeOffset.MinValue, "foo");

            // Act
            projection.Handle(htmlEvent);

            // Assert
            Assert.AreEqual(30, projection.Result()["john_smith"]["160111"].Amount);
        }
    }
}
