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

        public async Task<byte[]> GetByteArrayAsync(string url)
        {
            using var httpClient = _httpClientFactory.CreateClient();
            return await httpClient.GetByteArrayAsync(url);
        }

        public async Task<T?> GetFromJsonAsync<T>(string url)
        {
            using var httpClient = _httpClientFactory.CreateClient();
            return await httpClient.GetFromJsonAsync<T>(url);
        }
    }
}
