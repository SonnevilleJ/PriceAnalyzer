using System;
using System.Linq;
using Sonneville.PriceTools.AutomatedTrading.Implementation;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools.AutomatedTrading
{
    public class TradingProcess
    {
        private readonly IPortfolio _portfolio;
        private readonly IAnalysisEngine _analysisEngine;
        private readonly IPriceSeriesProvider _priceSeriesProvider;
        private readonly IBrokerage _brokerage;


        public TradingProcess(IPortfolio portfolio, IAnalysisEngine analysisEngine, IPriceSeriesProvider priceSeriesProvider, IBrokerage brokerage)
        {
            _portfolio = portfolio;
            _analysisEngine = analysisEngine;
            _priceSeriesProvider = priceSeriesProvider;
            _brokerage = brokerage;
        }

        public void Execute(DateTime dateTime)
        {
            _portfolio.GetAvailableCash(dateTime);

            foreach (var position in _portfolio.Positions)
            {
                var priceSeries = _priceSeriesProvider.GetPriceSeries(position.Ticker);
                var pendingTransactions = _brokerage.GetOpenOrders();
                var newTransactions = _brokerage.GetTransactions(position.Ticker, position.Tail, dateTime);
                foreach (var transaction in newTransactions)
                {
                    position.AddTransaction(transaction);   
                }
                
                var calculatedOrders = _analysisEngine.DetermineOrdersFor(_portfolio, priceSeries, dateTime,
                dateTime.CurrentPeriodClose(Resolution.Days), pendingTransactions);
                _brokerage.SubmitOrders(calculatedOrders);
            }
        }
    }
}