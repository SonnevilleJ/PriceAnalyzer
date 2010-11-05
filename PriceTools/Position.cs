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
        /// <param name="close">The optional transaction which closed this trade.
        /// The OrderType for closing transactions must be either <see cref="OrderType.Sell"/> or <see cref="OrderType.BuyToCover"/> and must match the <see cref="OrderType"/> of the opening transaction.</param>
        public Position(ITransaction open, ITransaction close = null)
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

        /// <summary>
        /// Gets the value of the Position, minus the costs for the opening transaction.
        /// </summary>
        public decimal Value
        {
            get
            {
                decimal openValue = (_open.Price * decimal.Parse((0 - _open.Shares).ToString())) - _open.Commission;
                decimal closeValue = _close == null ? 0 : (_close.Price * decimal.Parse(_close.Shares.ToString())) - _close.Commission;

                return closeValue + openValue;

                // Below is application code which requires access to a database of prices.
                //decimal originalPrice = _open.Price;
                //decimal commissions = _close != null ? _open.Commission + _close.Commission : _open.Commission;
                //decimal currentPrice = PriceUtil.GetPerSharePrice(_open.Ticker);
                //double shares = _close != null ? _open.Shares - _close.Shares : _open.Shares;
                //return ((originalPrice - currentPrice) - commissions) * decimal.Parse(shares.ToString("M"));
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
