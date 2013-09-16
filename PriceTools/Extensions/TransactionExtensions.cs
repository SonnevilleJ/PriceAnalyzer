using System;
using System.Collections.Generic;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools
{
    public static class TransactionExtensions
    {
        private static readonly Dictionary<Type, bool> Dictionary = new Dictionary<Type, bool>();

        static TransactionExtensions()
        {
            Dictionary.Add(typeof (Buy), true);
            Dictionary.Add(typeof (SellShort), true);
            Dictionary.Add(typeof (DividendReinvestment), true);

            Dictionary.Add(typeof (Sell), false);
            Dictionary.Add(typeof (BuyToCover), false);
        }

        /// <summary>
        /// Gets a value indicating whether or not a transaction opens or increases an investment in a position.
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static bool IsOpeningTransaction(this Transaction transaction)
        {
            try
            {
                return Dictionary[transaction.GetType()];
            }
            catch (KeyNotFoundException)
            {
                return false;
            }
        }
    }
}