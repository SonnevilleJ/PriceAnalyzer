using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Constructs CashAccount objects.
    /// </summary>
    public static class CashAccountFactory
    {
        /// <summary>
        /// Constructs a new CashAccount.
        /// </summary>
        /// <returns></returns>
        public static CashAccount ConstructCashAccount()
        {
            return new CashAccountImpl();
        }

        /// <summary>
        /// Constructs a new CashAccount which supports borrowing on margin.
        /// </summary>
        /// <returns></returns>
        public static MarginableCashAccount ConstructMarginableCashAccount()
        {
            return new MarginableCashAccountImpl();
        }

        /// <summary>
        /// Constructs a new CashAccount which supports borrowing on margin.
        /// </summary>
        /// <param name="maximumMargin">The maximum amount of margin allowed on the account.</param>
        /// <returns></returns>
        public static MarginableCashAccount ConstructMarginableCashAccount(decimal maximumMargin)
        {
            var target = ConstructMarginableCashAccount();
            target.MaximumMargin = maximumMargin;
            return target;
        }
    }
}
