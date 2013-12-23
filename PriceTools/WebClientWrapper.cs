using System.IO;
using System.Net;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Wraps a <see cref="System.Net.WebClient"/> in an <see cref="IWebClient"/>.
    /// </summary>
    public class WebClientWrapper : IWebClient
    {
        private readonly WebClient _webClient;

        /// <summary>
        /// Creates a <see cref="System.Net.WebClient"/>.
        /// </summary>
        public WebClientWrapper()
        {
            _webClient = new WebClient {Proxy = {Credentials = CredentialCache.DefaultNetworkCredentials}};
        }

        ~WebClientWrapper()
        {
            Dispose(false);
        }

        /// <summary>
        /// Opens a readable stream for the data downloaded from a resource with the URI specified as a <see cref="T:System.Uri"/>
        /// </summary>
        public Stream OpenRead(string address)
        {
            return _webClient.OpenRead(address);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
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