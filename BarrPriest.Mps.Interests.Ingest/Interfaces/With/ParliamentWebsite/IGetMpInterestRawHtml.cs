using System;
using System.Collections.Generic;
using System.Text;

namespace BarrPriest.Mps.Interests.Ingest.Interfaces.With.ParliamentWebsite
{
    public interface IGetMpInterestRawHtml
    {
        RawHtmlData MpDataFrom(string url);
    }
}
