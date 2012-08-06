using System;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Contains event data for the <see cref="PricePeriod.NewDataAvailable"/> event.
    /// </summary>
    public class NewPriceDataAvailableEventArgs : EventArgs
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
