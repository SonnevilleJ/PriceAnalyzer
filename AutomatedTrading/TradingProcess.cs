using System;
using System.Collections.Generic;
using Sonneville.PriceTools.AutomatedTrading.Implementation;
using Sonneville.PriceTools.Data;
using Sonneville.PriceTools.Google;
using Sonneville.PriceTools.Implementation;

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
        private readonly IPriceDataProvider _priceDataProvider;

        public TradingProcess(IAnalysisEngine analysisEngine, IPriceSeriesFactory priceSeriesProvider, IBrokerage brokerage)
            : this(analysisEngine, priceSeriesProvider, brokerage,
                new PriceDataProvider(new GooglePriceHistoryQueryUrlBuilder(), new GooglePriceHistoryCsvFileFactory()))
        {
        }

        public TradingProcess(IAnalysisEngine analysisEngine, IPriceSeriesFactory priceSeriesProvider, IBrokerage brokerage, IPriceDataProvider priceDataProvider)
        {
            _analysisEngine = analysisEngine;
            _priceSeriesProvider = priceSeriesProvider;
            _brokerage = brokerage;
            _priceDataProvider = priceDataProvider;
        }

        public void Execute(IPortfolio portfolio, DateTime tail, IEnumerable<string> tickers)
        {
            var openOrders = _brokerage.GetOpenOrders();
            UpdatePortfolio(portfolio, tail, openOrders);
            
            foreach (var ticker in tickers)
            {
                var priceSeries = _priceSeriesProvider.ConstructPriceSeries(ticker);
                _priceDataProvider.UpdatePriceSeries(priceSeries, DateTime.Today.AddDays(-30), tail, Resolution.Days);

                var calculatedOrders = _analysisEngine.DetermineOrdersFor(portfolio, priceSeries, tail,
                tail.CurrentPeriodClose(Resolution.Days), openOrders);
                _brokerage.SubmitOrders(calculatedOrders);
            }
        }

        private void UpdatePortfolio(IPortfolio portfolio, DateTime tail, IList<Order> openOrders)
        {
            portfolio.OpenOrders = openOrders;

            var newTransactions = _brokerage.GetTransactions(portfolio.Tail, tail);
            foreach (var transaction in newTransactions)
            {
                portfolio.AddTransaction(transaction);
            }
        }
    }
}