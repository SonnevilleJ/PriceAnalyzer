using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sonneville.PriceTools
{
    public interface ITransaction
    {
        DateTime Date
        {
            get;
        }

        OrderType OrderType
        {
            get;
        }

        string Ticker
        {
            get;
        }

        double Shares
        {
            get;
        }

        decimal Price
        {
            get;
        }

        decimal Commission
        {
            get;
        }
    }
}
