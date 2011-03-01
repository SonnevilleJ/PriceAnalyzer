using System;
using System.Collections.Generic;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Contains event data for the <see cref="IPriceSeries.NewPriceDataAvailable"/> event.
    /// </summary>
    public class NewPriceDataAvailableEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets an array of date indices which are now available in the <see cref="IPriceSeries"/>.
        /// </summary>
        public IEnumerable<DateTime> Indices { get; set; }
    }
}
