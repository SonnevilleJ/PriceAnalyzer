namespace Sonneville.PriceTools
{
    /// <summary>
    /// A class storing extension methods for <see cref="Transaction"/> objects.
    /// </summary>
    public static class TransactionExtentions
    {
        /// <summary>
        /// Returns a value indicating whether a target <see cref="PricePeriod"/> is equal to the original <see cref="PricePeriod"/>.
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
    }
}
