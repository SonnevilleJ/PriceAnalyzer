using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools.Data.Csv
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
        /// Represents the Date column in a table of <see cref="ShareTransaction"/> elements.
        /// </summary>
        Date,

        /// <summary>
        /// Represents the OrderType column in a table of <see cref="ShareTransaction"/> elements.
        /// </summary>
        OrderType,

        /// <summary>
        /// Represents the Symbol column in a table of <see cref="ShareTransaction"/> elements.
        /// </summary>
        Symbol,

        /// <summary>
        /// Represents the Shares column in a table of <see cref="ShareTransaction"/> elements.
        /// </summary>
        Shares,

        /// <summary>
        /// Represents the PricePerShare column in a table of <see cref="ShareTransaction"/> elements.
        /// </summary>
        PricePerShare,

        /// <summary>
        /// Represents the Commission column in a table of <see cref="ShareTransaction"/> elements.
        /// </summary>
        Commission,

        /// <summary>
        /// Represents the Total Basis column in a table of <see cref="ShareTransaction"/> elements.
        /// </summary>
        TotalBasis
    }
}