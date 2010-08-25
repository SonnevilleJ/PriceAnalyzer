using System;
using System.Runtime.Serialization;

namespace Sonneville.PriceTools
{
    public interface IPriceTuple : IPricePeriod, ISerializable
    {
        void AddPeriod(PricePeriod period);
        IPricePeriod ToPricePeriod();
        IPricePeriod[] Periods { get; }
        IPricePeriod this[int i] { get; }
    }
}
