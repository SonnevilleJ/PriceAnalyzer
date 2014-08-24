using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools
{
    public class CashAccountFactory : ICashAccountFactory
    {
        public ICashAccount ConstructCashAccount()
        {
            return new CashAccount();
        }

        public IMarginableCashAccount ConstructMarginableCashAccount()
        {
            return new MarginableCashAccount();
        }

        public IMarginableCashAccount ConstructMarginableCashAccount(decimal maximumMargin)
        {
            var target = ConstructMarginableCashAccount();
            target.MaximumMargin = maximumMargin;
            return target;
        }
    }
}
