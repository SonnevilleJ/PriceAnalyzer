using System;
using System.Collections.Generic;
using Sonneville.PriceTools.AutomatedTrading.Implementation;
using Sonneville.PriceTools.Data;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools.AutomatedTrading
{
    public interface ISecurityBasketCalculator
    {
        decimal CalculateCost(ISecurityBasket basket, DateTime settlementDate);

        decimal CalculateProceeds(ISecurityBasket basket, DateTime settlementDate);

        decimal CalculateCommissions(ISecurityBasket basket, DateTime settlementDate);

        decimal CalculateMarketValue(ISecurityBasket basket, IPriceDataProvider provider, DateTime settlementDate, IPriceHistoryCsvFileFactory priceHistoryCsvFileFactory);

        decimal GetHeldShares(IEnumerable<IShareTransaction> shareTransactions, DateTime dateTime);

        decimal GetHeldShares(ICollection<CashTransaction> cashTransactions, DateTime dateTime);

        decimal CalculateAverageCost(Position position, DateTime settlementDate);

        decimal CalculateNetProfit(ISecurityBasket basket, DateTime settlementDate);

        decimal CalculateGrossProfit(ISecurityBasket basket, DateTime settlementDate);

        decimal? CalculateAnnualGrossReturn(ISecurityBasket basket, DateTime settlementDate);

        decimal? CalculateAnnualNetReturn(ISecurityBasket basket, DateTime settlementDate);

        decimal? CalculateNetReturn(ISecurityBasket basket, DateTime settlementDate);

        decimal? CalculateGrossReturn(ISecurityBasket basket, DateTime settlementDate);

        decimal CalculateAverageProfit(ISecurityBasket basket, DateTime settlementDate);

        decimal CalculateMedianProfit(ISecurityBasket basket, DateTime settlementDate);

        decimal CalculateStandardDeviation(ISecurityBasket basket, DateTime settlementDate);
    }
}