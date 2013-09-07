using System;
using System.Globalization;

namespace Sonneville.PriceTools.Implementation
{
    /// <summary>
    /// Represents a price quote for a financial security.
    /// </summary>
    [Serializable]
    public class PriceTick : IEquatable<PriceTick>
    {
        /// <summary>
        /// Constructs a PriceTick.
        /// </summary>
        /// <param name="settlementDate">The <see cref="DateTime"/> for which the quote is valid.</param>
        /// <param name="price">The quoted price.</param>
        /// <param name="volume">The number of shares for which the quote is valid.</param>
        internal PriceTick(DateTime settlementDate, decimal price, long? volume = null)
        {
            if (price <= 0)
                throw new ArgumentOutOfRangeException("price", price, Strings.PriceTickImpl_PriceTickImpl_Quoted_price_must_be_greater_than_zero_);
            if (volume.HasValue && volume <= 0)
                throw new ArgumentOutOfRangeException("volume", volume, Strings.PriceTickImpl_PriceTickImpl_Quoted_volume__if_specified__must_be_greater_than_zero_);

            SettlementDate = settlementDate;
            Price = price;
            Volume = volume;
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}: {1} shares @ {2:c}", SettlementDate, Volume, Price);
        }

        /// <summary>
        /// The <see cref="DateTime"/> which the price quote is made.
        /// </summary>
        public DateTime SettlementDate { get; private set; }

        /// <summary>
        /// The price at which the security is available.
        /// </summary>
        public decimal Price { get; private set; }

        /// <summary>
        /// The number of shares traded.
        /// </summary>
        public long? Volume { get; private set; }

        #region Equality

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(PriceTick other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return SettlementDate == other.SettlementDate &&
                   Price == other.Price &&
                   ((Volume == null && other.Volume == null) || Volume == other.Volume);
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
            return Equals(obj as PriceTick);
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
                var result = SettlementDate.GetHashCode();
                result = (result*397) ^ Price.GetHashCode();
                result = (result*397) ^ (Volume.HasValue ? Volume.Value.GetHashCode() : 0);
                return result;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(PriceTick left, PriceTick right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(PriceTick left, PriceTick right)
        {
            return !Equals(left, right);
        }

        #endregion
    }
}
