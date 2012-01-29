﻿using System.Collections.Generic;
using System.Linq;

namespace Sonneville.PriceTools.AutomatedTrading
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
