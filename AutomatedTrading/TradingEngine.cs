using System;
using System.Collections.Generic;
using Sonneville.PriceTools.AutomatedTrading.Implementation;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools.AutomatedTrading
{
    public class TradingEngine
    {
        private readonly IPortfolio _currentlyHeldStuff;
        private readonly IncreaseInvestmentLogic _increaseInvestmentLogic = new IncreaseInvestmentLogic();
        private readonly DecreaseInvestmentLogic _decreaseInvestmentLogic;

        public TradingEngine(ISecurityBasketCalculator securityBasketCalculator, IPortfolio currentlyHeldStuff)
        {
            _currentlyHeldStuff = currentlyHeldStuff;
            _decreaseInvestmentLogic = new DecreaseInvestmentLogic(securityBasketCalculator, currentlyHeldStuff);
        }

        public IList<Order> DetermineOrdersFor(IPriceSeries stockBeingConsidered, DateTime startDate, DateTime endDate)
        {
            var orders = new List<Order>();

            if (_currentlyHeldStuff.GetAvailableCash(endDate) >= stockBeingConsidered[endDate] &&
                _increaseInvestmentLogic.ShouldExpandPosition(stockBeingConsidered, startDate, endDate))
            {
                var order = _increaseInvestmentLogic.ExpandPosition(stockBeingConsidered, endDate);
                orders.Add(order);
            }
            if (_decreaseInvestmentLogic.ShouldReducePosition(stockBeingConsidered, startDate, endDate))
            {
                var order = _decreaseInvestmentLogic.ReducePosition(stockBeingConsidered, endDate);
                orders.Add(order);
            }

            return orders;
        }
    }
}