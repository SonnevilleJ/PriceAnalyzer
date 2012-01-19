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
        public static ICashAccount ConstructCashAccount()
        {
            return new CashAccountImpl();
        }
    }
}
