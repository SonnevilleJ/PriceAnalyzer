using System.Collections.Generic;
using System.IO;
using Sonneville.PriceTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Sonneville.PriceTools.SamplePriceData;
using Sonneville.PriceTools.Services;

namespace Sonneville.PriceToolsTest
{
    /// <summary>
    ///This is a test class for Portfolio and is intended
    ///to contain all Portfolio Unit Tests
    ///</summary>
    [TestClass]
    public class PortfolioTest
    {
        [TestInitialize]
        public void TestInitialize()
        {
            Settings.SetDefaultSettings();
            Settings.CanConnectToInternet = false;
        }

        [TestMethod]
        public void ConstructorTest1()
        {
            IPortfolio target = new Portfolio();

            Assert.AreEqual(string.Empty, target.CashTicker);
            Assert.AreEqual(0, target.GetAvailableCash(DateTime.Now));
            Assert.AreEqual(0, target.Positions.Count);
        }

        [TestMethod]
        public void ConstructorTest2()
        {
            const string ticker = "FDRXX";  // Fidelity Cash Reserves
            IPortfolio target = new Portfolio(ticker);

            Assert.AreEqual(ticker, target.CashTicker);
            Assert.AreEqual(0, target.GetAvailableCash(DateTime.Now));
            Assert.AreEqual(0, target.Positions.Count);
        }

        [TestMethod]
        public void ConstructorTest3()
        {
            DateTime openDate = new DateTime(2011, 2, 20);
            const decimal amount = 10000m;
            const string ticker = "FDRXX";  // Fidelity Cash Reserves
            IPortfolio target = new Portfolio(openDate, amount, ticker);

            Assert.AreEqual(ticker, target.CashTicker);
            Assert.AreEqual(amount, target.GetAvailableCash(openDate));
            Assert.AreEqual(0, target.Positions.Count);
        }

        [TestMethod]
        public void ConstructorTest4()
        {
            var csvFile = new FidelityTransactionHistoryCsvFile(new ResourceStream(TestData.FidelityTransactions));
            var ticker = String.Empty;

            IPortfolio target = new Portfolio(csvFile);

            Assert.AreEqual(ticker, target.CashTicker);
        }

        [TestMethod]
        public void DepositTest1()
        {
            DateTime openDate = new DateTime(2011, 2, 20);
            const decimal amount = 10000m;
            IPortfolio target = new Portfolio();

            target.Deposit(openDate, amount);

            Assert.AreEqual(amount, target.GetAvailableCash(openDate));
        }

        [TestMethod]
        public void DepositTest2()
        {
            DateTime openDate = new DateTime(2011, 2, 20);
            const decimal amount = 10000m;
            IPortfolio target = new Portfolio();

            Deposit deposit = new Deposit
                                  {
                                      SettlementDate = openDate,
                                      Amount = amount
                                  };
            target.Deposit(deposit);

            Assert.AreEqual(amount, target.GetAvailableCash(openDate));
        }

        [TestMethod]
        public void WithdrawalTest1()
        {
            DateTime openDate = new DateTime(2011, 2, 20);
            const decimal amount = 10000m;
            IPortfolio target = new Portfolio();

            target.Deposit(openDate, amount);
            target.Withdraw(openDate, amount);

            Assert.AreEqual(0, target.GetAvailableCash(openDate));
        }

        [TestMethod]
        public void WithdrawalTest2()
        {
            DateTime openDate = new DateTime(2011, 2, 20);
            const decimal amount = 10000m;
            IPortfolio target = new Portfolio();

            Deposit deposit = new Deposit
                                  {
                                      SettlementDate = openDate,
                                      Amount = amount
                                  };
            Withdrawal withdrawal = new Withdrawal
                                        {
                                            SettlementDate = openDate,
                                            Amount = amount
                                        };
            target.Deposit(deposit);
            target.Withdraw(withdrawal);

            Assert.AreEqual(0, target.GetAvailableCash(openDate));
        }

        [TestMethod]
        public void GetAvailableCashNoTransactions()
        {
            IPortfolio target = new Portfolio();

            const decimal expectedCash = 0;
            decimal availableCash = target.GetAvailableCash(DateTime.Now);
            Assert.AreEqual(expectedCash, availableCash);
        }

