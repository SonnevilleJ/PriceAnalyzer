﻿using Sonneville.PriceTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Sonneville.Utilities;

namespace Sonneville.PriceToolsTest
{


    /// <summary>
    ///This is a test class for TransactionTest and is intended
    ///to contain all TransactionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TransactionTest
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


        #region Constructor Tests (Using Buy)

        /// <summary>
        ///A test for ITransaction Constructor
        ///</summary>
        [TestMethod()]
        public void TransactionConstructorTest1()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2000, 1, 1);
            const OrderType type = OrderType.Buy;
            const decimal price = 100.0m;      // bought at $100.00 per share

            const double shares = 1;
            const decimal commission = 0.00m;
            ITransaction target = TransactionFactory.CreateTransaction(date, type, ticker, price);
            Assert.AreEqual(ticker, target.Ticker);
            Assert.AreEqual(date, target.SettlementDate);
            Assert.AreEqual(type, target.OrderType);
            Assert.AreEqual(price, target.Price);
            Assert.AreEqual(shares, target.Shares);
            Assert.AreEqual(commission, target.Commission);
        }

        /// <summary>
        ///A test for ITransaction Constructor
        ///</summary>
        [TestMethod()]
        public void TransactionConstructorTest2()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2000, 1, 1);
            const OrderType type = OrderType.Buy;
            const decimal price = 100.0m;      // bought at $100.00 per share
            const double shares = 5;           // bought 5 shares

            const decimal commission = 0.00m;
            ITransaction target = TransactionFactory.CreateTransaction(date, type, ticker, price, shares);
            Assert.AreEqual(ticker, target.Ticker);
            Assert.AreEqual(date, target.SettlementDate);
            Assert.AreEqual(type, target.OrderType);
            Assert.AreEqual(price, target.Price);
            Assert.AreEqual(shares, target.Shares);
            Assert.AreEqual(commission, target.Commission);
        }

        #endregion

        #region Buy Tests

        /// <summary>
        ///A test for ITransaction Constructor
        ///</summary>
        [TestMethod()]
        public void BuyTest()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2000, 1, 1);
            const OrderType type = OrderType.Buy;
            const decimal price = 100.0m; // bought at $100.00 per share
            const double shares = 5; // bought 5 shares
            const decimal commission = 7.95m; // bought with $7.95 commission

            ITransaction target = TransactionFactory.CreateTransaction(date, type, ticker, price, shares, commission);
            Assert.AreEqual(ticker, target.Ticker);
            Assert.AreEqual(date, target.SettlementDate);
            Assert.AreEqual(type, target.OrderType);
            Assert.AreEqual(price, target.Price);
            Assert.AreEqual(shares, target.Shares);
            Assert.AreEqual(commission, target.Commission);
        }

        /// <summary>
        ///A test for Shares
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void BuyWithNegativeSharesTest()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2000, 1, 1);
            const OrderType type = OrderType.Buy;
            const decimal price = 100.0m;      // bought at $100.00 per share
            const double shares = -5;           // bought 5 shares
            const decimal commission = 7.95m;  // bought with $7.95 commission
            TransactionFactory.CreateTransaction(date, type, ticker, price, shares, commission);
        }

        /// <summary>
        ///A test for Price
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void BuyWithNegativePriceTest()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2000, 1, 1);
            const OrderType type = OrderType.Buy;
            const decimal price = -100.0m;      // bought at $-100.00 per share - error
            const double shares = 5;           // bought 5 shares
            const decimal commission = 7.95m;  // bought with $7.95 commission
            TransactionFactory.CreateTransaction(date, type, ticker, price, shares, commission);
        }

        /// <summary>
        ///A test for Commission
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void BuyWithNegativeCommissionTest()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2000, 1, 1);
            const OrderType type = OrderType.Buy;
            const decimal price = 100.0m;      // bought at $100.00 per share
            const double shares = 5;           // bought 5 shares
            const decimal commission = -7.95m;  // bought with $7.95 commission
            TransactionFactory.CreateTransaction(date, type, ticker, price, shares, commission);
        }

        [TestMethod()]
        public void SerializeBuyTransactionTest()
        {
            const string ticker = "DE";
            DateTime purchaseDate = new DateTime(2001, 1, 1);
            const OrderType type = OrderType.Buy;
            const decimal buyPrice = 100.0m;    // $100.00 per share
            const double shares = 5;            // 5 shares
            const decimal commission = 5.0m;    // with $5 commission

            ITransaction target = TransactionFactory.CreateTransaction(purchaseDate, type, ticker, buyPrice, shares, commission);

            TestUtilities.VerifySerialization(target);
        }

        #endregion

        #region Sell Tests

        /// <summary>
        ///A test for Shares
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SellWithNegativeSharesTest()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2000, 1, 1);
            const OrderType type = OrderType.Sell;
            const decimal price = 100.0m;      // sold at $100.00 per share
            const double shares = -5;           // sold 5 shares
            const decimal commission = 7.95m;  // sold with $7.95 commission
            TransactionFactory.CreateTransaction(date, type, ticker, price, shares, commission);
        }

        /// <summary>
        ///A test for Price
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SellWithNegativePriceTest()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2000, 1, 1);
            const OrderType type = OrderType.Sell;
            const decimal price = -100.0m;      // sold at $-100.00 per share - error
            const double shares = 5;           // sold 5 shares
            const decimal commission = 7.95m;  // sold with $7.95 commission
            TransactionFactory.CreateTransaction(date, type, ticker, price, shares, commission);
        }

        /// <summary>
        ///A test for Commission
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SellWithNegativeCommissionTest()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2000, 1, 1);
            const OrderType type = OrderType.Sell;
            const decimal price = 100.0m;      // sold at $100.00 per share
            const double shares = 5;           // sold 5 shares
            const decimal commission = -7.95m;  // sold with $7.95 commission
            TransactionFactory.CreateTransaction(date, type, ticker, price, shares, commission);
        }

        /// <summary>
        ///A test for ITransaction Constructor
        ///</summary>
        [TestMethod()]
        public void SellTest()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2000, 1, 1);
            const OrderType type = OrderType.Buy;
            const decimal price = 100.0m; // bought at $100.00 per share
            const double shares = 5; // bought 5 shares
            const decimal commission = 7.95m; // bought with $7.95 commission

            ITransaction target = TransactionFactory.CreateTransaction(date, type, ticker, price, shares, commission);
            Assert.AreEqual(ticker, target.Ticker);
            Assert.AreEqual(date, target.SettlementDate);
            Assert.AreEqual(type, target.OrderType);
            Assert.AreEqual(price, target.Price);
            Assert.AreEqual(shares, target.Shares);
            Assert.AreEqual(commission, target.Commission);
        }

        #endregion

        #region SellShort Tests

        /// <summary>
        ///A test for Shares
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SellShortWithNegativeSharesTest()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2000, 1, 1);
            const OrderType type = OrderType.SellShort;
            const decimal price = 100.0m;      // sold at $100.00 per share
            const double shares = -5;           // sold 5 shares
            const decimal commission = 7.95m;  // sold with $7.95 commission
            TransactionFactory.CreateTransaction(date, type, ticker, price, shares, commission);
        }

        /// <summary>
        ///A test for Price
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SellShortWithNegativePriceTest()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2000, 1, 1);
            const OrderType type = OrderType.SellShort;
            const decimal price = -100.0m;      // sold at $-100.00 per share - error
            const double shares = 5;           // sold 5 shares
            const decimal commission = 7.95m;  // sold with $7.95 commission
            TransactionFactory.CreateTransaction(date, type, ticker, price, shares, commission);
        }

        /// <summary>
        ///A test for Commission
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SellShortWithNegativeCommissionTest()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2000, 1, 1);
            const OrderType type = OrderType.SellShort;
            const decimal price = 100.0m;      // sold at $100.00 per share
            const double shares = 5;           // sold 5 shares
            const decimal commission = -7.95m;  // sold with $7.95 commission
            TransactionFactory.CreateTransaction(date, type, ticker, price, shares, commission);
        }

        /// <summary>
        ///A test for ITransaction Constructor
        ///</summary>
        [TestMethod()]
        public void SellShortTest()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2000, 1, 1);
            const OrderType type = OrderType.SellShort;
            const decimal price = 100.0m; // sold at $100.00 per share
            const double shares = 5; // sold 5 shares
            const decimal commission = 7.95m; // sold with $7.95 commission

            ITransaction target = TransactionFactory.CreateTransaction(date, type, ticker, price, shares, commission);
            Assert.AreEqual(ticker, target.Ticker);
            Assert.AreEqual(date, target.SettlementDate);
            Assert.AreEqual(type, target.OrderType);
            Assert.AreEqual(price, target.Price);
            Assert.AreEqual(shares, target.Shares);
            Assert.AreEqual(commission, target.Commission);
        }

        #endregion

        #region BuyToCover Tests

        /// <summary>
        ///A test for Shares
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void BuyToCoverWithNegativeSharesTest()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2000, 1, 1);
            const OrderType type = OrderType.BuyToCover;
            const decimal price = 100.0m;      // bought at $100.00 per share
            const double shares = -5;          // bought 5 shares
            const decimal commission = 7.95m;  // bought with $7.95 commission
            TransactionFactory.CreateTransaction(date, type, ticker, price, shares, commission);
        }

        /// <summary>
        ///A test for Price
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void BuyToCoverWithNegativePriceTest()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2000, 1, 1);
            const OrderType type = OrderType.BuyToCover;
            const decimal price = -100.0m;     // bought at $-100.00 per share - error
            const double shares = 5;           // bought 5 shares
            const decimal commission = 7.95m;  // bought with $7.95 commission
            TransactionFactory.CreateTransaction(date, type, ticker, price, shares, commission);
        }

        /// <summary>
        ///A test for Commission
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void BuyToCoverWithNegativeCommissionTest()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2000, 1, 1);
            const OrderType type = OrderType.BuyToCover;
            const decimal price = 100.0m;      // bought at $100.00 per share
            const double shares = 5;           // bought 5 shares
            const decimal commission = -7.95m; // bought with $7.95 commission
            TransactionFactory.CreateTransaction(date, type, ticker, price, shares, commission);
        }

        /// <summary>
        ///A test for ITransaction Constructor
        ///</summary>
        [TestMethod()]
        public void BuyToCoverTest()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2000, 1, 1);
            const OrderType type = OrderType.BuyToCover;
            const decimal price = 100.0m;       // bought at $100.00 per share
            const double shares = 5;            // bought 5 shares
            const decimal commission = 7.95m;   // bought with $7.95 commission

            ITransaction target = TransactionFactory.CreateTransaction(date, type, ticker, price, shares, commission);
            Assert.AreEqual(ticker, target.Ticker);
            Assert.AreEqual(date, target.SettlementDate);
            Assert.AreEqual(type, target.OrderType);
            Assert.AreEqual(price, target.Price);
            Assert.AreEqual(shares, target.Shares);
            Assert.AreEqual(commission, target.Commission);
        }

        #endregion

        #region Deposit Tests

        /// <summary>
        ///A test for Date
        ///</summary>
        [TestMethod()]
        public void DepositDateTest()
        {
            DateTime dateTime = new DateTime(2011, 1, 9);
            const decimal amount = 1000m;
            ITransaction target = TransactionFactory.CreateDeposit(dateTime, amount);

            DateTime expectedDate = dateTime;
            DateTime actualDate = target.SettlementDate;
            Assert.AreEqual(expectedDate, actualDate);
        }

        /// <summary>
        ///A test for OrderType
        ///</summary>
        [TestMethod()]
        public void DepositOrderTypeTest()
        {
            DateTime dateTime = new DateTime(2011, 1, 9);
            const decimal amount = 1000m;
            ITransaction target = TransactionFactory.CreateDeposit(dateTime, amount);

            const OrderType expectedType = OrderType.Deposit;
            OrderType actualType = target.OrderType;
            Assert.AreEqual(expectedType, actualType);
        }

        /// <summary>
        ///A test for Price
        ///</summary>
        [TestMethod()]
        public void DepositPricePositiveTest()
        {
            DateTime dateTime = new DateTime(2011, 1, 9);
            const decimal amount = 1000m;
            ITransaction target = TransactionFactory.CreateDeposit(dateTime, amount);

            const decimal expectedPrice = 1.00m; // Deposits return positive price
            decimal actualPrice = target.Price;
            Assert.AreEqual(expectedPrice, actualPrice);
        }

        /// <summary>
        ///A test for Price
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DepositPriceNegativeTest()
        {
            DateTime dateTime = new DateTime(2011, 1, 9);
            const decimal amount = -1000m;

            ITransaction target = TransactionFactory.CreateDeposit(dateTime, amount);
        }

        /// <summary>
        ///A test for Shares
        ///</summary>
        [TestMethod()]
        public void DepositSharesTest()
        {
            DateTime dateTime = new DateTime(2011, 1, 9);
            const decimal amount = 1000m;
            ITransaction target = TransactionFactory.CreateDeposit(dateTime, amount);

            const double expectedShares = (double)amount;
            double actualShares = target.Shares;
            Assert.AreEqual(expectedShares, actualShares);
        }

        /// <summary>
        ///A test for Shares
        ///</summary>
        [TestMethod()]
        public void DepositCommissionTest()
        {
            DateTime dateTime = new DateTime(2011, 1, 9);
            const decimal amount = 1000m;
            ITransaction target = TransactionFactory.CreateDeposit(dateTime, amount);

            const decimal expectedCommission = 0.00m;
            decimal actualCommission = target.Commission;
            Assert.AreEqual(expectedCommission, actualCommission);
        }
        
        [TestMethod()]
        public void DepositTickerTest()
        {
            DateTime dateTime = new DateTime(2011, 1, 9);
            const decimal amount = 1000m;
            ITransaction target = TransactionFactory.CreateDeposit(dateTime, amount);

            string expectedTicker = String.Empty;
            string actualTicker = target.Ticker;
            Assert.AreEqual(expectedTicker, actualTicker);
        }

        #endregion

        #region Withdrawal Tests

        /// <summary>
        ///A test for Date
        ///</summary>
        [TestMethod()]
        public void WithdrawalDateTest()
        {
            DateTime dateTime = new DateTime(2011, 1, 9);
            const decimal amount = 1000m;
            ITransaction target = TransactionFactory.CreateWithdrawal(dateTime, amount);

            DateTime expectedDate = dateTime;
            DateTime actualDate = target.SettlementDate;
            Assert.AreEqual(expectedDate, actualDate);
        }

        /// <summary>
        ///A test for OrderType
        ///</summary>
        [TestMethod()]
        public void WithdrawalOrderTypeTest()
        {
            DateTime dateTime = new DateTime(2011, 1, 9);
            const decimal amount = 1000m;
            ITransaction target = TransactionFactory.CreateWithdrawal(dateTime, amount);

            const OrderType expectedType = OrderType.Withdrawal;
            OrderType actualType = target.OrderType;
            Assert.AreEqual(expectedType, actualType);
        }

        /// <summary>
        ///A test for Price
        ///</summary>
        [TestMethod()]
        public void WithdrawalPricePositiveTest()
        {
            DateTime dateTime = new DateTime(2011, 1, 9);
            const decimal amount = 1000m;
            ITransaction target = TransactionFactory.CreateWithdrawal(dateTime, amount);

            const decimal expectedPrice = 1.00m; // Withdrawals return positive price
            decimal actualPrice = target.Price;
            Assert.AreEqual(expectedPrice, actualPrice);
        }

        /// <summary>
        ///A test for Price
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void WithdrawalPriceNegativeTest()
        {
            DateTime dateTime = new DateTime(2011, 1, 9);
            const decimal amount = -1000m;
            ITransaction target = TransactionFactory.CreateWithdrawal(dateTime, amount);
        }

        /// <summary>
        ///A test for Shares
        ///</summary>
        [TestMethod()]
        public void WithdrawalSharesTest()
        {
            DateTime dateTime = new DateTime(2011, 1, 9);
            const decimal amount = 1000m;
            ITransaction target = TransactionFactory.CreateWithdrawal(dateTime, amount);

            const double expectedShares = (double)amount;
            double actualShares = target.Shares;
            Assert.AreEqual(expectedShares, actualShares);
        }

        /// <summary>
        ///A test for Commission
        ///</summary>
        [TestMethod()]
        public void WithdrawalCommissionPositiveTest()
        {
            DateTime dateTime = new DateTime(2011, 1, 9);
            const decimal amount = 1000m;
            ITransaction target = TransactionFactory.CreateWithdrawal(dateTime, amount);

            const decimal expectedCommission = 0.00m;
            decimal actualCommission = target.Commission;
            Assert.AreEqual(expectedCommission, actualCommission);
        }
        
        [TestMethod()]
        public void WithdrawalTickerTest()
        {
            DateTime dateTime = new DateTime(2011, 1, 9);
            const decimal amount = 1000m;
            ITransaction target = TransactionFactory.CreateWithdrawal(dateTime, amount);

            string expectedTicker = String.Empty;
            string actualTicker = target.Ticker;
            Assert.AreEqual(expectedTicker, actualTicker);
        }

        #endregion

        #region DividendReceipt Tests

        /// <summary>
        ///A test for Shares
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DividendReceiptWithNegativeSharesTest()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2011, 1, 15);
            const OrderType type = OrderType.DividendReceipt;
            const decimal price = 100.0m;      // bought at $100.00 per share
            const double shares = -5;          // bought 5 shares
            TransactionFactory.CreateTransaction(date, type, ticker, price, shares);
        }

        /// <summary>
        ///A test for Shares
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DividendReceiptWithPositiveSharesTest()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2011, 1, 15);
            const OrderType type = OrderType.DividendReceipt;
            const decimal price = 100.0m;       // bought at $100.00 per share
            const double shares = 5;            // bought 5 shares
            TransactionFactory.CreateTransaction(date, type, ticker, price, shares);
        }

        /// <summary>
        ///A test for Price
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DividendReceiptWithNegativePriceTest()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2011, 1, 15);
            const OrderType type = OrderType.DividendReceipt;
            const decimal price = -100.0m;     // bought at $-100.00 per share - error
            TransactionFactory.CreateTransaction(date, type, ticker, price);
        }

        /// <summary>
        ///A test for Commission
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DividendReceiptWithNegativeCommissionTest()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2011, 1, 15);
            const OrderType type = OrderType.DividendReceipt;
            const decimal price = 100.0m;      // bought at $100.00 per share
            const double shares = 0;           // bought 5 shares
            const decimal commission = -7.95m; // bought with $7.95 commission
            TransactionFactory.CreateTransaction(date, type, ticker, price, shares, commission);
        }

        /// <summary>
        ///A test for Commission
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DividendReceiptWithPositiveCommissionTest()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2011, 1, 15);
            const OrderType type = OrderType.DividendReceipt;
            const decimal price = 100.0m;      // bought at $100.00 per share
            const double shares = 0;           // bought 5 shares
            const decimal commission = 7.95m;  // bought with $7.95 commission
            TransactionFactory.CreateTransaction(date, type, ticker, price, shares, commission);
        }

        /// <summary>
        ///A test for ITransaction Constructor
        ///</summary>
        [TestMethod()]
        public void DividendReceiptTest()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2011, 1, 15);
            const OrderType type = OrderType.DividendReceipt;
            const decimal price = 100.0m;   // bought at $100.00 per share
            const double shares = 0;        // bought 5 shares
            const decimal commission = 0;   // bought with $7.95 commission

            ITransaction target = TransactionFactory.CreateTransaction(date, type, ticker, price, shares, commission);
            Assert.AreEqual(ticker, target.Ticker);
            Assert.AreEqual(date, target.SettlementDate);
            Assert.AreEqual(type, target.OrderType);
            Assert.AreEqual(price, target.Price);
            Assert.AreEqual(shares, target.Shares);
            Assert.AreEqual(commission, target.Commission);
        }

        #endregion

        #region DividendReinvestment Tests

        /// <summary>
        ///A test for Shares
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DividendReinvestmentWithNegativeSharesTest()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2011, 1, 15);
            const OrderType type = OrderType.DividendReinvestment;
            const decimal price = 100.0m;      // bought at $100.00 per share
            const double shares = -5;          // bought 5 shares
            TransactionFactory.CreateTransaction(date, type, ticker, price, shares);
        }

        /// <summary>
        ///A test for Shares
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DividendReinvestmentWithPositiveSharesTest()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2011, 1, 15);
            const OrderType type = OrderType.DividendReinvestment;
            const decimal price = 100.0m;       // bought at $100.00 per share
            const double shares = 5;            // bought 5 shares
            TransactionFactory.CreateTransaction(date, type, ticker, price, shares);
        }

        /// <summary>
        ///A test for Price
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DividendReinvestmentWithNegativePriceTest()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2011, 1, 15);
            const OrderType type = OrderType.DividendReinvestment;
            const decimal price = -100.0m;     // bought at $-100.00 per share - error
            TransactionFactory.CreateTransaction(date, type, ticker, price);
        }

        /// <summary>
        ///A test for Commission
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DividendReinvestmentWithNegativeCommissionTest()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2011, 1, 15);
            const OrderType type = OrderType.DividendReinvestment;
            const decimal price = 100.0m;      // bought at $100.00 per share
            const double shares = 0;           // bought 5 shares
            const decimal commission = -7.95m; // bought with $7.95 commission
            TransactionFactory.CreateTransaction(date, type, ticker, price, shares, commission);
        }

        /// <summary>
        ///A test for Commission
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DividendReinvestmentWithPositiveCommissionTest()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2011, 1, 15);
            const OrderType type = OrderType.DividendReinvestment;
            const decimal price = 100.0m;      // bought at $100.00 per share
            const double shares = 0;           // bought 5 shares
            const decimal commission = 7.95m;  // bought with $7.95 commission
            TransactionFactory.CreateTransaction(date, type, ticker, price, shares, commission);
        }

        /// <summary>
        ///A test for ITransaction Constructor
        ///</summary>
        [TestMethod()]
        public void DividendReinvestmentTest()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2011, 1, 15);
            const OrderType type = OrderType.DividendReinvestment;
            const decimal price = 100.0m;   // bought at $100.00 per share
            const double shares = 0;        // bought 5 shares
            const decimal commission = 0;   // bought with $7.95 commission

            ITransaction target = TransactionFactory.CreateTransaction(date, type, ticker, price, shares, commission);
            Assert.AreEqual(ticker, target.Ticker);
            Assert.AreEqual(date, target.SettlementDate);
            Assert.AreEqual(type, target.OrderType);
            Assert.AreEqual(price, target.Price);
            Assert.AreEqual(shares, target.Shares);
            Assert.AreEqual(commission, target.Commission);
        }

        #endregion
    }
}
