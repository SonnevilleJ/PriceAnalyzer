﻿using System;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Factory object which creates ITransaction objects.
    /// </summary>
    public static class TransactionFactory
    {
        /// <summary>
        /// Constructs a Transaction.
        /// </summary>
        /// <param name="asOfDate">The asOfDate and time this Transaction took place.</param>
        /// <param name="type">The <see cref="PriceTools.OrderType"/> of this Transaction.</param>
        /// <param name="ticker">The ticker of the security bought or sold.</param>
        /// <param name="price">The price at which the Transaction took place.</param>
        public static ITransaction CreateTransaction(DateTime date, OrderType type, string ticker, decimal price)
        {
            return CreateTransaction(date, type, ticker, price, 1.0);
        }

        /// <summary>
        /// Constructs a Transaction.
        /// </summary>
        /// <param name="asOfDate">The asOfDate and time this Transaction took place.</param>
        /// <param name="type">The <see cref="PriceTools.OrderType"/> of this Transaction.</param>
        /// <param name="ticker">The ticker of the security bought or sold.</param>
        /// <param name="price">The price at which the Transaction took place.</param>
        /// <param name="shares">The optional number of shares which were traded. Default = 1</param>
        public static ITransaction CreateTransaction(DateTime date, OrderType type, string ticker, decimal price, double shares)
        {
            return CreateTransaction(date, type, ticker, price, shares, 0.00m);
        }

        /// <summary>
        /// Constructs a Transaction.
        /// </summary>
        /// <param name="asOfDate">The asOfDate and time this Transaction took place.</param>
        /// <param name="type">The <see cref="PriceTools.OrderType"/> of this Transaction.</param>
        /// <param name="ticker">The ticker of the security bought or sold.</param>
        /// <param name="price">The price at which the Transaction took place.</param>
        /// <param name="shares">The optional number of shares which were traded. Default = 1</param>
        /// <param name="commission">The optional commission paid for this Transaction. Default = $0.00</param>
        public static ITransaction CreateTransaction(DateTime date, OrderType type, string ticker, decimal price, double shares, decimal commission)
        {
            return new Transaction(date, type, ticker, price, shares, commission);

            //switch (type)
            //{
            //    case OrderType.Buy:
            //        return new Transaction(asOfDate, OrderType.Buy, ticker, price, shares, commission);
            //    case OrderType.BuyToCover:
            //        return new Transaction(asOfDate, OrderType.BuyToCover, ticker, price, shares, commission);
            //    case OrderType.Deposit:
            //        return new Deposit(asOfDate, price);
            //    case OrderType.DividendReceipt:
            //        return new Transaction(asOfDate, OrderType.DividendReceipt, ticker, price, shares, commission);
            //    case OrderType.DividendReinvestment:
            //        return new Transaction(asOfDate, OrderType.DividendReinvestment, ticker, price, shares, commission);
            //    case OrderType.Sell:
            //        return new Transaction(asOfDate, OrderType.Sell, ticker, price, shares, commission);
            //    case OrderType.SellShort:
            //        return new Transaction(asOfDate, OrderType.SellShort, ticker, price, shares, commission);
            //    case OrderType.Withdrawal:
            //        return new Withdrawal(asOfDate, price, ticker);
            //    default:
            //        throw new InvalidOperationException(String.Format("Cannot create ITransaction of type: {0}", type));
            //}
        }

        /// <summary>
        /// Constructs a Deposit.
        /// </summary>
        /// <param name="asOfDate">The asOfDate and time this Deposit took place.</param>
        /// <param name="amount">The amount of cash deposited.</param>
        public static ITransaction CreateDeposit(DateTime date, decimal amount)
        {
            return new Deposit(date, amount);
        }
        
        /// <summary>
        /// Constructs a Withdrawal.
        /// </summary>
        /// <param name="asOfDate">The asOfDate and time this Withdrawal took place.</param>
        /// <param name="amount">The amount of cash withdrawn.</param>
        public static ITransaction CreateWithdrawal(DateTime date, decimal amount)
        {
            return new Withdrawal(date, amount);
        }
    }
}
