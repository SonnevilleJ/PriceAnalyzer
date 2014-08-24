namespace Sonneville.PriceTools
{
    public interface IMarginableCashAccount : ICashAccount
    {
        decimal MaximumMargin { get; set; }
    }
}