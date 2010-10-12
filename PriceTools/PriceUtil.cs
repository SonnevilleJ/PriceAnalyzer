using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sonneville.PriceTools
{
    public static class PriceUtil
    {
        public static decimal GetPerSharePrice(string ticker)
        {
            return GetPerSharePrice(ticker, DateTime.Now);
        }

        private static decimal GetPerSharePrice(string ticker, DateTime date)
        {
            throw new NotImplementedException();
        }

        public static string DecimalToCurrency(decimal value)
        {
            return value.ToString("C");
        }
    }
}
