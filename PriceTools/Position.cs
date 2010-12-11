using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// A trade made for a financial security. A Position must have an opening transaction, and optionally, a closing transaction.
    /// </summary>
    [Serializable]
    public class Position : IPosition
    {
        private ITransaction _open;
        private ITransaction _close;

        /// <summary>
        /// Constructs a Position from an opening transaction and an optional closing transaction.
        /// </summary>
        /// <param name="open">The transaction which opened this trade.
        /// The OrderTYpe for opening transactions must be either <see cref="OrderType.Buy"/> or <see cref="OrderType.SellShort"/>.</param>
        public Position(ITransaction open)
            : this(open, null)
        { }

        /// <summary>
        /// Constructs a Position from an opening transaction and an optional closing transaction.
        /// </summary>
        /// <param name="open">The transaction which opened this trade.
        /// The OrderTYpe for opening transactions must be either <see cref="OrderType.Buy"/> or <see cref="OrderType.SellShort"/>.</param>
        /// <param name="close">The optional transaction which closed this trade.
        /// The OrderType for closing transactions must be either <see cref="OrderType.Sell"/> or <see cref="OrderType.BuyToCover"/> and must match the <see cref="OrderType"/> of the opening transaction.</param>
        public Position(ITransaction open, ITransaction close)
        {
            if (open == null)
            {
                throw new ArgumentNullException("open", "Opening ITransaction cannot be null.");
            }
            _open = open;
            _close = close;

            Validate();
        }

        private Position(SerializationInfo info, StreamingContext context)
        {
            _open = (ITransaction)info.GetValue("Open", typeof(ITransaction));
            _close = (ITransaction)info.GetValue("Close", typeof(ITransaction));
        }

        /// <summary>
        /// Gets or sets the opening transaction of this Position.
        /// </summary>
        public ITransaction Open
        {
            get { return _open; }
            set { _open = value; }
        }

        /// <summary>
        /// Gets or sets the closing transaction of this Position.
        /// </summary>
        public ITransaction Close
        {
            get { return _close; }
            set { _close = value; }
        }

        private TimeSpan Duration
        {
            get
            {
                return new TimeSpan(_close.Date.Ticks - _open.Date.Ticks);
            }
        }

        private decimal PurchaseValue
        {
            get { return 0 - (_open.Price*(decimal) _open.Shares); }
        }

        private decimal SaleValue
        {
            get { return _close == null ? 0 : (_close.Price*(decimal) _close.Shares); }
        }

        /// <summary>
        /// Gets the raw value of the Position, not including commissions..
        /// </summary>
        public decimal RawValue
        {
            get
            {
                return SaleValue + PurchaseValue;
            }
        }

        /// <summary>
        /// Gets the total value of the Position, including commissions.
        /// </summary>
        public decimal TotalValue
        {
            get
            {
                if (_close != null)
                {
                    return (SaleValue - Close.Commission) + (PurchaseValue - Open.Commission);
                }
                else
                {
                    return PurchaseValue - Open.Commission;
                }
            }
        }

        /// <summary>
        /// Gets the raw rate of return for this Position, not accounting for commissions.
        /// </summary>
        public decimal RawReturn
        {
            get
            {
                if (_close == null)
                {
                    throw new InvalidOperationException("Cannot calculate raw return for an open position.");
                }
                return 0 - decimal.Divide(RawValue, PurchaseValue);
            }
        }

        /// <summary>
        /// Gets the total rate of return for this Position, after commissions.
        /// </summary>
        public decimal TotalReturn
        {
            get
            {
                if(_close == null)
                {
                    throw new InvalidOperationException("Cannot calculate return for an open position.");
                }
                return decimal.Divide(SaleValue - Close.Commission, 0 - PurchaseValue - Open.Commission) - 1.0m;
            }
        }

        /// <summary>
        /// Gets the total rate of return on an annual basis for this Position.
        /// </summary>
        /// <remarks>Assumes a year has 365 days.</remarks>
        public decimal TotalAnnualReturn
        {
            get
            {
                if(_close == null)
                {
                    throw new InvalidOperationException("Cannot calculate return for an open position.");
                }
                return (TimeSpan.TicksPerDay*365/Duration.Ticks)*TotalReturn;
            }
        }

        private void Validate()
        {
            switch (_open.OrderType)
            {
                case OrderType.Buy:
                case OrderType.SellShort:
                    break;
                default:
                    // Not an opening transaction
                    throw new InvalidPositionException("Opening transaction must be of type Buy or SellShort.");
            }
            if (_close == null)
            {
                // Not much to validate with a position that's still open.
            }
            else
            {
                bool mismatch = true;
                switch (_open.OrderType)
                {
                    case OrderType.Buy:
                        if (_close.OrderType == OrderType.Sell)
                        {
                            mismatch = false;
                        }
                        break;
                    case OrderType.SellShort:
                        if (_close.OrderType == OrderType.BuyToCover)
                        {
                            mismatch = false;
                        }
                        break;
                }
                if (mismatch)
                {
                    throw new InvalidPositionException("Transaction types must match.");
                }
                if (_close.Date < _open.Date)
                {
                    throw new InvalidPositionException(
                        "Closing transaction date must occur after opening transaction date.");
                }
                if (_close.Shares > _open.Shares)
                {
                    throw new InvalidPositionException(
                        "Shares traded in closing transaction must be less than or equal to shares traded in opening transaction.");
                }
            }
        }
    }
}
