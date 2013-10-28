using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Harness.WinRT {
    public interface IHttpClient : IDependency {
        Task<HttpResponseMessage> Request(
            HttpMethod method, string url, HttpContent content = null,
            Dictionary<string, string> headers = null);
    }
}