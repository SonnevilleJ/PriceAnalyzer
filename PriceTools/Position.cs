using System;
using System.Runtime.Serialization;

namespace Sonneville.PriceTools
{
    /// <summary>
    ///   A trade made for a financial security. A Position is comprised of an opening transaction, and optionally, a closing transaction.
    /// </summary>
    [Serializable]
    public class Position : IPosition
    {
        private ITransaction _closingTransaction;
        private ITransaction _openingTransaction;

        /// <summary>
        ///   Constructs a Position from an opening transaction and an optional closing transaction.
        /// </summary>
        /// <param name = "openingTransaction">The transaction which opened this trade.
        ///   The OrderTYpe for opening transactions must be either <see cref = "OrderType.Buy" /> or <see cref = "OrderType.SellShort" />.</param>
        public Position(ITransaction openingTransaction)
            : this(openingTransaction, null)
        {
        }

        /// <summary>
        ///   Constructs a Position from an opening transaction and an optional closing transaction.
        /// </summary>
        /// <param name = "openingTransaction">The transaction which opened this trade.
        ///   The OrderTYpe for opening transactions must be either <see cref = "OrderType.Buy" /> or <see cref = "OrderType.SellShort" />.</param>
        /// <param name = "closingTransaction">The optional transaction which closed this trade.
        ///   The TransactionType for closing transactions must be either <see cref = "OrderType.Sell" /> or <see cref = "OrderType.BuyToCover" /> and must match the <see cref = "OrderType" /> of the opening transaction.</param>
        public Position(ITransaction openingTransaction, ITransaction closingTransaction)
        {
            if (openingTransaction == null)
            {
                throw new ArgumentNullException("openingTransaction", "Opening ITransaction cannot be null.");
            }
            _openingTransaction = openingTransaction;
            _closingTransaction = closingTransaction;

            Validate();
        }

        private Position(SerializationInfo info, StreamingContext context)
        {
            _openingTransaction = (ITransaction) info.GetValue("OpeningTransaction", typeof (ITransaction));
            _closingTransaction = (ITransaction) info.GetValue("ClosingTransaction", typeof (ITransaction));
        }

        private TimeSpan Duration
        {
            get { return new TimeSpan(_closingTransaction.SettlementDate.Ticks - _openingTransaction.SettlementDate.Ticks); }
        }

        private decimal PurchaseValue
        {
            get { return 0 - (_openingTransaction.Price*(decimal) _openingTransaction.Shares); }
        }

        private decimal SaleValue
        {
            get { return _closingTransaction == null ? 0 : (_closingTransaction.Price*(decimal) _closingTransaction.Shares); }
        }

        /// <summary>
        ///   Gets the raw value of the Position, not including commissions..
        /// </summary>
        public decimal RawValue
        {
            get { return SaleValue + PurchaseValue; }
        }

        /// <summary>
        ///   Gets the raw rate of return for this Position, not accounting for commissions.
        /// </summary>
        public decimal RawReturn
        {
            get
            {
                if (_closingTransaction == null)
                {
                    throw new InvalidOperationException("Cannot calculate raw return for an open position.");
                }
                return 0 - decimal.Divide(RawValue, PurchaseValue);
            }
        }

        /// <summary>
        ///   Gets the total rate of return for this Position, after commissions.
        /// </summary>
        public decimal TotalReturn
        {
            get
            {
                if (_closingTransaction == null)
                {
                    throw new InvalidOperationException("Cannot calculate return for an open position.");
                }
                return decimal.Divide(SaleValue - ClosingTransaction.Commission, 0 - PurchaseValue - OpeningTransaction.Commission) - 1.0m;
            }
        }

        /// <summary>
        ///   Gets the total rate of return on an annual basis for this Position.
        /// </summary>
        /// <remarks>
        ///   Assumes a year has 365 days.
        /// </remarks>
        public decimal TotalAnnualReturn
        {
            get
            {
                if (_closingTransaction == null)
                {
                    throw new InvalidOperationException("Cannot calculate return for an open position.");
                }
                return (TimeSpan.TicksPerDay*365/Duration.Ticks)*TotalReturn;
            }
        }

        #region IPosition Members

        /// <summary>
        ///   Gets or sets the opening transaction of this Position.
        /// </summary>
        public ITransaction OpeningTransaction
        {
            get { return _openingTransaction; }
            set { _openingTransaction = value; }
        }

        /// <summary>
        ///   Gets or sets the closing transaction of this Position.
        /// </summary>
        public ITransaction ClosingTransaction
        {
            get { return _closingTransaction; }
            set { _closingTransaction = value; }
        }

        public PositionStatus PositionStatus
        {
            get
            {
                return OpeningTransaction.Shares != ClosingTransaction.Shares ? PositionStatus.Open : PositionStatus.Closed;
            }
        }

        /// <summary>
        ///   Gets the total value of the Position, including commissions.
        /// </summary>
        public decimal TotalValue
        {
            get
            {
                if (_closingTransaction != null)
                {
                    return (SaleValue - ClosingTransaction.Commission) + (PurchaseValue - OpeningTransaction.Commission);
                }
                else
                {
                    return PurchaseValue - OpeningTransaction.Commission;
                }
            }
        }

        #endregion

        private void Validate()
        {
            switch (_openingTransaction.OrderType)
            {
                case OrderType.Buy:
                case OrderType.SellShort:
                    break;
                default:
                    // Not an opening transaction
                    throw new InvalidPositionException("Opening transaction must be of type Buy or SellShort.");
            }
            if (_closingTransaction == null)
            {
                // Not much to validate with a position that's still open.
            }
            else
            {
                bool mismatch = true;
                switch (_openingTransaction.OrderType)
                {
                    case OrderType.Buy:
                        if (_closingTransaction.OrderType == OrderType.Sell)
                        {
                            mismatch = false;
                        }
                        break;
                    case OrderType.SellShort:
                        if (_closingTransaction.OrderType == OrderType.BuyToCover)
                        {
                            mismatch = false;
                        }
                        break;
                }
                if (mismatch)
                {
                    throw new InvalidPositionException("Transaction types must match.");
                }
                if (_closingTransaction.SettlementDate < _openingTransaction.SettlementDate)
                {
                    throw new InvalidPositionException(
                        "Closing transaction date must occur after opening transaction date.");
                }
                if (_closingTransaction.Shares > _openingTransaction.Shares)
                {
                    throw new InvalidPositionException(
                        "Shares traded in closing transaction must be less than or equal to shares traded in opening transaction.");
                }
            }
        }
    }
}