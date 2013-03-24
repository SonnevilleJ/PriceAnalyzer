using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.Utilities.Serialization;
using TestUtilities.Sonneville.PriceTools;

namespace Test.Sonneville.PriceTools
{
    public abstract class ShareTransactionTestsBase
    {
        private static readonly ITransactionFactory _transactionFactory;

        static ShareTransactionTestsBase()
        {
            _transactionFactory = new TransactionFactory();
        }

        /// <summary>
        /// A test for serialization
        /// </summary>
        [TestMethod]
        public abstract void SerializeTest();

        /// <summary>
        /// A test for SettlementDate
        /// </summary>
        [TestMethod]
        public abstract void SettlementDateTest();

        /// <summary>
        /// A test for Ticker
        /// </summary>
        [TestMethod]
        public abstract void TickerTest();

        /// <summary>
        /// A test for Price
        /// </summary>
        [TestMethod]
        public abstract void PriceValidTest();

        /// <summary>
        /// A test for Price
        /// </summary>
        [TestMethod]
        public abstract void PriceInvalidTest();

        /// <summary>
        /// A test for Shares
        /// </summary>
        [TestMethod]
        public abstract void SharesValidTest();

        /// <summary>
        /// A test for Shares
        /// </summary>
        [TestMethod]
        public abstract void SharesInvalidTest();

        /// <summary>
        /// A test for Commission
        /// </summary>
        [TestMethod]
        public abstract void CommissionValidTest();

        /// <summary>
        /// A test for Commission
        /// </summary>
        [TestMethod]
        public abstract void CommissionInvalidTest();

        /// <summary>
        /// A test for TotalValue
        /// </summary>
        [TestMethod]
        public abstract void TotalValueTest();

        /// <summary>
        /// A test for long/short transaction type
        /// </summary>
        [TestMethod]
        public abstract void LongShortTest();

        /// <summary>
        /// A test for accumulation/distribution transaction type
        /// </summary>
        [TestMethod]
        public abstract void AccumulationDistributionTest();

        /// <summary>
        /// A test for opening/closing transaction type
        /// </summary>
        [TestMethod]
        public abstract void OpeningClosingTest();

        protected static void ShareTransactionSerializeTest(OrderType transactionType)
        {
            var settlementDate = new DateTime(2012, 1, 18);
            var ticker = GetValidTicker();
            var price = GetValidPrice(transactionType);
            var shares = GetValidShares();
            var commission = GetValidCommission(transactionType);

            var target = _transactionFactory.ConstructShareTransaction(transactionType, ticker, settlementDate, shares, price, commission);

            var xml = XmlSerializer.SerializeToXml(target);
            var result = XmlSerializer.DeserializeFromXml<IShareTransaction>(xml);

            GenericTestUtilities.AssertSameState(target, result);
        }

        protected static void ShareTransactionSettlementDateTest(OrderType transactionType)
        {
            var settlementDate = new DateTime(2012, 1, 18);
            var ticker = GetValidTicker();
            var price = GetValidPrice(transactionType);
            var shares = GetValidShares();
            var commission = GetValidCommission(transactionType);

            var target = _transactionFactory.ConstructShareTransaction(transactionType, ticker, settlementDate, shares, price, commission);

            var expected = settlementDate;
            var actual = target.SettlementDate;
            Assert.AreEqual(expected, actual);
        }

        protected static void ShareTransactionTickerTest(OrderType transactionType)
        {
            var settlementDate = new DateTime(2012, 1, 18);
            var ticker = GetValidTicker();
            var price = GetValidPrice(transactionType);
            var shares = GetValidShares();
            var commission = GetValidCommission(transactionType);

            var target = _transactionFactory.ConstructShareTransaction(transactionType, ticker, settlementDate, shares, price, commission);

            var expected = ticker;
            var actual = target.Ticker;
            Assert.AreEqual(expected, actual);
        }

        protected static void ShareTransactionPriceValidTest(OrderType transactionType)
        {
            var settlementDate = new DateTime(2012, 1, 18);
            var ticker = GetValidTicker();
            var price = GetValidPrice(transactionType);
            var shares = GetValidShares();
            var commission = GetValidCommission(transactionType);

            var target = _transactionFactory.ConstructShareTransaction(transactionType, ticker, settlementDate, shares, price, commission);

            var expected = GetValidPrice(transactionType);
            var actual = target.Price;
            Assert.AreEqual(expected, actual);
        }

