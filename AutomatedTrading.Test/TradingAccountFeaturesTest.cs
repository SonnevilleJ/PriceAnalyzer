using System;
using NUnit.Framework;

namespace Sonneville.PriceTools.AutomatedTrading.Test
{
    [TestFixture]
    public class TradingAccountFeaturesTest
    {
        private static readonly ITradingAccountFeaturesFactory _tradingAccountFeaturesFactory;

        static TradingAccountFeaturesTest()
        {
            _tradingAccountFeaturesFactory = new TradingAccountFeaturesFactory();
        }

        [Test]
        public void FactoryConstructorBasicSupportedOrderTypesTest()
        {
            var target = GetBasicTradingAccountFeatures();

            const OrderType expected = OrderType.Deposit | OrderType.Withdrawal | OrderType.Buy | OrderType.Sell;
            var actual = target.SupportedOrderTypes;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void BasicSupportsDepositTest()
        {
            var target = GetBasicTradingAccountFeatures();

            Assert.IsTrue(target.Supports(OrderType.Deposit));
        }

        [Test]
        public void BasicSupportsWithdrawalTest()
        {
            var target = GetBasicTradingAccountFeatures();

            Assert.IsTrue(target.Supports(OrderType.Withdrawal));
        }

        [Test]
        public void BasicSupportsBuyTest()
        {
            var target = GetBasicTradingAccountFeatures();

            Assert.IsTrue(target.Supports(OrderType.Buy));
        }

        [Test]
        public void BasicSupportsSellTest()
        {
            var target = GetBasicTradingAccountFeatures();

            Assert.IsTrue(target.Supports(OrderType.Sell));
        }

        [Test]
        public void BasicDoesNotSupportSellShortTest()
        {
            var target = GetBasicTradingAccountFeatures();

            Assert.IsFalse(target.Supports(OrderType.SellShort));
        }

        [Test]
        public void BasicDoesNotSupportBuyToCoverTest()
        {
            var target = GetBasicTradingAccountFeatures();

            Assert.IsFalse(target.Supports(OrderType.BuyToCover));
        }

        [Test]
        public void BasicDoesNotSupportMarginTest()
        {
            var target = GetBasicTradingAccountFeatures();

            Assert.IsFalse(target.IsMarginAccount);
        }

        [Test]
        public void BasicRequiresFullLeverageTest()
        {
            var target = GetBasicTradingAccountFeatures();

            Assert.AreEqual(1, target.MarginSchedule.GetLeverageRequirement(String.Empty));
        }

        [Test]
        public void FactoryConstructorShortSupportedOrderTypesTest()
        {
            var target = GetShortTradingAccountFeatures();

            const OrderType expected = OrderType.Deposit | OrderType.Withdrawal | OrderType.SellShort | OrderType.BuyToCover;
            var actual = target.SupportedOrderTypes;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ShortSupportsDepositTest()
        {
            var target = GetShortTradingAccountFeatures();

            Assert.IsTrue(target.Supports(OrderType.Deposit));
        }

        [Test]
        public void ShortSupportsWithdrawalTest()
        {
            var target = GetShortTradingAccountFeatures();

            Assert.IsTrue(target.Supports(OrderType.Withdrawal));
        }

        [Test]
        public void ShortDoesNotSupportBuyTest()
        {
            var target = GetShortTradingAccountFeatures();

            Assert.IsFalse(target.Supports(OrderType.Buy));
        }

        [Test]
        public void ShortDoesNotSupportSellTest()
        {
            var target = GetShortTradingAccountFeatures();

            Assert.IsFalse(target.Supports(OrderType.Sell));
        }

        [Test]
        public void ShortSupportsSellShortTest()
        {
            var target = GetShortTradingAccountFeatures();

            Assert.IsTrue(target.Supports(OrderType.SellShort));
        }

        [Test]
        public void ShortSupportsBuyToCoverTest()
        {
            var target = GetShortTradingAccountFeatures();

            Assert.IsTrue(target.Supports(OrderType.BuyToCover));
        }

        [Test]
        public void ShortDoesNotSupportMarginTest()
        {
            var target = GetShortTradingAccountFeatures();

            Assert.IsFalse(target.IsMarginAccount);
        }

        [Test]
        public void FactoryConstructorFullSupportedOrderTypesTest()
        {
            var target = GetFullTradingAccountFeatures();

            const OrderType expected = OrderType.Deposit | OrderType.Withdrawal | OrderType.Buy | OrderType.Sell | OrderType.SellShort | OrderType.BuyToCover;
            var actual = target.SupportedOrderTypes;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FullSupportsDepositTest()
        {
            var target = GetFullTradingAccountFeatures();

            Assert.IsTrue(target.Supports(OrderType.Deposit));
        }

        [Test]
        public void FullSupportsWithdrawalTest()
        {
            var target = GetFullTradingAccountFeatures();

            Assert.IsTrue(target.Supports(OrderType.Withdrawal));
        }

        [Test]
        public void FullSupportsBuyTest()
        {
            var target = GetFullTradingAccountFeatures();

            Assert.IsTrue(target.Supports(OrderType.Buy));
        }

        [Test]
        public void FullSupportsSellTest()
        {
            var target = GetFullTradingAccountFeatures();

            Assert.IsTrue(target.Supports(OrderType.Sell));
        }

        [Test]
        public void FullSupportsSellFullTest()
        {
            var target = GetFullTradingAccountFeatures();

            Assert.IsTrue(target.Supports(OrderType.SellShort));
        }

        [Test]
        public void FullSupportsBuyToCoverTest()
        {
            var target = GetFullTradingAccountFeatures();

            Assert.IsTrue(target.Supports(OrderType.BuyToCover));
        }

        [Test]
        public void FullDoesNotSupportMarginTest()
        {
            var target = GetFullTradingAccountFeatures();

            Assert.IsFalse(target.IsMarginAccount);
        }

        [Test]
        public void FactoryConstructorCustomSupportedOrderTypesTest()
        {
            // the ultimate suck account - you can deposit money and trade, but never withdraw!
            const OrderType orderTypes = OrderType.Deposit | OrderType.Buy | OrderType.Sell | OrderType.SellShort | OrderType.BuyToCover;

            var target = _tradingAccountFeaturesFactory.ConstructTradingAccountFeatures(orderTypes);

            var actual = target.SupportedOrderTypes;
            Assert.AreEqual(orderTypes, actual);
        }

        private static TradingAccountFeatures GetBasicTradingAccountFeatures()
        {
            return _tradingAccountFeaturesFactory.ConstructBasicTradingAccountFeatures();
        }

        private static TradingAccountFeatures GetShortTradingAccountFeatures()
        {
            return _tradingAccountFeaturesFactory.ConstructShortTradingAccountFeatures();
        }

        private static TradingAccountFeatures GetFullTradingAccountFeatures()
        {
            return _tradingAccountFeaturesFactory.ConstructFullTradingAccountFeatures();
        }
    }
}
