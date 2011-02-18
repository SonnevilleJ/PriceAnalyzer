namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a cash deposit to an <see cref="IPortfolio"/>.
    /// </summary>
    public sealed partial class Deposit : CashTransaction
    {
        #region Constructors

        /// <summary>
        /// Constructs a Deposit.
        /// </summary>
        public Deposit()
        {
            OrderType = OrderType.Deposit;
        }

        #endregion
    }
}