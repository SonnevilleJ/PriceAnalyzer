using System;
using System.Collections.Generic;
using System.Linq;

namespace Sonneville.PriceTools
{
    /// <summary>
    ///   A PricePeriod made from PriceQuotes.
    /// </summary>
    public class QuotedPricePeriod : PricePeriod
    {
        private readonly IList<PriceQuote> _priceQuotes = new List<PriceQuote>();

        /// <summary>
        /// The <see cref="PriceQuote"/>s contained within this QuotedPricePeriod.
        /// </summary>
        public IList<PriceQuote> PriceQuotes { get { return _priceQuotes; } }

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
        ///   Adds one or more <see cref = "IPriceQuote" />s to the IPriceSeries.
        /// </summary>
        /// <param name = "priceQuote">The <see cref = "IPriceQuote" />s to add.</param>
        public void AddPriceQuotes(params IPriceQuote[] priceQuote)
        {
            DateTime[] dates = new DateTime[priceQuote.Count()];
            for (int i = 0; i < priceQuote.Length; i++)
            {
                IPriceQuote quote = priceQuote[i];
                PriceQuotes.Add((PriceQuote) quote);
                dates[i] = quote.SettlementDate;
            }
            NewPriceDataAvailableEventArgs args = new NewPriceDataAvailableEventArgs
                                                      {
                                                          Indices = dates
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

        #region Equality Checks

        /// <summary>
        /// </summary>
        /// <param name = "left"></param>
        /// <param name = "right"></param>
        /// <returns></returns>
        public static bool operator ==(QuotedPricePeriod left, QuotedPricePeriod right)
        {
            if (ReferenceEquals(null, left)) return false;
            if (ReferenceEquals(null, right)) return false;

            bool priceQuotesMatch = false;
            if (left.PriceQuotes.Count == right.PriceQuotes.Count)
            {
                priceQuotesMatch = left.PriceQuotes.All(quote => right.PriceQuotes.Contains(quote));
            }

            return priceQuotesMatch &&
                left.Close == right.Close &&
                left.Head == right.Head &&
                left.High == right.High &&
                left.Low == right.Low &&
                left.Open == right.Open &&
                left.Tail == right.Tail &&
                left.Volume == right.Volume;
        }

        /// <summary>
        /// </summary>
        /// <param name = "left"></param>
        /// <param name = "right"></param>
        /// <returns></returns>
        public static bool operator !=(QuotedPricePeriod left, QuotedPricePeriod right)
        {
            return !(left == right);
        }

        #endregion

        #region Implementation of IEquatable<IPricePeriod>

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>. </param><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            return this == obj as QuotedPricePeriod;
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            unchecked
            {
                int result = 0;
                foreach (var priceQuote in PriceQuotes)
                {
                    result = (result*397) ^ priceQuote.GetHashCode();
                }
                return result;
            }
        }

        #endregion
    }
}