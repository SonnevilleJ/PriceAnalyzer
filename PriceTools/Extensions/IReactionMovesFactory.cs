using System.Collections.Generic;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Calculates <see cref="ReactionMove"/>s.
    /// </summary>
    public interface IReactionMovesFactory
    {
        /// <summary>
        /// Gets a collection of reaction moves observed in the PriceSeries.
        /// </summary>
        IEnumerable<ReactionMove> GetReactionMoves(IPriceSeries priceSeries);

        /// <summary>
        /// Gets a collection of reaction highs observed in the PriceSeries.
        /// </summary>
        IEnumerable<ReactionMove> GetReactionHighs(IPriceSeries priceSeries);

        /// <summary>
        /// Gets a collection of reaction lows observed in the PriceSeries.
        /// </summary>
        IEnumerable<ReactionMove> GetReactionLows(IPriceSeries priceSeries);
    }
}