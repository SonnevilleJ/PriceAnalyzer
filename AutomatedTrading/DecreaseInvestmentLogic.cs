using System;
using System.Linq;
using Sonneville.PriceTools.AutomatedTrading.Implementation;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools.AutomatedTrading
{
    public class DecreaseInvestmentLogic
    {
        private readonly ISecurityBasketCalculator _securityBasketCalculator;
        private readonly IPortfolio _currentlyHeldStuff;

        public DecreaseInvestmentLogic(ISecurityBasketCalculator securityBasketCalculator, IPortfolio currentlyHeldStuff)
        {
            _securityBasketCalculator = securityBasketCalculator;
            _currentlyHeldStuff = currentlyHeldStuff;
        }

        public Order ReducePosition(IPriceSeries stockBeingConsidered, DateTime dateTime)
        {
            var order = new Order();
            order.OrderType = OrderType.Sell;
            order.Shares = GetNumberOfShares(stockBeingConsidered.Ticker, dateTime);
            order.Issued = dateTime;
            order.Ticker = stockBeingConsidered.Ticker;
            return order;
        }

        public bool ShouldReducePosition(IPriceSeries stockBeingConsidered, DateTime startDate, DateTime endDate)
        {
            var priceDecreased = stockBeingConsidered[startDate] > stockBeingConsidered[endDate];
            var haveAnyShares = GetNumberOfShares(stockBeingConsidered.Ticker, endDate) > 0;

            return priceDecreased && haveAnyShares;
        }

        private decimal GetNumberOfShares(string ticker, DateTime dateTime)
        {
            var position = _currentlyHeldStuff.GetPosition(ticker);
            if (position != null)
            {
                var shareTransactions = position.Transactions.Cast<IShareTransaction>();
                return _securityBasketCalculator.GetHeldShares(shareTransactions, dateTime);
            }
            return 0;
        }
    }
}