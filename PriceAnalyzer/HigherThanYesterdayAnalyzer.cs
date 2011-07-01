using System.Collections.Generic;
using System.Linq;

namespace Sonneville.PriceAnalyzer
{
    public class HigherThanYesterdayAnalyzer : PriceSeriesAnalyzer
    {
        public HigherThanYesterdayAnalyzer()
        {
            PriceSeriesProperty = OHLC.Close;
        }

        protected override IEnumerable<AnalyzerEventArgs> GetTriggerPeriodsArgs()
        {
            var periods = PriceSeries.PricePeriods.OrderBy(p => p.Head).ToArray();
            var previousClose = GetValue(periods[0]);

            var args = new List<AnalyzerEventArgs>();
            for (int i = 1; i < periods.Length; i++)
            {
                var currentClose = GetValue(periods[i]);
                if(currentClose >= previousClose)
                {
                    var eventArgs = new AnalyzerEventArgs {DateTime = periods[i].Head};
                    args.Add(eventArgs);
                }
                previousClose = currentClose;
            }
            return args;
        }
    }
}