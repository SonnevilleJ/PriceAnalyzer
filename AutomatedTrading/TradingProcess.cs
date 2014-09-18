using System;
using Sonneville.PriceTools.AutomatedTrading.Implementation;

namespace Sonneville.PriceTools.AutomatedTrading
{
    public interface ITradingProcess
    {
        void Execute(IPortfolio portfolio, DateTime dateTime);
    }

    public class TradingProcess : ITradingProcess
    {
        private readonly IAnalysisEngine _analysisEngine;
        private readonly IPriceSeriesProvider _priceSeriesProvider;
        private readonly IBrokerage _brokerage;


        public TradingProcess(IAnalysisEngine analysisEngine, IPriceSeriesProvider priceSeriesProvider, IBrokerage brokerage)
        {
            _analysisEngine = analysisEngine;
            _priceSeriesProvider = priceSeriesProvider;
            _brokerage = brokerage;
        }

        public void Execute(IPortfolio portfolio, DateTime dateTime)
        {
            foreach (var position in portfolio.Positions)
            {
                var priceSeries = _priceSeriesProvider.GetPriceSeries(position.Ticker);
                var pendingTransactions = _brokerage.GetOpenOrders();
                var newTransactions = _brokerage.GetTransactions(position.Ticker, position.Tail, dateTime);
                foreach (var transaction in newTransactions)
                {
                    position.AddTransaction(transaction);   
                }
                
                var calculatedOrders = _analysisEngine.DetermineOrdersFor(portfolio, priceSeries, dateTime,
                dateTime.CurrentPeriodClose(Resolution.Days), pendingTransactions);
                _brokerage.SubmitOrders(calculatedOrders);
            }
        }
    }
}