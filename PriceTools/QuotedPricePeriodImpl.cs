using System;
using System.Collections.Generic;
using System.Linq;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools
{
    /// <summary>
    ///   A <see cref="PricePeriod"/> made from <see cref="PriceQuotes"/>.
    /// </summary>
    public interface IQuotedPricePeriod : IPricePeriod
    {
        /// <summary>
        /// The <see cref="PriceQuoteImpl"/>s contained within this QuotedPricePeriod.
        /// </summary>
        IList<PriceQuote> PriceQuotes { get; }

        /// <summary>
        ///   Adds one or more <see cref = "PriceQuote" />s to the PriceSeries.
        /// </summary>
        /// <param name = "priceQuotes">The <see cref = "PriceQuote" />s to add.</param>
        void AddPriceQuotes(params PriceQuote[] priceQuotes);
    }

    /// <summary>
    ///   A <see cref="PricePeriod"/> made from <see cref="PriceQuotes"/>.
    /// </summary>
    internal class QuotedPricePeriodImpl : PricePeriod, IQuotedPricePeriod
    {
        private readonly List<PriceQuote> _priceQuotes = new List<PriceQuote>();
        
        /// <summary>
        /// The <see cref="PriceQuoteImpl"/>s contained within this QuotedPricePeriod.
        /// </summary>
        public IList<PriceQuote> PriceQuotes { get { return _priceQuotes.AsReadOnly(); } }

        #region Overrides of IPricePeriod

        /// <summary>
        ///   Gets the total volume of trades during the IPricePeriod.
        /// </summary>
        public override long? Volume
        {
            get { return PriceQuotes.Sum(q => q.Volume); }
        }

        /// <summary>
        ///   Gets the first DateTime in the ITimeSeries.
        /// </summary>
        public override DateTime Head
        {
            get { return PriceQuotes.Min(q => q.SettlementDate); }
        }

        /// <summary>
        ///   Gets the last DateTime in the ITimeSeries.
        /// </summary>
        public override DateTime Tail
        {
            get { return PriceQuotes.Max(q => q.SettlementDate); }
        }

        /// <summary>
        /// Gets the values stored within the ITimeSeries.
        /// </summary>
        public override IDictionary<DateTime, decimal> Values
        {
            get
            {
                return PriceQuotes.ToDictionary(priceQuote => priceQuote.SettlementDate, priceQuote => priceQuote.Price);
            }
        }

        /// <summary>
        ///   Adds one or more <see cref = "PriceQuote" />s to the PriceSeries.
        /// </summary>
        /// <param name = "priceQuotes">The <see cref = "PriceQuote" />s to add.</param>
        public void AddPriceQuotes(params PriceQuote[] priceQuotes)
        {
            foreach (var quote in priceQuotes)
            {
                _priceQuotes.Add(quote);
            }
            var args = new NewPriceDataAvailableEventArgs
                           {
                               Head = priceQuotes.Min(quote => quote.SettlementDate),
                               Tail = priceQuotes.Max(quote => quote.SettlementDate)
                           };
            InvokeNewPriceDataAvailable(args);
        }

        /// <summary>
        ///   Gets the closing price for the IPricePeriod.
        /// </summary>
        public override decimal Close
        {
            get { return PriceQuotes.OrderBy(q => q.SettlementDate).Last().Price; }
        }

        /// <summary>
        ///   Gets the highest price that occurred during the IPricePeriod.
        /// </summary>
        public override decimal High
        {
            get { return PriceQuotes.Max(q => q.Price); }
        }

        /// <summary>
        ///   Gets the lowest price that occurred during the IPricePeriod.
        /// </summary>
        public override decimal Low
        {
            get { return PriceQuotes.Min(q => q.Price); }
        }

        /// <summary>
        ///   Gets the opening price for the IPricePeriod.
        /// </summary>
        public override decimal Open
        {
            get { return PriceQuotes.OrderBy(q => q.SettlementDate).First().Price; }
        }

        /// <summary>
        ///   Gets a value stored at a given DateTime index of the PricePeriod.
        /// </summary>
        /// <param name = "index">The DateTime of the desired value.</param>
        /// <returns>The value of the ITimeSeries as of the given DateTime.</returns>
        public override decimal this[DateTime index]
        {
            get { return PriceQuotes.Where(q => q.SettlementDate <= index).Last().Price; }
        }

        #endregion
    }
}