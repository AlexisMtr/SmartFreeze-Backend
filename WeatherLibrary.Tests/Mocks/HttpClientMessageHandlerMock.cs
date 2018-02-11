using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WeatherLibrary.Tests.Mocks
{
    public class HttpClientMessageHandlerMock : HttpMessageHandler
    {
        private readonly Dictionary<string, string> responseDictionary;

        public HttpClientMessageHandlerMock(Dictionary<string, string> responseDictionary)
        {
            this.responseDictionary = responseDictionary;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string response = string.Empty;
            foreach(var kv in this.responseDictionary)
            {
                if (request.RequestUri.AbsoluteUri.StartsWith(kv.Key))
                {
                    response = kv.Value;
                    break;
                }
            }
            
            return Task.FromResult(new HttpResponseMessage
            {
                Content = new StringContent(response)
            });
        }
    }
}
