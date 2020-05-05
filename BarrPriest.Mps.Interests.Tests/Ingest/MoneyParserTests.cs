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
            var sourceHtml =@"<p xmlns=""http://www.w3.org/1999/xhtml"" class=""indent"">17 October 2019, payment of £16,000 from Fiera Capital (UK) Ltd, 39 St James's Street, London SW1A 1JD, via Chartwell Partners Ltd, 26 High Street, Marlborough SN8 1LZ, for a speaking engagement. Hours: 4 hrs. Fee paid direct to charity. I consulted ACoBA about this one-off fee. (Registered 21 October 2019)</p>";
            
            var result = new MoneyParser().Parse(sourceHtml);

            Assert.AreEqual(16000m, result[0].Amount);
        }

        [Test]
        public void CanGetMultipleMoneyFromHtml()
        {
            var sourceHtml = @"<p xmlns=""http://www.w3.org/1999/xhtml"" class=""indent"">Name of donor: (1) Conservative Drug Policy Reform Group (CDPRG);  (2) Zimmer &amp; Co<br/>Address of donor: (1) Suite 15.17, Citibase, 15th Floor Millbank Tower, 21-24 Millbank, London SW1P 4Q;<br/>(2) 1139 Morgan Road, Suite 11, Montego Bay, Jamaica<br/>Estimate of the probable value (or amount of any donation): (1) Accommodation and subsistence, £521.76;<br/>(2) Flights, £3,262.19<br/>Destination of visit: Kingston, Jamaica<br/>Dates of visit: 23-26 September 2019<br/>Purpose of visit: To meet Jamaican Ministers and policy makers. I was also due to attend and speak at a conference on the regulatory and legislative framework around policy towards cannabis in Jamaica, but the visit was cut short.<br/>(Registered 24 October 2019)</p>";

            var result = new MoneyParser().Parse(sourceHtml);

            Assert.AreEqual(521.76m, result[0].Amount);

            Assert.AreEqual(3262.19m, result[1].Amount);
        }
    }
}
