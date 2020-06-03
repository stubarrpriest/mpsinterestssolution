using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BarrPriest.Mps.Interests.Ingest.Projections;
using NUnit.Framework;

namespace BarrPriest.Mps.Interests.Tests.Ingest.Projections
{
    [TestFixture]
    public class AmountByPublicationSetForEachMpProjectionExplorerTests
    {
        [Test]
        public void CanSumMoneyWhichHasLeftTheRegister()
        {
            // Arrange
            var data = new Dictionary<string, Dictionary<string, PublicationSetTotal>>()
            {
                {
                    "smith_dave", new Dictionary<string, PublicationSetTotal>()
                    {
                        { "150402", new PublicationSetTotal("150402", 2m) },
                        { "150502", new PublicationSetTotal("150502", 2m) },
                        { "150602", new PublicationSetTotal("150602", 1m) },
                        { "150702", new PublicationSetTotal("150702", 5m) },
                        { "150802", new PublicationSetTotal("150802", 1m) },
                    }
                },
            };

            var query = new AmountByPublicationSetForEachMpProjectionExplorer(data);

            // Act
            var result = query.TopHistoricalEarners(1);

            // Assert
            Assert.AreEqual(5m, result.First().Amount);
        }

        [Test]
        public void CanSumMoneyWhichHasLeftTheRegisterWhenDataIsNotSortedByPublicationSet()
        {
            // Arrange
            var data = new Dictionary<string, Dictionary<string, PublicationSetTotal>>()
            {
                {
                    "smith_dave", new Dictionary<string, PublicationSetTotal>()
                    {
                        { "150702", new PublicationSetTotal("150702", 5m) },
                        { "150402", new PublicationSetTotal("150402", 2m) },
                        { "150502", new PublicationSetTotal("150502", 2m) },
                        { "150602", new PublicationSetTotal("150602", 1m) },
                        { "150802", new PublicationSetTotal("150802", 1m) },
                    }
                },
            };

            var query = new AmountByPublicationSetForEachMpProjectionExplorer(data);

            // Act
            var result = query.TopHistoricalEarners(1);

            // Assert
            Assert.AreEqual(5m, result.First().Amount);
        }
    }
}
