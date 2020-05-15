using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BarrPriest.Mps.Interests.Ingest
{
    public class MoneyParser
    {
        private const string Pattern = "£[+-]?[0-9]{1,3}(?:,?[0-9]{3})*(?:\\.[0-9]{2})?";

        public List<MoneyParseResult> Parse(string htmlInput)
        {
            var result = new List<MoneyParseResult>();

            var regEx = new Regex(Pattern);

            var matches = regEx.Matches(htmlInput);

            foreach (Match match in matches)
            {
                result.Add(new MoneyParseResult(decimal.Parse(match.Value.Replace("£", string.Empty))));
            }

            return result;
        }
    }
}
