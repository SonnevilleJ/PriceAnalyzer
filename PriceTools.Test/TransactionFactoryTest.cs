using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools.Data;
using Sonneville.Utilities;

namespace Sonneville.PriceTools.Test
{
    [TestClass]
    public class TransactionFactoryTest
    {
        public abstract class CashTransactionTestsBase
        {
            /// <summary>
            /// A test for serialization
            /// </summary>
            public abstract void SerializeTest();

            /// <summary>
            ///A test for Amount
            ///</summary>
            public abstract void AmountInvalidTest();

            /// <summary>
            ///A test for SettlementDate
            ///</summary>
            public abstract void SettlementDateTest();

            /// <summary>
            ///A test for Amount
            ///</summary>
            public abstract void AmountValidTest();

            protected static void CashTransactionSerializeTest(OrderType transactionType)
            {
                var date = GetSettlementDate();
                var amount = GetValidAmount(transactionType);

                var target = TransactionFactory.ConstructCashTransaction(transactionType, date, amount);

                var xml = Serializer.SerializeToXml(target);
                var result = Serializer.DeserializeFromXml<CashTransaction>(xml);

                TestUtilities.AssertSameState(target, result);
            }

            protected static void CashTransactionSettlementDateTest(OrderType transactionType)
            {
                var date = GetSettlementDate();
                var amount = GetValidAmount(transactionType);

                var target = TransactionFactory.ConstructCashTransaction(transactionType, date, amount);

                var expected = date;
                var actual = target.SettlementDate;
                Assert.AreEqual(expected, actual);
            }

            protected static void CashTransactionAmountInvalidTest(OrderType transactionType)
            {
                var date = GetSettlementDate();
                var amount = GetInversePrice(transactionType);

                var target = TransactionFactory.ConstructCashTransaction(transactionType, date, amount);

                var expected = GetValidAmount(transactionType);
                var actual = target.Amount;
                Assert.AreEqual(expected, actual);
            }

            protected static void CashTransactionAmountValidTest(OrderType transactionType)
            {
                var date = GetSettlementDate();
                var amount = GetValidAmount(transactionType);

                var target = TransactionFactory.ConstructCashTransaction(transactionType, date, amount);

                var expected = GetValidAmount(transactionType);
                var actual = target.Amount;
                Assert.AreEqual(expected, actual);
            }

            private static DateTime GetSettlementDate()
            {
                return new DateTime(2000, 1, 1);
            }

            private static decimal GetValidAmount(OrderType transactionType)
            {
                switch (transactionType)
                {
                    case OrderType.Deposit:
                    case OrderType.DividendReceipt:
                        return 2.00m;
                    case OrderType.Withdrawal:
                        return -2.00m;
                    default:
                        throw new ArgumentOutOfRangeException("transactionType", transactionType, @"Unknown OrderType.");
                }
            }

            private static decimal GetInversePrice(OrderType transactionType)
            {
                return -GetValidAmount(transactionType);
            }
        }

        [TestClass]
        public class DepositTests : CashTransactionTestsBase
        {
            private const OrderType TransactionType = OrderType.Deposit;

            [TestMethod]
            public override void SerializeTest()
            {
                CashTransactionSerializeTest(TransactionType);
            }

            /// <summary>
            ///A test for Amount
            ///</summary>
            [TestMethod]
            public override void AmountInvalidTest()
            {
                CashTransactionAmountInvalidTest(TransactionType);
            }

            /// <summary>
            ///A test for SettlementDate
            ///</summary>
            [TestMethod]
            public override void SettlementDateTest()
            {
                CashTransactionSettlementDateTest(TransactionType);
            }

            /// <summary>
            ///A test for Amount
            ///</summary>
            [TestMethod]
            public override void AmountValidTest()
            {
                CashTransactionAmountValidTest(TransactionType);
            }
        }

        [TestClass]
        public class WithdrawalTests : CashTransactionTestsBase
        {
            private const OrderType TransactionType = OrderType.Withdrawal;

