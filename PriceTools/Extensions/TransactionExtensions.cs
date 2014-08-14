﻿using System;
using System.Collections.Generic;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools
{
    public static class TransactionExtensions
    {
        private static readonly Dictionary<Type, bool> OpeningTransactions = new Dictionary<Type, bool>();
        private static readonly Dictionary<Type, bool> ClosingTransactions = new Dictionary<Type, bool>();

        static TransactionExtensions()
        {
            OpeningTransactions.Add(typeof (Buy), true);
            OpeningTransactions.Add(typeof (SellShort), true);
            OpeningTransactions.Add(typeof (DividendReinvestment), true);
            OpeningTransactions.Add(typeof (Deposit), true);
            OpeningTransactions.Add(typeof (DividendReceipt), true);

            ClosingTransactions.Add(typeof (Sell), true);
            ClosingTransactions.Add(typeof (BuyToCover), true);
            ClosingTransactions.Add(typeof (Withdrawal), true);
        }

        /// <summary>
        /// Gets a value indicating whether or not a transaction opens or increases an investment in a position.
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static bool IsOpeningTransaction(this ITransaction transaction)
        {
            try
            {
                return OpeningTransactions[transaction.GetType()];
            }
            catch (KeyNotFoundException)
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether or not a transaction opens or increases an investment in a position.
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static bool IsClosingTransaction(this ITransaction transaction)
        {
            try
            {
                return ClosingTransactions[transaction.GetType()];
            }
            catch (KeyNotFoundException)
            {
                return false;
            }
        }
    }
}