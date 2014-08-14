using System;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools.AutomatedTrading
{
    public class IncreaseInvestmentLogic
    {
        public bool ShouldExpandPosition(IPriceSeries stockBeingConsidered, DateTime startDate, DateTime dateTime)
        {
            return stockBeingConsidered[dateTime] > stockBeingConsidered[startDate];
        }

        public Order ExpandPosition(IPriceSeries stockBeingConsidered, DateTime endDate)
        {
            var order = new Order();
            order.OrderType = OrderType.Buy;
            order.Shares = 1;
            order.Issued = endDate;
            order.Ticker = stockBeingConsidered.Ticker;
            return order;
        }
    }
}