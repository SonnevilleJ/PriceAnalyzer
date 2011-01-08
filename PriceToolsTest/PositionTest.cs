using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;

namespace Sonneville.PriceToolsTest
{
    
    
    /// <summary>
    ///This is a test class for PositionTest and is intended
    ///to contain all PositionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PositionTest
    {


        private TestContext testContextInstance;

        private ITransaction _buy;
        private ITransaction _sell;
        private ITransaction _buyToCover;
        private ITransaction _sellShort;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        [TestMethod()]
        public void OpenTransactionValueReturnsOpenOnly()
        {
            const string ticker = "DE";
            DateTime oDate = new DateTime(2000, 1, 1);
            const decimal oPrice = 100.0m;      // bought at $100.00 per share
            const double oShares = 5;           // bought 5 shares
            const decimal oCommission = 7.95m;  // bought with $7.95 commission
            ITransaction open = new Transaction(oDate, OrderType.Buy, ticker, oPrice, oShares, oCommission);

            IPosition target = new Position(open);

            // No closing transaction (still hold these shares) so Value should return negative value of purchase price minus any commissions.
            const decimal expected = -539.75m;
            decimal actual = target.GetValue(DateTime.Today);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void OpenAndCloseTransaction_ValueReturnsCloseMinusOpenValue()
        {
            const string ticker = "DE";
            DateTime oDate = new DateTime(2000, 1, 1);
            const decimal oPrice = 100.0m;      // bought at $100.00 per share
            const double oShares = 5;           // bought 5 shares
            const decimal oCommission = 7.95m;  // bought with $7.95 commission
            ITransaction open = new Transaction(oDate, OrderType.Buy, ticker, oPrice, oShares, oCommission);

            DateTime cDate = new DateTime(2001, 1, 1);
            const decimal cPrice = 110.0m;      // sold at $110.00 per share
            const double cShares = 5;           // sold 5 shares
            const decimal cCommission = 7.95m;  // sold with $7.95 commission
            ITransaction close = new Transaction(cDate, OrderType.Sell, ticker, cPrice, cShares, cCommission);

            IPosition target = new Position(open, close);

            // No longer hold these shares, so GetValue should return total profit (or negative loss) minus any commissions.
            const decimal expected = -29.5m; // $50.00 profit - $79.50 commissions = $29.5 loss
            decimal actual = target.GetValue(cDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullTransactionsThrowsException()
        {
            new Position(null);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithEmptyTransactionsThrowsException()
        {
            new Position(new ITransaction[0]);
        }

        [TestMethod]
        public void TestRawReturn()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2001, 1, 1);
            const decimal price = 100.0m;       // $100.00 per share
            const double shares = 5;            // 5 shares
            const decimal commission = 7.95m;   // with $7.95 commission

            Transaction buy = new Transaction(date, OrderType.Buy, ticker, price, shares, commission);
            Transaction sell = new Transaction(date, OrderType.Sell, ticker, price + 10m, shares, commission);

            Position target = new Position(buy, sell);

            const decimal expected = 0.10m; // 10% raw return on investment
            decimal actual = target.GetRawReturn(date);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestReturn()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2001, 1, 1);
            const decimal price = 100.0m;       // $100.00 per share
            const double shares = 5;            // 5 shares
            const decimal commission = 5.0m;    // with $5 commission

            Transaction buy = new Transaction(date, OrderType.Buy, ticker, price, shares, commission);
            Transaction sell = new Transaction(date, OrderType.Sell, ticker, price + 10m, shares, commission);

            Position target = new Position(buy, sell);

            const decimal expected = 0m; // 0% return; 100% of original investment
            decimal actual = target.GetTotalReturn(date);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestTotalAnnualReturn()
        {
            const string ticker = "DE";
            DateTime buyDate = new DateTime(2001, 1, 1);
            DateTime sellDate = new DateTime(2001, 3, 15); // sellDate is 0.20 * 365 = 73 days after buyDate
            const decimal buyPrice = 100.0m;    // $100.00 per share
            const decimal sellPrice = 120.0m;   // $110.00 per share
            const double shares = 5;            // 5 shares
            const decimal commission = 5.0m;    // with $5 commission

            Transaction buy = new Transaction(buyDate, OrderType.Buy, ticker, buyPrice, shares, commission);
            Transaction sell = new Transaction(sellDate, OrderType.Sell, ticker, sellPrice, shares, commission);

            Position target = new Position(buy, sell);

            const decimal expectedReturn = 0.1m; // 10% return; profit = $50; initial investment = $500
            decimal actualReturn = target.GetTotalReturn(sellDate);
            Assert.AreEqual(expectedReturn, actualReturn);

            const decimal expected = 0.5m; // 50% annual rate return
            decimal actual = target.GetTotalAnnualReturn(sellDate);
            Assert.AreEqual(expected, actual);
        }
    }
}
