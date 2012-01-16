using System.Collections.Generic;
using System.Linq;

namespace Sonneville.PriceTools.SecurityAnalysis
{
    public abstract class SinglePeriodAnalyzer : PriceSeriesAnalyzer
    {
        protected override IEnumerable<AnalyzerEventArgs> GetTriggerPeriodsArgs()
        {
            return PriceSeries.PricePeriods.Where(EvaluatePricePeriod).Select(period => new AnalyzerEventArgs {DateTime = period.Head});
        }

        protected abstract bool EvaluatePricePeriod(IPricePeriod pricePeriod);
    }
}
