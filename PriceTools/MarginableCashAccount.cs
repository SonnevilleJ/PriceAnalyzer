namespace Sonneville.PriceTools
{
    /// <summary>
    /// A <see cref="CashAccount"/> which allows trading on margin.
    /// </summary>
    public interface MarginableCashAccount : CashAccount
    {
        /// <summary>
        /// The maximum amount of margin allowed on the account.
        /// </summary>
        decimal MaximumMargin { get; set; }
    }
}