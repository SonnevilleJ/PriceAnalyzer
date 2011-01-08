using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sonneville.PriceTools
{
    /// <summary>
    ///   Represents a position taken using one or more <see cref = "ITransaction" />s.
    /// </summary>
    public interface IPosition : ITimeSeries, ISerializable
    {
        /// <summary>
        ///   Gets an enumeration of all <see cref = "ITransaction" />s in this IPosition.
        /// </summary>
        IEnumerable<ITransaction> Transactions { get; }

        /// <summary>
        ///   Gets the total number of currently held shares.
        /// </summary>
        double OpenShares { get; }

        /// <summary>
        ///   Gets the current <see cref = "PositionStatus" /> of this IPosition.
        /// </summary>
        PositionStatus PositionStatus { get; }

        /// <summary>
        ///   Gets the <see cref = "IPosition.PositionStatus" /> of this IPosition as of a given <see cref = "DateTime" />.
        /// </summary>
        /// <param name = "date">The <see cref = "DateTime" /> to use.</param>
        PositionStatus GetPositionStatus(DateTime date);

        /// <summary>
        ///   Adds an <see cref = "ITransaction" /> to this IPosition.
        ///   Note: An IPosition can only contain <see cref = "ITransaction" />s for a single ticker symbol.
        /// </summary>
        /// <param name = "transaction">The <see cref = "ITransaction" /> to add to the IPosition.</param>
        void AddTransaction(ITransaction transaction);

        /// <summary>
        ///   Gets the value of the IPortfolio as of a given date.
        /// </summary>
        /// <param name = "date">The <see cref = "DateTime" /> to use.</param>
        /// <param name = "considerCommissions">A value indicating whether commissions should be included in the result.</param>
        /// <returns>The value of the IPortfolio as of the given date.</returns>
        decimal GetValue(DateTime date, bool considerCommissions);

        /// <summary>
        ///   Gets the gross investment of this Position, ignoring any proceeds and commissions.
        /// </summary>
        /// <param name = "date">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The total amount spent on share purchases.</returns>
        decimal GetCost(DateTime date);

        /// <summary>
        ///   Gets the gross proceeds of this Position, ignoring all costs and commissions.
        /// </summary>
        /// <param name = "date">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The total amount of proceeds from share sales.</returns>
        decimal GetProceeds(DateTime date);

        /// <summary>
        ///   Gets the total commissions paid as of a given date.
        /// </summary>
        /// <param name = "date">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The total amount of commissions from <see cref = "ITransaction" />s.</returns>
        decimal GetCommissions(DateTime date);

        /// <summary>
        ///   Validates the IPosition.
        /// </summary>
        void Validate();

        /// <summary>
        ///   Gets the raw rate of return for this IPosition, not accounting for commissions.
        /// </summary>
        decimal GetRawReturn(DateTime date);

        /// <summary>
        ///   Gets the total rate of return for this IPosition, after commissions.
        /// </summary>
        decimal GetTotalReturn(DateTime date);

        /// <summary>
        ///   Gets the value of the IPortfolio as of a given date.
        /// </summary>
        /// <param name = "date">The <see cref = "DateTime" /> to use.</param>
        /// <returns>The value of the IPortfolio as of the given date.</returns>
        decimal GetValue(DateTime date);

        /// <summary>
        ///   Gets the total rate of return on an annual basis for this Position.
        /// </summary>
        /// <remarks>
        ///   Assumes a year has 365 days.
        /// </remarks>
        decimal GetTotalAnnualReturn(DateTime date);
    }
}