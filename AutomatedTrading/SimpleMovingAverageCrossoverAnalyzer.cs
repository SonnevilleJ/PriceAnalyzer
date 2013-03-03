using System;
using Sonneville.PriceTools.Data;
using Sonneville.PriceTools.TechnicalAnalysis;

namespace Sonneville.PriceTools.AutomatedTrading
{
    public class SimpleMovingAverageCrossoverAnalyzer : ContinuousAnalyzer
    {
        private readonly SimpleMovingAverage _simpleMovingAverage;

        public SimpleMovingAverageCrossoverAnalyzer(IPriceSeries priceSeries, ISignalProcessor signalProcessor, IPriceDataProvider priceDataProvider, int indicatorRange)
            : base(priceSeries, signalProcessor, priceDataProvider)
        {
            // ensure sufficient starting price data
            PriceDataProvider.UpdatePriceSeries(PriceSeries, DateTime.Now.SeekTradingPeriods(indicatorRange, priceDataProvider.BestResolution), DateTime.Now);

            _simpleMovingAverage = new SimpleMovingAverage(priceSeries, indicatorRange);
        }

        protected override void AnalyzePeriodo()
        {
            var currentDateTime = DateTime.Now;
            var previousDateTime = currentDateTime.PreviousPeriodClose(PriceSeries.Resolution);

            var previousBelow = PriceSeries[previousDateTime] - _simpleMovingAverage[previousDateTime] < 0;
            var currentBelow = PriceSeries[currentDateTime] - _simpleMovingAverage[currentDateTime] < 0;

            if(previousBelow && !currentBelow) SignalProcessor.Signal(PriceSeries, 1, 1);
            else if (!previousBelow && currentBelow) SignalProcessor.Signal(PriceSeries, -1, 1);
        }
    }
}