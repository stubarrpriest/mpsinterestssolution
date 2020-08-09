using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BarrPriest.Mps.Interests.Ingest;
using BarrPriest.Mps.Interests.Ingest.Events;
using BarrPriest.Mps.Interests.Ingest.Interfaces;
using BarrPriest.Mps.Interests.Ingest.Interfaces.With;
using FakeItEasy;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Timers;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace BarrPriest.Mps.Interests.Tests.Ingest
{
    [TestFixture]
    public class IngestDataTests
    {
        [Test]
        public async Task WhenRemotePublicationSetIsMoreRecentThanStoredPublicationSet()
        {
            // Arrange
            var timerStub = new TimerInfo(new StubTimerSchedule(), new ScheduleStatus());

            var remote = A.Fake<IGetRemotePublicationSets>();

            var local = A.Fake<IGetLocalPublicationSets>();

            var bus = A.Fake<IBus>();

            A.CallTo(() => local.MostRecentPublicationSetAsync()).Returns(Task.FromResult("200601"));

            A.CallTo(() => remote.NextPublicationSetsAfterAsync("200601")).Returns(Task.FromResult(new string[] { "200701" }));

            var ingestData = new IngestData(bus, local, remote);

            var logger = A.Fake<ILogger>();

            // Act
            await ingestData.CheckForNewData(timerStub, logger);

            // Assert
            A.CallTo(() => bus.PublishAsync(A<NewPublicationSetDiscoveredEvent>.That.Matches(x => x.PublicationSetName == "200701"))).MustHaveHappened();
        }

        [Test]
        public async Task WhenNoPublicationSetIsMoreRecentThanStoredPublicationSet()
        {
            // Arrange
            var timerStub = new TimerInfo(new StubTimerSchedule(), new ScheduleStatus());

            var remote = A.Fake<IGetRemotePublicationSets>();

            var local = A.Fake<IGetLocalPublicationSets>();

            var bus = A.Fake<IBus>();

            A.CallTo(() => local.MostRecentPublicationSetAsync()).Returns(Task.FromResult("200601"));

            A.CallTo(() => remote.NextPublicationSetsAfterAsync("200601")).Returns(Task.FromResult(new string[] { }));

            var ingestData = new IngestData(bus, local, remote);

            var logger = A.Fake<ILogger>();

            // Act
            await ingestData.CheckForNewData(timerStub, logger);

            // Assert
            A.CallTo(() => bus.PublishAsync(A<IEvent>.Ignored)).MustNotHaveHappened();
        }

        private class StubTimerSchedule : TimerSchedule
        {
            public override DateTime GetNextOccurrence(DateTime now)
            {
                throw new NotImplementedException();
            }
        }
    }
}
