namespace Sonneville.PriceTools.PriceAnalyzer
{
    public class TransactionSummary
    {
        public string Ticker { get; set; }
        public double BoughtPrice { get; set; }
        public double SoldPrice { get; set; }
        public double CurrentPrice { get; set; }
        public double Volume { get; set; }
        public double NetChange { get; set; }
    }
}