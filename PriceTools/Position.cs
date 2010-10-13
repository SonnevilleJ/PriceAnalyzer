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
        {}

        private Position(SerializationInfo info, StreamingContext context)
        {
            _open = (ITransaction)info.GetValue("Open", typeof(ITransaction));
            _close = (ITransaction) info.GetValue("Close", typeof (ITransaction));
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
                decimal originalPrice = _open.Price;
                decimal currentPrice = PriceUtil.GetPerSharePrice(_open.Ticker);
                decimal commissions = _open.Commission + _close.Commission;
                double shares = _close != null ? _open.Shares - _close.Shares : _open.Shares;

                return ((originalPrice - currentPrice) - commissions) * decimal.Parse(shares.ToString("M"));
            }
        }
    }
}
