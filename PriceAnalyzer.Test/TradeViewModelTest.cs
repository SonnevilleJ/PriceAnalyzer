using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using Sonneville.PriceTools.AutomatedTrading;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools.PriceAnalyzer.Test
{
    [TestFixture]
    public class TradeViewModelTest
    {
        private TradeViewModel _viewModel;
        private Mock<IBrokerage> _brokerageMock;
        private string _ticker;
        private int _volume;
        private decimal _sharePrice;
        private Expiration _expirationType;
        private OrderType _orderType;

        [SetUp]
        public void Setup()
        {
            _ticker = "DE";
            _volume = 5;
            _sharePrice = 5.00m;
            _expirationType = Expiration.NoExpiration;
            _orderType = OrderType.Buy;

            _brokerageMock = new Mock<IBrokerage>();
            _viewModel = new TradeViewModel(_brokerageMock.Object);
        }

        [Test]
        public void EmptyPriceStringShouldNotValidate()
        {
            var result = _viewModel.ValidatePrice("");

            Assert.IsFalse(result);
        }

        [Test]
        public void NonNumericPriceStringShouldNotValidate()
        {
            var result = _viewModel.ValidatePrice("five");

            Assert.IsFalse(result);
        }

        [Test]
        public void NumericPriceStringShouldValidate()
        {
            var result = _viewModel.ValidatePrice("5");

            Assert.IsTrue(result);
        }

        [Test]
        public void PriceOfZeroShouldNotValidate()
        {
            var result = _viewModel.ValidatePrice("0");

            Assert.IsFalse(result);
        }

        [Test]
        public void NegativePriceShouldNotValidate()
        {
            var result = _viewModel.ValidatePrice("-5");

            Assert.IsFalse(result);
        }

        [Test]
        public void EmptyVolumeStringShouldNotValidate()
        {
            var result = _viewModel.ValidateVolume("");

            Assert.IsFalse(result);
        }

        [Test]
        public void NonNumericVolumeStringShouldNotValidate()
        {
            var result = _viewModel.ValidateVolume("five");

            Assert.IsFalse(result);
        }

        [Test]
        public void NumericVolumeStringShouldValidate()
        {
            var result = _viewModel.ValidateVolume("5");

            Assert.IsTrue(result);
        }

        [Test]
        public void VolumeOfZeroShouldNotValidate()
        {
            var result = _viewModel.ValidateVolume("0");

            Assert.IsFalse(result);
        }

        [Test]
        public void NegativeVolumeShouldNotValidate()
        {
            var result = _viewModel.ValidateVolume("-5");

            Assert.IsFalse(result);
        }

        [Test]
        public void SubmitThrowsIfTickerIsNotSet()
        {
            CreateValidOrder(_viewModel);
            _viewModel.Ticker = "";

            var exceptionOccurred = false;
            try
            {
                _viewModel.Submit();
            }
            catch (ArgumentException e)
            {
                exceptionOccurred = true;
                Assert.AreEqual("Ticker", e.ParamName);
            }
            catch (Exception e)
            {
                Assert.Fail("Unexpected exception occurred: {0}", e);
            }

            Assert.IsTrue(exceptionOccurred);
        }

        [Test]
        public void SubmitThrowsIfVolumeIsNotSet()
        {
            CreateValidOrder(_viewModel);
            _viewModel.Volume = 0;

            var exceptionOccurred = false;
            try
            {
                _viewModel.Submit();
            }
            catch (ArgumentException e)
            {
                exceptionOccurred = true;
                Assert.AreEqual("Volume", e.ParamName);
            }
            catch (Exception e)
            {
                Assert.Fail("Unexpected exception occurred: {0}", e);
            }

            Assert.IsTrue(exceptionOccurred);
        }

        [Test]
        public void SubmitThrowsIfSharePriceIsNotSet()
        {
            CreateValidOrder(_viewModel);
            _viewModel.SharePrice = 0;

            var exceptionOccurred = false;
            try
            {
                _viewModel.Submit();
            }
            catch (ArgumentException e)
            {
                exceptionOccurred = true;
                Assert.AreEqual("SharePrice", e.ParamName);
            }
            catch (Exception e)
            {
                Assert.Fail("Unexpected exception occurred: {0}", e);
            }

            Assert.IsTrue(exceptionOccurred);
        }

        [Test]
        public void SubmitThrowsIfOrderTypeIsNotSet()
        {
            CreateValidOrder(_viewModel);
            _viewModel.OrderType = 0;

            var exceptionOccurred = false;
            try
            {
                _viewModel.Submit();
            }
            catch (ArgumentException e)
            {
                exceptionOccurred = true;
                Assert.AreEqual("OrderType", e.ParamName);
            }
            catch (Exception e)
            {
                Assert.Fail("Unexpected exception occurred: {0}", e);
            }

            Assert.IsTrue(exceptionOccurred);
        }

        [Test]
        public void SubmitValidOrderPassesToBrokerage()
        {
            CreateValidOrder(_viewModel);

            _viewModel.Submit();

            _brokerageMock.Verify(brokerage=>brokerage.SubmitOrders(It.Is<IEnumerable<Order>>(orders => ValidateOrders(orders))));
        }

        private bool ValidateOrders(IEnumerable<Order> orders)
        {
            var order = orders.Single();
            Assert.AreEqual(_ticker, order.Ticker);
            Assert.AreEqual(_volume, order.Shares);
            Assert.AreEqual(_sharePrice, order.Price);
            Assert.AreEqual(_orderType, order.OrderType);
            Assert.AreEqual(default(DateTime), order.Expiration);
            
            return true;
        }

        private void CreateValidOrder(TradeViewModel viewModel)
        {
            
            viewModel.Ticker = _ticker;
            viewModel.Volume = _volume;
            viewModel.SharePrice = _sharePrice;
            viewModel.ExpirationType = _expirationType;
            viewModel.OrderType = _orderType;
        }
    }
}