            [TestMethod]
            public override void SerializeTest()
            {
                CashTransactionSerializeTest(TransactionType);
            }

            /// <summary>
            ///A test for Amount
            ///</summary>
            [TestMethod]
            public override void AmountInvalidTest()
            {
                CashTransactionAmountInvalidTest(TransactionType);
            }

            /// <summary>
            ///A test for SettlementDate
            ///</summary>
            [TestMethod]
            public override void SettlementDateTest()
            {
                CashTransactionSettlementDateTest(TransactionType);
            }

            /// <summary>
            ///A test for Amount
            ///</summary>
            [TestMethod]
            public override void AmountValidTest()
            {
                CashTransactionAmountValidTest(TransactionType);
            }
        }

        [TestClass]
        public class DividendReceiptTests : CashTransactionTestsBase
        {
            private const OrderType TransactionType = OrderType.DividendReceipt;

            [TestMethod]
            public override void SerializeTest()
            {
                CashTransactionSerializeTest(TransactionType);
            }

            /// <summary>
            ///A test for Amount
            ///</summary>
            [TestMethod]
            public override void AmountInvalidTest()
            {
                CashTransactionAmountInvalidTest(TransactionType);
            }

            /// <summary>
            ///A test for SettlementDate
            ///</summary>
            [TestMethod]
            public override void SettlementDateTest()
            {
                CashTransactionSettlementDateTest(TransactionType);
            }

            /// <summary>
            ///A test for Amount
            ///</summary>
            [TestMethod]
            public override void AmountValidTest()
            {
                CashTransactionAmountValidTest(TransactionType);
            }
        }

        public abstract class ShareTransactionTestsBase
        {
            /// <summary>
            /// A test for serialization
            /// </summary>
            public abstract void SerializeTest();

            /// <summary>
            /// A test for SettlementDate
            /// </summary>
            public abstract void SettlementDateTest();

            /// <summary>
            /// A test for Ticker
            /// </summary>
            public abstract void TickerTest();

            /// <summary>
            /// A test for Price
            /// </summary>
            public abstract void PriceValidTest();

            /// <summary>
            /// A test for Price
            /// </summary>
            public abstract void PriceInvalidTest();

            /// <summary>
            /// A test for Shares
            /// </summary>
            public abstract void SharesValidTest();

            /// <summary>
            /// A test for Shares
            /// </summary>
            public abstract void SharesInvalidTest();

            /// <summary>
            /// A test for Commission
            /// </summary>
            public abstract void CommissionValidTest();

            /// <summary>
            /// A test for Commission
            /// </summary>
            public abstract void CommissionInvalidTest();

            /// <summary>
            /// A test for TotalValue
            /// </summary>
            public abstract void TotalValueTest();

            /// <summary>
            /// A test for long/short transaction type
            /// </summary>
            public abstract void LongShortTest();

            /// <summary>
            /// A test for accumulation/distribution transaction type
            /// </summary>
            public abstract void AccumulationDistributionTest();

            /// <summary>
            /// A test for opening/closing transaction type
            /// </summary>
            public abstract void OpeningClosingTest();

            protected static void ShareTransactionSerializeTest(OrderType transactionType)
            {
                var settlementDate = new DateTime(2012, 1, 18);
                var ticker = GetValidTicker();
                var price = GetValidPrice(transactionType);
                var shares = GetValidShares();
                var commission = GetValidCommission(transactionType);

                var target = TransactionFactory.ConstructShareTransaction(transactionType, settlementDate, ticker, price, shares, commission);

                var xml = Serializer.SerializeToXml(target);
                var result = Serializer.DeserializeFromXml<ShareTransaction>(xml);

                TestUtilities.AssertSameState(target, result);
            }

