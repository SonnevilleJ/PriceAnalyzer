using System;

namespace Sonneville.PriceTools
{
    [Flags]
    public enum OrderType
    {
        Deposit                 = 1,

        DividendReceipt         = 2,

        DividendReinvestment    = 4,

        Sell                    = 8,

        BuyToCover              = 16,

        Buy                     = 32,

        SellShort               = 64,

        Withdrawal              = 128
    }
}