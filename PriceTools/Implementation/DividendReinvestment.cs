using System;

namespace Sonneville.PriceTools.Implementation
{
    /// <summary>
    /// Represents a transaction to reinvest dividends.
    /// </summary>
    [Serializable]
    internal sealed class DividendReinvestment : ShareTransaction, IDividendReinvestment
    {
        /// <summary>
        /// Constructs a DividendReinvestment.
        /// </summary>
        internal DividendReinvestment(Guid factoryGuid, string ticker, DateTime settlementDate, decimal shares, decimal price, decimal commission)
            : base(factoryGuid, ticker, settlementDate, shares, Math.Abs(price), commission)
        {
            if (commission != 0) throw new ArgumentOutOfRangeException("commission", commission, Strings.ShareTransactionImpl_Commission_Commission_for_dividend_reinvestments_must_be_0_);
        }
    }
}