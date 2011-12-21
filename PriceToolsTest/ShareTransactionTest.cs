using Sonneville.PriceTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Sonneville.PriceToolsTest
{
    /// <summary>
    ///This is a test class for ShareTransactionTest and is intended
    ///to contain all ShareTransactionTest Unit Tests
    ///</summary>
    [TestClass]
    public class ShareTransactionTest
    {
        /// <summary>
        ///A test for TotalValue
        ///</summary>
        [TestMethod]
        public void TotalValueTest()
        {
            var settlementDate = new DateTime(2011, 12, 11);
            const OrderType orderType = OrderType.Buy;
            const string ticker = "DE";
            const decimal price = 100.00m;
            const int shares = 5;
            const decimal commission = 10.00m;

            var target = TransactionFactory.Instance.CreateShareTransaction(settlementDate, orderType, ticker, price, shares, commission);

            const decimal expected = (price*shares) + commission;
            var actual = target.TotalValue;
            Assert.AreEqual(expected, actual);
        }
    }
}
