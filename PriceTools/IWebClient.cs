using System.IO;

namespace Sonneville.PriceTools
{
    public interface IWebClient
    {
        Stream OpenRead(string address);
    }
}