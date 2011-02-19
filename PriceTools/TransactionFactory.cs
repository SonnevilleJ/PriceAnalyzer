using System;
using System.Globalization;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Factory object which creates IShareTransaction objects.
    /// </summary>
    public static class TransactionFactory
    {
        /// <summary>
        /// Constructs a ShareTransaction.
        /// </summary>
        /// <param name="date">The date and time this ShareTransaction took place.</param>
        /// <param name="type">The <see cref="OrderType"/> of this ShareTransaction.</param>
        /// <param name="ticker">The ticker of the security bought or sold.</param>
        /// <param name="price">The price at which the ShareTransaction took place.</param>
        /// <param name="shares">The optional number of shares which were traded. Default = 1</param>
        /// <param name="commission">The optional commission paid for this ShareTransaction. Default = $0.00</param>
        public static ShareTransaction CreateTransaction(DateTime date, OrderType type, string ticker, decimal price, double shares, decimal commission)
        {
            switch (type)
            {
                case OrderType.Deposit:
                case OrderType.Withdrawal:
                    throw new ArgumentOutOfRangeException("type", String.Format(CultureInfo.CurrentCulture, "Deposits and Withdrawals must be created with CreateDeposit() or CreateWithdrawal()."));
                case OrderType.Buy:
                    return new Buy
                               {
                                   SettlementDate = date,
                                   Ticker = ticker,
                                   Price = price,
                                   Shares = shares,
                                   Commission = commission
                               };
                case OrderType.BuyToCover:
                    return new BuyToCover
                               {
                                   SettlementDate = date,
                                   Ticker = ticker,
                                   Price = price,
                                   Shares = shares,
                                   Commission = commission
                               };
                case OrderType.DividendReceipt:
                    return new DividendReceipt
                               {
                                   SettlementDate = date,
                                   Ticker = ticker,
                                   Price = price,
                                   Shares = shares,
                                   Commission = commission
                               };
                case OrderType.DividendReinvestment:
                    return new DividendReinvestment
                               {
                                   SettlementDate = date,
                                   Ticker = ticker,
                                   Price = price,
                                   Shares = shares,
                                   Commission = commission
                               };
                case OrderType.Sell:
                    return new Sell
                               {
                                   SettlementDate = date,
                                   Ticker = ticker,
                                   Price = price,
                                   Shares = shares,
                                   Commission = commission
                               };
                case OrderType.SellShort:
                    return new SellShort
                               {
                                   SettlementDate = date,
                                   Ticker = ticker,
                                   Price = price,
                                   Shares = shares,
                                   Commission = commission
                               };
                default:
                    throw new ArgumentOutOfRangeException("type", String.Format(CultureInfo.CurrentCulture, "Unknown OrderType: {0}", type));
            }
        }
    }
}
