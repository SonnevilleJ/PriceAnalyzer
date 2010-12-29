using System.Collections.Generic;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a position taken using one or more ITransactions.
    /// </summary>
    public interface IPosition
    {
        /// <summary>
        /// Gets or sets the ITransaction which opened this position.
        /// </summary>
        ITransaction Open
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the ITransaction which closed this position.
        /// </summary>
        ITransaction Close
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the total value of this position, after commissions.
        /// </summary>
        decimal TotalValue
        {
            get;
        }

        /// <summary>
        /// Validates an IPosition.
        /// </summary>
        /// <returns>A value indicating if the instance is valid.</returns>
        void Validate();

        /// <summary>
        /// Validates an IPosition.
        /// </summary>
        /// <param name="errors">A list of any validation errors.</param>
        /// <returns>A value indicating if the instance is valid.</returns>
        bool Validate(out IList<string> errors);
    }
}
