using System;
using System.Collections.Generic;
using System.Linq;

namespace Sonneville.PriceTools.PortfolioStatistics
{
    public static class PortfolioStatistics
    {
        public static decimal KellyPercentage(this IEnumerable<IHolding> holdings)
        {
            long wins = 0;
            long losses = 0;
            decimal totalGain = 0;
            decimal totalLoss = 0;
            decimal trades = 0;

            foreach (var netProfit in holdings.Select(holding => holding.NetProfit()))
            {
                if (netProfit > 0)
                {
                    wins++;
                    totalGain += netProfit;
                }
                else
                {
                    losses++;
                    totalLoss += Math.Abs(netProfit);
                }
                trades++;
            }
            
            var winPercent = wins/trades;
            var lossPercent = losses/trades;
            var averageGain = totalGain/wins;
            var averageLoss = totalLoss/losses;
            var ratio = averageGain/averageLoss;

            return winPercent - (lossPercent/ratio);
        }
    }
}
