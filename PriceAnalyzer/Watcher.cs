using System.Collections.Generic;
using Sonneville.PriceTools;

namespace Sonneville.PriceAnalyzer
{
    public abstract class Watcher
    {
        public PriceSeries PriceSeries { get; set; }
        public event WatcherTriggerDelegate TriggerEvent;

        private void InvokeTriggerEvent(WatcherEventArgs e)
        {
            WatcherTriggerDelegate handler = TriggerEvent;
            if (handler != null) handler(this, e);
        }

        public void Execute()
        {
            foreach (var args in GetTriggerPeriodsArgs())
            {
                InvokeTriggerEvent(args);
            }
        }

        protected abstract IEnumerable<WatcherEventArgs> GetTriggerPeriodsArgs();
    }
}