        [TestMethod]
        public void GetValueNoTransactions()
        {
            IPortfolio target = new Portfolio();

            const decimal expectedValue = 0;
            decimal actualValue = target.CalculateValue(DateTime.Now);
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void GetAvailableCashOfDeposit()
        {
            DateTime dateTime = new DateTime(2011, 1, 8);
            const decimal openingDeposit = 10000m;
            IPortfolio target = new Portfolio(dateTime, openingDeposit);

            const decimal expectedCash = openingDeposit;
            decimal availableCash = target.GetAvailableCash(dateTime);
            Assert.AreEqual(expectedCash, availableCash);
        }

        [TestMethod]
        public void GetValueOfDeposit()
        {
            DateTime dateTime = new DateTime(2011, 1, 8);
            const decimal openingDeposit = 10000m;
            IPortfolio target = new Portfolio(dateTime, openingDeposit);

            const decimal expectedValue = openingDeposit;
            decimal actualValue = target.CalculateValue(dateTime);
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void GetAvailableCashAfterFullWithdrawal()
        {
            DateTime dateTime = new DateTime(2011, 1, 8);
            const decimal amount = 10000m;
            IPortfolio target = new Portfolio(dateTime, amount);

            DateTime withdrawalDate = dateTime.AddDays(1);
            target.Withdraw(dateTime.AddDays(1), amount);

            const decimal expectedCash = 0;
            decimal availableCash = target.GetAvailableCash(withdrawalDate);
            Assert.AreEqual(expectedCash, availableCash);
        }

        [TestMethod]
        public void GetValueAfterFullWithdrawal()
        {
            DateTime dateTime = new DateTime(2011, 1, 8);
            const decimal amount = 10000m;
            IPortfolio target = new Portfolio(dateTime, amount);

            DateTime withdrawalDate = dateTime.AddDays(1);
            target.Withdraw(dateTime.AddDays(1), amount);

            const decimal expectedValue = 0;
            decimal actualValue = target.CalculateValue(withdrawalDate);
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void GetRawReturnTest()
        {
            DateTime dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;
            IPortfolio target = new Portfolio(dateTime, deposit);

            DateTime buyDate = new DateTime(2011, 1, 10);
            DateTime sellDate = buyDate.AddDays(1);
            const string ticker = "DE";
            const decimal price = 100.00m;      // $100.00 per share
            const double shares = 5;            // 5 shares
            const decimal commission = 7.95m;   // with $7.95 commission
            Buy buy = new Buy
            {
                SettlementDate = buyDate,
                Ticker = ticker,
                Price = price,
                Shares = shares,
                Commission = commission
            };

            target.AddTransaction(buy);

            Assert.IsNull(target.CalculateRawReturn(sellDate));
        }

        [TestMethod]
        public void GetRawReturnFromClosedPortfolioTest()
        {
            DateTime dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;
            IPortfolio target = new Portfolio(dateTime, deposit);

            DateTime buyDate = new DateTime(2011, 1, 10);
            DateTime sellDate = buyDate.AddDays(1);
            const string ticker = "DE";
            const decimal price = 100.00m;      // $100.00 per share
            const double shares = 5;            // 5 shares
            const decimal commission = 7.95m;   // with $7.95 commission
            Buy buy = new Buy
            {
                SettlementDate = buyDate,
                Ticker = ticker,
                Price = price,
                Shares = shares,
                Commission = commission
            };
            Sell sell = new Sell
            {
                SettlementDate = sellDate,
                Ticker = ticker,
                Price = price,
                Shares = shares,
                Commission = commission
            };

            target.AddTransaction(buy);
            target.AddTransaction(sell);

            const decimal expected = 0.0m;      // 0% raw return on investment
            decimal? actual = target.CalculateRawReturn(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetTotalReturnTest()
        {
            DateTime dateTime = new DateTime(2001, 1, 1);
            const decimal deposit = 10000m;
            IPortfolio target = new Portfolio(dateTime, deposit);

            DateTime buyDate = new DateTime(2001, 1, 1);
            DateTime sellDate = buyDate.AddDays(1);
            const string ticker = "DE";
            const decimal price = 100.00m;       // $100.00 per share
            const double shares = 5;             // 5 shares
            const decimal commission = 5.00m;    // with $5 commission
            Buy buy = new Buy
            {
                SettlementDate = buyDate,
                Ticker = ticker,
                Price = price,
                Shares = shares,
                Commission = commission
            };

            target.AddTransaction(buy);

            Assert.IsNull(target.CalculateTotalReturn(sellDate));
        }

        [TestMethod]
        public void GetTotalReturnFromClosedPortfolioTest()
        {
            DateTime dateTime = new DateTime(2001, 1, 1);
            const decimal deposit = 10000m;
            IPortfolio target = new Portfolio(dateTime, deposit);

            DateTime buyDate = new DateTime(2001, 1, 1);
            DateTime sellDate = buyDate.AddDays(1);
            const string ticker = "DE";
            const decimal price = 100.00m;       // $100.00 per share
            const double shares = 5;             // 5 shares
            const decimal commission = 5.00m;    // with $5 commission
            Buy buy = new Buy
            {
                SettlementDate = buyDate,
                Ticker = ticker,
                Price = price,
                Shares = shares,
                Commission = commission
            };
            Sell sell = new Sell
            {
                SettlementDate = sellDate,
                Ticker = ticker,
                Price = price,
                Shares = shares,
                Commission = commission
            };

            target.AddTransaction(buy);
            target.AddTransaction(sell);

            const decimal expected = -0.02m;      // negative 2% return; 98% of original investment
            decimal? actual = target.CalculateTotalReturn(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetAverageAnnualReturnTest()
        {
            DateTime dateTime = new DateTime(2001, 1, 1);
            const decimal deposit = 505.00m;
            IPortfolio target = new Portfolio(dateTime, deposit);

            DateTime buyDate = new DateTime(2001, 1, 1);
            DateTime sellDate = new DateTime(2001, 3, 15); // sellDate is 0.20 * 365 = 73 days after buyDate
            const string ticker = "DE";
            const decimal buyPrice = 100.00m;       // $100.00 per share
            const double shares = 5;                // 5 shares
            const decimal commission = 5.00m;       // with $5 commission
            Buy buy = new Buy
            {
                SettlementDate = buyDate,
                Ticker = ticker,
                Price = buyPrice,
                Shares = shares,
                Commission = commission
            };

            target.AddTransaction(buy);

            Assert.IsNull(target.CalculateAverageAnnualReturn(sellDate));
        }

        [TestMethod]
        public void GetAverageAnnualReturnFromClosedPortfolioTest()
        {
            DateTime dateTime = new DateTime(2001, 1, 1);
            const decimal deposit = 505.00m;
            IPortfolio target = new Portfolio(dateTime, deposit);

            DateTime buyDate = new DateTime(2001, 1, 1);
            DateTime sellDate = new DateTime(2001, 3, 15); // sellDate is 0.20 * 365 = 73 days after buyDate
            const string ticker = "DE";
            const decimal buyPrice = 100.00m;       // $100.00 per share
            const decimal sellPrice = 112.00m;      // $112.00 per share
            const double shares = 5;                // 5 shares
            const decimal commission = 5.00m;       // with $5 commission
            Buy buy = new Buy
            {
                SettlementDate = buyDate,
                Ticker = ticker,
                Price = buyPrice,
                Shares = shares,
                Commission = commission
            };
            Sell sell = new Sell
            {
                SettlementDate = sellDate,
                Ticker = ticker,
                Price = sellPrice,
                Shares = shares,
                Commission = commission
            };

            target.AddTransaction(buy);
            target.AddTransaction(sell);

            const decimal expectedReturn = 0.1m;    // 10% return; profit = $50 after commissions; initial investment = $500
            decimal? actualReturn = target.CalculateTotalReturn(sellDate);
            Assert.AreEqual(expectedReturn, actualReturn);

            const decimal expected = 0.5m;          // 50% annual rate return
            decimal? actual = target.CalculateAverageAnnualReturn(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetInvestedValueFromEmptyPortfolio()
        {
            IPortfolio target = new Portfolio();
            Assert.AreEqual(0.0m, target.CalculateInvestedValue(DateTime.Now));
        }

        [TestMethod]
        public void GetInvestedValue()
        {
            Settings.CanConnectToInternet = true;

            DateTime dateTime = new DateTime(2011, 4, 8);
            const decimal deposit = 10000m;
            IPortfolio target = new Portfolio(dateTime, deposit);

            DateTime buyDate = new DateTime(2011, 4, 25);
            const string ticker = "DE";
            const decimal price = 50.00m;
            const double shares = 2;
            const decimal commission = 7.95m;
            Buy buy = new Buy
            {
                SettlementDate = buyDate,
                Ticker = ticker,
                Price = price,
                Shares = shares,
                Commission = commission
            };

            target.AddTransaction(buy);

            // If the price date falls within a period, CalculateInvestedValue will use price data from that period.
            // Because of this, I changed the price date to 11:59 pm rather than the next day (default of midnight).
            // Todo: adding a "market open/close times" feature would be a better fix; more convenient for the client.
            DateTime priceDate = new DateTime(2011, 4, 25, 23, 59, 59);
            
            const decimal expected = 189.44m; // closing price 25 April 2011 = $94.72 * 2 shares = 189.44
            decimal actual = target.CalculateInvestedValue(priceDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetInvestedValueFromClosedPortfolio()
        {
            DateTime dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;
            IPortfolio target = new Portfolio(dateTime, deposit);

            DateTime buyDate = new DateTime(2011, 4, 25);
            DateTime sellDate = buyDate.AddDays(1);
            const string ticker = "DE";
            const decimal price = 50.00m;
            const double shares = 2;
            const decimal commission = 7.95m;
            Buy buy = new Buy
            {
                SettlementDate = buyDate,
                Ticker = ticker,
                Price = price,
                Shares = shares,
                Commission = commission
            };
            Sell sell = new Sell
            {
                SettlementDate = sellDate,
                Ticker = ticker,
                Price = price,
                Shares = shares,
                Commission = commission
            };

            target.AddTransaction(buy);
            target.AddTransaction(sell);

            const decimal expected = 0.00m; // all shares sold = no value
            decimal actual = target.CalculateInvestedValue(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetCostFromEmptyPortfolio()
        {
            IPortfolio target = new Portfolio();
            Assert.AreEqual(0.0m, target.CalculateCost(DateTime.Now));
        }

        [TestMethod]
        public void GetCost()
        {
            DateTime dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;
            IPortfolio target = new Portfolio(dateTime, deposit);

            DateTime buyDate = new DateTime(2011, 4, 25);
            DateTime sellDate = buyDate.AddDays(1);
            const string ticker = "DE";
            const decimal price = 50.00m;
            const double shares = 2;
            const decimal commission = 7.95m;
            Buy buy = new Buy
            {
                SettlementDate = buyDate,
                Ticker = ticker,
                Price = price,
                Shares = shares,
                Commission = commission
            };
            Sell sell = new Sell
            {
                SettlementDate = sellDate,
                Ticker = ticker,
                Price = price,
                Shares = shares,
                Commission = commission
            };

            target.AddTransaction(buy);
            target.AddTransaction(sell);

            const decimal expected = 100.00m;
            decimal actual = target.CalculateCost(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetProceedsFromEmptyPortfolio()
        {
            IPortfolio target = new Portfolio();
            Assert.AreEqual(0.0m, target.CalculateProceeds(DateTime.Now));
        }

        [TestMethod]
        public void GetProceeds()
        {
            DateTime dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;
            IPortfolio target = new Portfolio(dateTime, deposit);

            DateTime buyDate = new DateTime(2011, 4, 25);
            DateTime sellDate = buyDate.AddDays(1);
            const string ticker = "DE";
            const decimal price = 50.00m;
            const double shares = 2;
            const decimal commission = 7.95m;
            Buy buy = new Buy
            {
                SettlementDate = buyDate,
                Ticker = ticker,
                Price = price,
                Shares = shares,
                Commission = commission
            };
            Sell sell = new Sell
            {
                SettlementDate = sellDate,
                Ticker = ticker,
                Price = price,
                Shares = shares,
                Commission = commission
            };

            target.AddTransaction(buy);
            target.AddTransaction(sell);

            const decimal expected = 100.00m;
            decimal actual = target.CalculateProceeds(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetCommissionsFromEmptyPortfolio()
        {
            IPortfolio target = new Portfolio();
            Assert.AreEqual(0.0m, target.CalculateCommissions(DateTime.Now));
        }

        [TestMethod]
        public void GetCommission()
        {
            DateTime dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;
            IPortfolio target = new Portfolio(dateTime, deposit);

            DateTime buyDate = new DateTime(2011, 4, 25);
            DateTime sellDate = buyDate.AddDays(1);
            const string ticker = "DE";
            const decimal price = 50.00m;
            const double shares = 2;
            const decimal commission = 7.95m;
            Buy buy = new Buy
            {
                SettlementDate = buyDate,
                Ticker = ticker,
                Price = price,
                Shares = shares,
                Commission = commission
            };
            Sell sell = new Sell
            {
                SettlementDate = sellDate,
                Ticker = ticker,
                Price = price,
                Shares = shares,
                Commission = commission
            };

            target.AddTransaction(buy);
            target.AddTransaction(sell);

            const decimal expected = 15.90m; // two $7.95 commissions paid
            decimal actual = target.CalculateCommissions(sellDate);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetPositionTest()
        {
            DateTime dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;
            IPortfolio target = new Portfolio(dateTime, deposit);
            
            const string ticker = "DE";
            var buy = new Buy {SettlementDate = dateTime, Shares = 5, Ticker = ticker};
            
            target.AddTransaction(buy);

            var position = target.GetPosition(ticker);
            Assert.IsTrue(position.Transactions.Contains(buy));
        }

        [TestMethod]
        public void GetPositionTestMissing()
        {
            var target = new Portfolio("FTEXX");
            Assert.IsNull(target.GetPosition("ASDF"));
        }

        [TestMethod]
        public void IndexerTest()
        {
            DateTime dateTime = new DateTime(2011, 1, 8);
            const decimal amount = 10000m;
            IPortfolio target = new Portfolio(dateTime, amount);

            decimal expectedValue = target.CalculateValue(dateTime);
            decimal? actualValue = target[dateTime];
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void AddTransactionDepositTest()
        {
            DateTime date = new DateTime(2011, 1, 8);
            const decimal amount = 10000m;
            IPortfolio target = new Portfolio();

            Deposit deposit = new Deposit
                                  {
                                      SettlementDate = date,
                                      Amount = amount
                                  };
            target.AddTransaction(deposit);

            Assert.IsTrue(target.Transactions.Contains(deposit));
        }

        [TestMethod]
        public void AddTransactionWithdrawalTest()
        {
            DateTime date = new DateTime(2011, 1, 8);
            const decimal amount = 10000m;
            IPortfolio target = new Portfolio();

            Deposit deposit = new Deposit
                                  {
                                      SettlementDate = date,
                                      Amount = amount
                                  };
            target.AddTransaction(deposit);

            Assert.IsTrue(target.Transactions.Contains(deposit));
        }

        [TestMethod]
        public void AddBuyTransactionTest()
        {
            DateTime dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;
            IPortfolio target = new Portfolio(dateTime, deposit);

            DateTime date = new DateTime(2011, 1, 9);
            const string ticker = "DE";
            const decimal price = 50.00m;
            const double shares = 2;
            Buy buy = new Buy {SettlementDate = date, Ticker = ticker, Price = price, Shares = shares};
            target.AddTransaction(buy);

            Assert.IsTrue(target.Transactions.Contains(buy));
        }

        [TestMethod]
        public void AddSellTransactionTest()
        {
            DateTime dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;
            IPortfolio target = new Portfolio(dateTime, deposit);

            DateTime buyDate = new DateTime(2011, 1, 9);
            const string ticker = "DE";
            const decimal price = 50.00m;
            const double shares = 2;
            Buy buy = new Buy
                          {
                              SettlementDate = buyDate,
                              Ticker = ticker,
                              Price = price,
                              Shares = shares
                          };
            Sell sell = new Sell
                            {
                                SettlementDate = buyDate.AddDays(1),
                                Ticker = ticker,
                                Price = price,
                                Shares = shares
                            };

            target.AddTransaction(buy);
            target.AddTransaction(sell);

            Assert.IsTrue(target.Transactions.Contains(buy)); 
            Assert.IsTrue(target.Transactions.Contains(sell));
        }

        [TestMethod]
        public void AddBuyToCoverTransactionTest()
        {
            DateTime dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;
            IPortfolio target = new Portfolio(dateTime, deposit);

            DateTime buyDate = new DateTime(2011, 1, 9);
            const string ticker = "DE";
            const decimal price = 50.00m;
            const double shares = 2;
            SellShort sellShort = new SellShort
                                      {
                                          SettlementDate = buyDate,
                                          Ticker = ticker,
                                          Price = price,
                                          Shares = shares
                                      };
            BuyToCover buyToCover = new BuyToCover
                                        {
                                            SettlementDate = buyDate.AddDays(1),
                                            Ticker = ticker,
                                            Price = price,
                                            Shares = shares
                                        };

            target.AddTransaction(sellShort);
            target.AddTransaction(buyToCover);

            Assert.IsTrue(target.Transactions.Contains(sellShort));
            Assert.IsTrue(target.Transactions.Contains(buyToCover));
        }

        [TestMethod]
        public void AddSellShortTransactionTest()
        {
            DateTime dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;
            IPortfolio target = new Portfolio(dateTime, deposit);

            DateTime date = new DateTime(2011, 1, 9);
            const string ticker = "DE";
            const decimal price = 50.00m;
            const double shares = 2;
            SellShort sellShort = new SellShort { SettlementDate = date, Ticker = ticker, Price = price, Shares = shares };
            target.AddTransaction(sellShort);

            Assert.IsTrue(target.Transactions.Contains(sellShort));
        }

        [TestMethod]
        public void AddDividendReceiptTransactionTest()
        {
            DateTime dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;
            IPortfolio target = new Portfolio(dateTime, deposit);

            DateTime buyDate = new DateTime(2011, 1, 9);
            const string ticker = "DE";
            const decimal price = 50.00m;
            const double shares = 2;
            Buy buy = new Buy
                          {
                              SettlementDate = buyDate,
                              Ticker = ticker,
                              Price = price,
                              Shares = shares
                          };
            DividendReceipt dividendReceipt = new DividendReceipt
                                                  {
                                                      SettlementDate = buyDate.AddDays(1),
                                                      Amount = 1 * (decimal)shares
                                                  };

            target.AddTransaction(buy);
            target.AddTransaction(dividendReceipt);

            Assert.IsTrue(target.Transactions.Contains(buy));
            Assert.IsTrue(target.Transactions.Contains(dividendReceipt));
        }

        [TestMethod]
        public void AddDividendReinvestmentTransactionTest()
        {
            DateTime dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;
            IPortfolio target = new Portfolio(dateTime, deposit);

            DateTime buyDate = new DateTime(2011, 1, 9);
            const string ticker = "DE";
            const decimal price = 50.00m;
            const double shares = 2;
            Buy buy = new Buy
                          {
                              SettlementDate = buyDate,
                              Ticker = ticker,
                              Price = price,
                              Shares = shares
                          };
            DividendReinvestment dividendReinvestment = new DividendReinvestment
                                                            {
                                                                SettlementDate = buyDate.AddDays(1),
                                                                Ticker = ticker,
                                                                Price = 1,
                                                                Shares = shares
                                                            };

            target.AddTransaction(buy);
            target.AddTransaction(dividendReinvestment);

            Assert.IsTrue(target.Transactions.Contains(buy));
            Assert.IsTrue(target.Transactions.Contains(dividendReinvestment));
        }

        [TestMethod]
        public void PositionTest_OnePosition_TwoCashTransactions()
        {
            Settings.CanConnectToInternet = true;
            DateTime dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;
            IPortfolio target = new Portfolio(dateTime, deposit);

            DateTime buyDate = new DateTime(2011, 1, 9);
            const string ticker = "DE";
            const decimal price = 50.00m;
            const double shares = 2;
            Buy buy = new Buy {SettlementDate = buyDate, Ticker = ticker, Price = price, Shares = shares};
            target.AddTransaction(buy);

            const decimal withdrawal = 5000m;
            DateTime withdrawalDate = dateTime.AddDays(1);
            target.Withdraw(withdrawalDate, withdrawal);

            const int expectedTransactions = 1;
            int actualTransactions = target.Positions.Count;
            Assert.AreEqual(expectedTransactions, actualTransactions);

            // DE price @ 7 Jan 2011 = $84.34
            // invested value should be $84.34 * 5 shares = $168.68
            // starting cash = 10,000
            // purchase cost = 50.00 * 2 shares = 100.00
            // withdrawal = 5,000
            // total value should be = 10,000 - 100.00 - 5,000 + 168.68 = 5068.68
            
            const decimal expectedValue = 5068.68m;
            decimal actualValue = target.CalculateValue(buyDate);
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void TransactionsTest()
        {
            DateTime dateTime = new DateTime(2011, 1, 8);
            const decimal deposit = 10000m;
            IPortfolio target = new Portfolio(dateTime, deposit);   // first transaction: deposit

            DateTime buyDate = new DateTime(2011, 1, 9);
            const string de = "DE";
            const string msft = "MSFT";
            const decimal price = 50.00m;
            const double shares = 2;

            // second transaction: implicit withdrawal from buy1
            // third transaction: buy1
            // fourth transaction: implicit withdrawal from buy2
            // fifth transaction: buy2
            Buy buy1 = new Buy {SettlementDate = buyDate, Ticker = de, Price = price, Shares = shares};
            Buy buy2 = new Buy {SettlementDate = buyDate, Ticker = msft, Price = price, Shares = shares};
            target.AddTransaction(buy1);
            target.AddTransaction(buy2);
            
            const int expectedTransactions = 5;
            int actualTransactions = target.Transactions.Count;
            Assert.AreEqual(expectedTransactions, actualTransactions);

            Assert.IsTrue(target.Transactions.Contains(buy1));
            Assert.IsTrue(target.Transactions.Contains(buy2));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WithdrawWithoutAvailableCash()
        {
            DateTime dateTime = new DateTime(2011, 1, 8);
            const decimal amount = 10000m;
            IPortfolio target = new Portfolio(dateTime, amount);

            DateTime buyDate = new DateTime(2011, 1, 9);
            const string ticker = "DE";
            const decimal price = 50.00m;
            const double shares = 2;
            Buy buy = new Buy { SettlementDate = buyDate, Ticker = ticker, Price = price, Shares = shares };
            target.AddTransaction(buy);

            DateTime withdrawalDate = dateTime.AddDays(1);
            target.Withdraw(withdrawalDate, amount);
        }

        [TestMethod]
        public void HeadTestWithPosition()
        {
            DateTime dateTime = new DateTime(2011, 1, 8);
            const decimal amount = 10000m;
            IPortfolio target = new Portfolio();

            target.Deposit(dateTime, amount);

            DateTime buyDate = dateTime.AddDays(1);
            Buy buy = new Buy
                          {
                              SettlementDate = buyDate,
                              Ticker = "DE",
                              Price = 100.00m,
                              Shares = 10,
                              Commission = 7.95m,
                          };
            target.AddTransaction(buy);

            DateTime expected = dateTime;
            DateTime actual = target.Head;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void HeadTestWithOneTransaction()
        {
            DateTime dateTime = new DateTime(2011, 1, 8);
            const decimal amount = 10000m;
            IPortfolio target = new Portfolio();

            target.Deposit(dateTime, amount);
            
            DateTime expected = dateTime;
            DateTime actual = target.Head;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void HeadTestWithTwoTransactions()
        {
            DateTime originalDate = new DateTime(2011, 1, 8);
            const decimal amount = 10000m;
            IPortfolio target = new Portfolio();

            target.Deposit(originalDate, amount);
            target.Deposit(originalDate.AddDays(10), amount);

            DateTime expected = originalDate;
            DateTime actual = target.Head;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TailTestWithPosition()
        {
            DateTime dateTime = new DateTime(2011, 1, 8);
            const decimal amount = 10000m;
            IPortfolio target = new Portfolio();

            target.Deposit(dateTime, amount);

            DateTime buyDate = dateTime.AddDays(1);
            Buy buy = new Buy
            {
                SettlementDate = buyDate,
                Ticker = "DE",
                Price = 100.00m,
                Shares = 10,
                Commission = 7.95m,
            };
            target.AddTransaction(buy);

            DateTime expected = buyDate;
            DateTime actual = target.Tail;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TailTestWithOneTransaction()
        {
            DateTime dateTime = new DateTime(2011, 1, 8);
            const decimal amount = 10000m;
            IPortfolio target = new Portfolio();

            target.Deposit(dateTime, amount);

            DateTime expected = dateTime;
            DateTime actual = target.Tail;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TailTestWithTwoTransactions()
        {
            DateTime originalDate = new DateTime(2011, 1, 8);
            const decimal amount = 10000m;
            IPortfolio target = new Portfolio();

            target.Deposit(originalDate, amount);
            target.Deposit(originalDate.AddDays(10), amount);

            DateTime expected = originalDate.AddDays(10);
            DateTime actual = target.Tail;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CashTickerSetCorrectly()
        {
            const string ticker = "FDRXX"; // Fidelity Cash Reserves
            IPortfolio target = new Portfolio(ticker);

            const string expected = ticker;
            string actual = target.CashTicker;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void HasValueTest()
        {
            DateTime testDate = new DateTime(2011, 1, 8);
            DateTime purchaseDate = testDate.AddDays(1);
            const decimal amount = 10000m;
            const string ticker = "FDRXX"; // Fidelity Cash Reserves
            IPortfolio target = new Portfolio(purchaseDate, amount, ticker);

            Assert.AreEqual(false, target.HasValueInRange(testDate));
            Assert.AreEqual(true, target.HasValueInRange(purchaseDate));
            Assert.AreEqual(true, target.HasValueInRange(purchaseDate.AddDays(1)));
        }

        [TestMethod]
        public void CalculateHoldingsTestWithOnePositionOneBuyOneSell()
        {
            DateTime dateTime = new DateTime(2011, 7, 26);
            const decimal deposit = 10000m;
            IPortfolio target = new Portfolio(dateTime, deposit);

            DateTime buyDate = new DateTime(2011, 7, 26);
            const string ticker = "DE";
            const decimal buyPrice = 50.00m;
            const double shares = 2;
            Buy buy = new Buy {SettlementDate = buyDate, Ticker = ticker, Price = buyPrice, Shares = shares};
            target.AddTransaction(buy);

            DateTime sellDate = new DateTime(2011, 9, 26);
            const decimal sellPrice = 75.00m;
            Sell sell = new Sell {SettlementDate = sellDate, Ticker = ticker, Price = sellPrice, Shares = shares};
            target.AddTransaction(sell);

            IList<IHolding> holdings = target.CalculateHoldings(sellDate);

            Assert.AreEqual(1, holdings.Count);
            var expected = new Holding
            {
                Ticker = ticker,
                Head = buyDate,
                Tail = sellDate,
                Shares = shares,
                OpenPrice = buyPrice * (decimal)shares,
                ClosePrice = sellPrice * (decimal)shares
            };

            Assert.IsTrue(holdings.Contains(expected));
        }

        [TestMethod]
        public void CalculateHoldingsTestWithOnePositionTwoBuysTwoSells()
        {
            DateTime testDate = new DateTime(2001, 1, 1);
            DateTime firstBuyDate = testDate.AddDays(1);
            DateTime secondBuyDate = firstBuyDate.AddDays(1);
            const string ticker = "DE";
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const double sharesBought = 5;      // 5 shares
            const decimal commission = 5.00m;   // with $5 commission

            const decimal deposit = 10000m;
            IPortfolio target = new Portfolio(testDate, deposit);

            target.AddTransaction(new Buy {SettlementDate = firstBuyDate, Ticker = ticker, Shares = sharesBought, Price = buyPrice, Commission = commission});
            target.AddTransaction(new Buy {SettlementDate = secondBuyDate, Ticker = ticker, Shares = sharesBought, Price = buyPrice, Commission = commission});

            DateTime firstSellDate = secondBuyDate.AddDays(2);
            DateTime secondSellDate = firstSellDate.AddDays(1);
            const decimal sellPrice = 75.00m;   // $75.00 per share
            const double sharesSold = 5;        // 5 shares

            target.AddTransaction(new Sell {SettlementDate = firstSellDate, Ticker = ticker, Shares = sharesSold, Price = sellPrice, Commission = commission});
            target.AddTransaction(new Sell {SettlementDate = secondSellDate, Ticker = ticker, Shares = sharesSold, Price = sellPrice, Commission = commission});

            IList<IHolding> holdings = target.CalculateHoldings(secondSellDate);

            Assert.AreEqual(2, holdings.Count);

            const double sharesInHolding = sharesSold;
            var expected1 = new Holding
            {
                Ticker = ticker,
                Head = secondBuyDate,
                Tail = secondSellDate,
                Shares = sharesInHolding,
                OpenPrice = buyPrice * (decimal)sharesInHolding,
                ClosePrice = sellPrice * (decimal)sharesInHolding
            };
            var expected2 = new Holding
            {
                Ticker = ticker,
                Head = firstBuyDate,
                Tail = firstSellDate,
                Shares = sharesInHolding,
                OpenPrice = buyPrice * (decimal)sharesInHolding,
                ClosePrice = sellPrice * (decimal)sharesInHolding
            };

            Assert.IsTrue(holdings.Contains(expected1));
            Assert.IsTrue(holdings.Contains(expected2));
        }

        [TestMethod]
        public void CalculateHoldingsTestWithTwoPositionsOneBuyOneSellEach()
        {
            DateTime testDate = new DateTime(2001, 1, 1);
            DateTime firstBuyDate = testDate.AddDays(1);
            DateTime secondBuyDate = firstBuyDate.AddDays(1);
            const string firstTicker = "DE";
            const string secondTicker = "IBM";
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const double sharesBought = 5;      // 5 shares
            const decimal commission = 5.00m;   // with $5 commission

            const decimal deposit = 10000m;
            IPortfolio target = new Portfolio(testDate, deposit);

            target.AddTransaction(new Buy { SettlementDate = firstBuyDate, Ticker = firstTicker, Shares = sharesBought, Price = buyPrice, Commission = commission });
            target.AddTransaction(new Buy { SettlementDate = secondBuyDate, Ticker = secondTicker, Shares = sharesBought, Price = buyPrice, Commission = commission });

            DateTime firstSellDate = secondBuyDate.AddDays(2);
            DateTime secondSellDate = firstSellDate.AddDays(1);
            const decimal sellPrice = 75.00m;   // $75.00 per share
            const double sharesSold = 5;        // 5 shares

            target.AddTransaction(new Sell { SettlementDate = firstSellDate, Ticker = firstTicker, Shares = sharesSold, Price = sellPrice, Commission = commission });
            target.AddTransaction(new Sell { SettlementDate = secondSellDate, Ticker = secondTicker, Shares = sharesSold, Price = sellPrice, Commission = commission });

            IList<IHolding> holdings = target.CalculateHoldings(secondSellDate);

            Assert.AreEqual(2, holdings.Count);

            const double sharesInHolding = sharesSold;
            var expected1 = new Holding
            {
                Ticker = secondTicker,
                Head = secondBuyDate,
                Tail = secondSellDate,
                Shares = sharesInHolding,
                OpenPrice = buyPrice * (decimal)sharesInHolding,
                ClosePrice = sellPrice * (decimal)sharesInHolding
            };
            var expected2 = new Holding
            {
                Ticker = firstTicker,
                Head = firstBuyDate,
                Tail = firstSellDate,
                Shares = sharesInHolding,
                OpenPrice = buyPrice * (decimal)sharesInHolding,
                ClosePrice = sellPrice * (decimal)sharesInHolding
            };
            
            Assert.IsTrue(holdings.Contains(expected1));
            Assert.IsTrue(holdings.Contains(expected2));
        }

        [TestMethod]
        public void CalculateHoldingsTestSortOrder()
        {
            DateTime testDate = new DateTime(2001, 1, 1);
            DateTime firstBuyDate = testDate.AddDays(1);
            DateTime secondBuyDate = firstBuyDate.AddDays(1);
            const string firstTicker = "DE";
            const string secondTicker = "IBM";
            const decimal buyPrice = 50.00m;    // $50.00 per share
            const double sharesBought = 5;      // 5 shares
            const decimal commission = 5.00m;   // with $5 commission

            const decimal deposit = 10000m;
            IPortfolio target = new Portfolio(testDate, deposit);

            target.AddTransaction(new Buy { SettlementDate = firstBuyDate, Ticker = firstTicker, Shares = sharesBought, Price = buyPrice, Commission = commission });
            target.AddTransaction(new Buy { SettlementDate = secondBuyDate, Ticker = secondTicker, Shares = sharesBought, Price = buyPrice, Commission = commission });

            DateTime firstSellDate = secondBuyDate.AddDays(2);
            DateTime secondSellDate = firstSellDate.AddDays(1);
            const decimal sellPrice = 75.00m;   // $75.00 per share
            const double sharesSold = 5;        // 5 shares

            target.AddTransaction(new Sell { SettlementDate = firstSellDate, Ticker = firstTicker, Shares = sharesSold, Price = sellPrice, Commission = commission });
            target.AddTransaction(new Sell { SettlementDate = secondSellDate, Ticker = secondTicker, Shares = sharesSold, Price = sellPrice, Commission = commission });

            IList<IHolding> holdings = target.CalculateHoldings(secondSellDate);

            Assert.AreEqual(2, holdings.Count);

            const double sharesInHolding = sharesSold;
            var expected1 = new Holding
            {
                Ticker = secondTicker,
                Head = secondBuyDate,
                Tail = secondSellDate,
                Shares = sharesInHolding,
                OpenPrice = buyPrice * (decimal)sharesInHolding,
                ClosePrice = sellPrice * (decimal)sharesInHolding
            };
            var expected2 = new Holding
            {
                Ticker = firstTicker,
                Head = firstBuyDate,
                Tail = firstSellDate,
                Shares = sharesInHolding,
                OpenPrice = buyPrice * (decimal)sharesInHolding,
                ClosePrice = sellPrice * (decimal)sharesInHolding
            };
            var holding1 = holdings[0];
            var holding2 = holdings[1];
            Assert.AreEqual(expected1, holding1);
            Assert.AreEqual(expected2, holding2);
        }

        #region Mock PriceSeriesProviders
        private class HourlyProvider : PriceSeriesProvider
        {
            public override Resolution BestResolution { get { return Resolution.Hours; } }
            #region Not Implemented
            public override string GetIndexTicker(StockIndex index) { throw new NotImplementedException(); }
            protected override string GetUrlBase() { throw new NotImplementedException(); }
            protected override string GetUrlTicker(string symbol) { throw new NotImplementedException(); }
            protected override string GetUrlHeadDate(DateTime head) { throw new NotImplementedException(); }
            protected override string GetUrlTailDate(DateTime tail) { throw new NotImplementedException(); }
            protected override string GetUrlResolution(Resolution resolution) { throw new NotImplementedException(); }
            protected override string GetUrlDividends() { throw new NotImplementedException(); }
            protected override string GetUrlCsvMarker() { throw new NotImplementedException(); }
            protected override PriceHistoryCsvFile CreatePriceHistoryCsvFile(Stream stream, DateTime head, DateTime tail) { throw new NotImplementedException(); }
            #endregion
        }

        private class DailyProvider : PriceSeriesProvider
        {
            public override Resolution BestResolution { get { return Resolution.Days; } }
            #region Not Implemented
            public override string GetIndexTicker(StockIndex index) { throw new NotImplementedException(); }
            protected override string GetUrlBase() { throw new NotImplementedException(); }
            protected override string GetUrlTicker(string symbol) { throw new NotImplementedException(); }
            protected override string GetUrlHeadDate(DateTime head) { throw new NotImplementedException(); }
            protected override string GetUrlTailDate(DateTime tail) { throw new NotImplementedException(); }
            protected override string GetUrlResolution(Resolution resolution) { throw new NotImplementedException(); }
            protected override string GetUrlDividends() { throw new NotImplementedException(); }
            protected override string GetUrlCsvMarker() { throw new NotImplementedException(); }
            protected override PriceHistoryCsvFile CreatePriceHistoryCsvFile(Stream stream, DateTime head, DateTime tail) { throw new NotImplementedException(); }
            #endregion
        }

        private class WeeklyProvider : PriceSeriesProvider
        {
            public override Resolution BestResolution { get { return Resolution.Weeks; } }
            #region Not Implemented
            public override string GetIndexTicker(StockIndex index) { throw new NotImplementedException(); }
            protected override string GetUrlBase() { throw new NotImplementedException(); }
            protected override string GetUrlTicker(string symbol) { throw new NotImplementedException(); }
            protected override string GetUrlHeadDate(DateTime head) { throw new NotImplementedException(); }
            protected override string GetUrlTailDate(DateTime tail) { throw new NotImplementedException(); }
            protected override string GetUrlResolution(Resolution resolution) { throw new NotImplementedException(); }
            protected override string GetUrlDividends() { throw new NotImplementedException(); }
            protected override string GetUrlCsvMarker() { throw new NotImplementedException(); }
            protected override PriceHistoryCsvFile CreatePriceHistoryCsvFile(Stream stream, DateTime head, DateTime tail) { throw new NotImplementedException(); }
            #endregion
        }
        #endregion

        [TestMethod]
        public void ResolutionEqualsResolutionOfPriceSeriesTest()
        {
            // As of this writing, Settings.PreferredPriceSeriesProvider is only read when target.Resolution is read.
            // This is because the Portfolio's individual positions perform lazy loading of their underlying PriceSeries.
            
            try
            {
                Settings.PreferredPriceSeriesProvider = new HourlyProvider();

                DateTime testDate = new DateTime(2001, 1, 1);
                DateTime firstBuyDate = testDate.AddDays(1);
                DateTime secondBuyDate = firstBuyDate.AddDays(1);
                const string firstTicker = "DE";
                const string secondTicker = "IBM";
                const decimal buyPrice = 50.00m;    // $50.00 per share
                const double sharesBought = 5;      // 5 shares
                const decimal commission = 5.00m;   // with $5 commission

                const decimal deposit = 10000m;
                IPortfolio target = new Portfolio(testDate, deposit);

                target.AddTransaction(new Buy { SettlementDate = firstBuyDate, Ticker = firstTicker, Shares = sharesBought, Price = buyPrice, Commission = commission });
                target.AddTransaction(new Buy { SettlementDate = secondBuyDate, Ticker = secondTicker, Shares = sharesBought, Price = buyPrice, Commission = commission });

                DateTime firstSellDate = secondBuyDate.AddDays(2);
                DateTime secondSellDate = firstSellDate.AddDays(1);
                const decimal sellPrice = 75.00m;   // $75.00 per share
                const double sharesSold = 5;        // 5 shares

                target.AddTransaction(new Sell { SettlementDate = firstSellDate, Ticker = firstTicker, Shares = sharesSold, Price = sellPrice, Commission = commission });
                target.AddTransaction(new Sell { SettlementDate = secondSellDate, Ticker = secondTicker, Shares = sharesSold, Price = sellPrice, Commission = commission });

                var expected = Settings.PreferredPriceSeriesProvider.BestResolution;
                var actual = target.Resolution;

                Assert.AreEqual(expected, actual);
            }
            finally
            {
                Settings.SetDefaultSettings();
            }
        }
    }
}
