using System.Collections.Generic;
using Sonneville.PriceTools;

namespace Sonneville.PriceAnalyzer
{
    public abstract class Analyzer
    {
        public ITimeSeries TimeSeries { get; set; }

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