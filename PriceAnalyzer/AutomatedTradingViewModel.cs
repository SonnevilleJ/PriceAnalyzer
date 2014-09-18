using System;
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

        public void Run(IPortfolio portfolio, DateTime startDate, DateTime endDate)
        {
            for (DateTime i = startDate; i < endDate; i = i.NextTradingPeriodClose(Resolution.Days))
            {
                if (_cancelEvent)
                {
                    break;
                }

                _tradingProcess.Execute(portfolio, i);
            }
        }

        public void Cancel()
        {
            _cancelEvent = true;
        }
    }
}