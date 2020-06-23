using BarrPriest.Mps.Interests.Ingest.Projections;
using NUnit.Framework;

namespace BarrPriest.Mps.Interests.Tests.Ingest.Projections
{
    public class GitHubPathHashTests
    {
        [Test]
        public void WhenGivenMpIdentifierProducesGitHubHash()
        {
            // Arrange
            var hashProducer = new GitHubPathHash(".html");

            // Act
            var result = hashProducer.From("johnson_boris");

            // Assert
            Assert.AreEqual("e04417f8e8366a266127b18afb76f71a", result);
        }
    }
}
