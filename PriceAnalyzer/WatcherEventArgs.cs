using System;

namespace Sonneville.PriceAnalyzer
{
    public class WatcherEventArgs : EventArgs
    {
        public DateTime DateTime { get; set; }
    }
}