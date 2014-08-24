namespace Sonneville.PriceTools
{
    public interface ICashAccountFactory
    {
        ICashAccount ConstructCashAccount();

        IMarginableCashAccount ConstructMarginableCashAccount();

        IMarginableCashAccount ConstructMarginableCashAccount(decimal maximumMargin);
    }
}