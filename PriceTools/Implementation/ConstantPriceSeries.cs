using System;
using System.Collections.Generic;

namespace Sonneville.PriceTools.Implementation
{
    internal class ConstantPriceSeries : IPriceSeries
    {
        private readonly decimal _price;

        internal ConstantPriceSeries(string ticker)
        {
            Ticker = ticker;
            _price = 1m;
        }

        public decimal this[DateTime dateTime]
        {
            get { return _price; }
        }

        public string Ticker { get; private set; }

        public DateTime Head { get; private set; }

        public DateTime Tail { get; private set; }

        public Resolution Resolution { get; private set; }

        public decimal Close { get { return _price; } }

        public decimal High { get { return _price; } }

        public decimal Low { get { return _price; } }

        public decimal Open { get { return _price; } }

        public long? Volume { get; private set; }

        public IEnumerable<ITimePeriod<decimal>> TimePeriods { get; private set; }

        public IEnumerable<IPricePeriod> PricePeriods { get; private set; }

        public void AddPriceData(IPricePeriod pricePeriod)
        {
            throw new NotSupportedException();
        }

        public void AddPriceData(IEnumerable<IPricePeriod> pricePeriods)
        {
            throw new NotSupportedException();
        }

        public bool Equals(IPriceSeries other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(IPricePeriod other)
        {
            throw new NotImplementedException();
        }
    }
}