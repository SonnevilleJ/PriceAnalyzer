using System;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a transaction to reinvest dividends.
    /// </summary>
    [Serializable]
    public sealed class DividendReinvestment : ShareTransaction, LongTransaction, AccumulationTransaction, OpeningTransaction
    {
        /// <summary>
        /// Constructs a DividendReinvestment.
        /// </summary>
        internal DividendReinvestment(string ticker, DateTime settlementDate, decimal shares, decimal price, decimal commission)
            : base(ticker, settlementDate, shares, Math.Abs(price), commission)
        {
            if (commission != 0) throw new ArgumentOutOfRangeException("commission", commission, Strings.ShareTransactionImpl_Commission_Commission_for_dividend_reinvestments_must_be_0_);
        }
    }
}