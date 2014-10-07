using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using Sonneville.PriceTools.AutomatedTrading.Implementation;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools.AutomatedTrading.Test
{
    [TestFixture]
    public class TradingProcessTest
    {
        private TradingProcess _tradingProcess;
        private Mock<IPortfolio> _portfolioMock;
        private Mock<IAnalysisEngine> _analysisEngineMock;
        private IList<IPosition> _positions;
        private Mock<IPosition> _dePositionMock;
        private Mock<IPosition> _ibmPositionMock;
        private Mock<IPriceSeriesFactory> _priceSeriesProviderMock;
        private Mock<IPriceSeries> _dePriceSeries;
        private Mock<IPriceSeries> _ibmPriceSeries;
        private Mock<IBrokerage> _brokerageMock;
        private IList<Order> _openOrders;
        private IList<Order> _unsubmittedOrders;
        private DateTime _executionDate;
        private string _deTicker;
        private string _ibmTicker;
        private IList<IShareTransaction> _newDeTransactions;
        private IList<IShareTransaction> _newIbmTransactions;
        private Mock<IShareTransaction> _deTransactionMock;
        private Mock<IShareTransaction> _ibmTransactionMock;
        private Mock<IShareTransaction> _ibmTransactionMock2;
        private IEnumerable<string> _tickers;

        [SetUp]
        public void Setup()
        {
            _deTicker = "DE";
            _ibmTicker = "IBM";
            _executionDate = new DateTime(2014, 8, 27);
            _tickers = new[] {_deTicker, _ibmTicker};

            _dePositionMock = new Mock<IPosition>();
            _dePositionMock.Setup(position => position.Ticker).Returns(_deTicker);
            _dePositionMock.Setup(position => position.Tail).Returns(new DateTime(2014, 7, 5));

            _ibmPositionMock = new Mock<IPosition>();
            _ibmPositionMock.Setup(position => position.Ticker).Returns(_ibmTicker);
            _ibmPositionMock.Setup(position => position.Tail).Returns(new DateTime(2014, 7, 5));

            _positions = new List<IPosition> {_dePositionMock.Object, _ibmPositionMock.Object};

            _portfolioMock = new Mock<IPortfolio>();
            _portfolioMock.Setup(portfolio => portfolio.Positions).Returns(_positions);
            _portfolioMock.Setup(portfolio => portfolio.GetPosition(_deTicker)).Returns(_dePositionMock.Object);
            _portfolioMock.Setup(portfolio => portfolio.GetPosition(_ibmTicker)).Returns(_ibmPositionMock.Object);
            _portfolioMock.Setup(portfolio => portfolio.Tail).Returns(new DateTime(2014, 7, 5));

            _dePriceSeries = new Mock<IPriceSeries>();
            _ibmPriceSeries = new Mock<IPriceSeries>();

            _priceSeriesProviderMock = new Mock<IPriceSeriesFactory>();
            _priceSeriesProviderMock.Setup(provider => provider.ConstructPriceSeries(_deTicker)).Returns(_dePriceSeries.Object);
            _priceSeriesProviderMock.Setup(provider => provider.ConstructPriceSeries(_ibmTicker)).Returns(_ibmPriceSeries.Object);

            _openOrders = new List<Order> {new Order()};

            _deTransactionMock = new Mock<IShareTransaction>();
            _deTransactionMock.Setup(transaction => transaction.Ticker).Returns(_deTicker);

            _ibmTransactionMock = new Mock<IShareTransaction>();
            _ibmTransactionMock.Setup(transaction => transaction.Ticker).Returns(_ibmTicker);

            _ibmTransactionMock2 = new Mock<IShareTransaction>();
            _ibmTransactionMock2.Setup(transaction => transaction.Ticker).Returns(_ibmTicker);

            _newDeTransactions = new List<IShareTransaction>{_deTransactionMock.Object};
            _newIbmTransactions = new List<IShareTransaction> {_ibmTransactionMock.Object, _ibmTransactionMock2.Object};

            _brokerageMock = new Mock<IBrokerage>();
            _brokerageMock.Setup(brokerage => brokerage.GetOpenOrders()).Returns(_openOrders);
            _brokerageMock.Setup(brokerage => brokerage.GetTransactions(_portfolioMock.Object.Tail, _executionDate))
                .Returns(_newDeTransactions.Concat(_newIbmTransactions));

            _unsubmittedOrders = new List<Order> {new Order()};

            _analysisEngineMock = new Mock<IAnalysisEngine>();
            _analysisEngineMock.Setup(engine => engine.DetermineOrdersFor(_portfolioMock.Object, _dePriceSeries.Object,
                _executionDate,
                _executionDate.CurrentPeriodClose(Resolution.Days),
                _openOrders)).Returns(_unsubmittedOrders);

            _tradingProcess = new TradingProcess(_analysisEngineMock.Object, _priceSeriesProviderMock.Object, _brokerageMock.Object);
        }

        [Test]
        public void ShouldDetermineStatusAndCreateOrdersAndSubmitOrders()
        {
            _tradingProcess.Execute(_portfolioMock.Object, _executionDate, _tickers);

            _brokerageMock.Verify(broker => broker.SubmitOrders(_unsubmittedOrders));
        }

        [Test]
        public void ShouldUpdatePortfolioPositions()
        {
            _tradingProcess.Execute(_portfolioMock.Object, _executionDate, _tickers);

            _brokerageMock.Verify(brokerage => brokerage.GetTransactions(_portfolioMock.Object.Tail, _executionDate));
            _brokerageMock.Verify(brokerage => brokerage.GetTransactions(_ibmPositionMock.Object.Tail, _executionDate));
            _portfolioMock.Verify(position => position.AddTransaction(_deTransactionMock.Object));
            _portfolioMock.Verify(position => position.AddTransaction(_ibmTransactionMock.Object));
            _portfolioMock.Verify(position => position.AddTransaction(_ibmTransactionMock2.Object));
        }

        [Test]
        public void ShouldUpdatePortfolioOpenOrders()
        {
            _tradingProcess.Execute(_portfolioMock.Object, _executionDate, _tickers);

            _portfolioMock.VerifySet(portfolio => portfolio.OpenOrders = _openOrders);
        }
    }
}
