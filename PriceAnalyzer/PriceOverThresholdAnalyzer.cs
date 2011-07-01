﻿using Sonneville.PriceTools;

namespace Sonneville.PriceAnalyzer
{
    public class PriceOverThresholdAnalyzer : PriceThresholdAnalyzer
    {
        protected override bool EvaluatePricePeriod(PricePeriod pricePeriod)
        {
            return pricePeriod.High >= Threshold;
        }
    }
}