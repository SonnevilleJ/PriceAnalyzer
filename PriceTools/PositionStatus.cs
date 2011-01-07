namespace Sonneville.PriceTools
{

    /// <summary>
    /// Identifies an <see cref="ITransaction"/> as either Open or Closed.
    /// </summary>
    public enum PositionStatus
    {
        /// <summary>
        /// Signifies that the <see cref="ITransaction"/> has presently held shares.
        /// </summary>
        Open,

        /// <summary>
        /// Signifies that the <see cref="ITransaction"/> has no presently held shares.
        /// </summary>
        Closed
    }

}
