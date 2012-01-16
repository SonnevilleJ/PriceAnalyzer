using System;
using System.Collections.Generic;

namespace Sonneville.PriceTools.SecurityAnalysis
{
    public abstract class Analyzer
    {
        private ITimeSeries _timeSeries;
        public ITimeSeries TimeSeries
        {
            get { return _timeSeries; }
            set
            {
                _timeSeries = value;
                ValidateTimeSeries();
            }
        }

        protected virtual void ValidateTimeSeries()
        {
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

        protected static AnalyzerEventArgs CreateEventArgs(DateTime dateTime)
        {
            return new AnalyzerEventArgs {DateTime = dateTime};
        }
    }
}