using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Harness.WinRT {
    public class HttpClient : IHttpClient {
        #region IHttpClient Members

        public async Task<HttpResponseMessage> Request(
            HttpMethod method, string url, HttpContent content = null,
            Dictionary<string, string> headers = null) {
            var req = new System.Net.Http.HttpClient(new HttpClientHandler());
            var message = new HttpRequestMessage(method, url);
            if (headers != null)
                foreach (var h in headers) {
                    //if (h.Key == "Content-Length")

                    message.Headers.Add(h.Key, h.Value);
                }

            if (content != null) message.Content = content;
            message.Method = method;
            HttpResponseMessage result = await req.SendAsync(message);
            return result;
        }

        #endregion
    }
}