namespace Sonneville.PriceTools
{
    public interface ICashAccountFactory
    {
        /// <summary>
        /// Constructs a new CashAccount.
        /// </summary>
        /// <returns></returns>
        ICashAccount ConstructCashAccount();

        /// <summary>
        /// Constructs a new CashAccount which supports borrowing on margin.
        /// </summary>
        /// <returns></returns>
        IMarginableCashAccount ConstructMarginableCashAccount();

        /// <summary>
        /// Constructs a new CashAccount which supports borrowing on margin.
        /// </summary>
        /// <param name="maximumMargin">The maximum amount of margin allowed on the account.</param>
        /// <returns></returns>
        IMarginableCashAccount ConstructMarginableCashAccount(decimal maximumMargin);
    }
}