﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a single account used to hold cash.
    /// </summary>
    public interface ICashAccount : ISerializable
    {
        /// <summary>
        /// Deposits cash into the ICashAccount.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> cash is deposited into the ICashAccount.</param>
        /// <param name="amount">The amount of cash deposited into the ICashAccount.</param>
        void Deposit(DateTime dateTime, decimal amount);

        /// <summary>
        /// Withdraws cash from the ICashAccount.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> cash is withdrawn from the ICashAccount.</param>
        /// <param name="amount">The amount of cash withdrawn from the ICashAccount.</param>
        void Withdraw(DateTime dateTime, decimal amount);

        /// <summary>
        /// Gets the ticker symbol this ICashAccount is invested in.
        /// </summary>
        string Ticker { get; }
    }
}
