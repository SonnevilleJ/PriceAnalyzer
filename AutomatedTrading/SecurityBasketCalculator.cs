using System;
using System.Collections.Generic;
using System.Linq;
using Sonneville.PriceTools.AutomatedTrading.Implementation;
using Sonneville.PriceTools.Data;
using Sonneville.PriceTools.Implementation;
using Sonneville.Statistics;

namespace Sonneville.PriceTools.AutomatedTrading
{
    public class SecurityBasketCalculator : ISecurityBasketCalculator
    {
        private readonly IPriceSeriesFactory _priceSeriesFactory;
        private readonly ITimeSeriesUtility _timeSeriesUtility;
        private readonly IProfitCalculator _profitCalculator;
        private readonly IHoldingFactory _holdingFactory;

        public SecurityBasketCalculator()
            : this(new TimeSeriesUtility(), new ProfitCalculator(), new PriceSeriesFactory(), new HoldingFactory())
        {
        }

        public SecurityBasketCalculator(
            ITimeSeriesUtility timeSeriesUtility,
            IProfitCalculator profitCalculator,
            IPriceSeriesFactory priceSeriesFactory,
            IHoldingFactory holdingFactory
            )
        {
            _timeSeriesUtility = timeSeriesUtility;
            _profitCalculator = profitCalculator;
            _priceSeriesFactory = priceSeriesFactory;
            _holdingFactory = holdingFactory;
        }

        public decimal CalculateCost(ISecurityBasket basket, DateTime settlementDate)
        {
            return basket.Transactions.AsParallel().Where(transaction => TransactionMatches(settlementDate, transaction)).Cast<ShareTransaction>()
                .Sum(transaction => transaction.Price * transaction.Shares);
        }

        private static bool TransactionMatches(DateTime settlementDate, ITransaction transaction)
        {
            return transaction.IsOpeningTransaction() && transaction.SettlementDate <= settlementDate && transaction is ShareTransaction;
        }

        public decimal CalculateProceeds(ISecurityBasket basket, DateTime settlementDate)
        {
            return -1 * basket.Transactions.AsParallel().Where(t => t is ShareTransaction).Cast<ShareTransaction>().Where(t => t.IsClosingTransaction())
                   .Where(transaction => transaction.SettlementDate <= settlementDate)
                   .Sum(transaction => transaction.Price * transaction.Shares);
        }

        public decimal CalculateCommissions(ISecurityBasket basket, DateTime settlementDate)
        {
            return basket.Transactions.AsParallel().Where(t=>t is ShareTransaction).Cast<ShareTransaction>()
                .Where(transaction => transaction.SettlementDate <= settlementDate)
                .Sum(transaction => transaction.Commission);
        }

        public decimal CalculateMarketValue(ISecurityBasket basket, IPriceDataProvider provider, DateTime settlementDate, IPriceHistoryCsvFileFactory priceHistoryCsvFileFactory)
        {
            var allTransactions = basket.Transactions.AsParallel().Where(t => t is ShareTransaction).Cast<ShareTransaction>();
            var groups = allTransactions.GroupBy(t => t.Ticker);

            var total = 0.00m;
            foreach (var transactions in groups)
            {
                var heldShares = GetHeldShares(transactions, settlementDate);
                if (heldShares == 0) continue;

                var priceSeries = _priceSeriesFactory.ConstructPriceSeries(transactions.First().Ticker);
                if (!_timeSeriesUtility.HasValueInRange(priceSeries, settlementDate))
                {
                    provider.UpdatePriceSeries(priceSeries, settlementDate, DateTime.Now, priceSeries.Resolution);
                }
                var price = priceSeries[settlementDate];
                total += heldShares * price;
            }
            return total;
        }

        public decimal GetHeldShares(IEnumerable<IShareTransaction> shareTransactions, DateTime dateTime)
        {
            var sum = 0m;
            foreach (var transaction in shareTransactions.Where(t=>t.SettlementDate <= dateTime))
            {
                if (transaction.IsOpeningTransaction()) sum += transaction.Shares;
                if (transaction.IsClosingTransaction()) sum -= transaction.Shares;
            }
            return sum;
        }

        public decimal GetHeldShares(ICollection<CashTransaction> cashTransactions, DateTime dateTime)
        {
            var sum = 0m;
            foreach (var transaction in cashTransactions.Where(ct=>ct.SettlementDate <= dateTime))
            {
                if (transaction.IsOpeningTransaction()) sum += transaction.Amount;
                if (transaction.IsClosingTransaction()) sum += transaction.Amount;
            }
            return sum;
        }

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

        public decimal CalculateNetProfit(ISecurityBasket basket, DateTime settlementDate)
        {
            return _holdingFactory.CalculateHoldings(basket, settlementDate).AsParallel().Sum(holding => ((holding.ClosePrice - holding.OpenPrice)*holding.Shares) - holding.OpenCommission - holding.CloseCommission);
        }

        public decimal CalculateGrossProfit(ISecurityBasket basket, DateTime settlementDate)
        {
            return _holdingFactory.CalculateHoldings(basket, settlementDate).AsParallel().Sum(holding => (holding.ClosePrice - holding.OpenPrice)*holding.Shares);
        }

        public decimal? CalculateAnnualGrossReturn(ISecurityBasket basket, DateTime settlementDate)
        {
            if(basket == null) throw new ArgumentNullException("basket", Strings.SecurityBasketExtensions_CalculateAnnualGrossReturn_Parameter_basket_cannot_be_null_);

            var totalReturn = CalculateGrossReturn(basket, settlementDate);
            return totalReturn == null ? null : Annualize(totalReturn.Value, basket.Transactions.Min(t => t.SettlementDate), basket.Transactions.Max(t => t.SettlementDate));
        }

        public decimal? CalculateAnnualNetReturn(ISecurityBasket basket, DateTime settlementDate)
        {
            var totalReturn = CalculateNetReturn(basket, settlementDate);
            return totalReturn == null ? null : Annualize(totalReturn.Value, basket.Transactions.Min(t => t.SettlementDate), basket.Transactions.Max(t => t.SettlementDate));
        }

        private decimal? Annualize(decimal totalReturn, DateTime head, DateTime tail)
        {
            // decimal division is imperfect around 25 decimal places. Round to 20 decimal places to reduce errors.
            var time = ((tail - head).Days / 365.0m);
            return Math.Round(totalReturn/time, 20);
        }

        public decimal? CalculateNetReturn(ISecurityBasket basket, DateTime settlementDate)
        {
            var allHoldings = _holdingFactory.CalculateHoldings(basket, settlementDate);
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

        public decimal? CalculateGrossReturn(ISecurityBasket basket, DateTime settlementDate)
        {
            var allHoldings = _holdingFactory.CalculateHoldings(basket, settlementDate);
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

        public decimal CalculateAverageProfit(ISecurityBasket basket, DateTime settlementDate)
        {
            return _holdingFactory.CalculateHoldings(basket, settlementDate).AsParallel().Average(holding => _profitCalculator.GrossProfit(holding));
        }

        public decimal CalculateMedianProfit(ISecurityBasket basket, DateTime settlementDate)
        {
            return _holdingFactory.CalculateHoldings(basket, settlementDate).Select(h => _profitCalculator.GrossProfit(h)).Median();
        }

        public decimal CalculateStandardDeviation(ISecurityBasket basket, DateTime settlementDate)
        {
            return _holdingFactory.CalculateHoldings(basket, settlementDate).Select(h => _profitCalculator.GrossProfit(h)).StandardDeviation();
        }
    }
}