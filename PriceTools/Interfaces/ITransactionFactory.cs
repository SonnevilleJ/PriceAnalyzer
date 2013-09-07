using System;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools
{
    public interface ITransactionFactory
    {
        /// <summary>
        /// Constructs a deposit-type transaction.
        /// </summary>
        /// <param name="settlementDate">The settlement date of the transaction.</param>
        /// <param name="amount">The amount of funds deposited.</param>
        /// <returns></returns>
        Deposit ConstructDeposit(DateTime settlementDate, decimal amount);

        /// <summary>
        /// Constructs a withdrawal-type transaction.
        /// </summary>
        /// <param name="settlementDate">The settlement date of the transaction.</param>
        /// <param name="amount">The amount of funds withdrawn.</param>
        /// <returns></returns>
        IWithdrawal ConstructWithdrawal(DateTime settlementDate, decimal amount);

        /// <summary>
        /// Constructs a dividend-type transaction where funds were received.
        /// </summary>
        /// <param name="settlementDate">The settlement date of the transaction.</param>
        /// <param name="amount">The amount of funds received.</param>
        /// <returns></returns>
        DividendReceipt ConstructDividendReceipt(DateTime settlementDate, decimal amount);

        /// <summary>
        /// Constructs a transaction where cash is exchanged.
        /// </summary>
        /// <param name="transactionType">The type of <see cref="CashTransaction"/> to construct.</param>
        /// <param name="settlementDate">The settlement date of the transaction.</param>
        /// <param name="amount">The amount of funds in the transaction.</param>
        /// <returns></returns>
        ICashTransaction ConstructCashTransaction(OrderType transactionType, DateTime settlementDate, decimal amount);

        /// <summary>
        /// Constructs a DividendReinvestment.
        /// </summary>
        /// <param name="ticker"></param>
        /// <param name="settlementDate"></param>
        /// <param name="shares"></param>
        /// <param name="price"></param>
        /// <param name="commission"></param>
        /// <returns></returns>
        DividendReinvestment ConstructDividendReinvestment(string ticker, DateTime settlementDate, decimal shares, decimal price, decimal commission = 0);

        /// <summary>
        /// Constructs a Buy.
        /// </summary>
        /// <param name="ticker"></param>
        /// <param name="settlementDate"></param>
        /// <param name="shares"></param>
        /// <param name="price"></param>
        /// <param name="commission"></param>
        /// <returns></returns>
        Buy ConstructBuy(string ticker, DateTime settlementDate, decimal shares, decimal price, decimal commission = 0.00m);

        /// <summary>
        /// Constructs a Sell.
        /// </summary>
        /// <param name="ticker"></param>
        /// <param name="settlementDate"></param>
        /// <param name="shares"></param>
        /// <param name="price"></param>
        /// <param name="commission"></param>
        /// <returns></returns>
        Sell ConstructSell(string ticker, DateTime settlementDate, decimal shares, decimal price, decimal commission = 0.00m);

        /// <summary>
        /// Constructs a BuyToCover.
        /// </summary>
        /// <param name="ticker"></param>
        /// <param name="settlementDate"></param>
        /// <param name="shares"></param>
        /// <param name="price"></param>
        /// <param name="commission"></param>
        /// <returns></returns>
        BuyToCover ConstructBuyToCover(string ticker, DateTime settlementDate, decimal shares, decimal price, decimal commission = 0.00m);

        /// <summary>
        /// Constructs a SellShort.
        /// </summary>
        /// <param name="ticker"></param>
        /// <param name="settlementDate"></param>
        /// <param name="shares"></param>
        /// <param name="price"></param>
        /// <param name="commission"></param>
        /// <returns></returns>
        ISellShort ConstructSellShort(string ticker, DateTime settlementDate, decimal shares, decimal price, decimal commission = 0.00m);

        /// <summary>
        /// Constructs a ShareTransaction.
        /// </summary>
        IShareTransaction ConstructShareTransaction(OrderType type, string ticker, DateTime settlementDate, decimal shares, decimal price, decimal commission);

        /// <summary>
        /// Constructs a Transaction.
        /// </summary>
        /// <param name="type">The <see cref="OrderType"/> of this ShareTransaction.</param>
        /// <param name="date">The date and time this ShareTransaction took place.</param>
        /// <param name="ticker">The ticker of the security bought or sold.</param>
        /// <param name="price">The price at which the ShareTransaction took place.</param>
        /// <param name="shares">The optional number of shares which were traded. Default = 1</param>
        /// <param name="commission">The optional commission paid for this ShareTransaction. Default = $0.00</param>
        ITransaction ConstructTransaction(OrderType type, DateTime date, string ticker, decimal price, decimal shares, decimal commission);
    }
}