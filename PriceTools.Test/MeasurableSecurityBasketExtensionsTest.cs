using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sonneville.PriceTools.Test
{
    [TestClass]
    public class MeasurableSecurityBasketExtensionsTest
    {
        [TestMethod]
        public void PositionCalculateAverageAnnualReturnTest()
        {
            const string ticker = "DE";
            var target = PositionFactory.CreatePosition(ticker);

            var buyDate = new DateTime(2001, 1, 1);
            var sellDate = new DateTime(2001, 3, 15); // sellDate is 0.20 * 365 = 73 days after buyDate
            const decimal buyPrice = 100.00m;       // $100.00 per share
            const decimal sellPrice = 112.00m;      // $112.00 per share
            const double shares = 5;                // 5 shares
            const decimal commission = 5.00m;       // with $5 commission

            target.Buy(buyDate, shares, buyPrice, commission);
            target.Sell(sellDate, shares, sellPrice, commission);

            const decimal expectedReturn = 0.1m;    // 10% return; profit = $50 after commissions; initial investment = $500
            var actualReturn = target.CalculateTotalReturn(sellDate);
            Assert.AreEqual(expectedReturn, actualReturn);

            const decimal expected = 0.5m;          // 50% annual rate return
            var actual = target.CalculateAverageAnnualReturn(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PositionCalculateAverageAnnualReturnWithoutProceedsTest()
        {
            const string ticker = "DE";
            var target = PositionFactory.CreatePosition(ticker);

            var buyDate = new DateTime(2001, 1, 1);
            var sellDate = new DateTime(2001, 3, 15); // sellDate is 0.20 * 365 = 73 days after buyDate
            const decimal price = 100.00m;          // $100.00 per share
            const double shares = 5;                // 5 shares
            const decimal commission = 5.00m;       // with $5 commission

            target.Buy(buyDate, shares, price, commission);

            Assert.IsNull(target.CalculateAverageAnnualReturn(sellDate));
        }

        [TestMethod]
        public void PositionCalculateRawReturnTest()
        {
            const string ticker = "DE";
            var target = PositionFactory.CreatePosition(ticker);

            var buyDate = new DateTime(2001, 1, 1);
            var sellDate = buyDate.AddDays(1);
            const decimal price = 100.00m;      // $100.00 per share
            const double shares = 5;            // 5 shares
            const decimal commission = 7.95m;   // with $7.95 commission

            target.Buy(buyDate, shares, price, commission);
            target.Sell(sellDate, shares, price + 10m, commission);

            const decimal expected = 0.1m;      // 10% raw return on investment
            var actual = target.CalculateRawReturn(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PositionCalculateRawReturnWithoutProceedsTest()
        {
            const string ticker = "DE";
            var target = PositionFactory.CreatePosition(ticker);

            var buyDate = new DateTime(2001, 1, 1);
            var sellDate = buyDate.AddDays(1);
            const decimal price = 100.00m;      // $100.00 per share
            const double shares = 5;            // 5 shares
            const decimal commission = 7.95m;   // with $7.95 commission

            target.Buy(buyDate, shares, price, commission);

            Assert.IsNull(target.CalculateRawReturn(sellDate));
        }

        [TestMethod]
        public void PositionCalculateTotalReturnTest()
        {
            const string ticker = "DE";
            var target = PositionFactory.CreatePosition(ticker);

            var buyDate = new DateTime(2001, 1, 1);
            var sellDate = buyDate.AddDays(1);
            const decimal price = 100.00m;       // $100.00 per share
            const double shares = 5;             // 5 shares
            const decimal commission = 5.00m;    // with $5 commission

            target.Buy(buyDate, shares, price, commission);
            target.Sell(sellDate, shares, price + 2.00m, commission);

            const decimal expected = 0.00m;      // 0% return; 100% of original investment
            var actual = target.CalculateTotalReturn(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PositionCalculateTotalReturnTest2()
        {
            const string ticker = "DE";
            var target = PositionFactory.CreatePosition(ticker);

            var buyDate = new DateTime(2001, 1, 1);
            var sellDate = buyDate.AddDays(1);
            const decimal price = 100.00m;       // $100.00 per share
            const double shares = 5;             // 5 shares
            const decimal commission = 0.00m;    // with $0 commission

            target.Buy(buyDate, shares, price, commission);
            target.Sell(sellDate, shares, price * 2m, commission);

            const decimal expected = 1.00m;      // 100% return; 200% of original investment
            var actual = target.CalculateTotalReturn(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PositionCalculateTotalReturnWithoutProceedsTest()
        {
            const string ticker = "DE";
            var target = PositionFactory.CreatePosition(ticker);

            var buyDate = new DateTime(2001, 1, 1);
            var sellDate = buyDate.AddDays(1);
            const decimal price = 100.00m;       // $100.00 per share
            const double shares = 5;             // 5 shares
            const decimal commission = 5.00m;    // with $5 commission

            target.Buy(buyDate, shares, price, commission);

            Assert.IsNull(target.CalculateTotalReturn(sellDate));
        }
    }
}
