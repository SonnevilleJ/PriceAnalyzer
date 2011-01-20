﻿using System;
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
        /// <param name="shares">The optional number of shares which were traded. Default = 1</param>
        /// <param name="commission">The optional commission paid for this Transaction. Default = $0.00</param>
        internal Transaction(DateTime date, OrderType type, string ticker, decimal price, double shares, decimal commission)
        {
            _date = date;
            _ticker = ticker.ToUpperInvariant();
            _type = type;
            _shares = shares;
            _price = price;
            _commission = commission;

            switch (type)
            {
                case OrderType.DividendReceipt:
                case OrderType.DividendReinvestment:
                    if (shares <= 0)
                    {
                        throw new ArgumentOutOfRangeException("shares", shares, "Dividend shares must be greater than zero.");
                    }
                    if (commission != 0)
                    {
                        throw new ArgumentOutOfRangeException("commission", commission, "Commission for dividends must be zero.");
                    }
                    if (price <= 0)
                    {
                        throw new ArgumentOutOfRangeException("price", price, String.Format("Price for dividends must be greater than {0}.", 0D));
                    }
                    break;
                case OrderType.Deposit:
                case OrderType.Withdrawal:
                    if (commission != 0)
                    {
                        throw new ArgumentOutOfRangeException("commission", commission, "Commission for dividends must be zero.");
                    }
                    break;
                case OrderType.Buy:
                case OrderType.BuyToCover:
                case OrderType.Sell:
                case OrderType.SellShort:
                    if (shares < 0)
                    {
                        throw new ArgumentOutOfRangeException("shares", shares, "Shares must be greater than or equal to 0.00");
                    }
                    if (price < 0.00m)
                    {
                        throw new ArgumentOutOfRangeException("price", price, "Price must be greater than or equal to 0.00");
                    }
                    if (commission < 0.00m)
                    {
                        throw new ArgumentOutOfRangeException("commission", commission, "Commission must be greater than or equal to 0.00");
                    }
                    break;
            }
        }

        #endregion

        #region Accessors

        /// <summary>
        /// Gets The date and time at which the Transaction occured.
        /// </summary>
        public virtual DateTime SettlementDate
        {
            get { return _date; }
        }

        /// <summary>
        /// Gets the TransactionType of this Transaction.
        /// </summary>
        public virtual OrderType OrderType
        {
            get { return _type; }
        }
        /// <summary>
        /// Gets the ticker of the security traded.
        /// </summary>
        public virtual string Ticker
        {
            get { return _ticker; }
        }

        /// <summary>
        /// Gets the number of shares traded.
        /// </summary>
        public virtual double Shares
        {
            get { return _shares; }
        }

        /// <summary>
        /// Gets the price at which the Transaction took place.
        /// </summary>
        public virtual decimal Price
        {
            get
            {
                switch(OrderType)
                {
                    case OrderType.Deposit:
                    case OrderType.DividendReceipt:
                    case OrderType.DividendReinvestment:
                    case OrderType.Buy:
                    case OrderType.SellShort:
                        return _price;
                    case OrderType.Withdrawal:
                    case OrderType.Sell:
                    case OrderType.BuyToCover:
                        return -1 * _price;
                    default:
                        throw new InvalidOperationException(String.Format("Unknown OrderType: {0}.", OrderType));
                }
            }
        }

        /// <summary>
        /// Gets the commission paid for this Transaction.
        /// </summary>
        public virtual decimal Commission
        {
            get { return _commission; }
        }

        #endregion

        #region Implementation of ISerializable

        /// <summary>
        /// Deserializes a Transaction object.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
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
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
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
