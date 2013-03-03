﻿using System;
using System.Globalization;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Factory object which creates ShareTransaction objects.
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
            return new Deposit(settlementDate, amount);
        }

        /// <summary>
        /// Constructs a withdrawal-type transaction.
        /// </summary>
        /// <param name="settlementDate">The settlement date of the transaction.</param>
        /// <param name="amount">The amount of funds withdrawn.</param>
        /// <returns></returns>
        public static Withdrawal ConstructWithdrawal(DateTime settlementDate, decimal amount)
        {
            return new Withdrawal(settlementDate, amount);
        }

        /// <summary>
        /// Constructs a dividend-type transaction where funds were received.
        /// </summary>
        /// <param name="settlementDate">The settlement date of the transaction.</param>
        /// <param name="amount">The amount of funds received.</param>
        /// <returns></returns>
        public static DividendReceipt ConstructDividendReceipt(DateTime settlementDate, decimal amount)
        {
            return new DividendReceipt(settlementDate, amount);
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
            return (CashTransaction) ConstructTransaction(transactionType, settlementDate, String.Empty, amount, 0, 0);
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
        public static DividendReinvestment ConstructDividendReinvestment(string ticker, DateTime settlementDate, decimal shares, decimal price, decimal commission = 0)
        {
            return new DividendReinvestment(ticker, settlementDate, shares, price, commission);
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
        public static Buy ConstructBuy(string ticker, DateTime settlementDate, decimal shares, decimal price, decimal commission = 0.00m)
        {
            return new Buy(ticker, settlementDate, shares, price, commission);
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
        public static Sell ConstructSell(string ticker, DateTime settlementDate, decimal shares, decimal price, decimal commission = 0.00m)
        {
            return new Sell(ticker, settlementDate, shares, price, commission);
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
        public static BuyToCover ConstructBuyToCover(string ticker, DateTime settlementDate, decimal shares, decimal price, decimal commission = 0.00m)
        {
            return new BuyToCover(ticker, settlementDate, shares, price, commission);
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
        public static SellShort ConstructSellShort(string ticker, DateTime settlementDate, decimal shares, decimal price, decimal commission = 0.00m)
        {
            return new SellShort(ticker, settlementDate, shares, price, commission);
        }

        /// <summary>
        /// Constructs a ShareTransaction.
        /// </summary>
        public static ShareTransaction ConstructShareTransaction(OrderType type, string ticker, DateTime settlementDate, decimal shares, decimal price, decimal commission)
        {
            return (ShareTransaction) ConstructTransaction(type, settlementDate, ticker, price, shares, commission);
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
        public static ITransaction ConstructTransaction(OrderType type, DateTime date, string ticker, decimal price, decimal shares, decimal commission)
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
