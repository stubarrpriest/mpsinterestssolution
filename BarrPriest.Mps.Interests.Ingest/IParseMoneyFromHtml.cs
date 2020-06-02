using System;
using System.Collections.Generic;
using System.Text;

namespace BarrPriest.Mps.Interests.Ingest
{
    public interface IParseMoneyFromHtml
    {
        List<MoneyParseResult> Parse(string htmlInput);
    }
}
