using System;
using System.Linq;
using NUnit.Framework;
using Sonneville.PriceTools.Data;
using Sonneville.PriceTools.SampleData;
using Sonneville.PriceTools.Yahoo;

namespace Sonneville.PriceTools.AutomatedTrading.Test
{
    [TestFixture]
    public class PortfolioTest
    {
        private IPortfolioFactory _portfolioFactory;
        private ITransactionFactory _transactionFactory;
        private IPriceHistoryCsvFileFactory _priceHistoryCsvFileFactory;
        private IPriceDataProvider _csvPriceDataProvider;
        private ISecurityBasketCalculator _securityBasketCalculator;
        private WebClientWrapper _webClientWrapper;

        [SetUp]
        public void Setup()
        {
            _securityBasketCalculator = new SecurityBasketCalculator();
            _transactionFactory = new TransactionFactory();
            _portfolioFactory = new PortfolioFactory(_transactionFactory, new CashAccountFactory(), _securityBasketCalculator, new PositionFactory(new PriceSeriesFactory(), _securityBasketCalculator), new PriceSeriesFactory());
            _priceHistoryCsvFileFactory = new YahooPriceHistoryCsvFileFactory();
            _webClientWrapper = new WebClientWrapper();
            _csvPriceDataProvider = new PriceDataProvider(_webClientWrapper, new YahooPriceHistoryQueryUrlBuilder(), _priceHistoryCsvFileFactory);
        }

        [TearDown]
        public void Teardown()
        {
            _webClientWrapper.Dispose();
        }

        [Test]
        public void ConstructorTest1()
        {
            var target = _portfolioFactory.ConstructPortfolio();

            Assert.AreEqual(string.Empty, target.CashTicker);
            Assert.AreEqual(0, target.GetAvailableCash(DateTime.Now));
            Assert.AreEqual(0, target.Positions.Count());
        }

        [Test]
        public void ConstructorTest2()
        {
            const string ticker = "FDRXX";  // Fidelity Cash Reserves
            var target = _portfolioFactory.ConstructPortfolio(ticker);

            Assert.AreEqual(ticker, target.CashTicker);
            Assert.AreEqual(0, target.GetAvailableCash(DateTime.Now));
            Assert.AreEqual(0, target.Positions.Count());
        }

        [Test]
        public void ConstructorTest3()
        {
            var openDate = new DateTime(2011, 2, 20);
            const decimal amount = 10000m;
            const string ticker = "FDRXX";  // Fidelity Cash Reserves
            var target = _portfolioFactory.ConstructPortfolio(ticker, openDate, amount);

            Assert.AreEqual(ticker, target.CashTicker);
            Assert.AreEqual(amount, target.GetAvailableCash(openDate));
            Assert.AreEqual(0, target.Positions.Count());
        }

        [Test]
        public void Constructor4Test()
        {
            var csvFile = SamplePortfolios.FidelityTaxable.TransactionHistory;
            var ticker = String.Empty;

            var target = _portfolioFactory.ConstructPortfolio(csvFile.Transactions);

            Assert.AreEqual(ticker, target.CashTicker);
        }

        [Test]
        public void DepositTest()
        {
            var openDate = new DateTime(2011, 2, 20);
            const decimal amount = 10000m;
            var deposit = _transactionFactory.ConstructDeposit(openDate, amount);

            var target = _portfolioFactory.ConstructPortfolio(deposit);
            
            Assert.AreEqual(amount, target.GetAvailableCash(openDate));
        }

        [Test]
        public void WithdrawalTest()
        {
            var openDate = new DateTime(2011, 2, 20);
            const decimal amount = 10000m;

            var deposit = _transactionFactory.ConstructDeposit(openDate, amount);
            var withdrawal = _transactionFactory.ConstructWithdrawal(openDate, amount);

            var target = _portfolioFactory.ConstructPortfolio(deposit, withdrawal);

            Assert.AreEqual(0, target.GetAvailableCash(openDate));
        }

        [Test]
        public void GetAvailableCashNoTransactions()
        {
            var target = _portfolioFactory.ConstructPortfolio();

            const decimal expectedCash = 0;
            var availableCash = target.GetAvailableCash(DateTime.Now);
            Assert.AreEqual(expectedCash, availableCash);
        }

        [Test]
        public void GetAvailableCashOfDeposit()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal openingDeposit = 10000m;
            var target = _portfolioFactory.ConstructPortfolio(dateTime, openingDeposit);

            const decimal expectedCash = openingDeposit;
            var availableCash = target.GetAvailableCash(dateTime);
            Assert.AreEqual(expectedCash, availableCash);
        }

