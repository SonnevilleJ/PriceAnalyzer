using Sonneville.PriceTools.AutomatedTrading.Implementation;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools.AutomatedTrading.Extensions
{
    public static class TransactionExtensions
    {
        public static void ApplyToPortfolio(this Transaction transaction, Portfolio portfolio)
        {
            var dividendReceipt = transaction as DividendReceipt;
            var deposit = transaction as Deposit;
            var withdrawal = transaction as Withdrawal;
            var dividendReinvestment = transaction as DividendReinvestment;
            var buy = transaction as Buy;
            var sellShort = transaction as SellShort;
            var sell = transaction as Sell;
            var buyToCover = transaction as BuyToCover;
            if (dividendReceipt != null)
            {
                portfolio.Deposit(dividendReceipt);
            }
            if (deposit != null)
            {
                portfolio.Deposit(deposit);
            }
            if (withdrawal != null)
            {
                portfolio.Withdraw(withdrawal);
            }
            if (dividendReinvestment != null)
            {
                if (dividendReinvestment.Ticker == portfolio.CashTicker)
                {
                    // DividendReceipt already deposited into cash account,
                    // so no need to "buy" the CashTicker. Do nothing.
                }
                else
                {
                    portfolio.Withdraw(dividendReinvestment.SettlementDate, dividendReinvestment.TotalValue);
                    portfolio.AddToPosition(dividendReinvestment);
                }
            }
            if (buy != null)
            {
                portfolio.Withdraw(buy.SettlementDate, buy.TotalValue);
                portfolio.AddToPosition(buy);
            }
            if (sellShort != null)
            {
                portfolio.Withdraw(sellShort.SettlementDate, sellShort.TotalValue);
                portfolio.AddToPosition(sellShort);
            }
            if (sell != null)
            {
                portfolio.AddToPosition(sell);
                portfolio.Deposit(sell.SettlementDate, sell.TotalValue);
            }
            if (buyToCover != null)
            {
                portfolio.AddToPosition(buyToCover);
                portfolio.Deposit(buyToCover.SettlementDate, buyToCover.TotalValue);
            }
        }
    }
}