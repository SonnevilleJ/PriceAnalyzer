using System;
using System.Globalization;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Factory object which creates ShareTransaction objects.
    /// </summary>
    public class TransactionFactory : ITransactionFactory
    {
        /// <summary>
        /// Constructs a deposit-type transaction.
        /// </summary>
        /// <param name="settlementDate">The settlement date of the transaction.</param>
        /// <param name="amount">The amount of funds deposited.</param>
        /// <returns></returns>
        public IDeposit ConstructDeposit(DateTime settlementDate, decimal amount)
        {
            return new DepositImpl(settlementDate, amount);
        }

        /// <summary>
        /// Constructs a withdrawal-type transaction.
        /// </summary>
        /// <param name="settlementDate">The settlement date of the transaction.</param>
        /// <param name="amount">The amount of funds withdrawn.</param>
        /// <returns></returns>
        public IWithdrawal ConstructWithdrawal(DateTime settlementDate, decimal amount)
        {
            return new WithdrawalImpl(settlementDate, amount);
        }

        /// <summary>
        /// Constructs a dividend-type transaction where funds were received.
        /// </summary>
        /// <param name="settlementDate">The settlement date of the transaction.</param>
        /// <param name="amount">The amount of funds received.</param>
        /// <returns></returns>
        public IDividendReceipt ConstructDividendReceipt(DateTime settlementDate, decimal amount)
        {
            return new DividendReceiptImpl(settlementDate, amount);
        }

        /// <summary>
        /// Constructs a transaction where cash is exchanged.
        /// </summary>
        /// <param name="transactionType">The type of <see cref="CashTransactionImpl"/> to construct.</param>
        /// <param name="settlementDate">The settlement date of the transaction.</param>
        /// <param name="amount">The amount of funds in the transaction.</param>
        /// <returns></returns>
        public ICashTransaction ConstructCashTransaction(OrderType transactionType, DateTime settlementDate, decimal amount)
        {
            return (CashTransactionImpl) ConstructTransaction(transactionType, settlementDate, String.Empty, amount, 0, 0);
        }

        /// <summary>
        /// Constructs a DividendReinvestment.
        /// </summary>
        /// <param name="ticker"></param>
        /// <param name="settlementDate"></param>
        /// <param name="shares"></param>
        /// <param name="price"></param>
        /// <param name="commission"></param>
        /// <returns></returns>
        public IDividendReinvestment ConstructDividendReinvestment(string ticker, DateTime settlementDate, decimal shares, decimal price, decimal commission = 0)
        {
            return new DividendReinvestmentImpl(ticker, settlementDate, shares, price, commission);
        }

        /// <summary>
        /// Constructs a Buy.
        /// </summary>
        /// <param name="ticker"></param>
        /// <param name="settlementDate"></param>
        /// <param name="shares"></param>
        /// <param name="price"></param>
        /// <param name="commission"></param>
        /// <returns></returns>
        public IBuy ConstructBuy(string ticker, DateTime settlementDate, decimal shares, decimal price, decimal commission = 0.00m)
        {
            return new BuyImpl(ticker, settlementDate, shares, price, commission);
        }

        /// <summary>
        /// Constructs a Sell.
        /// </summary>
        /// <param name="ticker"></param>
        /// <param name="settlementDate"></param>
        /// <param name="shares"></param>
        /// <param name="price"></param>
        /// <param name="commission"></param>
        /// <returns></returns>
        public ISell ConstructSell(string ticker, DateTime settlementDate, decimal shares, decimal price, decimal commission = 0.00m)
        {
            return new SellImpl(ticker, settlementDate, shares, price, commission);
        }

        /// <summary>
        /// Constructs a BuyToCover.
        /// </summary>
        /// <param name="ticker"></param>
        /// <param name="settlementDate"></param>
        /// <param name="shares"></param>
        /// <param name="price"></param>
        /// <param name="commission"></param>
        /// <returns></returns>
        public IBuyToCover ConstructBuyToCover(string ticker, DateTime settlementDate, decimal shares, decimal price, decimal commission = 0.00m)
        {
            return new BuyToCoverImpl(ticker, settlementDate, shares, price, commission);
        }

        /// <summary>
        /// Constructs a SellShort.
        /// </summary>
        /// <param name="ticker"></param>
        /// <param name="settlementDate"></param>
        /// <param name="shares"></param>
        /// <param name="price"></param>
        /// <param name="commission"></param>
        /// <returns></returns>
        public ISellShort ConstructSellShort(string ticker, DateTime settlementDate, decimal shares, decimal price, decimal commission = 0.00m)
        {
            return new SellShortImpl(ticker, settlementDate, shares, price, commission);
        }

        /// <summary>
        /// Constructs a ShareTransaction.
        /// </summary>
        public IShareTransaction ConstructShareTransaction(OrderType type, string ticker, DateTime settlementDate, decimal shares, decimal price, decimal commission)
        {
            return (ShareTransactionImpl) ConstructTransaction(type, settlementDate, ticker, price, shares, commission);
        }

        /// <summary>
        /// Constructs a Transaction.
        /// </summary>
        /// <param name="type">The <see cref="OrderType"/> of this ShareTransaction.</param>
        /// <param name="date">The date and time this ShareTransaction took place.</param>
        /// <param name="ticker">The ticker of the security bought or sold.</param>
        /// <param name="price">The price at which the ShareTransaction took place.</param>
        /// <param name="shares">The optional number of shares which were traded. Default = 1</param>
        /// <param name="commission">The optional commission paid for this ShareTransaction. Default = $0.00</param>
        public ITransaction ConstructTransaction(OrderType type, DateTime date, string ticker, decimal price, decimal shares, decimal commission)
        {
            switch (type)
            {
                case OrderType.Deposit:
                    return ConstructDeposit(date, price);
                case OrderType.Withdrawal:
                    return ConstructWithdrawal(date, price);
                case OrderType.Buy:
                    return ConstructBuy(ticker, date, shares, price, commission);
                case OrderType.BuyToCover:
                    return ConstructBuyToCover(ticker, date, shares, price, commission);
                case OrderType.DividendReceipt:
                    return ConstructDividendReceipt(date, price);
                case OrderType.DividendReinvestment:
                    return ConstructDividendReinvestment(ticker, date, shares, price, commission);
                case OrderType.Sell:
                    return ConstructSell(ticker, date, shares, price, commission);
                case OrderType.SellShort:
                    return ConstructSellShort(ticker, date, shares, price, commission);
                default:
                    throw new ArgumentOutOfRangeException("type", String.Format(CultureInfo.CurrentCulture, Strings.TransactionFactory_CreateTransaction_Unknown_OrderType___0_, type));
            }
        }
    }
}
