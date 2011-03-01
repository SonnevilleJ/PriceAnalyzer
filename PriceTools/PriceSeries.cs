using System;
using System.Linq;

namespace Sonneville.PriceTools
{
    /// <summary>
    ///   Represents a time series of PriceQuotes.
    /// </summary>
    public partial class PriceSeries : IPriceSeries
    {
        #region Overrides of IPricePeriod

        /// <summary>
        ///   Gets the total volume of trades during the IPricePeriod.
        /// </summary>
        public long? Volume
        {
            get { return EFVolume ?? PriceQuotes.Sum(q => q.Volume); }
            set { EFVolume = value; }
        }

        /// <summary>
        ///   Gets the data point stored at a given index within this IPriceSeries.
        /// </summary>
        /// <param name = "index">The index to retrieve.</param>
        /// <returns>The data point stored at the given index.</returns>
        IPriceQuote IPriceSeries.this[DateTime index]
        {
            get { return PriceQuotes.Where(q => q.SettlementDate <= index).OrderBy(q => q.SettlementDate).Last(); }
        }

        /// <summary>
        ///   Gets the first DateTime in the ITimeSeries.
        /// </summary>
        public DateTime Head
        {
            get { return EFHead ?? PriceQuotes.Min(q => q.SettlementDate); }
            set { EFHead = value; }
        }

        /// <summary>
        ///   Gets the last DateTime in the ITimeSeries.
        /// </summary>
        public DateTime Tail
        {
            get { return EFTail ?? PriceQuotes.Max(q => q.SettlementDate); }
            set { EFTail = value; }
        }

        /// <summary>
        ///   Determines if the ITimeSeries has a valid value for a given date.
        /// </summary>
        /// <param name = "settlementDate">The date to check.</param>
        /// <returns>A value indicating if the ITimeSeries has a valid value for the given date.</returns>
        public bool HasValue(DateTime settlementDate)
        {
            return settlementDate >= Head && settlementDate <= Tail;
        }

        /// <summary>
        ///   Event which is invoked when new price data is available for the IPriceSeries.
        /// </summary>
        public event EventHandler<NewPriceDataAvailableEventArgs> NewPriceDataAvailable;

        /// <summary>
        ///   Gets a <see cref = "IPriceSeries.TimeSpan" /> value indicating the length of time covered by this IPriceSeries.
        /// </summary>
        public TimeSpan TimeSpan
        {
            get { return Tail - Head; }
        }

        /// <summary>
        ///   Adds one or more <see cref = "IPriceQuote" />s to the IPriceSeries.
        /// </summary>
        /// <param name = "priceQuote">The <see cref = "IPriceQuote" />s to add.</param>
        public void AddPriceQuote(params IPriceQuote[] priceQuote)
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
            if (NewPriceDataAvailable != null)
            {
                NewPriceDataAvailable(this, args);
            }
        }

        /// <summary>
        ///   Gets the last price for the IPricePeriod.
        /// </summary>
        public decimal Last
        {
            get { return Close; }
        }

        /// <summary>
        ///   Gets the closing price for the IPricePeriod.
        /// </summary>
        public decimal Close
        {
            get { return EFClose ?? PriceQuotes.OrderBy(q => q.SettlementDate).Last().Price; }
            set { EFClose = value; }
        }

        /// <summary>
        ///   Gets the highest price that occurred during the IPricePeriod.
        /// </summary>
        public decimal? High
        {
            get { return EFHigh ?? PriceQuotes.Max(q => q.Price); }
            set { EFHigh = value; }
        }

        /// <summary>
        ///   Gets the lowest price that occurred during  the IPricePeriod.
        /// </summary>
        public decimal? Low
        {
            get { return EFLow ?? PriceQuotes.Min(q => q.Price); }
            set { EFLow = value; }
        }

        /// <summary>
        ///   Gets the opening price for the IPricePeriod.
        /// </summary>
        public decimal? Open
        {
            get { return EFOpen ?? PriceQuotes.OrderBy(q => q.SettlementDate).First().Price; }
            set { EFOpen = value; }
        }

        /// <summary>
        ///   Gets a value stored at a given DateTime index of the PricePeriod.
        /// </summary>
        /// <param name = "index">The DateTime of the desired value.</param>
        /// <returns>The value of the ITimeSeries as of the given DateTime.</returns>
        public decimal this[DateTime index]
        {
            get { return ((IPriceSeries)this)[index].Price; }
        }

        #endregion

        #region Equality Checks

        /// <summary>
        /// </summary>
        /// <param name = "left"></param>
        /// <param name = "right"></param>
        /// <returns></returns>
        public static bool operator ==(PriceSeries left, PriceSeries right)
        {
            if (ReferenceEquals(null, left)) return false;
            if (ReferenceEquals(null, right)) return false;

            bool priceQuotesMatch = false;
            if (left.PriceQuotes.Count == right.PriceQuotes.Count)
            {
                priceQuotesMatch = left.PriceQuotes.All(quote => right.PriceQuotes.Contains(quote));
            }

            return priceQuotesMatch &&
                left.EFClose == right.EFClose &&
                left.EFHead == right.EFHead &&
                left.EFHigh == right.EFHigh &&
                left.EFLow == right.EFLow &&
                left.EFOpen == right.EFOpen &&
                left.EFTail == right.EFTail &&
                left.EFVolume == right.EFVolume;
        }

        /// <summary>
        /// </summary>
        /// <param name = "left"></param>
        /// <param name = "right"></param>
        /// <returns></returns>
        public static bool operator !=(PriceSeries left, PriceSeries right)
        {
            return !(left == right);
        }

        #endregion

        #region Implementation of IEquatable<IPriceSeries>

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(IPriceSeries other)
        {
            return Equals((object)other);
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>. </param><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            return this == obj as PriceSeries;
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
            return base.GetHashCode();
        }

        #endregion
    }
}