﻿using System;
using System.Collections.Generic;
using System.Linq;
using Sonneville.PriceTools.AutomatedTrading.Implementation;
using Sonneville.PriceTools.Data;
using Sonneville.PriceTools.Implementation;
using Sonneville.Statistics;

namespace Sonneville.PriceTools.AutomatedTrading
{
    /// <summary>
    /// A class which holds extension methods for <see cref="ISecurityBasket"/>.
    /// </summary>
    public class SecurityBasketCalculator : ISecurityBasketCalculator
    {
        private readonly ITimeSeriesUtility _timeSeriesUtility;
        private readonly IProfitCalculator _profitCalculator;

        public SecurityBasketCalculator()
        {
            _timeSeriesUtility = new TimeSeriesUtility();
            _profitCalculator = new ProfitCalculator();
        }

        /// <summary>
        ///   Gets the gross investment of this Position, ignoring any proceeds and commissions.
        /// </summary>
        /// <param name="basket"></param>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The total amount spent on share purchases as a negative number.</returns>
        public decimal CalculateCost(ISecurityBasket basket, DateTime settlementDate)
        {
            return basket.Transactions.AsParallel().Where(t => t is ShareTransaction).Cast<ShareTransaction>().Where(t => t.IsOpeningTransaction())
                .Where(transaction => transaction.SettlementDate <= settlementDate)
                .Sum(transaction => transaction.Price * transaction.Shares);
        }

        /// <summary>
        ///   Gets the gross proceeds of this Position, ignoring all totalCosts and commissions.
        /// </summary>
        /// <param name="basket"></param>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The total amount of proceeds from share sales as a positive number.</returns>
        public decimal CalculateProceeds(ISecurityBasket basket, DateTime settlementDate)
        {
            return -1 * basket.Transactions.AsParallel().Where(t => t is ShareTransaction).Cast<ShareTransaction>().Where(t => t.IsClosingTransaction())
                   .Where(transaction => transaction.SettlementDate <= settlementDate)
                   .Sum(transaction => transaction.Price * transaction.Shares);
        }

        /// <summary>
        ///   Gets the total commissions paid as of a given date.
        /// </summary>
        /// <param name="basket"></param>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The total amount of commissions from <see cref = "ShareTransaction" />s as a negative number.</returns>
        public decimal CalculateCommissions(ISecurityBasket basket, DateTime settlementDate)
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
        /// <param name="priceHistoryCsvFileFactory"></param>
        /// <returns>The value of the shares held in the Position as of the given date.</returns>
        public decimal CalculateMarketValue(ISecurityBasket basket, IPriceDataProvider provider, DateTime settlementDate, IPriceHistoryCsvFileFactory priceHistoryCsvFileFactory)
        {
            var allTransactions = basket.Transactions.AsParallel().Where(t => t is ShareTransaction).Cast<ShareTransaction>();
            var groups = allTransactions.GroupBy(t => t.Ticker);

            var total = 0.00m;
            foreach (var transactions in groups)
            {
                var heldShares = GetHeldShares(transactions, settlementDate);
                if (heldShares == 0) continue;

                var priceSeries = new PriceSeriesFactory().ConstructPriceSeries(transactions.First().Ticker);
                if (!_timeSeriesUtility.HasValueInRange(priceSeries, settlementDate)) new PriceSeriesRetriever().UpdatePriceData(priceSeries, provider, settlementDate, priceHistoryCsvFileFactory);
                var price = priceSeries[settlementDate];
                total += heldShares * price;
            }
            return total;
        }

        /// <summary>
        ///   Gets the net shares held at a given date.
        /// </summary>
        /// <param name="shareTransactions"></param>
        /// <param name = "dateTime">The <see cref = "DateTime" /> to use.</param>
        public decimal GetHeldShares(IEnumerable<ShareTransaction> shareTransactions, DateTime dateTime)
        {
            var sum = 0m;
            foreach (var transaction in shareTransactions.Where(t=>t.SettlementDate <= dateTime))
            {
                if (transaction.IsOpeningTransaction()) sum += transaction.Shares;
                if (transaction.IsClosingTransaction()) sum -= transaction.Shares;
            }
            return sum;
        }

