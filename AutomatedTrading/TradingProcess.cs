using System;
using System.Collections.Generic;
using Sonneville.PriceTools.AutomatedTrading.Implementation;

namespace Sonneville.PriceTools.AutomatedTrading
{
    public interface ITradingProcess
    {
        void Execute(IPortfolio portfolio, DateTime dateTime, IEnumerable<string> tickers);
    }

    public class TradingProcess : ITradingProcess
    {
        private readonly IAnalysisEngine _analysisEngine;
        private readonly IPriceSeriesFactory _priceSeriesProvider;
        private readonly IBrokerage _brokerage;


        public TradingProcess(IAnalysisEngine analysisEngine, IPriceSeriesFactory priceSeriesProvider, IBrokerage brokerage)
        {
            _analysisEngine = analysisEngine;
            _priceSeriesProvider = priceSeriesProvider;
            _brokerage = brokerage;
        }

        public void Execute(IPortfolio portfolio, DateTime dateTime, IEnumerable<string> tickers)
        {
            foreach (var ticker in tickers)
            {
                var position = portfolio.GetPosition(ticker);
                var priceSeries = _priceSeriesProvider.ConstructPriceSeries(ticker);
                var pendingTransactions = _brokerage.GetOpenOrders();
                var newTransactions = _brokerage.GetTransactions(ticker, position.Tail, dateTime);
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