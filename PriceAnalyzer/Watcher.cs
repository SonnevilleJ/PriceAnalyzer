using System.Linq;
using Sonneville.PriceTools;

namespace Sonneville.PriceAnalyzer
{
    public abstract class Watcher
    {
        public PriceSeries PriceSeries { get; set; }
        public decimal Threshold { get; set; }
        public event WatcherTriggerDelegate TriggerEvent;

        private void InvokeTriggerEvent(WatcherEventArgs e)
        {
            WatcherTriggerDelegate handler = TriggerEvent;
            if (handler != null) handler(this, e);
        }

        protected abstract bool Evaluate(PricePeriod pricePeriod);

        public void Execute()
        {
            foreach (var args in
                PriceSeries.PricePeriods.Where(Evaluate).Select(period => new WatcherEventArgs {DateTime = period.Head}))
            {
                InvokeTriggerEvent(args);
            }
        }
    }
}