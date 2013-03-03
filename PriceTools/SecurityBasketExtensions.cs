using System;
using System.Collections.Generic;
using System.Linq;

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
        public static IList<IHolding> CalculateHoldings(this IEnumerable<ITransaction> transactions, DateTime settlementDate)
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
    }
}
