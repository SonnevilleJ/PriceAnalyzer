using System;
using System.Collections.Generic;
using System.Linq;
using Sonneville.PriceTools.AutomatedTrading.Implementation;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools.AutomatedTrading
{
    public class TradingEngine
    {
        private readonly ISecurityBasketCalculator _securityBasketCalculator;
        private readonly IPortfolio _currentlyHeldStuff;

        public TradingEngine(ISecurityBasketCalculator securityBasketCalculator, IPortfolio currentlyHeldStuff)
        {
            _securityBasketCalculator = securityBasketCalculator;
            _currentlyHeldStuff = currentlyHeldStuff;
        }

        public IEnumerable<Order> DetermineOrdersFor(IPriceSeries stockBeingConsidered, DateTime startDate, DateTime endDate)
        {
            var sharesToBuy = CalculateSharesToBuy(stockBeingConsidered, startDate, endDate);
            if (HaveAvailableFunds(stockBeingConsidered, endDate) && sharesToBuy > 0)
            {
                yield return CreateOrder(OrderType.Buy, stockBeingConsidered.Ticker, sharesToBuy, endDate);
            }
            var sharesToSell = CalculateSharesToSell(stockBeingConsidered, startDate, endDate);
            if (sharesToSell > 0)
            {
                yield return CreateOrder(OrderType.Sell, stockBeingConsidered.Ticker, sharesToSell, endDate);
            }
        }

        private bool HaveAvailableFunds(IPriceSeries stockBeingConsidered, DateTime endDate)
        {
            return _currentlyHeldStuff.GetAvailableCash(endDate) >= stockBeingConsidered[endDate];
        }

        private decimal CalculateSharesToBuy(IPriceSeries stockBeingConsidered, DateTime startDate, DateTime endDate)
        {
            var shouldBuy = stockBeingConsidered[endDate] > stockBeingConsidered[startDate];
            if (shouldBuy)
            {
                return 1;
            }
            return 0;
        }

        private decimal CalculateSharesToSell(IPriceSeries stockBeingConsidered, DateTime startDate, DateTime endDate)
        {
            var shouldSell = stockBeingConsidered[startDate] > stockBeingConsidered[endDate];
            if (shouldSell)
            {
                var position = _currentlyHeldStuff.GetPosition(stockBeingConsidered.Ticker);
                if (position != null)
                {
                    var shareTransactions = position.Transactions.Cast<IShareTransaction>();
                    var shares = _securityBasketCalculator.GetHeldShares(shareTransactions, endDate);
                    return shares;
                }
            }
            return 0;
        }

        private static Order CreateOrder(OrderType orderType, string ticker, decimal shares, DateTime issueDate)
        {
            return new Order
            {
                OrderType = orderType,
                Shares = shares,
                Issued = issueDate,
                Ticker = ticker
            };
        }
    }
}