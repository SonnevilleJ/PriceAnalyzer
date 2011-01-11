﻿namespace Sonneville.PriceTools.Data
{
    /// <summary>
    /// Columns used in a Transaction CSV file.
    /// </summary>
    public enum TransactionColumn
    {
        /// <summary>
        /// Represents a column that is unused.
        /// </summary>
        None = 0,

        /// <summary>
        /// Represents the Date column in a table of <see cref="ITransaction"/> elements.
        /// </summary>
        Date,

        /// <summary>
        /// Represents the OrderType column in a table of <see cref="ITransaction"/> elements.
        /// </summary>
        OrderType,

        /// <summary>
        /// Represents the Symbol column in a table of <see cref="ITransaction"/> elements.
        /// </summary>
        Symbol,

        /// <summary>
        /// Represents the Shares column in a table of <see cref="ITransaction"/> elements.
        /// </summary>
        Shares,

        /// <summary>
        /// Represents the PricePerShare column in a table of <see cref="ITransaction"/> elements.
        /// </summary>
        PricePerShare,

        /// <summary>
        /// Represents the Commission column in a table of <see cref="ITransaction"/> elements.
        /// </summary>
        Commission
    }
}