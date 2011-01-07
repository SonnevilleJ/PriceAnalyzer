namespace Sonneville.PriceTools
{
    /// <summary>
    ///   Represents a position taken using one or more <see cref="ITransaction"/>s.
    /// </summary>
    public interface IPosition
    {
        /// <summary>
        ///   Gets or sets the <see cref="ITransaction"/> which opened this IPosition.
        /// </summary>
        ITransaction OpeningTransaction { get; set; }

        /// <summary>
        ///   Gets or sets the <see cref="ITransaction"/> which closed this IPosition.
        /// </summary>
        ITransaction ClosingTransaction { get; set; }

        /// <summary>
        ///   Gets the <see cref="PositionStatus"/> of this IPosition.
        /// </summary>
        PositionStatus PositionStatus { get; }

        /// <summary>
        ///   Gets the total value of this IPosition, after commissions.
        /// </summary>
        decimal TotalValue { get; }
    }
}