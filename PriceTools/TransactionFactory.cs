using System;

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
        /// <param name="date">The date and time this Transaction took place.</param>
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
        /// <param name="date">The date and time this Transaction took place.</param>
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
        /// <param name="date">The date and time this Transaction took place.</param>
        /// <param name="type">The <see cref="PriceTools.OrderType"/> of this Transaction.</param>
        /// <param name="ticker">The ticker of the security bought or sold.</param>
        /// <param name="price">The price at which the Transaction took place.</param>
        /// <param name="shares">The optional number of shares which were traded. Default = 1</param>
        /// <param name="commission">The optional commission paid for this Transaction. Default = $0.00</param>
        public static ITransaction CreateTransaction(DateTime date, OrderType type, string ticker, decimal price, double shares, decimal commission)
        {
            switch (type)
            {
                case OrderType.Buy:
                    return new Transaction(date, OrderType.Buy, ticker, price, shares, commission);
                case OrderType.BuyToCover:
                    return new Transaction(date, OrderType.BuyToCover, ticker, price, shares, commission);
                case OrderType.Deposit:
                    return new Deposit(date, price, ticker);
                case OrderType.Dividend:
                    return new Transaction(date, OrderType.Dividend, ticker, price, shares, commission);
                case OrderType.Sell:
                    return new Transaction(date, OrderType.Sell, ticker, price, shares, commission);
                case OrderType.SellShort:
                    return new Transaction(date, OrderType.SellShort, ticker, price, shares, commission);
                case OrderType.Withdrawal:
                    return new Withdrawal(date, price, ticker);
                default:
                    throw new InvalidOperationException(String.Format("Cannot create ITransaction of type: {0}", type));
            }
        }

        /// <summary>
        /// Constructs a Deposit.
        /// </summary>
        /// <param name="date">The date and time this Deposit took place.</param>
        /// <param name="amount">The amount of cash deposited.</param>
        public static ITransaction CreateDeposit(DateTime date, decimal amount)
        {
            return CreateDeposit(date, amount, Deposit.DefaultTicker);
        }

        /// <summary>
        /// Constructs a Deposit.
        /// </summary>
        /// <param name="date">The date and time this Deposit took place.</param>
        /// <param name="amount">The amount of cash deposited.</param>
        /// <param name="ticker">The ticker purchased with this deposit.</param>
        public static ITransaction CreateDeposit(DateTime date, decimal amount, string ticker)
        {
            return new Deposit(date, amount, ticker);
        }
        
        /// <summary>
        /// Constructs a Withdrawal.
        /// </summary>
        /// <param name="date">The date and time this Withdrawal took place.</param>
        /// <param name="amount">The amount of cash withdrawn.</param>
        public static ITransaction CreateWithdrawal(DateTime date, decimal amount)
        {
            return CreateWithdrawal(date, amount, Deposit.DefaultTicker);
        }

        /// <summary>
        /// Constructs a Withdrawal.
        /// </summary>
        /// <param name="date">The date and time this Withdrawal took place.</param>
        /// <param name="amount">The amount of cash withdrawn.</param>
        /// <param name="ticker">The ticker sold with this withdrawal.</param>
        public static ITransaction CreateWithdrawal(DateTime date, decimal amount, string ticker)
        {
            return new Withdrawal(date, amount, ticker);
        }
    }
}
