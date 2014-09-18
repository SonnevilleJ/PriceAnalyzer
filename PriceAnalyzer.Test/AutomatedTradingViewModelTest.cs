using System;
using Moq;
using NUnit.Framework;
using Sonneville.PriceTools.AutomatedTrading;
using Sonneville.PriceTools.AutomatedTrading.Implementation;

namespace Sonneville.PriceTools.PriceAnalyzer.Test
{
    [TestFixture]
    public class AutomatedTradingViewModelTest
    {
        private Mock<ITradingProcess> _tradingProcessMock;
        private AutomatedTradingViewModel _viewModel;
        private Mock<IPortfolio> _portfolioMock;
        private DateTime _startDate;
        private DateTime _endDate;

        [SetUp]
        public void Setup()
        {
            _startDate = new DateTime(2014, 1, 1);
            _endDate = new DateTime(2014, 1, 30);

            _portfolioMock = new Mock<IPortfolio>();
            _tradingProcessMock = new Mock<ITradingProcess>();
            _viewModel = new AutomatedTradingViewModel(_tradingProcessMock.Object);
        }

        [Test]
        public void ShouldCallTradingProcessForEachPeriodClose()
        {
            _viewModel.Run(_portfolioMock.Object, _startDate, _endDate);

            for (var i = _startDate; i < _endDate; i=i.NextTradingPeriodClose(Resolution.Days))
            {
                _tradingProcessMock.Verify(tradingProcess => tradingProcess.Execute(_portfolioMock.Object, i));
            }
            _tradingProcessMock.Verify(tradingProcess => tradingProcess.Execute(_portfolioMock.Object, It.Is<DateTime>(dateTime => dateTime < _startDate)), Times.Never());
            _tradingProcessMock.Verify(tradingProcess => tradingProcess.Execute(_portfolioMock.Object, It.Is<DateTime>(dateTime => dateTime > _endDate)), Times.Never());
        }

        [Test]
        public void CancelShouldPreventTradingProcessFromBeingCalled()
        {
            _viewModel.Cancel();
            _viewModel.Run(_portfolioMock.Object, _startDate, _endDate);

            _tradingProcessMock.Verify(tradingProcess => tradingProcess.Execute(_portfolioMock.Object, It.IsAny<DateTime>()), Times.Never());
        }

        [Test]
        public void ShouldNotCallTradingProcessAfterCancelIsClicked()
        {
            _tradingProcessMock
                .Setup(tradingProcess => tradingProcess.Execute(_portfolioMock.Object, _startDate))
                .Callback(() => _viewModel.Cancel());

            _viewModel.Run(_portfolioMock.Object, _startDate, _endDate);

            _tradingProcessMock.Verify(tradingProcess => tradingProcess.Execute(_portfolioMock.Object, _startDate), Times.Once());
            _tradingProcessMock.Verify(tradingProcess => tradingProcess.Execute(_portfolioMock.Object, It.Is<DateTime>(dateTime => dateTime < _startDate)), Times.Never());
            _tradingProcessMock.Verify(tradingProcess => tradingProcess.Execute(_portfolioMock.Object, It.Is<DateTime>(dateTime => dateTime > _startDate)), Times.Never());
        }
    }
}