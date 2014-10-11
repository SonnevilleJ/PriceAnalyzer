using System;
using System.Collections.Generic;
using System.Windows.Documents;
using Sonneville.PriceTools.AutomatedTrading;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools.PriceAnalyzer
{
    public class TradeViewModel
    {
        private readonly IBrokerage _brokerage;

        public TradeViewModel(IBrokerage brokerage)
        {
            _brokerage = brokerage;
        }

        public string Ticker { get; set; }
        public decimal Volume { get; set; }
        public decimal SharePrice { get; set; }
        public Expiration ExpirationType { get; set; }
        public DateTime ExpirationDateTime { get; set; }
        public OrderType OrderType { get; set; }

        public bool ValidateVolume(string text)
        {
            double value;
            var result = double.TryParse(text, out value);

            if (value <= 0) result = false;
            return result;
        }

        public bool ValidatePrice(string text)
        {
            decimal value;
            var result = decimal.TryParse(text, out value);

            if (value <= 0) result = false;
            return result;
        }

        public void Submit()
        {
            if (string.IsNullOrEmpty(Ticker)) throw new ArgumentException("", "Ticker");
            if (Volume == 0) throw new ArgumentException("", "Volume");
            if (SharePrice == 0) throw new ArgumentException("", "SharePrice");
            if (OrderType == 0 ) throw new ArgumentException("", "OrderType");

            var order = new Order
            {
                Issued = DateTime.Now,
                Ticker = Ticker,
                Shares = Volume,
                Price = SharePrice,
                OrderType = OrderType
            };
            var list = new List<Order> {order};
            _brokerage.SubmitOrders(list);
        }
    }
}