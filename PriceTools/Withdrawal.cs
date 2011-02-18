namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a cash withdrawal from an <see cref="IPortfolio"/>.
    /// </summary>
    public sealed partial class Withdrawal : CashTransaction
    {
        #region Constructors

        /// <summary>
        /// Constructs a Withdrawal.
        /// </summary>
        public Withdrawal()
        {
            OrderType = OrderType.Withdrawal;
        }

        #endregion
    }
}