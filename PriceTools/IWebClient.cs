using System;
using System.IO;

namespace Sonneville.PriceTools
{
    public interface IWebClient : IDisposable
    {
        Stream OpenRead(string address);
    }
}