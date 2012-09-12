using System.Collections.Generic;
using System.Linq;

namespace Sonneville.PriceTools.AutomatedTrading
{
    public class LowerThanYesterdayAnalyzer : PriceSeriesAnalyzer
    {
        protected override IEnumerable<AnalyzerEventArgs> GetTriggerPeriodsArgs()
        {
            var periods = PriceSeries.PricePeriods.OrderBy(p => p.Head).ToArray();
            var previousClose = TimePeriod[periods[0].Head];

            var args = new List<AnalyzerEventArgs>();
            for (var i = 1; i < periods.Length; i++)
            {
                var currentClose = TimePeriod[periods[i].Head];
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