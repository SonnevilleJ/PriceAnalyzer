using System.Collections.Generic;
using System.Linq;

namespace Sonneville.PriceAnalyzer
{
    public class HigherThanYesterdayAnalyzer : Analyzer
    {
        protected override IEnumerable<AnalyzerEventArgs> GetTriggerPeriodsArgs()
        {
            var periods = PricePeriods.OrderBy(p => p.Head).ToArray();
            var previousClose = periods.ElementAt(0).Close;

            var args = new List<AnalyzerEventArgs>();
            for (int i = 1; i < periods.Length; i++)
            {
                var currentClose = periods[i].Close;
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