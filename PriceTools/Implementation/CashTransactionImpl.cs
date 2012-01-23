﻿using System;

namespace Sonneville.PriceTools.Implementation
{
    /// <summary>
    /// Represents a transaction for an <see cref="CashAccount"/>.
    /// </summary>
    [Serializable]
    internal abstract class CashTransactionImpl : CashTransaction
    {
        #region Constructors

        /// <summary>
        /// Constructs a CashTransaction.
        /// </summary>
        protected internal CashTransactionImpl()
        {
        }

        #endregion

        #region Implementation of Transaction

        /// <summary>
        ///   Gets the DateTime that the Transaction occurred.
        /// </summary>
        public DateTime SettlementDate { get; set; }

        #endregion

        #region Implementation of CashTransaction

        private decimal _amount;

        /// <summary>
        ///   Gets the amount of cash in this CashTransaction.
        /// </summary>
        public decimal Amount
        {
            get { return _amount; }
            set
            {
                // ensure Amount is negative for Withdrawal
                var amount = Math.Abs(value);
                if (this is Withdrawal)
                {
                    _amount = -amount;
                }
                else
                {
                    _amount = amount;
                }
            }
        }

        #endregion
    }
}
