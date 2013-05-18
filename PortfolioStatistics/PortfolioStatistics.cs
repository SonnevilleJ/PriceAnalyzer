using System.Collections.Generic;
using System.Linq;

namespace Sonneville.PriceTools.PortfolioStatistics
{
    public static class PortfolioStatistics
    {
        public static decimal KellyPercentage(this IEnumerable<IHolding> holdings)
        {
            var netProfits = holdings.Select(holding => holding.NetProfit());
            var wins = netProfits.Where(x => x > 0);
            var losses = netProfits.Where(x => x <= 0);
            var totalGain = wins.Sum(x => x);
            var totalLoss = 0 - losses.Sum(x => x);

            var winPercent = wins.Count()/(decimal) netProfits.Count();
            var lossPercent = losses.Count()/(decimal) netProfits.Count();
            var averageGain = totalGain/wins.Count();
            var averageLoss = totalLoss/losses.Count();
            var ratio = averageGain/averageLoss;

            return winPercent - (lossPercent/ratio);
        }
    }
}
