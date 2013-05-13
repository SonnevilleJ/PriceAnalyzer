using System.IO;
using System.Net;

namespace Sonneville.PriceTools
{
    public class WebClientWrapper : IWebClient
    {
        private readonly WebClient _webClient;

        public WebClientWrapper()
        {
            _webClient = new WebClient {Proxy = {Credentials = CredentialCache.DefaultNetworkCredentials}};
        }

        public Stream OpenRead(string address)
        {
            return _webClient.OpenRead(address);
        }
    }
}