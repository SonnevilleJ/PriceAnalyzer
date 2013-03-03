using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sonneville.PriceTools.Yahoo;

namespace Sonneville.PriceTools.AutomatedTrading
{
    /// <summary>
    /// A trading engine which monitors price signals and issues orders to buy and sell a security.
    /// </summary>
    public class ContinuousTradingEngine : ISignalProcessor
    {
        #region Private Members

        private readonly object _syncroot = new object();
        private bool _isRunning;
        private readonly IList<string> _tradableTickers = new List<string> {"DE", "MSFT", "IBM", "GOOG", "AAPL"};
        private readonly IList<ContinuousAnalyzer> _analyzers = new List<ContinuousAnalyzer>(); 

        #endregion

        #region Client Control

        public void Start()
        {
            lock(_syncroot)
            {
                if (_isRunning) throw new InvalidOperationException("Cannot start execution of the ContinuousTradingEngine because it has already been started.");

                _isRunning = true;
                foreach (var analyzer in _tradableTickers.Select(PriceSeriesFactory.ConstructPriceSeries)
                    .Select(priceSeries => new SimpleMovingAverageCrossoverAnalyzer(priceSeries, this, new YahooPriceDataProvider(), 20)))
                {
                    _analyzers.Add(analyzer);
                    analyzer.Start();
                }
                
            }
        }

        public void Stop()
        {
            lock(_syncroot)
            {
                while (_analyzers.Any())
                {
                    var analyzer = _analyzers.First();
                    _analyzers.RemoveAt(0);
                    analyzer.Stop();
                }
                _isRunning = false;
            }
        }

        #endregion

        public void Signal(IPriceSeries priceSeries, double direction, double magnitude)
        {
            throw new NotImplementedException();
        }
    }
}
