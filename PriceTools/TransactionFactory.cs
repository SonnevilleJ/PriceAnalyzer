using System;
using System.Globalization;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Factory object which creates IShareTransaction objects.
    /// </summary>
    public static class TransactionFactory
    {
        /// <summary>
        /// Constructs a deposit-type transaction.
        /// </summary>
        /// <param name="settlementDate">The settlement date of the transaction.</param>
        /// <param name="amount">The amount of funds deposited.</param>
        /// <returns></returns>
        public static Deposit ConstructDeposit(DateTime settlementDate, decimal amount)
        {
            return new DepositImpl {SettlementDate = settlementDate, Amount = amount};
        }

        /// <summary>
        /// Constructs a withdrawal-type transaction.
        /// </summary>
        /// <param name="settlementDate">The settlement date of the transaction.</param>
        /// <param name="amount">The amount of funds withdrawn.</param>
        /// <returns></returns>
        public static Withdrawal ConstructWithdrawal(DateTime settlementDate, decimal amount)
        {
            return new WithdrawalImpl {SettlementDate = settlementDate, Amount = amount};
        }

        /// <summary>
        /// Constructs a dividend-type transaction where funds were received.
        /// </summary>
        /// <param name="settlementDate">The settlement date of the transaction.</param>
        /// <param name="amount">The amount of funds received.</param>
        /// <returns></returns>
        public static DividendReceipt ConstructDividendReceipt(DateTime settlementDate, decimal amount)
        {
            return new DividendReceiptImpl {SettlementDate = settlementDate, Amount = amount};
        }

        /// <summary>
        /// Constructs a transaction where cash is exchanged.
        /// </summary>
        /// <param name="transactionType">The type of <see cref="CashTransaction"/> to construct.</param>
        /// <param name="settlementDate">The settlement date of the transaction.</param>
        /// <param name="amount">The amount of funds in the transaction.</param>
        /// <returns></returns>
        public static CashTransaction ConstructCashTransaction(OrderType transactionType, DateTime settlementDate, decimal amount)
        {
            switch (transactionType)
            {
                case OrderType.Deposit:
                    return ConstructDeposit(settlementDate, amount);
                case OrderType.Withdrawal:
                    return ConstructWithdrawal(settlementDate, amount);
                case OrderType.DividendReceipt:
                    return ConstructDividendReceipt(settlementDate, amount);
                default:
                    throw new ArgumentOutOfRangeException("transactionType", transactionType, string.Format(Strings.TransactionFactory_ConstructCashTransaction_Cannot_create_a_CashTransaction_for_an_OrderType_of__0__, transactionType));
            }
        }

        /// <summary>
        /// Constructs a DividendReinvestment.
        /// </summary>
        /// <param name="settlementDate"></param>
        /// <param name="ticker"></param>
        /// <param name="price"></param>
        /// <param name="shares"></param>
        /// <param name="commission"></param>
        /// <returns></returns>
        public static IDividendReinvestment ConstructDividendReinvestment(DateTime settlementDate, string ticker, decimal price, double shares, decimal commission = 0.00m)
        {
            return new DividendReinvestment {SettlementDate = settlementDate, Ticker = ticker, Price = price, Shares = shares, Commission = commission};
        }

        /// <summary>
        /// Constructs a Buy.
        /// </summary>
        /// <param name="settlementDate"></param>
        /// <param name="ticker"></param>
        /// <param name="price"></param>
        /// <param name="shares"></param>
        /// <param name="commission"></param>
        /// <returns></returns>
        public static Buy ConstructBuy(DateTime settlementDate, string ticker, decimal price, double shares, decimal commission = 0.00m)
        {
            return new BuyImpl {SettlementDate = settlementDate, Ticker = ticker, Price = price, Shares = shares, Commission = commission};
        }

        /// <summary>
        /// Constructs a Sell.
        /// </summary>
        /// <param name="settlementDate"></param>
        /// <param name="ticker"></param>
        /// <param name="price"></param>
        /// <param name="shares"></param>
        /// <param name="commission"></param>
        /// <returns></returns>
        public static Sell ConstructSell(DateTime settlementDate, string ticker, decimal price, double shares, decimal commission = 0.00m)
        {
            return new SellImpl {SettlementDate = settlementDate, Ticker = ticker, Price = price, Shares = shares, Commission = commission};
        }

        /// <summary>
        /// Constructs a BuyToCover.
        /// </summary>
        /// <param name="settlementDate"></param>
        /// <param name="ticker"></param>
        /// <param name="price"></param>
        /// <param name="shares"></param>
        /// <param name="commission"></param>
        /// <returns></returns>
        public static BuyToCover ConstructBuyToCover(DateTime settlementDate, string ticker, decimal price, double shares, decimal commission = 0.00m)
        {
            return new BuyToCover {SettlementDate = settlementDate, Ticker = ticker, Price = price, Shares = shares, Commission = commission};
        }

        /// <summary>
        /// Constructs a SellShort.
        /// </summary>
        /// <param name="settlementDate"></param>
        /// <param name="ticker"></param>
        /// <param name="price"></param>
        /// <param name="shares"></param>
        /// <param name="commission"></param>
        /// <returns></returns>
        public static SellShort ConstructSellShort(DateTime settlementDate, string ticker, decimal price, double shares, decimal commission = 0.00m)
        {
            return new SellShort {SettlementDate = settlementDate, Ticker = ticker, Price = price, Shares = shares, Commission = commission};
        }

        /// <summary>
        /// Constructs a ShareTransaction.
        /// </summary>
        public static IShareTransaction ConstructShareTransaction(OrderType type, DateTime settlementDate, string ticker, decimal price, double shares, decimal commission)
        {
            return (IShareTransaction) CreateTransaction(settlementDate, type, ticker, price, shares, commission);
        }

        /// <summary>
        /// Constructs a Transaction.
        /// </summary>
        /// <param name="date">The date and time this ShareTransaction took place.</param>
        /// <param name="type">The <see cref="OrderType"/> of this ShareTransaction.</param>
        /// <param name="ticker">The ticker of the security bought or sold.</param>
        /// <param name="price">The price at which the ShareTransaction took place.</param>
        /// <param name="shares">The optional number of shares which were traded. Default = 1</param>
        /// <param name="commission">The optional commission paid for this ShareTransaction. Default = $0.00</param>
        public static ITransaction CreateTransaction(DateTime date, OrderType type, string ticker, decimal price, double shares, decimal commission)
        {
            switch (type)
            {
                case OrderType.Deposit:
                    return ConstructDeposit(date, price);
                case OrderType.Withdrawal:
                    return ConstructWithdrawal(date, price);
                case OrderType.Buy:
                    return ConstructBuy(date, ticker, price, shares, commission);
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
                    return ConstructDividendReceipt(date, price);
                case OrderType.DividendReinvestment:
                    return ConstructDividendReinvestment(date, ticker, price, shares, commission);
                case OrderType.Sell:
                    return ConstructSell(date, ticker, price, shares, commission);
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
                    throw new ArgumentOutOfRangeException("type", String.Format(CultureInfo.CurrentCulture, Strings.TransactionFactory_CreateTransaction_Unknown_OrderType___0_, type));
            }
        }
    }
}
