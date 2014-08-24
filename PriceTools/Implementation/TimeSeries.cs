using System;
using System.Collections.Generic;
using System.Linq;

namespace Sonneville.PriceTools.Implementation
{
    internal class TimeSeries<TPeriodValue> : ITimeSeries<ITimePeriod<TPeriodValue>, TPeriodValue>
    {
        private readonly IEnumerable<ITimePeriod<TPeriodValue>> _periods = new List<ITimePeriod<TPeriodValue>>();

        internal TimeSeries(IEnumerable<ITimePeriod<TPeriodValue>> list)
        {
            _periods = list;
        }

        public TPeriodValue this[DateTime dateTime]
        {
            get { return TimePeriods.First(p => dateTime >= p.Head && dateTime <= p.Tail)[dateTime]; }
        }

        public DateTime Head { get { return TimePeriods.First().Head; } }

        public DateTime Tail { get { return TimePeriods.Last().Tail; } }

        public Resolution Resolution { get { return TimePeriods.Min(p => p.Resolution); } }

        public IEnumerable<ITimePeriod<TPeriodValue>> TimePeriods
        {
            get { return _periods; }
        }
    }
}