namespace Sonneville.PriceTools
{
    /// <summary>
    /// A <see cref="ICashAccount"/> which allows trading on margin.
    /// </summary>
    public interface IMarginableCashAccount : ICashAccount
    {
        /// <summary>
        /// The maximum amount of margin allowed on the account.
        /// </summary>
        decimal MaximumMargin { get; set; }
    }
}