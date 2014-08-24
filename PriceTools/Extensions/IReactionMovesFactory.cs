using System.Collections.Generic;

namespace Sonneville.PriceTools
{
    public interface IReactionMovesFactory
    {
        IEnumerable<ReactionMove> GetReactionMoves(IPriceSeries priceSeries);

        IEnumerable<ReactionMove> GetReactionHighs(IPriceSeries priceSeries);

        IEnumerable<ReactionMove> GetReactionLows(IPriceSeries priceSeries);
    }
}