namespace Sonneville.PriceTools.PriceAnalyzer
{
    public class ChartFactory
    {
        public IChart CreateNewChart()
        {
            return new CandleStickChart ();
        }
    }
}