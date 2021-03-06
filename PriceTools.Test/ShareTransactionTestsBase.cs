using System;
using NUnit.Framework;
using Sonneville.PriceTools.Implementation;
using Sonneville.PriceTools.TestUtilities;
using Sonneville.Utilities.Serialization;

namespace Sonneville.PriceTools.Test
{
    public abstract class ShareTransactionTestsBase
    {
        protected static readonly ITransactionFactory TransactionFactory;

        static ShareTransactionTestsBase()
        {
            TransactionFactory = new TransactionFactory();
        }

        [Test]
        public abstract void SerializeTest();

        [Test]
        public abstract void SettlementDateTest();

        [Test]
        public abstract void TickerTest();

        [Test]
        public abstract void PriceValidTest();

        [Test]
        public abstract void PriceInvalidTest();

        [Test]
        public abstract void SharesValidTest();

        [Test]
        public abstract void SharesInvalidTest();

        [Test]
        public abstract void CommissionValidTest();

        [Test]
        public abstract void CommissionInvalidTest();

        [Test]
        public abstract void TotalValueTest();

        protected static void ShareTransactionSerializeTest(OrderType transactionType)
        {
            var settlementDate = new DateTime(2012, 1, 18);
            var ticker = GetValidTicker();
            var price = GetValidPrice(transactionType);
            var shares = GetValidShares();
            var commission = GetValidCommission(transactionType);

            var target = TransactionFactory.ConstructShareTransaction(transactionType, ticker, settlementDate, shares, price, commission);

            var xml = XmlSerializer.SerializeToXml(target);
            var result = XmlSerializer.DeserializeFromXml<ShareTransaction>(xml);

            GenericTestUtilities.AssertSameReflectedProperties(target, result);
        }

        protected static void ShareTransactionSettlementDateTest(OrderType transactionType)
        {
            var settlementDate = new DateTime(2012, 1, 18);
            var ticker = GetValidTicker();
            var price = GetValidPrice(transactionType);
            var shares = GetValidShares();
            var commission = GetValidCommission(transactionType);

            var target = TransactionFactory.ConstructShareTransaction(transactionType, ticker, settlementDate, shares, price, commission);

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

            var target = TransactionFactory.ConstructShareTransaction(transactionType, ticker, settlementDate, shares, price, commission);

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

            var target = TransactionFactory.ConstructShareTransaction(transactionType, ticker, settlementDate, shares, price, commission);

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

            var target = TransactionFactory.ConstructShareTransaction(transactionType, ticker, settlementDate, shares, price, commission);

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

            var target = TransactionFactory.ConstructShareTransaction(transactionType, ticker, settlementDate, shares, price, commission);

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

            TransactionFactory.ConstructShareTransaction(transactionType, ticker, settlementDate, shares, price, commission);
        }

        protected static void ShareTransactionCommissionValidTest(OrderType transactionType)
        {
            var settlementDate = new DateTime(2012, 1, 18);
            var ticker = GetValidTicker();
            var price = GetValidPrice(transactionType);
            var shares = GetValidShares();
            var commission = GetValidCommission(transactionType);

            var target = TransactionFactory.ConstructShareTransaction(transactionType, ticker, settlementDate, shares, price, commission);

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

            TransactionFactory.ConstructShareTransaction(transactionType, ticker, settlementDate, shares, price, commission);
        }

        protected static void ShareTransactionTotalValueTest(OrderType transactionType)
        {
            var settlementDate = new DateTime(2012, 1, 18);
            var ticker = GetValidTicker();
            var price = GetValidPrice(transactionType);
            var shares = GetValidShares();
            var commission = GetValidCommission(transactionType);

            var target = TransactionFactory.ConstructShareTransaction(transactionType, ticker, settlementDate, shares, price, commission);

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
    }
}