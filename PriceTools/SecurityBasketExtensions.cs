using System;
using System.Collections.Generic;
using System.Linq;
using Sonneville.PriceTools.Data;
using Sonneville.PriceTools.Extensions;
using Statistics;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// A class which holds extension methods for <see cref="SecurityBasket"/>.
    /// </summary>
    public static class SecurityBasketExtensions
    {
        /// <summary>
        /// Gets an <see cref="IList{IHolding}"/> from the transactions in the Position.
        /// </summary>
        /// <param name="basket"></param>
        /// <param name="settlementDate">The latest date used to include a transaction in the calculation.</param>
        /// <returns>An <see cref="IList{IHolding}"/> of the transactions in the Position.</returns>
        public static IList<IHolding> CalculateHoldings(this SecurityBasket basket, DateTime settlementDate)
        {
            return CalculateHoldings(basket.Transactions, settlementDate);
        }

        /// <summary>
        /// Gets an <see cref="IList{IHolding}"/> from the transactions in the Position.
        /// </summary>
        /// <param name="transactions"></param>
        /// <param name="settlementDate">The latest date used to include a transaction in the calculation.</param>
        /// <returns>An <see cref="IList{IHolding}"/> of the transactions in the Position.</returns>
        public static IList<IHolding> CalculateHoldings(this IEnumerable<Transaction> transactions, DateTime settlementDate)
        {
            var result = new List<IHolding>();
            var groups = transactions.Where(t => t is ShareTransaction).Cast<ShareTransaction>().GroupBy(t => t.Ticker);
            foreach (var grouping in groups)
            {
                var buys = grouping.Where(t => t is OpeningTransaction).Where(t => t.SettlementDate < settlementDate).OrderBy(t => t.SettlementDate);
                var buysUsed = 0;
                var unusedSharesInCurrentBuy = 0.0m;
                ShareTransaction buy = null;

                var sells = grouping.Where(t => t is ClosingTransaction).Where(t => t.SettlementDate <= settlementDate).OrderBy(t => t.SettlementDate);
                foreach (var sell in sells)
                {
                    // collect shares from most recent buy
                    var sharesToMatch = sell.Shares;
                    while (sharesToMatch > 0)
                    {
                        // find a matching purchase and record a new holding
                        // must keep track of remaining shares in corresponding purchase
                        if (unusedSharesInCurrentBuy == 0)
                        {
                            buy = buys.Skip(buysUsed).First();
                            unusedSharesInCurrentBuy = buy.Shares;
                        }

                        if (buy != null)
                        {
                            var availableShares = unusedSharesInCurrentBuy;
                            var neededShares = sharesToMatch;
                            decimal shares;
                            if (availableShares >= neededShares)
                            {
                                shares = neededShares;
                                unusedSharesInCurrentBuy = availableShares - shares;
                                if (unusedSharesInCurrentBuy == 0) buysUsed++;
                            }
                            else
                            {
                                shares = availableShares;
                                unusedSharesInCurrentBuy -= shares;
                                buysUsed++;
                            }
                            var holding = new Holding
                                              {
                                                  Ticker = sell.Ticker,
                                                  Head = buy.SettlementDate,
                                                  Tail = sell.SettlementDate,
                                                  Shares = shares,
                                                  OpenPrice = buy.Price,
                                                  OpenCommission = buy.Commission,
                                                  ClosePrice = -1 * sell.Price,
                                                  CloseCommission = sell.Commission
                                              };
                            result.Add(holding);

                            sharesToMatch -= shares;
                        }
                    }
                }
            }
            return result.OrderBy(h => h.Tail).ToList();
        }

        /// <summary>
        ///   Gets the gross investment of this Position, ignoring any proceeds and commissions.
        /// </summary>
        /// <param name="basket"></param>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The total amount spent on share purchases as a negative number.</returns>
        public static decimal CalculateCost(this SecurityBasket basket, DateTime settlementDate)
        {
            return basket.Transactions.AsParallel().Where(t => t is ShareTransaction).Cast<ShareTransaction>().Where(t => t is OpeningTransaction)
                .Where(transaction => transaction.SettlementDate <= settlementDate)
                .Sum(transaction => transaction.Price * transaction.Shares);
        }

        /// <summary>
        ///   Gets the gross proceeds of this Position, ignoring all totalCosts and commissions.
        /// </summary>
        /// <param name="basket"></param>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The total amount of proceeds from share sales as a positive number.</returns>
        public static decimal CalculateProceeds(this SecurityBasket basket, DateTime settlementDate)
        {
            return -1 * basket.Transactions.AsParallel().Where(t => t is ShareTransaction).Cast<ShareTransaction>().Where(t => t is ClosingTransaction)
                   .Where(transaction => transaction.SettlementDate <= settlementDate)
                   .Sum(transaction => transaction.Price * transaction.Shares);
        }

        /// <summary>
        ///   Gets the total commissions paid as of a given date.
        /// </summary>
        /// <param name="basket"></param>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The total amount of commissions from <see cref = "ShareTransaction" />s as a negative number.</returns>
        public static decimal CalculateCommissions(this SecurityBasket basket, DateTime settlementDate)
        {
            return basket.Transactions.AsParallel().Where(t=>t is ShareTransaction).Cast<ShareTransaction>()
                .Where(transaction => transaction.SettlementDate <= settlementDate)
                .Sum(transaction => transaction.Commission);
        }

        /// <summary>
        ///   Gets the value of any shares held the Position as of a given date.
        /// </summary>
        /// <param name="basket"></param>
        /// <param name="provider">The <see cref="IPriceDataProvider"/> to use when requesting price data.</param>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The value of the shares held in the Position as of the given date.</returns>
        public static decimal CalculateMarketValue(this SecurityBasket basket, IPriceDataProvider provider, DateTime settlementDate)
        {
            var allTransactions = basket.Transactions.AsParallel().Where(t => t is ShareTransaction).Cast<ShareTransaction>();
            var groups = allTransactions.GroupBy(t => t.Ticker);

            var total = 0.00m;
            foreach (var transactions in groups)
            {
                var heldShares = transactions.GetHeldShares(settlementDate);
                if (heldShares == 0) continue;

                var priceSeries = PriceSeriesFactory.CreatePriceSeries(transactions.First().Ticker);
                if (!priceSeries.HasValueInRange(settlementDate)) priceSeries.UpdatePriceData(provider, settlementDate);
                var price = priceSeries[settlementDate];
                total += heldShares * price;
            }
            return total;
        }

        /// <summary>
        ///   Gets the net shares held at a given date.
        /// </summary>
        /// <param name="shareTransactions"></param>
        /// <param name = "date">The <see cref = "DateTime" /> to use.</param>
        public static decimal GetHeldShares(this IEnumerable<ShareTransaction> shareTransactions, DateTime date)
        {
            var sum = 0m;
            foreach (var transaction in shareTransactions)
            {
                if (transaction is OpeningTransaction) sum += transaction.Shares;
                if (transaction is ClosingTransaction) sum -= transaction.Shares;
            }
            return sum;
        }

        /// <summary>
        ///   Gets the cumulative number of shares that have ever been owned before a given date.
        /// </summary>
        /// <param name="shareTransactions"></param>
        /// <param name = "date">The <see cref = "DateTime" /> to use.</param>
        private static decimal GetOpenedShares(this IEnumerable<ShareTransaction> shareTransactions, DateTime date)
        {
            return shareTransactions.AsParallel().Where(t => t is OpeningTransaction && t.SettlementDate <= date).Sum(t => t.Shares);
        }

        /// <summary>
        ///   Gets the total number of shares that were owned but are no longer owned.
        /// </summary>
        /// <param name="shareTransactions"></param>
        /// <param name = "date">The <see cref = "DateTime" /> to use.</param>
        private static decimal GetClosedShares(this IEnumerable<ShareTransaction> shareTransactions, DateTime date)
        {
            return shareTransactions.AsParallel().Where(t => t is ClosingTransaction && t.SettlementDate <= date).Sum(t => t.Shares);
        }

        /// <summary>
        ///   Gets the average cost of all held shares in a <see cref="Position"/> as of a given date.
        /// </summary>
        /// <param name="position">The <see cref="Position"/> for which to calculate average cost.</param>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The average cost of all shares held at <paramref name = "settlementDate" />.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static decimal CalculateAverageCost(this Position position, DateTime settlementDate)
        {
            var transactions = position.Transactions.Cast<ShareTransaction>()
                .Where(transaction => transaction.SettlementDate <= settlementDate)
                .OrderBy(transaction => transaction.SettlementDate).ToList();
            var count = transactions.Count();

            var totalCost = 0.00m;
            var shares = 0.0m;

            for (var i = 0; i < count; i++)
            {
                if (transactions[i] is OpeningTransaction)
                {
                    totalCost += (transactions[i].Price*transactions[i].Shares);
                    shares += transactions[i].Shares;
                }
                else if (transactions[i] is ClosingTransaction)
                {
                    totalCost -= ((totalCost/shares)*transactions[i].Shares);
                    shares -= transactions[i].Shares;
                }
            }

            return totalCost / shares;
        }

        /// <summary>
        ///   Gets the net profit of this SecurityBasket, including any commissions, as of a given date.
        /// </summary>
        /// <param name="basket"></param>
        /// <param name="settlementDate">The <see cref="DateTime"/> to use.</param>
        public static decimal CalculateNetProfit(this SecurityBasket basket, DateTime settlementDate)
        {
            var allHoldings = basket.CalculateHoldings(settlementDate);
            if (allHoldings.Count == 0) return 0;

            var positionGroups = allHoldings.GroupBy(h => h.Ticker);
            return (from holdings in positionGroups
                    from holding in holdings
                    let open = holding.OpenPrice
                    let close = holding.ClosePrice
                    let profit = close - open
                    let shares = holding.Shares
                    select (profit*shares) - holding.OpenCommission - holding.CloseCommission).Sum();
        }

        /// <summary>
        ///   Gets the gross profit of this SecurityBasket, excluding any commissions, as of a given date.
        /// </summary>
        /// <param name="basket"></param>
        /// <param name="settlementDate">The <see cref="DateTime"/> to use.</param>
        public static decimal CalculateGrossProfit(this SecurityBasket basket, DateTime settlementDate)
        {
            var allHoldings = basket.CalculateHoldings(settlementDate);
            if (allHoldings.Count == 0) return 0;

            var positionGroups = allHoldings.GroupBy(h => h.Ticker);

            return (from holdings in positionGroups
                    from holding in holdings
                    let open = holding.OpenPrice
                    let close = holding.ClosePrice
                    let profit = close - open
                    let shares = holding.Shares
                    select profit*shares).Sum();
        }

        /// <summary>
        ///   Gets the net rate of return on an annual basis for this SecurityBasket, after commissions.
        /// </summary>
        /// <param name="basket"></param>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <remarks>
        ///   Assumes a year has 365 days.
        /// </remarks>
        public static decimal? CalculateAnnualGrossReturn(this SecurityBasket basket, DateTime settlementDate)
        {
            if(basket == null) throw new ArgumentNullException("basket", Strings.SecurityBasketExtensions_CalculateAnnualGrossReturn_Parameter_basket_cannot_be_null_);

            var totalReturn = basket.CalculateGrossReturn(settlementDate);
            return totalReturn == null ? null : Annualize(totalReturn.Value, basket.Tail, basket.Head);
        }

        /// <summary>
        ///   Gets the net rate of return on an annual basis for this SecurityBasket, after commissions.
        /// </summary>
        /// <param name="basket"></param>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <remarks>
        ///   Assumes a year has 365 days.
        /// </remarks>
        public static decimal? CalculateAnnualNetReturn(this SecurityBasket basket, DateTime settlementDate)
        {
            var totalReturn = basket.CalculateNetReturn(settlementDate);
            return totalReturn == null ? null : Annualize(totalReturn.Value, basket.Tail, basket.Head);
        }

        private static decimal? Annualize(decimal totalReturn, DateTime tail, DateTime head)
        {
            // decimal division is imperfect around 25 decimal places. Round to 20 decimal places to reduce errors.
            var time = ((tail - head).Days / 365.0m);
            return Math.Round(totalReturn/time, 20);
        }

        /// <summary>
        ///   Gets the net rate of return for this SecurityBasket, after commissions.
        /// </summary>
        /// <param name="basket"></param>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>Returns the net rate of return, after commission, expressed as a percentage. Returns null if return cannot be calculated.</returns>
        public static decimal? CalculateNetReturn(this SecurityBasket basket, DateTime settlementDate)
        {
            var allHoldings = basket.CalculateHoldings(settlementDate);
            if (allHoldings.Count == 0) return null;

            var totalShares = allHoldings.Sum(h => h.Shares);
            var positionGroups = allHoldings.GroupBy(h => h.Ticker);
            return (from holdings in positionGroups
                    let positionShares = holdings.Sum(h => h.Shares)
                    from holding in holdings
                    let open = holding.OpenPrice
                    let close = holding.ClosePrice
                    let profit = close - open - ((holding.OpenCommission + holding.CloseCommission)/holding.Shares)
                    let increase = profit/open
                    select increase*((holding.Shares/positionShares)*(positionShares/totalShares))).Sum();
        }

        /// <summary>
        ///   Gets the gross rate of return for this SecurityBasket, not accounting for commissions.
        /// </summary>
        /// <param name="basket"></param>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>Returns the gross rate of return, before commission, expressed as a percentage. Returns null if return cannot be calculated.</returns>
        public static decimal? CalculateGrossReturn(this SecurityBasket basket, DateTime settlementDate)
        {
            var allHoldings = basket.CalculateHoldings(settlementDate);
            if (allHoldings.Count == 0) return null;

            var totalShares = allHoldings.Sum(h => h.Shares);
            var positionGroups = allHoldings.GroupBy(h => h.Ticker);
            return (from holdings in positionGroups
                    let positionShares = holdings.Sum(h => h.Shares)
                    from holding in holdings
                    let open = holding.OpenPrice
                    let close = holding.ClosePrice
                    let profit = close - open
                    let increase = profit/open
                    select increase*((holding.Shares/positionShares)*(positionShares/totalShares))).Sum();
        }

        /// <summary>
        /// Calculates the mean gross profit of a basket's <see cref="IHolding"/>s.
        /// </summary>
        /// <param name="basket"></param>
        /// <param name="settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>Returns the mean gross profit from a <see cref="SecurityBasket"/>.</returns>
        public static decimal CalculateAverageProfit(this SecurityBasket basket, DateTime settlementDate)
        {
            return basket.CalculateHoldings(settlementDate).AsParallel().Average(holding => holding.GrossProfit());
        }

        /// <summary>
        /// Calculates the median gross profit of a basket's <see cref="IHolding"/>s.
        /// </summary>
        /// <param name="basket"></param>
        /// <param name="settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>Returns the median gross profit from a <see cref="SecurityBasket"/>.</returns>
        public static decimal CalculateMedianProfit(this SecurityBasket basket, DateTime settlementDate)
        {
            return basket.CalculateHoldings(settlementDate).Select(h => h.GrossProfit()).Median();
        }

        /// <summary>
        /// Calculates the standard deviation of all profits in a <see cref="SecurityBasket"/>.
        /// </summary>
        /// <param name="basket"></param>
        /// <param name="settlementDate">The <see cref="DateTime"/> to use.</param>
        /// <returns>Returns the standard deviation of the profits from a <see cref="SecurityBasket"/>.</returns>
        public static decimal CalculateStandardDeviation(this SecurityBasket basket, DateTime settlementDate)
        {
            return basket.CalculateHoldings(settlementDate).Select(h => h.GrossProfit()).StandardDeviation();
        }
    }
}