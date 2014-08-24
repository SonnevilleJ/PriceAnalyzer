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

        ~WebClientWrapper()
        {
            Dispose(false);
        }

        public Stream OpenRead(string address)
        {
            return _webClient.OpenRead(address);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_webClient != null)
                {
                    _webClient.Dispose();
                }
            }
        }
    }
}