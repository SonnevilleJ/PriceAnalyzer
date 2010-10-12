using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Sonneville.PriceTools
{
    [Serializable]
    public class Position : IPosition
    {
        private ITransaction _open;
        private ITransaction _close;

        public Position(ITransaction open, ITransaction close = null)
        {}

        public Position(SerializationInfo info, StreamingContext context)
        {
            _open = (ITransaction)info.GetValue("Open", typeof(ITransaction));
            _close = (ITransaction) info.GetValue("Close", typeof (ITransaction));
        }

        public ITransaction Open
        {
            get { return _open; }
            set { _open = value; }
        }

        public ITransaction Close
        {
            get { return _close; }
            set { _close = value; }
        }

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
