﻿using Sonneville.PriceTools;

namespace Sonneville.PriceAnalyzer
{
    public class PriceOverThresholdAnalyzer : PriceThresholdAnalyzer
    {
        protected override bool EvaluatePricePeriod(IPricePeriod pricePeriod)
        {
            return pricePeriod.High >= Threshold;
        }
    }
}
