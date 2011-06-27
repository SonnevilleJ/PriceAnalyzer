using System.Linq;
using System.Reflection;
using Sonneville.PriceTools;

namespace Sonneville.PriceAnalyzer
{
    public abstract class Watcher
    {
        public PriceSeries PriceSeries { get; set; }
        public decimal Threshold { get; set; }
        public abstract PropertyInfo Property { get; }
        public event TriggerDelegate TriggerEvent;

        private void InvokeTriggerEvent(WatcherEventArgs e)
        {
            TriggerDelegate handler = TriggerEvent;
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

    public class PriceOverThresholdWatcher : Watcher
    {
        public override PropertyInfo Property
        {
            get
            {
                return (typeof(PricePeriod)).GetProperty("High");
            }
        }

        protected override bool Evaluate(PricePeriod pricePeriod)
        {
            return (decimal) Property.GetValue(pricePeriod, null) >= Threshold;
        }
    }

    public delegate void TriggerDelegate(object sender, WatcherEventArgs e);
}
