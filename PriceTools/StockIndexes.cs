using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// A group of common stock indexes.
    /// </summary>
    public static class StockIndexes
    {
        /// <summary>
        /// Gets the ticker symbol for the Standard &amp; Poors 500 index.
        /// </summary>
        public static string StandardAndPoors500
        {
            get
            {
                return "SPX";
            }
        }

        /// <summary>
        /// Gets the ticker symbol for the Dow Jones Industrial Average.
        /// </summary>
        public static string DowJonesIndustrialAverage
        {
            get
            {
                return "INDU";
            }
        }
    }
}
