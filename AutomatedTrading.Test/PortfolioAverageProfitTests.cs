using System;
using NUnit.Framework;

namespace Sonneville.PriceTools.AutomatedTrading.Test
{
    [TestFixture]
    public class PortfolioAverageProfitTests
    {
        private IPortfolioFactory _portfolioFactory;
        private ITransactionFactory _transactionFactory;
        private ISecurityBasketCalculator _securityBasketCalculator;

        [SetUp]
        public void Setup()
        {
            _securityBasketCalculator = new SecurityBasketCalculator();
            _portfolioFactory = new PortfolioFactory(new TransactionFactory(), new CashAccountFactory(), _securityBasketCalculator, new PositionFactory(new PriceSeriesFactory(), _securityBasketCalculator), new PriceSeriesFactory());
            _transactionFactory = new TransactionFactory();
        }

        [Test]
        public void CalculateAverageProfitOneGainOneLoss()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;

            var buyDate = new DateTime(2011, 1, 10);
            var sellDate = buyDate.AddDays(1);
            const string de = "DE";
            const string msft = "MSFT";
            const decimal dePriceBought = 100.00m;
            const decimal dePriceSold = 110.00m;
            const decimal msftPriceBought = 50.00m;
            const decimal msftPriceSold = 40.00m;
            const decimal sharesBought = 10;
            const decimal commission = 7.95m;
            const decimal sharesSold = sharesBought - 2;

            var deBuy = _transactionFactory.ConstructBuy(de, buyDate, sharesBought, dePriceBought, commission);
            var deSell = _transactionFactory.ConstructSell(de, sellDate, sharesSold, dePriceSold, commission);
            var msftBuy = _transactionFactory.ConstructBuy(msft, buyDate, sharesBought, msftPriceBought, commission);
            var msftSell = _transactionFactory.ConstructSell(msft, sellDate, sharesSold, msftPriceSold, commission);

            var target = _portfolioFactory.ConstructPortfolio(dateTime, deposit, deBuy, deSell, msftBuy, msftSell);

            var deProfit = _securityBasketCalculator.CalculateAverageProfit(target.GetPosition(de), sellDate);
            var msftProfit = _securityBasketCalculator.CalculateAverageProfit(target.GetPosition(msft), sellDate);

            var expected = deProfit + msftProfit;
            var actual = _securityBasketCalculator.CalculateAverageProfit(target, sellDate);
            Assert.AreEqual(expected, actual);
        }
    }
}