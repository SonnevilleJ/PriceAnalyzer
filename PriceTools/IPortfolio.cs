﻿using System;
using System.Collections.Generic;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a portfolio of investments.
    /// </summary>
    public interface IPortfolio : ITimeSeries
    {
        /// <summary>
        /// Gets an <see cref="IList{IPosition}"/> of open positions held in this Portfolio.
        /// </summary>
        IList<IPosition> OpenPositions { get; }

        /// <summary>
        /// Gets the current total value of this Portfolio.
        /// </summary>
        decimal GetValue();

        /// <summary>
        /// Gets the total value of this Portfolio as of a given <see cref="DateTime"/>.
        /// </summary>
        /// <param name="asOfDate">The <see cref="DateTime"/> of which the value should be retrieved.</param>
        /// <returns>The total value of this Portfolio as of the given <see cref="DateTime"/>.</returns>
        decimal GetValue(DateTime asOfDate);
    }
}
