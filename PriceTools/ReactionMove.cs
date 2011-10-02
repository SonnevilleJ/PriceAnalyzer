using System;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// A intermediate price peak (high) or trough (low) observed within an <see cref="IPriceSeries"/>.
    /// </summary>
    public struct ReactionMove
    {
        /// <summary>
        /// The DateTime of the reaction move.
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Identifies the direction of the reaction move.
        /// </summary>
        public HighLow HighLow { get; set; }

        /// <summary>
        /// The price peak/trough of the reaction move.
        /// </summary>
        public decimal Reaction { get; set; }
    }
}
