using System;

namespace Sonneville.PriceAnalyzer
{
    public class AnalyzerEventArgs : EventArgs
    {
        public DateTime DateTime { get; set; }
    }
}