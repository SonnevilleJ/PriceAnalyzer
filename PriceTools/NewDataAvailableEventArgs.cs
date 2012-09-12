using System;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Contains event data for the <see cref="PricePeriodImpl.NewDataAvailable"/> event.
    /// </summary>
    public class NewDataAvailableEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the head of the date range for which prices are now available.
        /// </summary>
        public DateTime Head { get; set; }

        /// <summary>
        /// Gets the tail of the date range for which prices are now available.
        /// </summary>
        public DateTime Tail { get; set; }
    }
}
