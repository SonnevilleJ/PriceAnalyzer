using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sonneville.PriceTools.Test
{
    [TestClass]
    public class PortfolioCalculationTests
    {
        [TestMethod]
        public void PortfolioCalculateGrossReturnTest()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;
            var target = PortfolioFactory.ConstructPortfolio(dateTime, deposit);

            var buyDate = new DateTime(2011, 1, 10);
            var sellDate = buyDate.AddDays(1);
            const string de = "DE";
            const string msft = "MSFT";
            const decimal dePriceBought = 100.00m;
            const decimal dePriceSold = 110.00m;
            const decimal msftPriceBought = 50.00m;
            const decimal msftPriceSold = 60.00m;
            const double sharesBought = 10;
            const decimal commission = 7.95m;
            const double sharesSold = sharesBought - 2;
            var deBuy = TransactionFactory.ConstructBuy(buyDate, de, dePriceBought, sharesBought, commission);
            var deSell = TransactionFactory.ConstructSell(sellDate, de, dePriceSold, sharesSold, commission);
            var msftBuy = TransactionFactory.ConstructBuy(buyDate, msft, msftPriceBought, sharesBought, commission);
            var msftSell = TransactionFactory.ConstructSell(sellDate, msft, msftPriceSold, sharesSold, commission);

            target.AddTransaction(deBuy);
            target.AddTransaction(deSell);
            target.AddTransaction(msftBuy);
            target.AddTransaction(msftSell);

            var grossProfit = target.CalculateGrossProfit(sellDate);

            const decimal deReturn = (dePriceSold / dePriceBought) - 1;
            var deProfit = target.GetPosition(de).CalculateGrossProfit(sellDate);
            var deWeight = deProfit / grossProfit;
            
            const decimal msftReturn = (msftPriceSold / msftPriceBought) - 1;
            var msftProfit = target.GetPosition(msft).CalculateGrossProfit(sellDate);
            var msftWeight = msftProfit / grossProfit;

            var expected = (deReturn*deWeight) + (msftReturn*msftWeight);
            var actual = target.CalculateGrossReturn(sellDate);
            Assert.AreEqual(expected, actual);
        }
    }
}