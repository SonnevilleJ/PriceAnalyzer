using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    }
}
