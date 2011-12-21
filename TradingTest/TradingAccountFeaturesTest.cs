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
            var target = TradingAccountFeatures.Factory.CreateBasicTradingAccountFeatures();

            const OrderType expected = OrderType.Deposit | OrderType.Withdrawal | OrderType.Buy | OrderType.Sell;
            var actual = target.SupportedOrderTypes;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void BasicSupportsDepositTest()
        {
            var target = TradingAccountFeatures.Factory.CreateBasicTradingAccountFeatures();

            Assert.IsTrue(target.Supports(OrderType.Deposit));
        }

        [TestMethod]
        public void BasicSupportsWithdrawalTest()
        {
            var target = TradingAccountFeatures.Factory.CreateBasicTradingAccountFeatures();

            Assert.IsTrue(target.Supports(OrderType.Withdrawal));
        }

        [TestMethod]
        public void BasicSupportsBuyTest()
        {
            var target = TradingAccountFeatures.Factory.CreateBasicTradingAccountFeatures();

            Assert.IsTrue(target.Supports(OrderType.Buy));
        }

        [TestMethod]
        public void BasicSupportsSellTest()
        {
            var target = TradingAccountFeatures.Factory.CreateBasicTradingAccountFeatures();

            Assert.IsTrue(target.Supports(OrderType.Sell));
        }

        [TestMethod]
        public void BasicDoesNotSupportSellShortTest()
        {
            var target = TradingAccountFeatures.Factory.CreateBasicTradingAccountFeatures();

            Assert.IsFalse(target.Supports(OrderType.SellShort));
        }

        [TestMethod]
        public void BasicDoesNotSupportBuyToCoverTest()
        {
            var target = TradingAccountFeatures.Factory.CreateBasicTradingAccountFeatures();

            Assert.IsFalse(target.Supports(OrderType.BuyToCover));
        }

        [TestMethod]
        public void FactoryConstructorShortSupportedOrderTypesTest()
        {
            var target = TradingAccountFeatures.Factory.CreateShortTradingAccountFeatures();

            const OrderType expected = OrderType.Deposit | OrderType.Withdrawal | OrderType.SellShort | OrderType.BuyToCover;
            var actual = target.SupportedOrderTypes;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ShortSupportsDepositTest()
        {
            var target = TradingAccountFeatures.Factory.CreateShortTradingAccountFeatures();

            Assert.IsTrue(target.Supports(OrderType.Deposit));
        }

        [TestMethod]
        public void ShortSupportsWithdrawalTest()
        {
            var target = TradingAccountFeatures.Factory.CreateShortTradingAccountFeatures();

            Assert.IsTrue(target.Supports(OrderType.Withdrawal));
        }

        [TestMethod]
        public void ShortDoesNotSupportBuyTest()
        {
            var target = TradingAccountFeatures.Factory.CreateShortTradingAccountFeatures();

            Assert.IsFalse(target.Supports(OrderType.Buy));
        }

        [TestMethod]
        public void ShortDoesNotSupportSellTest()
        {
            var target = TradingAccountFeatures.Factory.CreateShortTradingAccountFeatures();

            Assert.IsFalse(target.Supports(OrderType.Sell));
        }

        [TestMethod]
        public void ShortSupportsSellShortTest()
        {
            var target = TradingAccountFeatures.Factory.CreateShortTradingAccountFeatures();

            Assert.IsTrue(target.Supports(OrderType.SellShort));
        }

        [TestMethod]
        public void ShortSupportsBuyToCoverTest()
        {
            var target = TradingAccountFeatures.Factory.CreateShortTradingAccountFeatures();

            Assert.IsTrue(target.Supports(OrderType.BuyToCover));
        }

        [TestMethod]
        public void FactoryConstructorFullSupportedOrderTypesTest()
        {
            var target = TradingAccountFeatures.Factory.CreateFullTradingAccountFeatures();

            const OrderType expected = OrderType.Deposit | OrderType.Withdrawal | OrderType.Buy | OrderType.Sell | OrderType.SellShort | OrderType.BuyToCover;
            var actual = target.SupportedOrderTypes;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void FullSupportsDepositTest()
        {
            var target = TradingAccountFeatures.Factory.CreateFullTradingAccountFeatures();

            Assert.IsTrue(target.Supports(OrderType.Deposit));
        }

        [TestMethod]
        public void FullSupportsWithdrawalTest()
        {
            var target = TradingAccountFeatures.Factory.CreateFullTradingAccountFeatures();

            Assert.IsTrue(target.Supports(OrderType.Withdrawal));
        }

        [TestMethod]
        public void FullSupportsBuyTest()
        {
            var target = TradingAccountFeatures.Factory.CreateFullTradingAccountFeatures();

            Assert.IsTrue(target.Supports(OrderType.Buy));
        }

        [TestMethod]
        public void FullSupportsSellTest()
        {
            var target = TradingAccountFeatures.Factory.CreateFullTradingAccountFeatures();

            Assert.IsTrue(target.Supports(OrderType.Sell));
        }

        [TestMethod]
        public void FullSupportsSellFullTest()
        {
            var target = TradingAccountFeatures.Factory.CreateFullTradingAccountFeatures();

            Assert.IsTrue(target.Supports(OrderType.SellShort));
        }

        [TestMethod]
        public void FullSupportsBuyToCoverTest()
        {
            var target = TradingAccountFeatures.Factory.CreateFullTradingAccountFeatures();

            Assert.IsTrue(target.Supports(OrderType.BuyToCover));
        }

        [TestMethod]
        public void FactoryConstructorCustomSupportedOrderTypesTest()
        {
            // the ultimate suck account - you can deposit money and trade, but never withdraw!
            const OrderType orderTypes = OrderType.Deposit | OrderType.Buy | OrderType.Sell | OrderType.SellShort | OrderType.BuyToCover;
            
            var target = TradingAccountFeatures.Factory.CreateCustomTradingAccountFeatures(orderTypes);

            var actual = target.SupportedOrderTypes;
            Assert.AreEqual(orderTypes, actual);
        }
    }
}
