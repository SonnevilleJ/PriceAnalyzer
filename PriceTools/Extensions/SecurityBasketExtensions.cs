using System;
using System.Collections.Generic;
using System.Linq;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// A class which holds extension methods for <see cref="ISecurityBasket"/>.
    /// </summary>
    public static class SecurityBasketExtensions
    {
        private static readonly IHoldingFactory HoldingFactory;

        static SecurityBasketExtensions()
        {
            HoldingFactory = new HoldingFactory();
        }

        /// <summary>
        /// Gets an <see cref="IList{Holding}"/> from the transactions in the Position.
        /// </summary>
        /// <param name="basket"></param>
        /// <param name="settlementDate">The latest date used to include a transaction in the calculation.</param>
        /// <returns>An <see cref="IList{Holding}"/> of the transactions in the Position.</returns>
        public static IList<Holding> CalculateHoldings(this ISecurityBasket basket, DateTime settlementDate)
        {
            return CalculateHoldings(basket.Transactions, settlementDate);
        }

        /// <summary>
        /// Gets an <see cref="IList{Holding}"/> from the transactions in the Position.
        /// </summary>
        /// <param name="transactions"></param>
        /// <param name="settlementDate">The latest date used to include a transaction in the calculation.</param>
        /// <returns>An <see cref="IList{Holding}"/> of the transactions in the Position.</returns>
        public static IList<Holding> CalculateHoldings(this IEnumerable<Transaction> transactions, DateTime settlementDate)
        {
            var result = new List<Holding>();
            var groups = transactions.Where(t => t is ShareTransaction).Cast<ShareTransaction>().GroupBy(t => t.Ticker);
            foreach (var grouping in groups)
            {
                var buys = grouping.Where(t => t.IsOpeningTransaction()).Where(t => t.SettlementDate < settlementDate).OrderBy(t => t.SettlementDate);
                var buysUsed = 0;
                var unusedSharesInCurrentBuy = 0.0m;
                ShareTransaction buy = null;

                var sells = grouping.Where(t => t.IsClosingTransaction()).Where(t => t.SettlementDate <= settlementDate).OrderBy(t => t.SettlementDate);
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
                            var holding = HoldingFactory.ConstructHolding(sell.Ticker, buy.SettlementDate, sell.SettlementDate, shares, buy.Price, buy.Commission, -1*sell.Price, sell.Commission);
                            result.Add(holding);

                            sharesToMatch -= shares;
                        }
                    }
                }
            }
            return result.OrderBy(h => h.Tail).ToList();
        }
    }
}