        /// <summary>
        ///   Gets the average cost of all held shares in a <see cref="Position"/> as of a given date.
        /// </summary>
        /// <param name="position">The <see cref="Position"/> for which to calculate average cost.</param>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The average cost of all shares held at <paramref name = "settlementDate" />.</returns>
        public decimal CalculateAverageCost(Position position, DateTime settlementDate)
        {
            var transactions = position.Transactions.Cast<ShareTransaction>()
                .Where(transaction => transaction.SettlementDate <= settlementDate)
                .OrderBy(transaction => transaction.SettlementDate).ToList();
            var count = transactions.Count();

            var totalCost = 0.00m;
            var shares = 0.0m;

            for (var i = 0; i < count; i++)
            {
                if (transactions[i].IsOpeningTransaction())
                {
                    totalCost += (transactions[i].Price*transactions[i].Shares);
                    shares += transactions[i].Shares;
                }
                else if (transactions[i].IsClosingTransaction())
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
        public decimal CalculateNetProfit(ISecurityBasket basket, DateTime settlementDate)
        {
            return basket.CalculateHoldings(settlementDate).AsParallel().Sum(holding => ((holding.ClosePrice - holding.OpenPrice)*holding.Shares) - holding.OpenCommission - holding.CloseCommission);
        }

        /// <summary>
        ///   Gets the gross profit of this SecurityBasket, excluding any commissions, as of a given date.
        /// </summary>
        /// <param name="basket"></param>
        /// <param name="settlementDate">The <see cref="DateTime"/> to use.</param>
        public decimal CalculateGrossProfit(ISecurityBasket basket, DateTime settlementDate)
        {
            return basket.CalculateHoldings(settlementDate).AsParallel().Sum(holding => (holding.ClosePrice - holding.OpenPrice)*holding.Shares);
        }

        /// <summary>
        ///   Gets the net rate of return on an annual basis for this SecurityBasket, after commissions.
        /// </summary>
        /// <param name="basket"></param>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <remarks>
        ///   Assumes a year has 365 days.
        /// </remarks>
        public decimal? CalculateAnnualGrossReturn(ISecurityBasket basket, DateTime settlementDate)
        {
            if(basket == null) throw new ArgumentNullException("basket", Strings.SecurityBasketExtensions_CalculateAnnualGrossReturn_Parameter_basket_cannot_be_null_);

            var totalReturn = CalculateGrossReturn(basket, settlementDate);
            return totalReturn == null ? null : Annualize(totalReturn.Value, basket.Transactions.Min(t => t.SettlementDate), basket.Transactions.Max(t => t.SettlementDate));
        }

        /// <summary>
        ///   Gets the net rate of return on an annual basis for this SecurityBasket, after commissions.
        /// </summary>
        /// <param name="basket"></param>
        /// <param name = "settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <remarks>
        ///   Assumes a year has 365 days.
        /// </remarks>
        public decimal? CalculateAnnualNetReturn(ISecurityBasket basket, DateTime settlementDate)
        {
            var totalReturn = CalculateNetReturn(basket, settlementDate);
            return totalReturn == null ? null : Annualize(totalReturn.Value, basket.Transactions.Min(t => t.SettlementDate), basket.Transactions.Max(t => t.SettlementDate));
        }

        /// <summary>
        /// Calculates an annual rate of return for a given date range.
        /// </summary>
        /// <param name="totalReturn"></param>
        /// <param name="head"></param>
        /// <param name="tail"></param>
        /// <returns></returns>
        private decimal? Annualize(decimal totalReturn, DateTime head, DateTime tail)
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
        public decimal? CalculateNetReturn(ISecurityBasket basket, DateTime settlementDate)
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
        public decimal? CalculateGrossReturn(ISecurityBasket basket, DateTime settlementDate)
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
        /// Calculates the mean gross profit of a basket's <see cref="Holding"/>s.
        /// </summary>
        /// <param name="basket"></param>
        /// <param name="settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>Returns the mean gross profit from a <see cref="ISecurityBasket"/>.</returns>
        public decimal CalculateAverageProfit(ISecurityBasket basket, DateTime settlementDate)
        {
            return basket.CalculateHoldings(settlementDate).AsParallel().Average(holding => _profitCalculator.GrossProfit(holding));
        }

        /// <summary>
        /// Calculates the median gross profit of a basket's <see cref="Holding"/>s.
        /// </summary>
        /// <param name="basket"></param>
        /// <param name="settlementDate">The <see cref = "DateTime" /> to use.</param>
        /// <returns>Returns the median gross profit from a <see cref="ISecurityBasket"/>.</returns>
        public decimal CalculateMedianProfit(ISecurityBasket basket, DateTime settlementDate)
        {
            return basket.CalculateHoldings(settlementDate).Select(h => _profitCalculator.GrossProfit(h)).Median();
        }

        /// <summary>
        /// Calculates the standard deviation of all profits in a <see cref="ISecurityBasket"/>.
        /// </summary>
        /// <param name="basket"></param>
        /// <param name="settlementDate">The <see cref="DateTime"/> to use.</param>
        /// <returns>Returns the standard deviation of the profits from a <see cref="ISecurityBasket"/>.</returns>
        public decimal CalculateStandardDeviation(ISecurityBasket basket, DateTime settlementDate)
        {
            return basket.CalculateHoldings(settlementDate).Select(h => _profitCalculator.GrossProfit(h)).StandardDeviation();
        }
    }
}