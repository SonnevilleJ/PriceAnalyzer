using System.Collections.Generic;
using System.Linq;
using Sonneville.PriceTools.Implementation;
using Sonneville.Statistics;

namespace Sonneville.PriceTools.AutomatedTrading.Test
{
    public class CalculationHelper
    {
        /// <summary>
        /// Calculates the expected result of a call to CalculateStandardDeviation on a <see cref="ISecurityBasket"/>.
        /// </summary>
        /// <param name="holdings"></param>
        /// <returns></returns>
        public static decimal GetExpectedStandardDeviation(IEnumerable<Holding> holdings)
        {
            var values = holdings.Select(h => h.GrossProfit()).ToArray();
            if (values.Count() <= 1) return 0;

            var average = values.Average();
            var squares = values.Select(value => (value - average)*(value - average));
            var sum = squares.Sum();
            return ((sum/values.Count()) - 1).SquareRoot();
        }

        /// <summary>
        /// Calculates the expected result of a call to CalculateMedianProfit on a <see cref="ISecurityBasket"/>.
        /// </summary>
        /// <param name="holdings"></param>
        /// <returns></returns>
        public static decimal GetExpectedMedianProfit(IEnumerable<Holding> holdings)
        {
            var list = holdings.OrderBy(holding => holding.GrossProfit()).ToList();
            if (list.Count == 0) return 0.00m;

            var midpoint = (list.Count/2);
            if (list.Count%2 == 0)
            {
                return (list[midpoint - 1].GrossProfit() + list[midpoint].GrossProfit())/2;
            }
            return list[midpoint].GrossProfit();
        }

        /// <summary>
        /// Calculates the expected result of a call to CalculateNetProfit on a single Position.
        /// </summary>
        public static decimal GetExpectedNetProfit(decimal openingPrice, decimal openingCommission,
                                                   decimal closingShares, decimal closingPrice,
                                                   decimal closingCommission)
        {
            return ((closingPrice - openingPrice)*closingShares) - openingCommission - closingCommission;
        }

        /// <summary>
        /// Calculates the expected result of a call to CalculateGrossProfit on a single Position.
        /// </summary>
        public static decimal GetExpectedGrossProfit(decimal openingPrice, decimal shares, decimal closingPrice)
        {
            return (closingPrice - openingPrice)*shares;
        }
    }
}
