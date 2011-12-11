using System;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a price quote for a financial security.
    /// </summary>
    public class PriceQuote : IPriceQuote
    {
        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return string.Format("{0}: {1} shares @ {2:c}", SettlementDate, Volume, Price);
        }

        #region Equality Checks

        /// <summary>
        /// </summary>
        /// <param name = "left"></param>
        /// <param name = "right"></param>
        /// <returns></returns>
        public static bool operator ==(PriceQuote left, PriceQuote right)
        {
            if (ReferenceEquals(null, left)) return false;
            if (ReferenceEquals(null, right)) return false;

            return (left.SettlementDate == right.SettlementDate &&
                    left.Price == right.Price &&
                    left.Volume == right.Volume);
        }

        /// <summary>
        /// </summary>
        /// <param name = "left"></param>
        /// <param name = "right"></param>
        /// <returns></returns>
        public static bool operator !=(PriceQuote left, PriceQuote right)
        {
            return !(left == right);
        }

        #endregion

        #region Implementation of IEquatable<IPriceQuote>

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(IPriceQuote other)
        {
            return Equals(other as object);
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
            return this == obj as PriceQuote;
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
                int result = SettlementDate.GetHashCode();
                result = (result*397) ^ Price.GetHashCode();
                result = (result*397) ^ (Volume.HasValue ? Volume.Value.GetHashCode() : 0);
                return result;
            }
        }
        #endregion

        #region Implementation of IPriceQuote

        /// <summary>
        /// The <see cref="DateTime"/> which the price quote is made.
        /// </summary>
        public DateTime SettlementDate { get; set; }

        /// <summary>
        /// The price at which the security is available.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// The number of shares traded.
        /// </summary>
        public long? Volume { get; set; }

        #endregion
    }
}
