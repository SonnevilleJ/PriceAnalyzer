using System;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools.TestUtilities
{
    public static class TradingAccountUtilities
    {
        private static readonly ITransactionFactory TransactionFactory;

        static TradingAccountUtilities()
        {
            TransactionFactory = new TransactionFactory();
        }

        public static ShareTransaction CreateShareTransaction(DateTime settlementDate, Order order, decimal commission)
        {
            return TransactionFactory.ConstructShareTransaction(order.OrderType, order.Ticker, settlementDate, order.Shares, order.Price, commission);
        }
    }
}