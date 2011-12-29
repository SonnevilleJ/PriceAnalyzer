using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Sonneville.PriceTools.Extensions;

namespace Sonneville.PriceTools.Trading
{
    /// <summary>
    /// A trading strategy which issues orders to buy and sell a security.
    /// </summary>
    public abstract class TradingStrategy
    {
        private bool _isRunning;
        private IList<IPricePeriod> _pricePeriods;
        private Task _task;
        private CancellationTokenSource _tokenSource;

        protected DateTime StartDateTime { get; set; }

        public IPriceSeries PriceSeries { get; set; }

        public ITradingAccount TradingAccount { get; set; }

        /// <summary>
        /// Signlas the TradingStrategy to start processing.
        /// </summary>
        public void Start()
        {
            if (_isRunning)
                throw new InvalidOperationException("Cannot start execution of the TradingStrategy because it has already been started.");
            if (TradingAccount == null)
                throw new InvalidOperationException("A TradingAccount is required before executing the TradingStrategy.");
            if(PriceSeries == null)
                throw new InvalidOperationException("A PriceSeries is required before executing the TradingStrategy.");

            _isRunning = true;
            _tokenSource = new CancellationTokenSource();
            var token = _tokenSource.Token;
            _task = new Task(() => Execute(token), token);
            
            _task.Start();
        }

        /// <summary>
        /// Signals the TradingStrategy to stop processing and waits for it to complete.
        /// </summary>
        public void Stop()
        {
            _tokenSource.Cancel();
            _task.Wait();

            _isRunning = false;
            _pricePeriods = null;
            _task = null;
            _tokenSource = null;
        }

        protected void ProcessPeriod(int index)
        {
            var period = _pricePeriods[index];

            const bool timeToBuy = true;

            if (timeToBuy)
            {
                var order = CreateOrder(period.Tail);

                TradingAccount.Submit(order);
            }
        }

        protected abstract Order CreateOrder(DateTime issued);

        private void Execute(CancellationToken token)
        {
            _pricePeriods = PriceSeries.GetPricePeriods(PriceSeries.Resolution, StartDateTime, DateTime.Now);

            for (var i = 0; i < _pricePeriods.Count; i++)
            {
                token.ThrowIfCancellationRequested();
                ProcessPeriod(i);
            }
        }
    }
}
