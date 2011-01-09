using System;
using System.Runtime.Serialization;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// A transaction (or order) for a financial security.
    /// </summary>
    [Serializable]
    public class Transaction : ITransaction
    {
        #region Private Members
        
        private readonly DateTime _date;
        private readonly OrderType _type;
        private readonly string _ticker;
        private readonly double _shares;
        private readonly decimal _price;
        private readonly decimal _commission;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a Transaction.
        /// </summary>
        /// <param name="date">The date and time this Transaction took place.</param>
        /// <param name="type">The <see cref="PriceTools.OrderType"/> of this Transaction.</param>
        /// <param name="ticker">The ticker of the security bought or sold.</param>
        /// <param name="price">The price at which the Transaction took place.</param>
        public Transaction(DateTime date, OrderType type, string ticker, decimal price)
            : this(date, type, ticker, price, 1.0)
        { }

        /// <summary>
        /// Constructs a Transaction.
        /// </summary>
        /// <param name="date">The date and time this Transaction took place.</param>
        /// <param name="type">The <see cref="PriceTools.OrderType"/> of this Transaction.</param>
        /// <param name="ticker">The ticker of the security bought or sold.</param>
        /// <param name="price">The price at which the Transaction took place.</param>
        /// <param name="shares">The optional number of shares which were traded. Default = 1</param>
        public Transaction(DateTime date, OrderType type, string ticker, decimal price, double shares)
            : this(date, type, ticker, price, shares, 0.0m)
        { }

        /// <summary>
        /// Constructs a Transaction.
        /// </summary>
        /// <param name="date">The date and time this Transaction took place.</param>
        /// <param name="type">The <see cref="PriceTools.OrderType"/> of this Transaction.</param>
        /// <param name="ticker">The ticker of the security bought or sold.</param>
        /// <param name="price">The price at which the Transaction took place.</param>
        /// <param name="shares">The optional number of shares which were traded. Default = 1</param>
        /// <param name="commission">The optional commission paid for this Transaction. Default = $0.00</param>
        public Transaction(DateTime date, OrderType type, string ticker, decimal price, double shares, decimal commission)
        {
            _date = date;
            _ticker = ticker.ToUpperInvariant();

            if (type == OrderType.Withdrawal || type == OrderType.Deposit)
            {
                throw new ArgumentOutOfRangeException("type", type,
                                                      "Deposits and Withdrawals must use Deposit or Withdrawal instead of Transaction.");
            }
            _type = type;

            if (shares < 0)
            {
                throw new ArgumentOutOfRangeException("shares", shares, "Shares must be greater than or equal to 0.00");
            }
            _shares = shares;

            if(price < 0.00m)
            {
                throw new ArgumentOutOfRangeException("price", price, "Price must be greater than or equal to 0.00");
            }
            _price = price;

            if(commission < 0.00m)
            {
                throw new ArgumentOutOfRangeException("commission", commission, "Commission must be greater than or equal to 0.00");
            }
            _commission = commission;
        }

        #endregion

        #region Accessors

        /// <summary>
        /// Gets the date and time at which the Transaction occured.
        /// </summary>
        public DateTime SettlementDate
        {
            get { return _date; }
        }

        /// <summary>
        /// Gets the TransactionType of this Transaction.
        /// </summary>
        public OrderType OrderType
        {
            get { return _type; }
        }
        /// <summary>
        /// Gets the ticker of the security traded.
        /// </summary>
        public string Ticker
        {
            get { return _ticker; }
        }

        /// <summary>
        /// Gets the number of shares traded.
        /// </summary>
        public double Shares
        {
            get { return _shares; }
        }

        /// <summary>
        /// Gets the price at which the Transaction took place.
        /// </summary>
        public decimal Price
        {
            get { return _price; }
        }

        /// <summary>
        /// Gets the commission paid for this Transaction.
        /// </summary>
        public decimal Commission
        {
            get { return _commission; }
        }

        #endregion

        #region Implementation of ISerializable

        protected Transaction(SerializationInfo info, StreamingContext context)
        {
            _date = info.GetDateTime("Date");
            _type = (OrderType) info.GetValue("Type", typeof (OrderType));
            _price = info.GetDecimal("Price");
            _shares = info.GetDouble("Shares");
            _ticker = info.GetString("Ticker");
            _commission = info.GetDecimal("Commission");
        }

        /// <summary>
        /// Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo"/> with the data needed to serialize the target object.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> to populate with data. </param><param name="context">The destination (see <see cref="T:System.Runtime.Serialization.StreamingContext"/>) for this serialization. </param><exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Date", _date);
            info.AddValue("Type", _type);
            info.AddValue("Price", _price);
            info.AddValue("Shares", _shares);
            info.AddValue("Ticker", _ticker);
            info.AddValue("Commission", _commission);
        }

        #endregion

        #region Equality Checks

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator==(Transaction left, Transaction right)
        {
            return (left._commission == right._commission &&
                    left._date == right._date &&
                    left._price == right._price &&
                    left._shares == right._shares &&
                    left._ticker == right._ticker &&
                    left._type == right._type);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(Transaction left, Transaction right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(ITransaction other)
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
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(Transaction)) return false;
            return this == (Transaction)obj;
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
                int result = _date.GetHashCode();
                result = (result * 397) ^ _commission.GetHashCode();
                result = (result * 397) ^ _date.GetHashCode();
                result = (result * 397) ^ _price.GetHashCode();
                result = (result * 397) ^ _shares.GetHashCode();
                result = (result * 397) ^ _ticker.GetHashCode();
                result = (result * 397) ^ _type.GetHashCode();
                return result;
            }
        }

        #endregion
    }
}