        protected static void ShareTransactionPriceInvalidTest(OrderType transactionType)
        {
            var settlementDate = new DateTime(2012, 1, 18);
            var ticker = GetValidTicker();
            var price = GetInvalidPrice(transactionType);
            var shares = GetValidShares();
            var commission = GetValidCommission(transactionType);

            var target = _transactionFactory.ConstructShareTransaction(transactionType, ticker, settlementDate, shares, price, commission);

            var expected = GetValidPrice(transactionType);
            var actual = target.Price;
            Assert.AreEqual(expected, actual);
        }

        protected static void ShareTransactionSharesValidTest(OrderType transactionType)
        {
            var settlementDate = new DateTime(2012, 1, 18);
            var ticker = GetValidTicker();
            var price = GetValidPrice(transactionType);
            var shares = GetValidShares();
            var commission = GetValidCommission(transactionType);

            var target = _transactionFactory.ConstructShareTransaction(transactionType, ticker, settlementDate, shares, price, commission);

            var expected = GetValidShares();
            var actual = target.Shares;
            Assert.AreEqual(expected, actual);
        }

        protected static void ShareTransactionSharesInvalidTest(OrderType transactionType)
        {
            var settlementDate = new DateTime(2012, 1, 18);
            var ticker = GetValidTicker();
            var price = GetValidPrice(transactionType);
            var shares = GetInvalidShares();
            var commission = GetValidCommission(transactionType);

            _transactionFactory.ConstructShareTransaction(transactionType, ticker, settlementDate, shares, price, commission);
        }

        protected static void ShareTransactionCommissionValidTest(OrderType transactionType)
        {
            var settlementDate = new DateTime(2012, 1, 18);
            var ticker = GetValidTicker();
            var price = GetValidPrice(transactionType);
            var shares = GetValidShares();
            var commission = GetValidCommission(transactionType);

            var target = _transactionFactory.ConstructShareTransaction(transactionType, ticker, settlementDate, shares, price, commission);

            var expected = GetValidCommission(transactionType);
            var actual = target.Commission;
            Assert.AreEqual(expected, actual);
        }

        protected static void ShareTransactionCommissionInvalidTest(OrderType transactionType)
        {
            var settlementDate = new DateTime(2012, 1, 18);
            var ticker = GetValidTicker();
            var price = GetValidPrice(transactionType);
            var shares = GetValidShares();
            var commission = GetInvalidCommission(transactionType);

            _transactionFactory.ConstructShareTransaction(transactionType, ticker, settlementDate, shares, price, commission);
        }

        protected static void ShareTransactionTotalValueTest(OrderType transactionType)
        {
            var settlementDate = new DateTime(2012, 1, 18);
            var ticker = GetValidTicker();
            var price = GetValidPrice(transactionType);
            var shares = GetValidShares();
            var commission = GetValidCommission(transactionType);

            var target = _transactionFactory.ConstructShareTransaction(transactionType, ticker, settlementDate, shares, price, commission);

            var expected = Math.Round(price*shares, 2) + commission;
            var actual = target.TotalValue;
            Assert.AreEqual(expected, actual);
        }

        private static string GetValidTicker()
        {
            return "DE";
        }

        private static decimal GetValidPrice(OrderType transactionType)
        {
            switch (transactionType)
            {
                case OrderType.DividendReinvestment:
                case OrderType.Buy:
                case OrderType.SellShort:
                    return 100.00m;
                case OrderType.BuyToCover:
                case OrderType.Sell:
                    return -100.00m;
                default:
                    throw new ArgumentOutOfRangeException("transactionType", transactionType, string.Format(@"Unknown OrderType: {0}.", transactionType));
            }
        }

        private static decimal GetInvalidPrice(OrderType transactionType)
        {
            return -GetValidPrice(transactionType);
        }

        private static decimal GetValidShares()
        {
            return 50m;
        }

        private static decimal GetInvalidShares()
        {
            return -GetValidShares();
        }

        private static decimal GetValidCommission(OrderType transactionType)
        {
            switch (transactionType)
            {
                case OrderType.Buy:
                case OrderType.BuyToCover:
                case OrderType.Sell:
                case OrderType.SellShort:
                    return 7.95m;
                case OrderType.DividendReinvestment:
                    return 0.00m;
                default:
                    throw new ArgumentOutOfRangeException("transactionType", transactionType, string.Format(@"Unknown OrderType: {0}.", transactionType));
            }
        }

        private static decimal GetInvalidCommission(OrderType transactionType)
        {
            return -GetValidCommission(transactionType);
        }

        protected static bool ShareTransactionInheritanceTest(IShareTransaction transaction, Type expected)
        {
            return transaction.GetType().GetInterfaces().Any(type => type == expected);
        }
    }
}