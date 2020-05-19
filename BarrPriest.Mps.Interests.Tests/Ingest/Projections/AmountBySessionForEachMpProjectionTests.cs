using System;
using System.Collections.Generic;
using System.Text;
using BarrPriest.Mps.Interests.Ingest.Events;
using BarrPriest.Mps.Interests.Ingest.Projections;
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
            var projection = new AmountByPublicationSetForEachMpProjection();

            var htmlEvent = new RawHtmlDataAcquiredEvent("https://somewebsite/160111/some_mp.htm", DateTimeOffset.MinValue, "foo");

            // Act
            projection.Handle(htmlEvent);

            // Assert
            Assert.AreEqual(1, projection.Result().Count);
        }
    }
}
