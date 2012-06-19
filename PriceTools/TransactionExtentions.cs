using System.Collections.Generic;
using System.Linq;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// A class storing extension methods for <see cref="Transaction"/> objects.
    /// </summary>
    public static class TransactionExtentions
    {
        /// <summary>
        /// Returns a value indicating whether a target <see cref="Transaction"/> is equal to the original <see cref="Transaction"/>.
        /// </summary>
        public static bool IsEqual(this Transaction original, Transaction target)
        {
            if (original == null || target == null) return false;

            if ((original is Buy && target is Buy) ||
                (original is BuyToCover && target is BuyToCover) ||
                (original is DividendReinvestment && target is DividendReinvestment) ||
                (original is Sell && target is Sell) ||
                (original is SellShort && target is SellShort))
            {
                var oShareTransaction = original as ShareTransaction;
                var tShareTransaction = target as ShareTransaction;

                return (oShareTransaction != null && tShareTransaction != null &&
                        oShareTransaction.SettlementDate == tShareTransaction.SettlementDate &&
                        oShareTransaction.Ticker == tShareTransaction.Ticker &&
                        oShareTransaction.Shares == tShareTransaction.Shares &&
                        oShareTransaction.Price == tShareTransaction.Price &&
                        oShareTransaction.Commission == tShareTransaction.Commission);
            }

            if((original is Deposit && target is Deposit) ||
                (original is DividendReceipt && target is DividendReceipt) ||
                (original is Withdrawal && target is Withdrawal))
            {
                var oCashTransaction = original as CashTransaction;
                var tCashTransaction = target as CashTransaction;

                return (oCashTransaction != null && tCashTransaction != null &&
                        oCashTransaction.SettlementDate == tCashTransaction.SettlementDate &&
                        oCashTransaction.Amount == tCashTransaction.Amount);
            }

            return false;
        }

        /// <summary>
        /// Returns a value indicating whether a target <see cref="IEnumerable{Transaction}"/> is equal to the original <see cref="IEnumerable{Transaction}"/>.
        /// </summary>
        /// <param name="original"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool IsEquivalent(this IEnumerable<Transaction> original, IEnumerable<Transaction> target)
        {
            var oParallel = original.AsParallel();
            var tParallel = target.AsParallel();
            return oParallel.All(transaction => oParallel.Where(t => t.IsEqual(transaction)).Count() == tParallel.Where(t => t.IsEqual(transaction)).Count());
        }
    }
}
