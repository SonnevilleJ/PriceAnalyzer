using System;

namespace Sonneville.PriceTools
{
    public struct ReactionMove
    {
        public DateTime DateTime { get; set; }

        public HighLow HighLow { get; set; }

        public decimal Reaction { get; set; }
    }
}
