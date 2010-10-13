using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// A utility capable of performing several commonly performed tasks involving price data.
    /// </summary>
    public static class PriceUtil
    {
        public static decimal GetPerSharePrice(string ticker)
        {
            return GetPerSharePrice(ticker, DateTime.Now);
        }

        public static decimal GetPerSharePrice(string ticker, DateTime date)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Formats a decimal value (typically the price of a security) as currency.
        /// </summary>
        /// <param name="value">The price value to format.</param>
        /// <returns>A currency formatted string."</returns>
        public static string FormatDecimalAsCurrency(decimal value)
        {
            return value.ToString("C");
        }
    }
}
