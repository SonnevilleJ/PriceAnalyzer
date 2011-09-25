using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.Utilities;

namespace Sonneville.PriceToolsTest
{
    [TestClass]
    public class Serialization
    {
        [TestMethod]
        public void SerializeBuyTest()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2001, 1, 1);
            const decimal price = 100.00m;      // $100.00 per share
            const double shares = 5;            // 5 shares
            const decimal commission = 5.0m;    // with $5 commission

            IShareTransaction expected = new Buy
            {
                SettlementDate = date,
                Ticker = ticker,
                Price = price,
                Shares = shares,
                Commission = commission
            };

            IShareTransaction actual = TestUtilities.Serialize(expected);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SerializeBuyToCoverTransactionTest()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2001, 1, 1);
            const decimal price = -100.00m;     // $100.00 per share
            const double shares = 5;            // 5 shares
            const decimal commission = 5.0m;    // with $5 commission

            IShareTransaction expected = new BuyToCover
            {
                SettlementDate = date,
                Ticker = ticker,
                Price = price,
                Shares = shares,
                Commission = commission
            };

            IShareTransaction actual = TestUtilities.Serialize(expected);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SerializeCashAccountTest()
        {
            DateTime date = new DateTime(2011, 1, 16);
            const decimal amount = 10000m;
            ICashAccount target = new CashAccount();
            target.Deposit(date, amount);
            target.Withdraw(date, amount);

            ICashAccount expected = target;
            ICashAccount actual = TestUtilities.Serialize(target);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SerializeDepositTest()
        {
            DateTime date = new DateTime(2001, 1, 1);
            const decimal amount = 100.00m;   // $100.00

            ICashTransaction expected = new Deposit
            {
                SettlementDate = date,
                Amount = amount
            };

            ICashTransaction actual = TestUtilities.Serialize(expected);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SerializeDividendReceiptTransactionTest()
        {
            DateTime date = new DateTime(2001, 1, 17);
            const decimal price = 2.00m;        // $2.00 per share
            const double shares = 5;            // received 5 shares

            DividendReceipt expected = new DividendReceipt
            {
                SettlementDate = date,
                Amount = price * (decimal)shares
            };

            ICashTransaction actual = TestUtilities.Serialize(expected);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SerializeDividendReinvestmentTransactionTest()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2001, 1, 1);
            const decimal price = 2.00m;        // $2.00 per share
            const double shares = 5;            // received 5 shares

            IShareTransaction expected = new DividendReinvestment
            {
                SettlementDate = date,
                Ticker = ticker,
                Price = price,
                Shares = shares,
            };

            IShareTransaction actual = TestUtilities.Serialize(expected);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SerializePortfolioTest()
        {
            DateTime testDate = new DateTime(2011, 1, 8);
            DateTime purchaseDate = testDate.AddDays(1);
            const decimal amount = 10000m;
            const string ticker = "FDRXX"; // Fidelity Cash Reserves
            IPortfolio target = new Portfolio(purchaseDate, amount, ticker);

            decimal expected = target.CalculateValue(purchaseDate);
            decimal actual = TestUtilities.Serialize(target).CalculateValue(purchaseDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SerializePositionTest()
        {
            const string ticker = "DE";
            IPosition target = PositionFactory.CreatePosition(ticker);

            DateTime testDate = new DateTime(2001, 1, 1);
            DateTime purchaseDate = testDate.AddDays(1);
            const decimal buyPrice = 100.00m;   // $100.00 per share
            const double shares = 5;            // 5 shares
            const decimal commission = 5.00m;   // with $5 commission

            target.Buy(purchaseDate, shares, buyPrice, commission);

            decimal expected = target.CalculateValue(purchaseDate);
            decimal actual = TestUtilities.Serialize(target).CalculateValue(purchaseDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SerializePriceQuoteTest()
        {
            DateTime quotedDateTime = new DateTime(2011, 2, 28);
            const int price = 10;
            const int volume = 1000;

            IPriceQuote target = new PriceQuote
            {
                SettlementDate = quotedDateTime,
                Price = price,
                Volume = volume
            };

            IPriceQuote actual = TestUtilities.Serialize(target);
            Assert.AreEqual(target, actual);
        }

        [TestMethod]
        public void SerializePriceSeriesTest()
        {
            PricePeriod p1 = TestUtilities.CreatePeriod1();
            PricePeriod p2 = TestUtilities.CreatePeriod2();
            PricePeriod p3 = TestUtilities.CreatePeriod3();

            PriceSeries target = PriceSeriesFactory.CreatePriceSeries("test");
            target.DataPeriods.Add(p1);
            target.DataPeriods.Add(p2);
            target.DataPeriods.Add(p3);

            PriceSeries actual = TestUtilities.Serialize(target);
            Assert.AreEqual(target, actual);
        }

        [TestMethod]
        public void SerializeQuotedPricePeriodTest()
        {
            IPriceQuote q1 = TestUtilities.CreateQuote1();
            IPriceQuote q2 = TestUtilities.CreateQuote2();
            IPriceQuote q3 = TestUtilities.CreateQuote3();

            QuotedPricePeriod target = new QuotedPricePeriod();
            target.AddPriceQuotes(q1, q2, q3);

            QuotedPricePeriod actual = TestUtilities.Serialize(target);
            Assert.AreEqual(target, actual);
        }

        [TestMethod]
        public void SerializeSellShortTransactionTest()
        {
            const string ticker = "DE";
            DateTime settlementDate = new DateTime(2001, 1, 1);
            const decimal price = 100.00m;   // $100.00 per share
            const double shares = 5;            // 5 shares
            const decimal commission = 5.0m;    // with $5 commission

            IShareTransaction expected = new SellShort
            {
                SettlementDate = settlementDate,
                Ticker = ticker,
                Price = price,
                Shares = shares,
                Commission = commission,
            };

            IShareTransaction actual = TestUtilities.Serialize(expected);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SerializeSellTransactionTest()
        {
            const string ticker = "DE";
            DateTime date = new DateTime(2001, 1, 1);
            const decimal price = 100.00m;   // $100.00 per share
            const double shares = 5;            // 5 shares
            const decimal commission = 5.0m;    // with $5 commission

            ShareTransaction expected = new Sell
            {
                SettlementDate = date,
                Ticker = ticker,
                Price = price,
                Shares = shares,
                Commission = commission
            };

            ShareTransaction actual = TestUtilities.Serialize(expected);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SerializeStaticPricePeriodTest()
        {
            DateTime head = new DateTime(2011, 3, 13);
            DateTime tail = head.AddDays(1);
            const decimal open = 10.00m;
            const decimal high = 11.00m;
            const decimal low = 9.00m;
            const decimal close = 10.00m;
            const long volume = 1000;

            StaticPricePeriod target = PricePeriodFactory.CreateStaticPricePeriod(head, tail, open, high, low, close, volume);

            StaticPricePeriod actual = TestUtilities.Serialize(target);
            Assert.AreEqual(target, actual);
        }

        [TestMethod]
        public void SerializeWithdrawalTransactionTest()
        {
            DateTime date = new DateTime(2001, 1, 1);
            const decimal amount = 100.00m;   // $100.00

            ICashTransaction expected = new Withdrawal
            {
                SettlementDate = date,
                Amount = amount
            };

            ICashTransaction actual = TestUtilities.Serialize(expected);
            Assert.AreEqual(expected, actual);
        }
    }
}
