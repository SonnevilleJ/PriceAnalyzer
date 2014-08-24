using System;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools
{
    public interface ITransactionFactory
    {
        Deposit ConstructDeposit(DateTime settlementDate, decimal amount);

        Withdrawal ConstructWithdrawal(DateTime settlementDate, decimal amount);

        DividendReceipt ConstructDividendReceipt(DateTime settlementDate, decimal amount);

        CashTransaction ConstructCashTransaction(OrderType transactionType, DateTime settlementDate, decimal amount);

        DividendReinvestment ConstructDividendReinvestment(string ticker, DateTime settlementDate, decimal shares, decimal price, decimal commission = 0);

        Buy ConstructBuy(string ticker, DateTime settlementDate, decimal shares, decimal price, decimal commission = 0.00m);

        Sell ConstructSell(string ticker, DateTime settlementDate, decimal shares, decimal price, decimal commission = 0.00m);

        BuyToCover ConstructBuyToCover(string ticker, DateTime settlementDate, decimal shares, decimal price, decimal commission = 0.00m);

        SellShort ConstructSellShort(string ticker, DateTime settlementDate, decimal shares, decimal price, decimal commission = 0.00m);

        ShareTransaction ConstructShareTransaction(OrderType type, string ticker, DateTime settlementDate, decimal shares, decimal price, decimal commission);

        Transaction ConstructTransaction(OrderType type, DateTime date, string ticker, decimal price, decimal shares, decimal commission);
    }
}