namespace BarrPriest.Mps.Interests.Ingest
{
    public class MoneyParseResult
    {
        public MoneyParseResult(decimal amount)
        {
            this.Amount = amount;
        }

        public decimal Amount { get; }
    }
}
