using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools.Data.Csv
{
    public enum TransactionColumn
    {
        None = 0,

        Date,

        OrderType,

        Symbol,

        Shares,

        PricePerShare,

        Commission,

        TotalBasis
    }
}