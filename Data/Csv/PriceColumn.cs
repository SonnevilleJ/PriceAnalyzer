﻿namespace Sonneville.PriceTools.Data.Csv
{
    /// <summary>
    /// Columns used in a price history CSV file.
    /// </summary>
    public enum PriceColumn
    {
        /// <summary>
        /// Represents a column that is unused.
        /// </summary>
        None = 0,

        /// <summary>
        /// Represents the Date column in a table of static <see cref="PricePeriod"/> data.
        /// </summary>
        Date,

        /// <summary>
        /// Represents the Open column in a table of static <see cref="PricePeriod"/> data.
        /// </summary>
        Open,

        /// <summary>
        /// Represents the High column in a table of static <see cref="PricePeriod"/> data.
        /// </summary>
        High,

        /// <summary>
        /// Represents the Low column in a table of static <see cref="PricePeriod"/> data.
        /// </summary>
        Low,

        /// <summary>
        /// Represents the Close column in a table of static <see cref="PricePeriod"/> data.
        /// </summary>
        Close,

        /// <summary>
        /// Represents the Volume column in a table of static <see cref="PricePeriod"/> data.
        /// </summary>
        Volume,

        /// <summary>
        /// Represents the Dividends column in a table of static <see cref="PricePeriod"/> data.
        /// </summary>
        Dividends
    }
}