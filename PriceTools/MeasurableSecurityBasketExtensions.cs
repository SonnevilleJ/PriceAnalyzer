using System;
using System.Collections.Generic;
using System.Linq;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// A class which holds extension methods for <see cref="MeasurableSecurityBasket"/>.
    /// </summary>
    public static class MeasurableSecurityBasketExtensions
    {
        /// <summary>
        /// Gets an <see cref="IList{IHolding}"/> from the transactions in the Position.
        /// </summary>
        /// <param name="basket"></param>
        /// <param name="settlementDate">The latest date used to include a transaction in the calculation.</param>
        /// <returns>An <see cref="IList{IHolding}"/> of the transactions in the Position.</returns>
        public static IList<IHolding> CalculateHoldings(this MeasurableSecurityBasket basket, DateTime settlementDate)
        {
            var result = new List<IHolding>();
            var groups = basket.Transactions.Where(t => t is ShareTransaction).Cast<ShareTransaction>().GroupBy(t => t.Ticker);
            foreach (var transactions in groups)
            {
                var buys = transactions.Where(t => t is OpeningTransaction).Where(t => t.SettlementDate < settlementDate).OrderByDescending(t => t.SettlementDate);
                var buysUsed = 0;
                var unusedSharesInCurrentBuy = 0.0;
                ShareTransaction buy = null;

                var sells = transactions.Where(t => t is ClosingTransaction).Where(t => t.SettlementDate <= settlementDate).OrderByDescending(t => t.SettlementDate);
                foreach (var sell in sells)
                {
                    // collect shares from most recent buy
                    var sharesToMatch = sell.Shares;
                    while (sharesToMatch > 0)
                    {
                        // find a matching purchase and record a new holding
                        // must keep track of remaining shares in corresponding purchase
                        if (unusedSharesInCurrentBuy == 0) buy = buys.Skip(buysUsed).First();

                        if (buy != null)
                        {
                            var availableShares = unusedSharesInCurrentBuy > 0 ? unusedSharesInCurrentBuy : buy.Shares;
                            var neededShares = sharesToMatch;
                            double shares;
                            if (availableShares >= neededShares)
                            {
                                shares = neededShares;
                                unusedSharesInCurrentBuy = availableShares - shares;
                                if (unusedSharesInCurrentBuy == 0) buysUsed++;
                            }
                            else
                            {
                                shares = availableShares;
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
            return result.OrderByDescending(h => h.Tail).ToList();
        }

        /// <summary>
        ///   Gets the gross investment of this Position, ignoring any proceeds and commissions.
        /// </summary>
        /// <param name="basket"></param>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The total amount spent on share purchases as a negative number.</returns>
        public static decimal CalculateCost(this MeasurableSecurityBasket basket, DateTime settlementDate)
        {
            return basket.Transactions.Where(t => t is ShareTransaction).Cast<ShareTransaction>().AsParallel().Where(t => t is OpeningTransaction)
                .Where(transaction => transaction.SettlementDate <= settlementDate)
                .Sum(transaction => transaction.Price * (decimal)transaction.Shares);
        }

        /// <summary>
        ///   Gets the gross proceeds of this Position, ignoring all totalCosts and commissions.
        /// </summary>
        /// <param name="basket"></param>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The total amount of proceeds from share sales as a positive number.</returns>
        public static decimal CalculateProceeds(this MeasurableSecurityBasket basket, DateTime settlementDate)
        {
            return -1 * basket.Transactions.Where(t => t is ShareTransaction).Cast<ShareTransaction>().AsParallel().Where(t => t is ClosingTransaction)
                   .Where(transaction => transaction.SettlementDate <= settlementDate)
                   .Sum(transaction => transaction.Price * (decimal)transaction.Shares);
        }

        /// <summary>
        ///   Gets the total commissions paid as of a given date.
        /// </summary>
        /// <param name="basket"></param>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The total amount of commissions from <see cref = "ShareTransaction" />s as a negative number.</returns>
        public static decimal CalculateCommissions(this MeasurableSecurityBasket basket, DateTime settlementDate)
        {
            return basket.Transactions.Where(t=>t is ShareTransaction).Cast<ShareTransaction>().AsParallel()
                .Where(transaction => transaction.SettlementDate <= settlementDate)
                .Sum(transaction => transaction.Commission);
        }

        /// <summary>
        ///   Gets the net shares held at a given date.
        /// </summary>
        /// <param name="shareTransactions"></param>
        /// <param name = "date">The <see cref = "DateTime" /> to use.</param>
        public static double GetHeldShares(this IEnumerable<ShareTransaction> shareTransactions, DateTime date)
        {
            return shareTransactions.GetOpenedShares(date) - shareTransactions.GetClosedShares(date);
        }

        /// <summary>
        ///   Gets the cumulative number of shares that have ever been owned before a given date.
        /// </summary>
        /// <param name="shareTransactions"></param>
        /// <param name = "date">The <see cref = "DateTime" /> to use.</param>
        private static double GetOpenedShares(this IEnumerable<ShareTransaction> shareTransactions, DateTime date)
        {
            var transactions = shareTransactions.Where(t => t is OpeningTransaction);
            return transactions.Where(transaction => transaction.SettlementDate <= date).Sum(transaction => transaction.Shares);
        }

        /// <summary>
        ///   Gets the total number of shares that were owned but are no longer owned.
        /// </summary>
        /// <param name="shareTransactions"></param>
        /// <param name = "date">The <see cref = "DateTime" /> to use.</param>
        private static double GetClosedShares(this IEnumerable<ShareTransaction> shareTransactions, DateTime date)
        {
            var transactions = shareTransactions.Where(t => t is ClosingTransaction);
            return transactions.Where(transaction => transaction.SettlementDate <= date).Sum(transaction => transaction.Shares);
        }

        /// <summary>
        ///   Gets the average cost of all held shares in a <see cref="Position"/> as of a given date.
        /// </summary>
        /// <param name="position">The <see cref="Position"/> for which to calculate average cost.</param>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The average cost of all shares held at <paramref name = "settlementDate" />.</returns>
        public static decimal CalculateAverageCost(this Position position, DateTime settlementDate)
        {
            var transactions = position.Transactions.Cast<ShareTransaction>()
                .Where(transaction => transaction.SettlementDate <= settlementDate)
                .OrderBy(transaction => transaction.SettlementDate).ToList();
            var count = transactions.Count();

            var totalCost = 0.00m;
            var shares = 0.0;

            for (var i = 0; i < count; i++)
            {
                if (transactions[i] is OpeningTransaction)
                {
                    totalCost += (transactions[i].Price*(decimal) transactions[i].Shares);
                    shares += transactions[i].Shares;
                }
                else if (transactions[i] is ClosingTransaction)
                {
                    totalCost -= ((totalCost/(decimal) shares)*(decimal) transactions[i].Shares);
                    shares -= transactions[i].Shares;
                }
            }

            return totalCost / (decimal)shares;
        }

        /// <summary>
        ///   Gets the net profit of this MeasurableSecurityBasket, including any commissions, as of a given date.
        /// </summary>
        /// <param name="basket"></param>
        /// <param name="settlementDate">The <see cref="DateTime"/> to use.</param>
        public static decimal CalculateNetProfit(this MeasurableSecurityBasket basket, DateTime settlementDate)
        {
            var allHoldings = basket.CalculateHoldings(settlementDate);
            if (allHoldings.Count == 0) return 0;

            var positionGroups = allHoldings.GroupBy(h => h.Ticker);
            return (from holdings in positionGroups
                    from holding in holdings
                    let open = holding.OpenPrice
                    let close = holding.ClosePrice
                    let profit = close - open
                    let shares = (decimal) holding.Shares
                    select (profit*shares) - holding.OpenCommission - holding.CloseCommission).Sum();
        }

        /// <summary>
        ///   Gets the gross profit of this MeasurableSecurityBasket, excluding any commissions, as of a given date.
        /// </summary>
        /// <param name="basket"></param>
        /// <param name="settlementDate">The <see cref="DateTime"/> to use.</param>
        public static decimal CalculateGrossProfit(this MeasurableSecurityBasket basket, DateTime settlementDate)
        {
            var allHoldings = basket.CalculateHoldings(settlementDate);
            if (allHoldings.Count == 0) return 0;

            var positionGroups = allHoldings.GroupBy(h => h.Ticker);
            return (from holdings in positionGroups
                    from holding in holdings
                    let open = holding.OpenPrice
                    let close = holding.ClosePrice
                    let profit = close - open
                    let shares = (decimal) holding.Shares
                    select profit*shares).Sum();
        }

        /// <summary>
        ///   Gets the net rate of return on an annual basis for this MeasurableSecurityBasket, after commissions.
        /// </summary>
        /// <param name="basket"></param>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <remarks>
        ///   Assumes a year has 365 days.
        /// </remarks>
        public static decimal? CalculateAnnualGrossReturn(this MeasurableSecurityBasket basket, DateTime settlementDate)
        {
            var totalReturn = basket.CalculateGrossReturn(settlementDate);
            return totalReturn == null ? null : Annualize(totalReturn.Value, basket.Tail, basket.Head);
        }

        /// <summary>
        ///   Gets the net rate of return on an annual basis for this MeasurableSecurityBasket, after commissions.
        /// </summary>
        /// <param name="basket"></param>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <remarks>
        ///   Assumes a year has 365 days.
        /// </remarks>
        public static decimal? CalculateAnnualNetReturn(this MeasurableSecurityBasket basket, DateTime settlementDate)
        {
            var totalReturn = basket.CalculateNetReturn(settlementDate);
            return totalReturn == null ? null : Annualize(totalReturn.Value, basket.Tail, basket.Head);
        }

        private static decimal? Annualize(decimal totalReturn, DateTime tail, DateTime head)
        {
            var time = ((tail - head).Days / 365.0m);
            return totalReturn/time;
        }

        /// <summary>
        ///   Gets the net rate of return for this MeasurableSecurityBasket, after commissions.
        /// </summary>
        /// <param name="basket"></param>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>Returns the net rate of return, after commission, expressed as a percentage. Returns null if return cannot be calculated.</returns>
        public static decimal? CalculateNetReturn(this MeasurableSecurityBasket basket, DateTime settlementDate)
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
                    let profit = close - open - ((holding.OpenCommission + holding.CloseCommission)/(decimal) holding.Shares)
                    let increase = profit/open
                    select increase*(decimal) ((holding.Shares/positionShares)*(positionShares/totalShares))).Sum();
        }

        /// <summary>
        ///   Gets the gross rate of return for this MeasurableSecurityBasket, not accounting for commissions.
        /// </summary>
        /// <param name="basket"></param>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>Returns the gross rate of return, before commission, expressed as a percentage. Returns null if return cannot be calculated.</returns>
        public static decimal? CalculateGrossReturn(this MeasurableSecurityBasket basket, DateTime settlementDate)
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
                    select increase*(decimal) ((holding.Shares/positionShares)*(positionShares/totalShares))).Sum();
        }

    }
}