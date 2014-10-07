using System;
using System.Collections.Generic;
using Sonneville.PriceTools.AutomatedTrading.Implementation;

namespace Sonneville.PriceTools.AutomatedTrading
{
    public interface ITradingProcess
    {
        void Execute(IPortfolio portfolio, DateTime tail, IEnumerable<string> tickers);
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

        public void Execute(IPortfolio portfolio, DateTime tail, IEnumerable<string> tickers)
        {
            UpdatePortfolio(portfolio, tail);

            foreach (var ticker in tickers)
            {
                var priceSeries = _priceSeriesProvider.ConstructPriceSeries(ticker);

                var pendingTransactions = _brokerage.GetOpenOrders();
                var calculatedOrders = _analysisEngine.DetermineOrdersFor(portfolio, priceSeries, tail,
                tail.CurrentPeriodClose(Resolution.Days), pendingTransactions);
                _brokerage.SubmitOrders(calculatedOrders);
            }
        }

        private void UpdatePortfolio(IPortfolio portfolio, DateTime tail)
        {
            var head = portfolio.Tail;
            var newTransactions = _brokerage.GetTransactions(head, tail);
            foreach (var transaction in newTransactions)
            {
                portfolio.AddTransaction(transaction);
            }
        }
    }
}