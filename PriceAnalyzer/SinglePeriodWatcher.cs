using System.Collections.Generic;
using System.Linq;
using Sonneville.PriceTools;

namespace Sonneville.PriceAnalyzer
{
    public abstract class SinglePeriodWatcher : Watcher
    {
        protected override IEnumerable<WatcherEventArgs> GetTriggerPeriodsArgs()
        {
            return PriceSeries.PricePeriods.Where(EvaluatePricePeriod).Select(period => new WatcherEventArgs {DateTime = period.Head});
        }

        protected abstract bool EvaluatePricePeriod(PricePeriod pricePeriod);
    }
}
