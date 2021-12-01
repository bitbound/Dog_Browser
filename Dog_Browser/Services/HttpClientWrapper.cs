using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Dog_Browser.Services
{
    public interface IHttpClientWrapper
    {
        Task<byte[]> GetByteArrayAsync(string url);
        Task<T?> GetFromJsonAsync<T>(string url);
    }

    public class HttpClientWrapper : IHttpClientWrapper
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HttpClientWrapper(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public Task<byte[]> GetByteArrayAsync(string url)
        {
            using var httpClient = _httpClientFactory.CreateClient();
            return httpClient.GetByteArrayAsync(url);
        }

        public Task<T?> GetFromJsonAsync<T>(string url)
        {
            using var httpClient = _httpClientFactory.CreateClient();
            return httpClient.GetFromJsonAsync<T>(url);
        }
    }
}
