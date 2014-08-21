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
            if (_currentlyHeldStuff.GetAvailableCash(endDate) >= stockBeingConsidered[endDate] &&
                stockBeingConsidered[endDate] > stockBeingConsidered[startDate])
            {
                yield return CreateOrder(OrderType.Buy, stockBeingConsidered.Ticker, 1, endDate);
            }
            if (stockBeingConsidered[startDate] > stockBeingConsidered[endDate])
            {
                var position = _currentlyHeldStuff.GetPosition(stockBeingConsidered.Ticker);
                if (position != null)
                {
                    var shareTransactions = position.Transactions.Cast<IShareTransaction>();
                    var shares = _securityBasketCalculator.GetHeldShares(shareTransactions, endDate);
                    yield return CreateOrder(OrderType.Sell, stockBeingConsidered.Ticker, shares, endDate);
                }
            }
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