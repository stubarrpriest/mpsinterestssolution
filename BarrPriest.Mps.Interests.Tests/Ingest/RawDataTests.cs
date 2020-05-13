using System;
using System.Collections.Generic;
using System.Text;
using BarrPriest.Mps.Interests.Ingest.Interfaces.With;
using BarrPriest.Mps.Interests.Ingest.Interfaces.With.ParliamentWebsite;
using NUnit.Framework;

namespace BarrPriest.Mps.Interests.Tests.Ingest
{
    [TestFixture]
    public class RawDataTests
    {
        [Test]
        public void WhenConstructedWithUrl()
        {
            // Arrange
            var url = @"https://publications.parliament.uk/pa/cm/cmregmem/191105/some_mp.htm";

            // Act
            var rawData = new RawHtmlData(url, DateTimeOffset.MinValue, string.Empty);

            // Assert
            Assert.AreEqual("191105", rawData.PublicationSet);
        }

        [Test]
        public void WhenDerivingLikelyPublicationDate()
        {
            // Arrange
            var url = @"https://publications.parliament.uk/pa/cm/cmregmem/191105/some_mp.htm";

            // Act
            var rawData = new RawHtmlData(url, DateTimeOffset.MinValue, string.Empty);

            // Assert
            Assert.AreEqual(new DateTime(2019, 11, 5), rawData.LikelyPublicationDate);
        }
    }
}
