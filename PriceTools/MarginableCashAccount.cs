namespace Sonneville.PriceTools
{
    public interface MarginableCashAccount : CashAccount
    {
        /// <summary>
        /// The maximum amount of margin allowed on the account.
        /// </summary>
        decimal MaximumMargin { get; set; }
    }
}