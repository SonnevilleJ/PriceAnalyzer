using System;
using System.Collections.Generic;
using Sonneville.PriceTools.AutomatedTrading;
using Sonneville.PriceTools.AutomatedTrading.Implementation;

namespace Sonneville.PriceTools.PriceAnalyzer
{
    public class AutomatedTradingViewModel
    {

        private readonly ITradingProcess _tradingProcess;
        private bool _cancelEvent;

        public AutomatedTradingViewModel(ITradingProcess tradingProcess)
        {
            _tradingProcess = tradingProcess;
            
        }

        public void Run(IPortfolio portfolio, DateTime startDate, DateTime endDate, IEnumerable<string> tickers)
        {
            for (DateTime i = startDate; i < endDate; i = i.NextTradingPeriodClose(Resolution.Days))
            {
                if (_cancelEvent)
                {
                    break;
                }

                _tradingProcess.Execute(portfolio, i, tickers);
            }
        }

        public void Cancel()
        {
            _cancelEvent = true;
        }
    }
}