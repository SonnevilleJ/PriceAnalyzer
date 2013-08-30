using System;
using System.Threading;
using System.Threading.Tasks;
using Sonneville.PriceTools.Data;

namespace Sonneville.PriceTools.AutomatedTrading
{
    public abstract class ContinuousAnalyzer : IDisposable
    {
        #region Private Members

        private readonly object _syncRoot = new object();

        private readonly IPriceDataProvider _priceDataProvider;
        private readonly IPriceSeries _priceSeries;
        private readonly ISignalProcessor _signalProcessor;

        #endregion
        
        #region Constructors

        protected ContinuousAnalyzer(IPriceSeries priceSeries, ISignalProcessor signalProcessor, IPriceDataProvider priceDataProvider)
        {
            _priceSeries = priceSeries;
            _signalProcessor = signalProcessor;
            _priceDataProvider = priceDataProvider;
        }

        ~ContinuousAnalyzer()
        {
            Dispose();
        }

        #endregion

        #region IDisposable Implementation

        public void Dispose()
        {
            Stop();
        }

        #endregion

        protected IPriceSeries PriceSeries
        {
            get { return _priceSeries; }
        }

        protected IPriceDataProvider PriceDataProvider
        {
            get { return _priceDataProvider; }
        }

        protected ISignalProcessor SignalProcessor
        {
            get { return _signalProcessor; }
        }

        private bool IsRunning { get; set; }

        private CancellationTokenSource CancellationTokenSource { get; set; }

        private Task Task { get; set; }

        #region Control Methods

        public void Start()
        {
            lock (_syncRoot)
            {
                if (IsRunning)
                    throw new InvalidOperationException(
                        "Cannot start execution of the ContinuousAnalyzer because it has already been started.");

                CancellationTokenSource = new CancellationTokenSource();
                var cancellationToken = CancellationTokenSource.Token;
                cancellationToken.ThrowIfCancellationRequested();

                IsRunning = true;
                Task = new Task(() => Execute(cancellationToken), cancellationToken);

                Task.Start();
            }
        }

        public void Stop()
        {
            if (CancellationTokenSource != null)
            {
                CancellationTokenSource.Cancel();
                CancellationTokenSource = null;
                Task = null;
                IsRunning = false;
            }
        }

        #endregion
        
        private void Execute(CancellationToken token)
        {
            var resetEvent = new AutoResetEvent(false);
            EventHandler<NewDataAvailableEventArgs> handler = (source, eventArgs) => resetEvent.Set();

            try
            {
                PriceSeries.NewDataAvailable += handler;

                while (true)
                {
                    resetEvent.WaitOne();

                    token.ThrowIfCancellationRequested();
                    AnalyzePeriodo();
                }
            }
            catch (TaskCanceledException)
            {
                PriceSeries.NewDataAvailable -= handler;
            }
        }

        protected abstract void AnalyzePeriodo();
    }
}