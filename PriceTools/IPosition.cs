using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sonneville.PriceTools
{
    public interface IPosition
    {
        ITransaction Open
        {
            get;
            set;
        }

        ITransaction Close
        {
            get;
            set;
        }

        decimal Value
        {
            get;
        }
    }
}
