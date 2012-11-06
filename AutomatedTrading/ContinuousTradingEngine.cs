using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Sonneville.PriceTools.Yahoo;

namespace Sonneville.PriceTools.AutomatedTrading
{
    public class ContinuousTradingEngine
    {
        #region Private Members

        private readonly object _syncroot = new object();
        private readonly IList<string> _tradableTickers = new List<string> {"DE", "MSFT", "IBM", "GOOG", "AAPL"};
        private CancellationTokenSource _cts;
        private Task _engineTask;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1);

        #endregion

        #region Client Control

        public void Start()
        {
            lock(_syncroot)
            {
                if (_engineTask == null)
                {
                    _cts = new CancellationTokenSource();
                    _engineTask = Task.Factory.StartNew(() => Execute(_cts.Token));
                }
            }
        }

        public void Stop()
        {
            lock(_syncroot)
            {
                if (_engineTask != null)
                {
                    _cts.Cancel();
                    try
                    {
                        _semaphore.Release();
                        _engineTask.Wait();
                    }
                    finally
                    {
                        _cts = null;
                        _engineTask = null;
                    }
                }
            }
        }

        #endregion

        private void Execute(CancellationToken token)
        {
            SubscribeToPriceUpdates(token);
            while (true)
            {
                if (token.IsCancellationRequested) break;

                Iterate();
                _semaphore.Wait(token);
            }
        }

        private void SubscribeToPriceUpdates(CancellationToken token)
        {
            var provider = new YahooPriceDataProvider();
            for (int i = 0; i < _tradableTickers.Count; i++)
            {
                if (i % 10 == 0 && token.IsCancellationRequested) return;

                var priceSeries = PriceSeriesFactory.ConstructPriceSeries(_tradableTickers[i]);
                priceSeries.NewDataAvailable += (sender, e) => _semaphore.Release();
                provider.StartAutoUpdate(priceSeries);
            }
        }

        private void Iterate()
        {
            var message = String.Format("{0}: Iterating...", DateTime.Now);
            Log(message);

            // make money here
            //throw new NotImplementedException();
        }

        private static void Log(string message)
        {
            using(var writer = new StreamWriter(File.OpenWrite(@"C:\Users\Mediacenter\Documents\output.txt")))
            {
                writer.WriteLine(message);
            }
        }
    }
}
