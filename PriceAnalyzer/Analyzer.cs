using System.Collections.Generic;
using System.Linq;
using Sonneville.PriceTools;

namespace Sonneville.PriceAnalyzer
{
    public abstract class Analyzer
    {
        private PricePeriod[] _pricePeriods;

        public PriceSeries PriceSeries
        {
            set
            {
                _pricePeriods = value.PricePeriods.ToArray();
            }
        }

        public PricePeriod[] PricePeriods
        {
            get { return _pricePeriods; }
            set { _pricePeriods = value; }
        }
        public event AnalyzerTriggerDelegate TriggerEvent;

        private void InvokeTriggerEvent(AnalyzerEventArgs e)
        {
            AnalyzerTriggerDelegate handler = TriggerEvent;
            if (handler != null) handler(this, e);
        }

        public void Execute()
        {
            foreach (var args in GetTriggerPeriodsArgs())
            {
                InvokeTriggerEvent(args);
            }
        }

        protected abstract IEnumerable<AnalyzerEventArgs> GetTriggerPeriodsArgs();
    }
}
