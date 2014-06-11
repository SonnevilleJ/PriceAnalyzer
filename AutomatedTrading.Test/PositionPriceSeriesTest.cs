﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools.AutomatedTrading.Implementation;
using Sonneville.PriceTools.Data.Csv;
using Sonneville.PriceTools.Google;
using Sonneville.PriceTools.SampleData;

namespace Sonneville.PriceTools.AutomatedTrading.Test
{
    [TestClass]
    public class PositionPriceSeriesTest
    {
        private PositionFactory _positionFactory;
        private TransactionFactory _transactionFactory;

        [TestInitialize]
        public void Initialize()
        {
            _positionFactory = new PositionFactory();
            _transactionFactory = new TransactionFactory();
        }

        [TestMethod]
        public void PriceSeriesConstructionTest()
        {
            var priceData = SamplePriceDatas.IBM_Daily;
            var position = CreateEncompassingPosition(priceData);

            var csvPriceDataProvider = new CsvPriceDataProvider(new GooglePriceHistoryQueryUrlBuilder(), new GooglePriceHistoryCsvFileFactory());
            var priceSeries = _positionFactory.ConstructPriceSeries(position, csvPriceDataProvider);

            foreach (var pricePeriod in priceData.PricePeriods)
            {
                var expected = pricePeriod.Close;
                var actual = priceSeries[pricePeriod.Tail];
                Assert.AreEqual(expected, actual);
            }
        }

        private Position CreateEncompassingPosition(SamplePriceData priceData)
        {
            var head = priceData.PriceSeries.Head;
            var tail = priceData.PriceSeries.Tail;
            var open = priceData.PriceSeries.Open;
            var close = priceData.PriceSeries.Close;

            var ticker = priceData.Ticker;
            var buy = _transactionFactory.ConstructBuy(ticker, head, 1, open);
            var sell = _transactionFactory.ConstructSell(ticker, tail, 1, close);
            return _positionFactory.ConstructPosition(ticker, buy, sell);
        }
    }
}