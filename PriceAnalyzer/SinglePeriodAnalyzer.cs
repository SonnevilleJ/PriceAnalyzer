using System.Collections.Generic;
using System.Linq;
using Sonneville.PriceTools;

namespace Sonneville.PriceAnalyzer
{
    public abstract class SinglePeriodAnalyzer : PriceSeriesAnalyzer
    {
        protected override IEnumerable<AnalyzerEventArgs> GetTriggerPeriodsArgs()
        {
            return PriceSeries.PricePeriods.Where(EvaluatePricePeriod).Select(period => new AnalyzerEventArgs {DateTime = period.Head});
        }

        protected abstract bool EvaluatePricePeriod(PricePeriod pricePeriod);
    }
}
