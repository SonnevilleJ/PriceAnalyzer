using System;

namespace Sonneville.PriceTools.AutomatedTrading
{
    public class OrderStatus
    {
        public string Ticker { get; set; }
        public Guid Id { get; set; }
        public decimal Price { get; set; }
        public decimal Shares { get; set; }
        public OrderType OrderType { get; set; }
        public DateTime SubmitTime { get; set; }
    }
}