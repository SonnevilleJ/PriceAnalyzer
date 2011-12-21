using System;
using Sonneville.PriceTools;
using Sonneville.PriceTools.Trading;

namespace TradingTest
{
    public static class TransactionFactoryExtensions
    {
        public static IShareTransaction CreateShareTransaction(this TransactionFactory factory, DateTime settlementDate, Order order, decimal commission)
        {
            return factory.CreateShareTransaction(settlementDate, order.OrderType, order.Ticker, order.Price, order.Shares, commission);
        }
    }
}
