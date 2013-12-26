using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sonneville.PriceTools.AutomatedTrading.Test
{
    [TestClass]
    public class TradingAccountFeaturesTest
    {
        private static readonly ITradingAccountFeaturesFactory _tradingAccountFeaturesFactory;

        static TradingAccountFeaturesTest()
        {
            _tradingAccountFeaturesFactory = new TradingAccountFeaturesFactory();
        }

        [TestMethod]
        public void FactoryConstructorBasicSupportedOrderTypesTest()
        {
            var target = GetBasicTradingAccountFeatures();

            const OrderType expected = OrderType.Deposit | OrderType.Withdrawal | OrderType.Buy | OrderType.Sell;
            var actual = target.SupportedOrderTypes;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void BasicSupportsDepositTest()
        {
            var target = GetBasicTradingAccountFeatures();

            Assert.IsTrue(target.Supports(OrderType.Deposit));
        }

        [TestMethod]
        public void BasicSupportsWithdrawalTest()
        {
            var target = GetBasicTradingAccountFeatures();

            Assert.IsTrue(target.Supports(OrderType.Withdrawal));
        }

        [TestMethod]
        public void BasicSupportsBuyTest()
        {
            var target = GetBasicTradingAccountFeatures();

            Assert.IsTrue(target.Supports(OrderType.Buy));
        }

        [TestMethod]
        public void BasicSupportsSellTest()
        {
            var target = GetBasicTradingAccountFeatures();

            Assert.IsTrue(target.Supports(OrderType.Sell));
        }

        [TestMethod]
        public void BasicDoesNotSupportSellShortTest()
        {
            var target = GetBasicTradingAccountFeatures();

            Assert.IsFalse(target.Supports(OrderType.SellShort));
        }

        [TestMethod]
        public void BasicDoesNotSupportBuyToCoverTest()
        {
            var target = GetBasicTradingAccountFeatures();

            Assert.IsFalse(target.Supports(OrderType.BuyToCover));
        }

        [TestMethod]
        public void BasicDoesNotSupportMarginTest()
        {
            var target = GetBasicTradingAccountFeatures();

            Assert.IsFalse(target.IsMarginAccount);
        }

        [TestMethod]
        public void BasicRequiresFullLeverageTest()
        {
            var target = GetBasicTradingAccountFeatures();

            Assert.AreEqual(1, target.MarginSchedule.GetLeverageRequirement(String.Empty));
        }

        [TestMethod]
        public void FactoryConstructorShortSupportedOrderTypesTest()
        {
            var target = GetShortTradingAccountFeatures();

            const OrderType expected = OrderType.Deposit | OrderType.Withdrawal | OrderType.SellShort | OrderType.BuyToCover;
            var actual = target.SupportedOrderTypes;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ShortSupportsDepositTest()
        {
            var target = GetShortTradingAccountFeatures();

            Assert.IsTrue(target.Supports(OrderType.Deposit));
        }

        [TestMethod]
        public void ShortSupportsWithdrawalTest()
        {
            var target = GetShortTradingAccountFeatures();

            Assert.IsTrue(target.Supports(OrderType.Withdrawal));
        }

        [TestMethod]
        public void ShortDoesNotSupportBuyTest()
        {
            var target = GetShortTradingAccountFeatures();

            Assert.IsFalse(target.Supports(OrderType.Buy));
        }

        [TestMethod]
        public void ShortDoesNotSupportSellTest()
        {
            var target = GetShortTradingAccountFeatures();

            Assert.IsFalse(target.Supports(OrderType.Sell));
        }

        [TestMethod]
        public void ShortSupportsSellShortTest()
        {
            var target = GetShortTradingAccountFeatures();

            Assert.IsTrue(target.Supports(OrderType.SellShort));
        }

        [TestMethod]
        public void ShortSupportsBuyToCoverTest()
        {
            var target = GetShortTradingAccountFeatures();

            Assert.IsTrue(target.Supports(OrderType.BuyToCover));
        }

        [TestMethod]
        public void ShortDoesNotSupportMarginTest()
        {
            var target = GetShortTradingAccountFeatures();

            Assert.IsFalse(target.IsMarginAccount);
        }

        [TestMethod]
        public void FactoryConstructorFullSupportedOrderTypesTest()
        {
            var target = GetFullTradingAccountFeatures();

            const OrderType expected = OrderType.Deposit | OrderType.Withdrawal | OrderType.Buy | OrderType.Sell | OrderType.SellShort | OrderType.BuyToCover;
            var actual = target.SupportedOrderTypes;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void FullSupportsDepositTest()
        {
            var target = GetFullTradingAccountFeatures();

            Assert.IsTrue(target.Supports(OrderType.Deposit));
        }

        [TestMethod]
        public void FullSupportsWithdrawalTest()
        {
            var target = GetFullTradingAccountFeatures();

            Assert.IsTrue(target.Supports(OrderType.Withdrawal));
        }

        [TestMethod]
        public void FullSupportsBuyTest()
        {
            var target = GetFullTradingAccountFeatures();

            Assert.IsTrue(target.Supports(OrderType.Buy));
        }

        [TestMethod]
        public void FullSupportsSellTest()
        {
            var target = GetFullTradingAccountFeatures();

            Assert.IsTrue(target.Supports(OrderType.Sell));
        }

        [TestMethod]
        public void FullSupportsSellFullTest()
        {
            var target = GetFullTradingAccountFeatures();

            Assert.IsTrue(target.Supports(OrderType.SellShort));
        }

        [TestMethod]
        public void FullSupportsBuyToCoverTest()
        {
            var target = GetFullTradingAccountFeatures();

            Assert.IsTrue(target.Supports(OrderType.BuyToCover));
        }

        [TestMethod]
        public void FullDoesNotSupportMarginTest()
        {
            var target = GetFullTradingAccountFeatures();

            Assert.IsFalse(target.IsMarginAccount);
        }

        [TestMethod]
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