            protected static void ShareTransactionSettlementDateTest(OrderType transactionType)
            {
                var settlementDate = new DateTime(2012, 1, 18);
                var ticker = GetValidTicker();
                var price = GetValidPrice(transactionType);
                var shares = GetValidShares();
                var commission = GetValidCommission(transactionType);

                var target = TransactionFactory.ConstructShareTransaction(transactionType, settlementDate, ticker, price, shares, commission);

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

                var target = TransactionFactory.ConstructShareTransaction(transactionType, settlementDate, ticker, price, shares, commission);

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

                var target = TransactionFactory.ConstructShareTransaction(transactionType, settlementDate, ticker, price, shares, commission);

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

                var target = TransactionFactory.ConstructShareTransaction(transactionType, settlementDate, ticker, price, shares, commission);

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

                var target = TransactionFactory.ConstructShareTransaction(transactionType, settlementDate, ticker, price, shares, commission);

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

                TransactionFactory.ConstructShareTransaction(transactionType, settlementDate, ticker, price, shares, commission);
            }

            protected static void ShareTransactionCommissionValidTest(OrderType transactionType)
            {
                var settlementDate = new DateTime(2012, 1, 18);
                var ticker = GetValidTicker();
                var price = GetValidPrice(transactionType);
                var shares = GetValidShares();
                var commission = GetValidCommission(transactionType);

                var target = TransactionFactory.ConstructShareTransaction(transactionType, settlementDate, ticker, price, shares, commission);

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

                TransactionFactory.ConstructShareTransaction(transactionType, settlementDate, ticker, price, shares, commission);
            }

            protected static void ShareTransactionTotalValueTest(OrderType transactionType)
            {
                var settlementDate = new DateTime(2012, 1, 18);
                var ticker = GetValidTicker();
                var price = GetValidPrice(transactionType);
                var shares = GetValidShares();
                var commission = GetValidCommission(transactionType);

                var target = TransactionFactory.ConstructShareTransaction(transactionType, settlementDate, ticker, price, shares, commission);

                var expected = Math.Round(price * (decimal) shares, 2) + commission;
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
                        throw new ArgumentOutOfRangeException("transactionType", transactionType, @"Unknown OrderType.");
                }
            }

            private static decimal GetInvalidPrice(OrderType transactionType)
            {
                return -GetValidPrice(transactionType);
            }

            private static double GetValidShares()
            {
                return 50.00;
            }

