using System;

namespace Sonneville.PriceTools.Implementation
{
    public struct Holding
    {
        public string Ticker { get; set; }

        public DateTime Head { get; set; }

        public DateTime Tail { get; set; }

        public decimal Shares { get; set; }

        public decimal OpenPrice { get; set; }

        public decimal OpenCommission { get; set; }

        public decimal ClosePrice { get; set; }

        public decimal CloseCommission { get; set; }
    }
}