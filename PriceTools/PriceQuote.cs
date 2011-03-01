namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a price quote for a financial security.
    /// </summary>
    public partial class PriceQuote : IPriceQuote
    {
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
                int result = _SettlementDate.GetHashCode();
                result = (result*397) ^ _Price.GetHashCode();
                result = (result*397) ^ (_Volume.HasValue ? _Volume.Value.GetHashCode() : 0);
                return result;
            }
        }
        #endregion
    }
}