            private static double GetInvalidShares()
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
                        throw new ArgumentOutOfRangeException("transactionType", transactionType, @"Unknown OrderType.");
                }
            }

            private static decimal GetInvalidCommission(OrderType transactionType)
            {
                return -GetValidCommission(transactionType);
            }

            protected static bool ShareTransactionInheritanceTest(ShareTransaction transaction, Type expected)
            {
                return transaction.GetType().GetInterfaces().Any(type => type == expected);
            }
        }

        [TestClass]
        public class DividendReinvestmentTests : ShareTransactionTestsBase
        {
            private const OrderType TransactionType = OrderType.DividendReinvestment;

            [TestMethod]
            public override void SerializeTest()
            {
                ShareTransactionSerializeTest(TransactionType);
            }

            /// <summary>
            ///A test for SettlementDate
            ///</summary>
            [TestMethod]
            public override void SettlementDateTest()
            {
                ShareTransactionSettlementDateTest(TransactionType);
            }

            /// <summary>
            ///A test for Ticker
            ///</summary>
            [TestMethod]
            public override void TickerTest()
            {
                ShareTransactionTickerTest(TransactionType);
            }

            /// <summary>
            ///A test for Price
            ///</summary>
            [TestMethod]
            public override void PriceValidTest()
            {
                ShareTransactionPriceValidTest(TransactionType);
            }

            /// <summary>
            ///A test for Price
            ///</summary>
            [TestMethod]
            public override void PriceInvalidTest()
            {
                ShareTransactionPriceInvalidTest(TransactionType);
            }

            /// <summary>
            ///A test for Shares
            ///</summary>
            [TestMethod]
            public override void SharesValidTest()
            {
                ShareTransactionSharesValidTest(TransactionType);
            }

            /// <summary>
            ///A test for Shares
            ///</summary>
            [TestMethod]
            [ExpectedException(typeof(ArgumentOutOfRangeException))]
            public override void SharesInvalidTest()
            {
                ShareTransactionSharesInvalidTest(TransactionType);
            }

            /// <summary>
            ///A test for Commission
            ///</summary>
            [TestMethod]
            public override void CommissionValidTest()
            {
                ShareTransactionCommissionValidTest(TransactionType);
            }

            /// <summary>
            ///A test for Commission
            ///</summary>
            [TestMethod]
            public override void CommissionInvalidTest()
            {
                // For DividendReinvestment types, the only valid commission is zero.
            }

            /// <summary>
            /// A test for TotalValue
            /// </summary>
            [TestMethod]
            public override void TotalValueTest()
            {
                ShareTransactionTotalValueTest(TransactionType);
            }

            /// <summary>
            /// A test for long/short transaction type
            /// </summary>
            [TestMethod]
            public override void LongShortTest()
            {
                var transaction = TransactionFactory.ConstructShareTransaction(TransactionType, new DateTime(2012, 2, 6), "DE", 100.00m, 5.0, 0.00m);
                Assert.IsTrue(ShareTransactionInheritanceTest(transaction, typeof(LongTransaction)));
            }

            /// <summary>
            /// A test for accumulation/distribution transaction type
            /// </summary>
            [TestMethod]
            public override void AccumulationDistributionTest()
            {
                var transaction = TransactionFactory.ConstructShareTransaction(TransactionType, new DateTime(2012, 2, 6), "DE", 100.00m, 5.0, 0.00m);
                Assert.IsTrue(ShareTransactionInheritanceTest(transaction, typeof(AccumulationTransaction)));
            }

            /// <summary>
            /// A test for opening/closing transaction type
            /// </summary>
            [TestMethod]
            public override void OpeningClosingTest()
            {
                var transaction = TransactionFactory.ConstructShareTransaction(TransactionType, new DateTime(2012, 2, 6), "DE", 100.00m, 5.0, 0.00m);
                Assert.IsTrue(ShareTransactionInheritanceTest(transaction, typeof(OpeningTransaction)));
            }
        }

        [TestClass]
        public class BuyTests : ShareTransactionTestsBase
        {
            private const OrderType TransactionType = OrderType.Buy;

            [TestMethod]
            public override void SerializeTest()
            {
                ShareTransactionSerializeTest(TransactionType);
            }

            /// <summary>
            ///A test for SettlementDate
            ///</summary>
            [TestMethod]
            public override void SettlementDateTest()
            {
                ShareTransactionSettlementDateTest(TransactionType);
            }

            /// <summary>
            ///A test for Ticker
            ///</summary>
            [TestMethod]
            public override void TickerTest()
            {
                ShareTransactionTickerTest(TransactionType);
            }

            /// <summary>
            ///A test for Price
            ///</summary>
            [TestMethod]
            public override void PriceValidTest()
            {
                ShareTransactionPriceValidTest(TransactionType);
            }

            /// <summary>
            ///A test for Price
            ///</summary>
            [TestMethod]
            public override void PriceInvalidTest()
            {
                ShareTransactionPriceInvalidTest(TransactionType);
            }

            /// <summary>
            ///A test for Shares
            ///</summary>
            [TestMethod]
            public override void SharesValidTest()
            {
                ShareTransactionSharesValidTest(TransactionType);
            }

            /// <summary>
            ///A test for Shares
            ///</summary>
            [TestMethod]
            [ExpectedException(typeof(ArgumentOutOfRangeException))]
            public override void SharesInvalidTest()
            {
                ShareTransactionSharesInvalidTest(TransactionType);
            }

            /// <summary>
            ///A test for Commission
            ///</summary>
            [TestMethod]
            public override void CommissionValidTest()
            {
                ShareTransactionCommissionValidTest(TransactionType);
            }

            /// <summary>
            ///A test for Commission
            ///</summary>
            [TestMethod]
            [ExpectedException(typeof(ArgumentOutOfRangeException))]
            public override void CommissionInvalidTest()
            {
                ShareTransactionCommissionInvalidTest(TransactionType);
            }

            /// <summary>
            /// A test for TotalValue
            /// </summary>
            [TestMethod]
            public override void TotalValueTest()
            {
                ShareTransactionTotalValueTest(TransactionType);
            }

            /// <summary>
            /// A test for long/short transaction type
            /// </summary>
            [TestMethod]
            public override void LongShortTest()
            {
                var transaction = TransactionFactory.ConstructShareTransaction(TransactionType, new DateTime(2012, 2, 6), "DE", 100.00m, 5.0, 0.00m);
                Assert.IsTrue(ShareTransactionInheritanceTest(transaction, typeof(LongTransaction)));
            }

            /// <summary>
            /// A test for accumulation/distribution transaction type
            /// </summary>
            [TestMethod]
            public override void AccumulationDistributionTest()
            {
                var transaction = TransactionFactory.ConstructShareTransaction(TransactionType, new DateTime(2012, 2, 6), "DE", 100.00m, 5.0, 0.00m);
                Assert.IsTrue(ShareTransactionInheritanceTest(transaction, typeof(AccumulationTransaction)));
            }

            /// <summary>
            /// A test for opening/closing transaction type
            /// </summary>
            [TestMethod]
            public override void OpeningClosingTest()
            {
                var transaction = TransactionFactory.ConstructShareTransaction(TransactionType, new DateTime(2012, 2, 6), "DE", 100.00m, 5.0, 0.00m);
                Assert.IsTrue(ShareTransactionInheritanceTest(transaction, typeof(OpeningTransaction)));
            }
        }

        [TestClass]
        public class BuyToCoverTests : ShareTransactionTestsBase
        {
            private const OrderType TransactionType = OrderType.BuyToCover;

            [TestMethod]
            public override void SerializeTest()
            {
                ShareTransactionSerializeTest(TransactionType);
            }

            /// <summary>
            ///A test for SettlementDate
            ///</summary>
            [TestMethod]
            public override void SettlementDateTest()
            {
                ShareTransactionSettlementDateTest(TransactionType);
            }

            /// <summary>
            ///A test for Ticker
            ///</summary>
            [TestMethod]
            public override void TickerTest()
            {
                ShareTransactionTickerTest(TransactionType);
            }

            /// <summary>
            ///A test for Price
            ///</summary>
            [TestMethod]
            public override void PriceValidTest()
            {
                ShareTransactionPriceValidTest(TransactionType);
            }

            /// <summary>
            ///A test for Price
            ///</summary>
            [TestMethod]
            public override void PriceInvalidTest()
            {
                ShareTransactionPriceInvalidTest(TransactionType);
            }

            /// <summary>
            ///A test for Shares
            ///</summary>
            [TestMethod]
            public override void SharesValidTest()
            {
                ShareTransactionSharesValidTest(TransactionType);
            }

            /// <summary>
            ///A test for Shares
            ///</summary>
            [TestMethod]
            [ExpectedException(typeof(ArgumentOutOfRangeException))]
            public override void SharesInvalidTest()
            {
                ShareTransactionSharesInvalidTest(TransactionType);
            }

            /// <summary>
            ///A test for Commission
            ///</summary>
            [TestMethod]
            public override void CommissionValidTest()
            {
                ShareTransactionCommissionValidTest(TransactionType);
            }

            /// <summary>
            ///A test for Commission
            ///</summary>
            [TestMethod]
            [ExpectedException(typeof(ArgumentOutOfRangeException))]
            public override void CommissionInvalidTest()
            {
                ShareTransactionCommissionInvalidTest(TransactionType);
            }

            /// <summary>
            /// A test for TotalValue
            /// </summary>
            [TestMethod]
            public override void TotalValueTest()
            {
                ShareTransactionTotalValueTest(TransactionType);
            }

            /// <summary>
            /// A test for long/short transaction type
            /// </summary>
            [TestMethod]
            public override void LongShortTest()
            {
                var transaction = TransactionFactory.ConstructShareTransaction(TransactionType, new DateTime(2012, 2, 6), "DE", 100.00m, 5.0, 0.00m);
                Assert.IsTrue(ShareTransactionInheritanceTest(transaction, typeof(ShortTransaction)));
            }

            /// <summary>
            /// A test for accumulation/distribution transaction type
            /// </summary>
            [TestMethod]
            public override void AccumulationDistributionTest()
            {
                var transaction = TransactionFactory.ConstructShareTransaction(TransactionType, new DateTime(2012, 2, 6), "DE", 100.00m, 5.0, 0.00m);
                Assert.IsTrue(ShareTransactionInheritanceTest(transaction, typeof(AccumulationTransaction)));
            }

            /// <summary>
            /// A test for opening/closing transaction type
            /// </summary>
            [TestMethod]
            public override void OpeningClosingTest()
            {
                var transaction = TransactionFactory.ConstructShareTransaction(TransactionType, new DateTime(2012, 2, 6), "DE", 100.00m, 5.0, 0.00m);
                Assert.IsTrue(ShareTransactionInheritanceTest(transaction, typeof(ClosingTransaction)));
            }
        }

        [TestClass]
        public class SellTests : ShareTransactionTestsBase
        {
            private const OrderType TransactionType = OrderType.Sell;

            [TestMethod]
            public override void SerializeTest()
            {
                ShareTransactionSerializeTest(TransactionType);
            }

            /// <summary>
            ///A test for SettlementDate
            ///</summary>
            [TestMethod]
            public override void SettlementDateTest()
            {
                ShareTransactionSettlementDateTest(TransactionType);
            }

            /// <summary>
            ///A test for Ticker
            ///</summary>
            [TestMethod]
            public override void TickerTest()
            {
                ShareTransactionTickerTest(TransactionType);
            }

            /// <summary>
            ///A test for Price
            ///</summary>
            [TestMethod]
            public override void PriceValidTest()
            {
                ShareTransactionPriceValidTest(TransactionType);
            }

            /// <summary>
            ///A test for Price
            ///</summary>
            [TestMethod]
            public override void PriceInvalidTest()
            {
                ShareTransactionPriceInvalidTest(TransactionType);
            }

            /// <summary>
            ///A test for Shares
            ///</summary>
            [TestMethod]
            public override void SharesValidTest()
            {
                ShareTransactionSharesValidTest(TransactionType);
            }

            /// <summary>
            ///A test for Shares
            ///</summary>
            [TestMethod]
            [ExpectedException(typeof(ArgumentOutOfRangeException))]
            public override void SharesInvalidTest()
            {
                ShareTransactionSharesInvalidTest(TransactionType);
            }

            /// <summary>
            ///A test for Commission
            ///</summary>
            [TestMethod]
            public override void CommissionValidTest()
            {
                ShareTransactionCommissionValidTest(TransactionType);
            }

            /// <summary>
            ///A test for Commission
            ///</summary>
            [TestMethod]
            [ExpectedException(typeof(ArgumentOutOfRangeException))]
            public override void CommissionInvalidTest()
            {
                ShareTransactionCommissionInvalidTest(TransactionType);
            }

            /// <summary>
            /// A test for TotalValue
            /// </summary>
            [TestMethod]
            public override void TotalValueTest()
            {
                ShareTransactionTotalValueTest(TransactionType);
            }

            /// <summary>
            /// A test for long/short transaction type
            /// </summary>
            [TestMethod]
            public override void LongShortTest()
            {
                var transaction = TransactionFactory.ConstructShareTransaction(TransactionType, new DateTime(2012, 2, 6), "DE", 100.00m, 5.0, 0.00m);
                Assert.IsTrue(ShareTransactionInheritanceTest(transaction, typeof(LongTransaction)));
            }

            /// <summary>
            /// A test for accumulation/distribution transaction type
            /// </summary>
            [TestMethod]
            public override void AccumulationDistributionTest()
            {
                var transaction = TransactionFactory.ConstructShareTransaction(TransactionType, new DateTime(2012, 2, 6), "DE", 100.00m, 5.0, 0.00m);
                Assert.IsTrue(ShareTransactionInheritanceTest(transaction, typeof(DistributionTransaction)));
            }

            /// <summary>
            /// A test for opening/closing transaction type
            /// </summary>
            [TestMethod]
            public override void OpeningClosingTest()
            {
                var transaction = TransactionFactory.ConstructShareTransaction(TransactionType, new DateTime(2012, 2, 6), "DE", 100.00m, 5.0, 0.00m);
                Assert.IsTrue(ShareTransactionInheritanceTest(transaction, typeof(ClosingTransaction)));
            }
        }

        [TestClass]
        public class SellShortTests : ShareTransactionTestsBase
        {
            private const OrderType TransactionType = OrderType.SellShort;

            [TestMethod]
            public override void SerializeTest()
            {
                ShareTransactionSerializeTest(TransactionType);
            }

            /// <summary>
            ///A test for SettlementDate
            ///</summary>
            [TestMethod]
            public override void SettlementDateTest()
            {
                ShareTransactionSettlementDateTest(TransactionType);
            }

            /// <summary>
            ///A test for Ticker
            ///</summary>
            [TestMethod]
            public override void TickerTest()
            {
                ShareTransactionTickerTest(TransactionType);
            }

            /// <summary>
            ///A test for Price
            ///</summary>
            [TestMethod]
            public override void PriceValidTest()
            {
                ShareTransactionPriceValidTest(TransactionType);
            }

            /// <summary>
            ///A test for Price
            ///</summary>
            [TestMethod]
            public override void PriceInvalidTest()
            {
                ShareTransactionPriceInvalidTest(TransactionType);
            }

            /// <summary>
            ///A test for Shares
            ///</summary>
            [TestMethod]
            public override void SharesValidTest()
            {
                ShareTransactionSharesValidTest(TransactionType);
            }

            /// <summary>
            ///A test for Shares
            ///</summary>
            [TestMethod]
            [ExpectedException(typeof(ArgumentOutOfRangeException))]
            public override void SharesInvalidTest()
            {
                ShareTransactionSharesInvalidTest(TransactionType);
            }

            /// <summary>
            ///A test for Commission
            ///</summary>
            [TestMethod]
            public override void CommissionValidTest()
            {
                ShareTransactionCommissionValidTest(TransactionType);
            }

            /// <summary>
            ///A test for Commission
            ///</summary>
            [TestMethod]
            [ExpectedException(typeof(ArgumentOutOfRangeException))]
            public override void CommissionInvalidTest()
            {
                ShareTransactionCommissionInvalidTest(TransactionType);
            }

            /// <summary>
            /// A test for TotalValue
            /// </summary>
            [TestMethod]
            public override void TotalValueTest()
            {
                ShareTransactionTotalValueTest(TransactionType);
            }

            /// <summary>
            /// A test for long/short transaction type
            /// </summary>
            [TestMethod]
            public override void LongShortTest()
            {
                var transaction = TransactionFactory.ConstructShareTransaction(TransactionType, new DateTime(2012, 2, 6), "DE", 100.00m, 5.0, 0.00m);
                Assert.IsTrue(ShareTransactionInheritanceTest(transaction, typeof(ShortTransaction)));
            }

            /// <summary>
            /// A test for accumulation/distribution transaction type
            /// </summary>
            [TestMethod]
            public override void AccumulationDistributionTest()
            {
                var transaction = TransactionFactory.ConstructShareTransaction(TransactionType, new DateTime(2012, 2, 6), "DE", 100.00m, 5.0, 0.00m);
                Assert.IsTrue(ShareTransactionInheritanceTest(transaction, typeof(DistributionTransaction)));
            }

            /// <summary>
            /// A test for opening/closing transaction type
            /// </summary>
            [TestMethod]
            public override void OpeningClosingTest()
            {
                var transaction = TransactionFactory.ConstructShareTransaction(TransactionType, new DateTime(2012, 2, 6), "DE", 100.00m, 5.0, 0.00m);
                Assert.IsTrue(ShareTransactionInheritanceTest(transaction, typeof(OpeningTransaction)));
            }
        }
    }
}
