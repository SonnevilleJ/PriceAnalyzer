using System;
using System.Collections.Generic;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools
{
    public interface IHoldingFactory
    {
        Holding ConstructHolding(decimal shares, decimal openPrice, decimal openCommission, decimal closePrice, decimal closeCommission);

        Holding ConstructHolding(string ticker, DateTime head, DateTime tail, decimal shares, decimal openPrice, decimal closePrice);

        Holding ConstructHolding(string ticker, DateTime head, DateTime tail, decimal shares, decimal openPrice, decimal openCommission, decimal closePrice, decimal closeCommission);
        
        IList<Holding> CalculateHoldings(ISecurityBasket basket, DateTime settlementDate);

        IList<Holding> CalculateHoldings(IEnumerable<ITransaction> transactions, DateTime settlementDate);
    }
}