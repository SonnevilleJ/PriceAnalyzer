using System;

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
            if(type == OrderType.Deposit || type == OrderType.Withdrwawal)
            {
                throw new ArgumentOutOfRangeException("type", type, "Deposits and Withdrawals must use Deposit or Withdrawal instead of Transaction.");
            }
            else
            {
                _type = type;
            }
            _ticker = ticker.ToUpperInvariant();

            if (shares < 0)
            {
                throw new ArgumentOutOfRangeException("shares", shares, "Shares must be greater than or equal to 0.00");
            }
            else
            {
                _shares = shares;
            }

            if(price < 0.00m)
            {
                throw new ArgumentOutOfRangeException("price", price, "Price must be greater than or equal to 0.00");
            }
            else
            {
                _price = price;
            }

            if(commission < 0.00m)
            {
                throw new ArgumentOutOfRangeException("commission", commission, "Commission must be greater than or equal to 0.00");
            }
            else
            {
                _commission = commission;
            }
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
    }
}
