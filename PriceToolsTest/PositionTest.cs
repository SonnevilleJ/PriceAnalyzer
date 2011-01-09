using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.Utilities;

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
        public void GetValueReturnsPurchasePriceIncludingCommissions()
        {
            const string ticker = "DE";
            DateTime oDate = new DateTime(2000, 1, 1);
            const decimal oPrice = 100.0m;      // bought at $100.00 per share
            const double oShares = 5;           // bought 5 shares
            const decimal oCommission = 7.95m;  // bought with $7.95 commission
            ITransaction open = new Transaction(oDate, OrderType.Buy, ticker, oPrice, oShares, oCommission);

            IPosition target = new Position(open);

            const double expectedShares = oShares;
            double actualShares = target.OpenShares;
            Assert.AreEqual(expectedShares, actualShares);

            // Still hold these shares, so GetValue should return negative value of purchase price minus any commissions.
            const decimal expectedValue = -539.75m;
            decimal actualValue = target.GetValue(DateTime.Today);
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod()]
        public void GetValueReturnsCorrectProfitIncludingCommissions()
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

            ITransaction buy = new Transaction(date, OrderType.Buy, ticker, price, shares, commission);
            ITransaction sell = new Transaction(date, OrderType.Sell, ticker, price + 10m, shares, commission);

            IPosition target = new Position(buy, sell);

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

            ITransaction buy = new Transaction(date, OrderType.Buy, ticker, price, shares, commission);
            ITransaction sell = new Transaction(date, OrderType.Sell, ticker, price + 10m, shares, commission);

            IPosition target = new Position(buy, sell);

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

            ITransaction buy = new Transaction(buyDate, OrderType.Buy, ticker, buyPrice, shares, commission);
            ITransaction sell = new Transaction(sellDate, OrderType.Sell, ticker, sellPrice, shares, commission);

            IPosition target = new Position(buy, sell);

            const decimal expectedReturn = 0.1m; // 10% return; profit = $50; initial investment = $500
            decimal actualReturn = target.GetTotalReturn(sellDate);
            Assert.AreEqual(expectedReturn, actualReturn);

            const decimal expected = 0.5m; // 50% annual rate return
            decimal actual = target.GetTotalAnnualReturn(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TransactionCountReturnsCorrectTransactionCount()
        {
            const string longTicker = "DE";
            const string shortTicker = "GM";
            DateTime buyDate = new DateTime(2001, 1, 1);
            DateTime sellDate = new DateTime(2001, 3, 15); // sellDate is 0.20 * 365 = 73 days after buyDate
            const decimal buyPrice = 100.0m;    // $100.00 per share
            const decimal sellPrice = 120.0m;   // $110.00 per share
            const double shares = 5;            // 5 shares
            const decimal commission = 5.0m;    // with $5 commission
            const decimal cashAmount = 10000m;  // $10,000 used for deposit and withdrawal

            ITransaction buy = new Transaction(buyDate, OrderType.Buy, longTicker, buyPrice, shares, commission);
            ITransaction sell = new Transaction(sellDate, OrderType.Sell, longTicker, sellPrice, shares, commission);
            ITransaction sellShort = new Transaction(buyDate, OrderType.SellShort, shortTicker, buyPrice, shares, commission);
            ITransaction buyToCover = new Transaction(sellDate, OrderType.BuyToCover, shortTicker, sellPrice, shares, commission);
            ITransaction deposit = new Deposit(buyDate, cashAmount);
            ITransaction withdrawal = new Withdrawal(sellDate, cashAmount);

            // Must create different positions because all transactions must use same ticker
            IPosition longPosition = new Position(buy, sell);
            IPosition shortPosition = new Position(sellShort, buyToCover);
            IPosition cashPosition = new Position(deposit, withdrawal);

            const int expected = 2;
            int longActual = longPosition.Transactions.Count;
            int shortActual = shortPosition.Transactions.Count;
            int cashActual = cashPosition.Transactions.Count;
            Assert.AreEqual(expected, longActual);
            Assert.AreEqual(expected, shortActual);
            Assert.AreEqual(expected, cashActual);
        }

        [TestMethod]
        public void HasValueTest()
        {
            const string ticker = "DE";
            DateTime testDate = new DateTime(2001, 1, 1);
            DateTime purchaseDate = testDate.AddDays(1);
            const decimal buyPrice = 100.0m;    // $100.00 per share
            const double shares = 5;            // 5 shares
            const decimal commission = 5.0m;    // with $5 commission

            ITransaction buy = new Transaction(purchaseDate, OrderType.Buy, ticker, buyPrice, shares, commission);

            IPosition target = new Position(buy);

            Assert.AreEqual(false, target.HasValue(testDate));
            Assert.AreEqual(true, target.HasValue(purchaseDate));
            Assert.AreEqual(true, target.HasValue(purchaseDate.AddDays(1)));
        }

        [TestMethod]
        public void SerializePositionTest()
        {
            const string ticker = "DE";
            DateTime testDate = new DateTime(2001, 1, 1);
            DateTime purchaseDate = testDate.AddDays(1);
            const decimal buyPrice = 100.0m;    // $100.00 per share
            const double shares = 5;            // 5 shares
            const decimal commission = 5.0m;    // with $5 commission

            ITransaction buy = new Transaction(purchaseDate, OrderType.Buy, ticker, buyPrice, shares, commission);

            IPosition expected = new Position(buy);

            TestUtilities.VerifySerialization(expected);
        }
    }
}