        [Test]
        public void GetAvailableCashAfterFullWithdrawal()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal amount = 10000m;
            var deposit = _transactionFactory.ConstructDeposit(dateTime, amount);

            var withdrawalDate = dateTime.AddDays(1);
            var withdrawal = _transactionFactory.ConstructWithdrawal(withdrawalDate, amount);

            var target = _portfolioFactory.ConstructPortfolio(deposit, withdrawal);

            const decimal expectedCash = 0;
            var availableCash = target.GetAvailableCash(withdrawalDate);
            Assert.AreEqual(expectedCash, availableCash);
        }

        [Test]
        public void GetInvestedValueFromEmptyPortfolio()
        {
            var target = _portfolioFactory.ConstructPortfolio();
            Assert.AreEqual(0.0m, _securityBasketCalculator.CalculateMarketValue(target, _csvPriceDataProvider, DateTime.Now, _priceHistoryCsvFileFactory));
        }

        [Test]
        public void GetInvestedValue()
        {
            var dateTime = new DateTime(2011, 4, 8);
            const decimal deposit = 10000m;

            var buyDate = new DateTime(2011, 4, 25);
            const string ticker = "DE";
            const decimal price = 50.00m;
            const decimal shares = 2;
            const decimal commission = 7.95m;
            var target = _portfolioFactory.ConstructPortfolio(dateTime, deposit, _transactionFactory.ConstructBuy(ticker, buyDate, shares, price, commission));

            // If the price date falls within a period, CalculateMarketValue will use price data from that period.
            // Because of this, I changed the price date to 11:59 pm rather than the next day (default of midnight).
            var priceDate = new DateTime(2011, 4, 25).CurrentPeriodClose(Resolution.Days);
            
            const decimal expected = 189.44m; // closing price 25 April 2011 = $94.72 * 2 shares = 189.44
            var actual = _securityBasketCalculator.CalculateMarketValue(target, _csvPriceDataProvider, priceDate, _priceHistoryCsvFileFactory);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetInvestedValueFromClosedPortfolio()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;
            
            var buyDate = new DateTime(2011, 4, 25);
            var sellDate = buyDate.AddDays(1);
            const string ticker = "DE";
            const decimal price = 50.00m;
            const decimal shares = 2;
            const decimal commission = 7.95m;
            var buy = _transactionFactory.ConstructBuy(ticker, buyDate, shares, price, commission);
            var sell = _transactionFactory.ConstructSell(ticker, sellDate, shares, price, commission);

            var target = _portfolioFactory.ConstructPortfolio(dateTime, deposit, buy, sell);

            const decimal expected = 0.00m; // all shares sold = no value
            var actual = _securityBasketCalculator.CalculateMarketValue(target, _csvPriceDataProvider, sellDate, _priceHistoryCsvFileFactory);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetCostFromEmptyPortfolio()
        {
            var target = _portfolioFactory.ConstructPortfolio();

            Assert.AreEqual(0.0m, _securityBasketCalculator.CalculateCost(target, DateTime.Now));
        }

        [Test]
        public void GetCost()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;

            var buyDate = new DateTime(2011, 4, 25);
            var sellDate = buyDate.AddDays(1);
            const string ticker = "DE";
            const decimal price = 50.00m;
            const decimal shares = 2;
            const decimal commission = 7.95m;
            var buy = _transactionFactory.ConstructBuy(ticker, buyDate, shares, price, commission);
            var sell = _transactionFactory.ConstructSell(ticker, sellDate, shares, price, commission);

            var target = _portfolioFactory.ConstructPortfolio(dateTime, deposit, buy, sell);

            const decimal expected = 100.00m;
            var actual = _securityBasketCalculator.CalculateCost(target, sellDate);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetProceedsFromEmptyPortfolio()
        {
            var target = _portfolioFactory.ConstructPortfolio();
            Assert.AreEqual(0.0m, _securityBasketCalculator.CalculateProceeds(target, DateTime.Now));
        }

        [Test]
        public void GetProceeds()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;

            var buyDate = new DateTime(2011, 4, 25);
            var sellDate = buyDate.AddDays(1);
            const string ticker = "DE";
            const decimal price = 50.00m;
            const decimal shares = 2;
            const decimal commission = 7.95m;
            var buy = _transactionFactory.ConstructBuy(ticker, buyDate, shares, price, commission);
            var sell = _transactionFactory.ConstructSell(ticker, sellDate, shares, price, commission);

            var target = _portfolioFactory.ConstructPortfolio(dateTime, deposit, buy, sell);

            const decimal expected = 100.00m;
            var actual = _securityBasketCalculator.CalculateProceeds(target, sellDate);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetCommissionsFromEmptyPortfolio()
        {
            var target = _portfolioFactory.ConstructPortfolio();
            Assert.AreEqual(0.0m, _securityBasketCalculator.CalculateCommissions(target, DateTime.Now));
        }

        [Test]
        public void GetCommission()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;

            var buyDate = new DateTime(2011, 4, 25);
            var sellDate = buyDate.AddDays(1);
            const string ticker = "DE";
            const decimal price = 50.00m;
            const decimal shares = 2;
            const decimal commission = 7.95m;
            var buy = _transactionFactory.ConstructBuy(ticker, buyDate, shares, price, commission);
            var sell = _transactionFactory.ConstructSell(ticker, sellDate, shares, price, commission);

            var target = _portfolioFactory.ConstructPortfolio(dateTime, deposit, buy, sell);

            const decimal expected = 15.90m; // two $7.95 commissions paid
            var actual = _securityBasketCalculator.CalculateCommissions(target, sellDate);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetPositionTest()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;
            
            const string ticker = "DE";
            var buy = _transactionFactory.ConstructBuy(ticker, dateTime, 5, 0.00m);

            var target = _portfolioFactory.ConstructPortfolio(dateTime, deposit, buy);

            var position = target.GetPosition(ticker);
            Assert.IsTrue(position.Transactions.Contains(buy));
        }

        [Test]
        public void GetPositionTestMissing()
        {
            var target = _portfolioFactory.ConstructPortfolio("FTEXX");
            Assert.IsNull(target.GetPosition("ASDF"));
        }

        [Test]
        public void IndexerReturnsCalculateGrossProfit()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal amount = 10000m;
            var target = _portfolioFactory.ConstructPortfolio(dateTime, amount);

            var expectedValue = _securityBasketCalculator.CalculateGrossProfit(target, dateTime);
            decimal? actualValue = target[dateTime];
            Assert.AreEqual(expectedValue, actualValue);
        }

        [Test]
        public void AddTransactionDepositTest()
        {
            var date = new DateTime(2011, 1, 8);
            const decimal amount = 10000m;
            var deposit = _transactionFactory.ConstructDeposit(date, amount);

            var target = _portfolioFactory.ConstructPortfolio(deposit);

            Assert.IsTrue(target.Transactions.Contains(deposit));
        }

        [Test]
        public void AddTransactionWithdrawalTest()
        {
            var date = new DateTime(2011, 1, 8);
            const decimal amount = 10000m;
            var deposit = _transactionFactory.ConstructDeposit(date, amount);

            var target = _portfolioFactory.ConstructPortfolio(deposit);

            Assert.IsTrue(target.Transactions.Contains(deposit));
        }

        [Test]
        public void AddBuyTransactionTest()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;

            var date = new DateTime(2011, 1, 9);
            const string ticker = "DE";
            const decimal price = 50.00m;
            const decimal shares = 2;
            var buy = _transactionFactory.ConstructBuy(ticker, date, shares, price);

            var target = _portfolioFactory.ConstructPortfolio(dateTime, deposit, buy);

            Assert.IsTrue(target.Transactions.Contains(buy));
        }

        [Test]
        public void AddSellTransactionTest()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;
            
            var buyDate = new DateTime(2011, 1, 9);
            const string ticker = "DE";
            const decimal price = 50.00m;
            const decimal shares = 2;
            var buy = _transactionFactory.ConstructBuy(ticker, buyDate, shares, price);
            var sell = _transactionFactory.ConstructSell(ticker, buyDate.AddDays(1), shares, price);

            var target = _portfolioFactory.ConstructPortfolio(dateTime, deposit, buy, sell);

            Assert.IsTrue(target.Transactions.Contains(buy)); 
            Assert.IsTrue(target.Transactions.Contains(sell));
        }

        [Test]
        public void AddBuyToCoverTransactionTest()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;
            
            var buyDate = new DateTime(2011, 1, 9);
            const string ticker = "DE";
            const decimal price = 50.00m;
            const decimal shares = 2;
            var sellShort = _transactionFactory.ConstructSellShort(ticker, buyDate, shares, price);
            var buyToCover = _transactionFactory.ConstructBuyToCover(ticker, buyDate.AddDays(1), shares, price);

            var target = _portfolioFactory.ConstructPortfolio(dateTime, deposit, sellShort, buyToCover);

            Assert.IsTrue(target.Transactions.Contains(sellShort));
            Assert.IsTrue(target.Transactions.Contains(buyToCover));
        }

        [Test]
        public void AddSellShortTransactionTest()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;
            
            var date = new DateTime(2011, 1, 9);
            const string ticker = "DE";
            const decimal price = 50.00m;
            const decimal shares = 2;
            var sellShort = _transactionFactory.ConstructSellShort(ticker, date, shares, price);

            var target = _portfolioFactory.ConstructPortfolio(dateTime, deposit, sellShort);

            Assert.IsTrue(target.Transactions.Contains(sellShort));
        }

        [Test]
        public void AddDividendReceiptTransactionTest()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;
            
            var buyDate = new DateTime(2011, 1, 9);
            const string ticker = "DE";
            const decimal price = 50.00m;
            const decimal shares = 2;
            var buy = _transactionFactory.ConstructBuy(ticker, buyDate, shares, price);
            var dividendReceipt = _transactionFactory.ConstructDividendReceipt(buyDate.AddDays(1), shares);

            var target = _portfolioFactory.ConstructPortfolio(dateTime, deposit, buy, dividendReceipt);

            Assert.IsTrue(target.Transactions.Contains(buy));
            Assert.IsTrue(target.Transactions.Contains(dividendReceipt));
        }

        [Test]
        public void AddDividendReinvestmentTransactionTest()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;

            var buyDate = new DateTime(2011, 1, 9);
            const string ticker = "DE";
            const decimal price = 50.00m;
            const decimal shares = 2;
            var buy = _transactionFactory.ConstructBuy(ticker, buyDate, shares, price);
            var dividendReinvestment = _transactionFactory.ConstructDividendReinvestment(ticker, buyDate.AddDays(1), shares, 1);

            var target = _portfolioFactory.ConstructPortfolio(dateTime, deposit, buy, dividendReinvestment);

            Assert.IsTrue(target.Transactions.Contains(buy));
            Assert.IsTrue(target.Transactions.Contains(dividendReinvestment));
        }

        [Test]
        public void TransactionsTest()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;

            var buyDate = new DateTime(2011, 1, 9);
            const string de = "DE";
            const string msft = "MSFT";
            const decimal price = 50.00m;
            const decimal shares = 2;

            // first transaction: opening deposit
            // second transaction: implicit withdrawal from buy1
            // third transaction: buy1
            // fourth transaction: implicit withdrawal from buy2
            // fifth transaction: buy2
            var buy1 = _transactionFactory.ConstructBuy(de, buyDate, shares, price);
            var buy2 = _transactionFactory.ConstructBuy(msft, buyDate, shares, price);

            var target = _portfolioFactory.ConstructPortfolio(dateTime, deposit, buy1, buy2);

            const int expectedTransactions = 5;
            var actualTransactions = target.Transactions.Count();
            Assert.AreEqual(expectedTransactions, actualTransactions);

            Assert.IsTrue(target.Transactions.Contains(buy1));
            Assert.IsTrue(target.Transactions.Contains(buy2));
        }

        [Test]
        public void WithdrawWithoutAvailableCash()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;
            
            var buyDate = new DateTime(2011, 1, 9);
            const string ticker = "DE";
            const decimal price = 50.00m;
            const decimal shares = 2;
            var buy = _transactionFactory.ConstructBuy(ticker, buyDate, shares, price);

            var withdrawalDate = dateTime.AddDays(1);
            var withdrawal = _transactionFactory.ConstructWithdrawal(withdrawalDate, deposit);

            var portfolio = _portfolioFactory.ConstructPortfolio(dateTime, deposit, buy, withdrawal);

            Assert.AreEqual(4, portfolio.Transactions.Count());
            Assert.AreEqual(0 - (price * shares), portfolio.GetAvailableCash(withdrawalDate));
        }

        [Test]
        public void HeadTestWhenEmpty()
        {
            var target = _portfolioFactory.ConstructPortfolio();

            // measure DayOfYear because ticks will be slightly different between calls
            var expected = DateTime.Now.DayOfYear;
            var actual = target.Head.DayOfYear;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void HeadTestWithPosition()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;
            
            var buyDate = dateTime.AddDays(1);
            var buy = _transactionFactory.ConstructBuy("DE", buyDate, 10, 100.00m, 7.95m);

            var target = _portfolioFactory.ConstructPortfolio(dateTime, deposit, buy);

            var expected = dateTime;
            var actual = target.Head;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void HeadTestWithOneTransaction()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal amount = 10000m;
            var target = _portfolioFactory.ConstructPortfolio(dateTime, amount);
            
            var expected = dateTime;
            var actual = target.Head;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void HeadTestWithTwoTransactions()
        {
            var originalDate = new DateTime(2011, 1, 8);
            const decimal amount = 10000m;

            var deposit = _transactionFactory.ConstructDeposit(originalDate, amount);
            var withdrawal = _transactionFactory.ConstructWithdrawal(originalDate.AddDays(10), amount);

            var target = _portfolioFactory.ConstructPortfolio(deposit, withdrawal);

            var expected = originalDate;
            var actual = target.Head;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TailTestWhenEmpty()
        {
            var target = _portfolioFactory.ConstructPortfolio();

            // measure DayOfYear because ticks will be slightly different between calls
            var expected = DateTime.Now.DayOfYear;
            var actual = target.Tail.DayOfYear;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TailTestWithPosition()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;
            
            var buyDate = dateTime.AddDays(1);
            var buy = _transactionFactory.ConstructBuy("DE", buyDate, 10, 100.00m, 7.95m);

            var target = _portfolioFactory.ConstructPortfolio(dateTime, deposit, buy);

            var expected = buyDate;
            var actual = target.Tail;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TailTestWithOneTransaction()
        {
            var dateTime = new DateTime(2011, 1, 8);
            const decimal amount = 10000m;
            var target = _portfolioFactory.ConstructPortfolio(dateTime, amount);

            var expected = dateTime;
            var actual = target.Tail;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TailTestWithTwoTransactions()
        {
            var originalDate = new DateTime(2011, 1, 8);
            const decimal amount = 10000m;
            var deposit1 = _transactionFactory.ConstructDeposit(originalDate, amount);
            var nextDate = originalDate.AddDays(10);
            var deposit2 = _transactionFactory.ConstructDeposit(nextDate, amount);

            var target = _portfolioFactory.ConstructPortfolio(deposit1, deposit2);

            var expected = nextDate;
            var actual = target.Tail;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CashTickerSetCorrectly()
        {
            const string ticker = "FDRXX"; // Fidelity Cash Reserves
            var target = _portfolioFactory.ConstructPortfolio(ticker);

            const string expected = ticker;
            var actual = target.CashTicker;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ConstructPriceSeriesFromOnePositionWithOneBuyAndSell()
        {
            var portfolio = _portfolioFactory.ConstructPortfolio();
            const string ticker = "DE";

            var buyDate = new DateTime(2014, 6, 24);
            const int sharesBought = 2;
            const decimal priceBought = 50.00m;
            var buy = _transactionFactory.ConstructBuy(ticker, buyDate, sharesBought, priceBought);
            portfolio.AddToPosition(buy);

            var sellDate = new DateTime(2014, 6, 25);
            const int sharesSold = 1;
            const decimal priceSold = 50.00m;
            var sell = _transactionFactory.ConstructSell(ticker, sellDate, sharesSold, priceSold);
            portfolio.AddToPosition(sell);

            var result = _portfolioFactory.ConstructPriceSeries(portfolio, _csvPriceDataProvider);

            const decimal closingPrice24th = 90.63m;
            const decimal closingPrice25th = 90.74m;
            Assert.AreEqual(closingPrice24th*sharesBought, result[buyDate]);
            Assert.AreEqual(closingPrice25th*(sharesBought - sharesSold), result[sellDate]);
        }

        [Test]
        public void ConstructPriceSeriesFromTwoPositions()
        {
            var portfolio = _portfolioFactory.ConstructPortfolio();
            var buyDate = new DateTime(2014, 6, 24);

            const string deTicker = "DE";
            const int deSharesBought = 2;
            const decimal dePriceBought = 50.00m;
            var deBuy = _transactionFactory.ConstructBuy(deTicker, buyDate, deSharesBought, dePriceBought);
            portfolio.AddToPosition(deBuy);

            const string ibmTicker = "IBM";
            const int ibmSharesBought = 2;
            const decimal ibmPriceBought = 50.00m;
            var ibmBuy = _transactionFactory.ConstructBuy(ibmTicker, buyDate, ibmSharesBought, ibmPriceBought);
            portfolio.AddToPosition(ibmBuy);

            var result = _portfolioFactory.ConstructPriceSeries(portfolio, _csvPriceDataProvider);

            const decimal deClosingPrice24th = 90.63m;
            const decimal ibmClosingPrice24th = 180.88m;
            const decimal expected = (deClosingPrice24th*deSharesBought) + (ibmClosingPrice24th*ibmSharesBought);
            Assert.AreEqual(expected, result[buyDate]);
        }
    }
}
