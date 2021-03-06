﻿using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Sonneville.PriceTools.AutomatedTrading;
using Sonneville.PriceTools.Data;
using Sonneville.PriceTools.SampleData;
using Sonneville.PriceTools.Yahoo;
using Sonneville.Utilities;

namespace Sonneville.PriceTools.Fidelity.Test
{
    [TestFixture]
    public class FidelityTransactionHistoryCsvFileTest
    {
        private IPortfolioFactory _portfolioFactory;
        private IPriceHistoryCsvFileFactory _priceHistoryCsvFileFactory;
        private ISecurityBasketCalculator _securityBasketCalculator;

        [SetUp]
        public void Setup()
        {
            _securityBasketCalculator = new SecurityBasketCalculator();
            _portfolioFactory = new PortfolioFactory(new TransactionFactory(), new CashAccountFactory(), _securityBasketCalculator, new PositionFactory(new PriceSeriesFactory(), _securityBasketCalculator), new PriceSeriesFactory());
            _priceHistoryCsvFileFactory = new YahooPriceHistoryCsvFileFactory();
        }

        [Test]
        public void ParsePortfolioAltrTest()
        {
            using (Stream csvStream = new ResourceStream(SamplePortfolios.FidelityTaxable.CsvString))
            {
                var target = new FidelityTransactionHistoryCsvFile(new TransactionFactory(), new HoldingFactory());
                target.Parse(csvStream);
                var portfolio = _portfolioFactory.ConstructPortfolio("FTEXX", target.Transactions);
                var settlementDate = new DateTime(2010, 11, 16);
                var provider = new PriceDataProvider(new WebClientWrapper(), new YahooPriceHistoryQueryUrlBuilder(), new YahooPriceHistoryCsvFileFactory());

                var altr = portfolio.Positions.First(p => p.Ticker == "ALTR");
                var investedValue = _securityBasketCalculator.CalculateMarketValue(altr, provider, settlementDate, _priceHistoryCsvFileFactory);
                Assert.AreEqual(0.00m, investedValue);
            }
        }

        [Test]
        public void ParsePortfolioNtapTest()
        {
            using (Stream csvStream = new ResourceStream(SamplePortfolios.FidelityTaxable.CsvString))
            {
                var target = new FidelityTransactionHistoryCsvFile(new TransactionFactory(), new HoldingFactory());
                target.Parse(csvStream);
                var portfolio = _portfolioFactory.ConstructPortfolio("FTEXX", target.Transactions);
                var settlementDate = new DateTime(2010, 11, 16);
                var provider = new PriceDataProvider(new WebClientWrapper(), new YahooPriceHistoryQueryUrlBuilder(), new YahooPriceHistoryCsvFileFactory());

                var ntap = portfolio.Positions.First(p => p.Ticker == "NTAP");
                var investedValue = _securityBasketCalculator.CalculateMarketValue(ntap, provider, settlementDate, _priceHistoryCsvFileFactory);
                Assert.AreEqual(0.00m, investedValue);
            }
        }

        [Test]
        public void ParsePortfolioNtctTest()
        {
            using (Stream csvStream = new ResourceStream(SamplePortfolios.FidelityTaxable.CsvString))
            {
                var target = new FidelityTransactionHistoryCsvFile(new TransactionFactory(), new HoldingFactory());
                target.Parse(csvStream);
                var portfolio = _portfolioFactory.ConstructPortfolio("FTEXX", target.Transactions);
                var settlementDate = new DateTime(2010, 11, 16);
                var provider = new PriceDataProvider(new WebClientWrapper(), new YahooPriceHistoryQueryUrlBuilder(), new YahooPriceHistoryCsvFileFactory());

                var ntct = portfolio.Positions.First(p => p.Ticker == "NTCT");
                var investedValue = _securityBasketCalculator.CalculateMarketValue(ntct, provider, settlementDate, _priceHistoryCsvFileFactory);
                Assert.AreEqual(0.00m, investedValue);
            }
        }

        [Test]
        public void ParsePortfolioPgTest()
        {
            using (Stream csvStream = new ResourceStream(SamplePortfolios.FidelityTaxable.CsvString))
            {
                var target = new FidelityTransactionHistoryCsvFile(new TransactionFactory(), new HoldingFactory());
                target.Parse(csvStream);
                var portfolio = _portfolioFactory.ConstructPortfolio("FTEXX", target.Transactions);
                var settlementDate = new DateTime(2010, 11, 16);
                var provider = new PriceDataProvider(new WebClientWrapper(), new YahooPriceHistoryQueryUrlBuilder(), new YahooPriceHistoryCsvFileFactory());

                var pg = portfolio.Positions.First(p => p.Ticker == "PG");
                var investedValue = _securityBasketCalculator.CalculateMarketValue(pg, provider, settlementDate, _priceHistoryCsvFileFactory);
                Assert.AreEqual(0.00m, investedValue);
            }
        }

        [Test]
        public void ParsePortfolioAvailableCashTest()
        {
            using (Stream csvStream = new ResourceStream(SamplePortfolios.FidelityTaxable.CsvString))
            {
                var target = new FidelityTransactionHistoryCsvFile(new TransactionFactory(), new HoldingFactory());
                target.Parse(csvStream);
                var portfolio = _portfolioFactory.ConstructPortfolio("FTEXX", target.Transactions);
                var settlementDate = new DateTime(2010, 11, 16);

                const decimal expectedAvailableCash = 2848.43m;
                var availableCash = portfolio.GetAvailableCash(settlementDate);
                Assert.AreEqual(expectedAvailableCash, availableCash);
            }
        }

        [Test]
        public void TickerTest()
        {
            var csvFile = new FidelityTransactionHistoryCsvFile(new TransactionFactory(), new HoldingFactory());
            csvFile.Parse(new ResourceStream(SamplePortfolios.FidelityTaxable.CsvString));
            var ticker = String.Empty;

            var target = _portfolioFactory.ConstructPortfolio(ticker, csvFile.Transactions);

            Assert.AreEqual(ticker, target.CashTicker);
        }

        [Test]
        public void PositionsTest()
        {
            var csvFile = new FidelityTransactionHistoryCsvFile(new TransactionFactory(), new HoldingFactory());
            csvFile.Parse(new ResourceStream(SamplePortfolios.FidelityTaxable.CsvString));
            var ticker = String.Empty;

            var target = _portfolioFactory.ConstructPortfolio(ticker, csvFile.Transactions);

            Assert.AreEqual(5, target.Positions.Count());
        }

        [Test]
        public void AvailableCashTest()
        {
            var csvFile = new FidelityTransactionHistoryCsvFile(new TransactionFactory(), new HoldingFactory());
            csvFile.Parse(new ResourceStream(SamplePortfolios.FidelityTaxable.CsvString));

            var target = _portfolioFactory.ConstructPortfolio("FTEXX", csvFile.Transactions);

            Assert.AreEqual(2848.43m, target.GetAvailableCash(new DateTime(2010, 11, 16)));
        }
    }
}
