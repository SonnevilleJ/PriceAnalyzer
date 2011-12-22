using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.Trading;

namespace Sonneville.TradingTest
{
    [TestClass]
    public class TradingAccountFeaturesTest
    {
        [TestMethod]
        public void FactoryConstructorBasicSupportedOrderTypesTest()
        {
            var target = TradingAccountFeaturesFactory.CreateBasicTradingAccountFeatures();

            const OrderType expected = OrderType.Deposit | OrderType.Withdrawal | OrderType.Buy | OrderType.Sell;
            var actual = target.SupportedOrderTypes;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void BasicSupportsDepositTest()
        {
            var target = TradingAccountFeaturesFactory.CreateBasicTradingAccountFeatures();

            Assert.IsTrue(target.Supports(OrderType.Deposit));
        }

        [TestMethod]
        public void BasicSupportsWithdrawalTest()
        {
            var target = TradingAccountFeaturesFactory.CreateBasicTradingAccountFeatures();

            Assert.IsTrue(target.Supports(OrderType.Withdrawal));
        }

        [TestMethod]
        public void BasicSupportsBuyTest()
        {
            var target = TradingAccountFeaturesFactory.CreateBasicTradingAccountFeatures();

            Assert.IsTrue(target.Supports(OrderType.Buy));
        }

        [TestMethod]
        public void BasicSupportsSellTest()
        {
            var target = TradingAccountFeaturesFactory.CreateBasicTradingAccountFeatures();

            Assert.IsTrue(target.Supports(OrderType.Sell));
        }

        [TestMethod]
        public void BasicDoesNotSupportSellShortTest()
        {
            var target = TradingAccountFeaturesFactory.CreateBasicTradingAccountFeatures();

            Assert.IsFalse(target.Supports(OrderType.SellShort));
        }

        [TestMethod]
        public void BasicDoesNotSupportBuyToCoverTest()
        {
            var target = TradingAccountFeaturesFactory.CreateBasicTradingAccountFeatures();

            Assert.IsFalse(target.Supports(OrderType.BuyToCover));
        }

        [TestMethod]
        public void FactoryConstructorShortSupportedOrderTypesTest()
        {
            var target = TradingAccountFeaturesFactory.CreateShortTradingAccountFeatures();

            const OrderType expected = OrderType.Deposit | OrderType.Withdrawal | OrderType.SellShort | OrderType.BuyToCover;
            var actual = target.SupportedOrderTypes;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ShortSupportsDepositTest()
        {
            var target = TradingAccountFeaturesFactory.CreateShortTradingAccountFeatures();

            Assert.IsTrue(target.Supports(OrderType.Deposit));
        }

        [TestMethod]
        public void ShortSupportsWithdrawalTest()
        {
            var target = TradingAccountFeaturesFactory.CreateShortTradingAccountFeatures();

            Assert.IsTrue(target.Supports(OrderType.Withdrawal));
        }

        [TestMethod]
        public void ShortDoesNotSupportBuyTest()
        {
            var target = TradingAccountFeaturesFactory.CreateShortTradingAccountFeatures();

            Assert.IsFalse(target.Supports(OrderType.Buy));
        }

        [TestMethod]
        public void ShortDoesNotSupportSellTest()
        {
            var target = TradingAccountFeaturesFactory.CreateShortTradingAccountFeatures();

            Assert.IsFalse(target.Supports(OrderType.Sell));
        }

        [TestMethod]
        public void ShortSupportsSellShortTest()
        {
            var target = TradingAccountFeaturesFactory.CreateShortTradingAccountFeatures();

            Assert.IsTrue(target.Supports(OrderType.SellShort));
        }

        [TestMethod]
        public void ShortSupportsBuyToCoverTest()
        {
            var target = TradingAccountFeaturesFactory.CreateShortTradingAccountFeatures();

            Assert.IsTrue(target.Supports(OrderType.BuyToCover));
        }

        [TestMethod]
        public void FactoryConstructorFullSupportedOrderTypesTest()
        {
            var target = TradingAccountFeaturesFactory.CreateFullTradingAccountFeatures();

            const OrderType expected = OrderType.Deposit | OrderType.Withdrawal | OrderType.Buy | OrderType.Sell | OrderType.SellShort | OrderType.BuyToCover;
            var actual = target.SupportedOrderTypes;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void FullSupportsDepositTest()
        {
            var target = TradingAccountFeaturesFactory.CreateFullTradingAccountFeatures();

            Assert.IsTrue(target.Supports(OrderType.Deposit));
        }

        [TestMethod]
        public void FullSupportsWithdrawalTest()
        {
            var target = TradingAccountFeaturesFactory.CreateFullTradingAccountFeatures();

            Assert.IsTrue(target.Supports(OrderType.Withdrawal));
        }

        [TestMethod]
        public void FullSupportsBuyTest()
        {
            var target = TradingAccountFeaturesFactory.CreateFullTradingAccountFeatures();

            Assert.IsTrue(target.Supports(OrderType.Buy));
        }

        [TestMethod]
        public void FullSupportsSellTest()
        {
            var target = TradingAccountFeaturesFactory.CreateFullTradingAccountFeatures();

            Assert.IsTrue(target.Supports(OrderType.Sell));
        }

        [TestMethod]
        public void FullSupportsSellFullTest()
        {
            var target = TradingAccountFeaturesFactory.CreateFullTradingAccountFeatures();

            Assert.IsTrue(target.Supports(OrderType.SellShort));
        }

        [TestMethod]
        public void FullSupportsBuyToCoverTest()
        {
            var target = TradingAccountFeaturesFactory.CreateFullTradingAccountFeatures();

            Assert.IsTrue(target.Supports(OrderType.BuyToCover));
        }

        [TestMethod]
        public void FactoryConstructorCustomSupportedOrderTypesTest()
        {
            // the ultimate suck account - you can deposit money and trade, but never withdraw!
            const OrderType orderTypes = OrderType.Deposit | OrderType.Buy | OrderType.Sell | OrderType.SellShort | OrderType.BuyToCover;
            
            var target = TradingAccountFeaturesFactory.CreateCustomTradingAccountFeatures(orderTypes);

            var actual = target.SupportedOrderTypes;
            Assert.AreEqual(orderTypes, actual);
        }
    }
}
