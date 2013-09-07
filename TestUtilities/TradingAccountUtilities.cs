using System;
using Sonneville.PriceTools;
using Sonneville.PriceTools.Implementation;

namespace TestUtilities.Sonneville.PriceTools
{
    public static class TradingAccountUtilities
    {
        private static readonly ITransactionFactory TransactionFactory;

        static TradingAccountUtilities()
        {
            TransactionFactory = new TransactionFactory();
        }

        /// <summary>
        /// Creates an <see cref="IShareTransaction"/> which would result from the perfect execution of <paramref name="order"/>.
        /// </summary>
        /// <param name="settlementDate">The <see cref="DateTime"/> to use as the SettlementDate for the resulting <see cref="IShareTransaction"/>.</param>
        /// <param name="order">The <see cref="Order"/> which should define the parameters for the resulting <see cref="IShareTransaction"/>.</param>
        /// <param name="commission">The commission that should be charged for the resulting <see cref="IShareTransaction"/>.</param>
        /// <returns></returns>
        public static IShareTransaction CreateShareTransaction(DateTime settlementDate, Order order, decimal commission)
        {
            return TransactionFactory.ConstructShareTransaction(order.OrderType, order.Ticker, settlementDate, order.Shares, order.Price, commission);
        }
    }
}