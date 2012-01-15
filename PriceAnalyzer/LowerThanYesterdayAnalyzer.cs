using System.Collections.Generic;
using System.Linq;

namespace Sonneville.PriceTools.Analysis
{
    public class LowerThanYesterdayAnalyzer : PriceSeriesAnalyzer
    {
        protected override IEnumerable<AnalyzerEventArgs> GetTriggerPeriodsArgs()
        {
            var periods = PriceSeries.PricePeriods.OrderBy(p => p.Head).ToArray();
            var previousClose = TimeSeries[periods[0].Head];

            var args = new List<AnalyzerEventArgs>();
            for (int i = 1; i < periods.Length; i++)
            {
                var currentClose = TimeSeries[periods[i].Head];
                if (currentClose < previousClose)
                {
                    args.Add(CreateEventArgs(periods[i].Head));
                }
                previousClose = currentClose;
            }
            return args;
        }
    }
}