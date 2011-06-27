﻿using System;
using System.Reflection;
using Sonneville.PriceTools;

namespace Sonneville.PriceAnalyzer
{
    public class PriceOverThresholdWatcher : Watcher
    {
        public override PropertyInfo Property
        {
            get
            {
                return (typeof(PricePeriod)).GetProperty("High");
            }
        }

        protected override bool Evaluate(PricePeriod pricePeriod)
        {
            return (decimal) Property.GetValue(pricePeriod, null) >= Threshold;
        }
    }
}