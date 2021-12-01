using Dog_Browser.BaseTypes;
using Dog_Browser.Dtos;
using Dog_Browser.Helpers;
using Dog_Browser.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Dog_Browser.Services
{
    public interface IDogBreedsApi
    {
        event EventHandler<Result<DogBreed[]>> ReceivedAllBreeds;
        event EventHandler<Result<DogImage>>? ReceivedDogImage;
        Task GetAllBreeds();
        Task GetRandomImage(string primaryBreed, string? subBreed = null);
    }

    public class DogBreedsApi : IDogBreedsApi
    {
        // This could go in a config file or something.
        private readonly string _allBreedsEndpoint = "https://dog.ceo/api/breeds/list/all";

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<DogBreedsApi> _logger;

        // Normally, I'd break this cache out into a separate service so it could be tested.
        private readonly MemoryCache _responseCache = new(new MemoryCacheOptions());
        private readonly ISystemTime _systemTime;
        public DogBreedsApi(IHttpClientFactory httpClientFactory, ISystemTime systemTime, ILogger<DogBreedsApi> logger)
        {
            _httpClientFactory = httpClientFactory;
            _systemTime = systemTime;
            _logger = logger;
        }

        public event EventHandler<Result<DogBreed[]>>? ReceivedAllBreeds;
        public event EventHandler<Result<DogImage>>? ReceivedDogImage;

        // Typically, I'd use async/await on API calls, so it still wouldn't tie
        // up the thread from which it was called.  I'm calling it this way because
        // of the specific requirement that the request be completed on a non-UI thread.
        public Task GetAllBreeds()
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (_responseCache.TryGetValue(_allBreedsEndpoint, out var cachedItem) &&
                        cachedItem is DogBreed[] cachedBreeds)
                    {
                        ReceivedAllBreeds?.Invoke(this, Result.Ok(cachedBreeds));
                        return;
                    }

                    using var httpClient = _httpClientFactory.CreateClient();

                    var response = await httpClient.GetFromJsonAsync<ApiResponseResult<Dictionary<string, string[]>>>(_allBreedsEndpoint);

                    if (response?.Status == ApiResponseStatus.Success &&
                        response.Message?.Any() == true)
                    {
                        var breeds = ApiResultsTransformer.ConvertAllBreedsResponse(response.Message);
                        _responseCache.Set(_allBreedsEndpoint, breeds, _systemTime.Now.AddHours(1));
                        ReceivedAllBreeds?.Invoke(this, Result.Ok(breeds));
                    }
                    else
                    {
                        ReceivedAllBreeds?.Invoke(this, Result.Fail<DogBreed[]>($"API call failed with code {response?.Code}."));
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while getting all dog breeds.");
                    ReceivedAllBreeds?.Invoke(this, Result.Fail<DogBreed[]>(ex));
                }
            });
        }

        public Task GetRandomImage(string primaryBreed, string? subBreed = null)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var imageUrlResult = await GetImageUrl(primaryBreed, subBreed);

                    if (!imageUrlResult.IsSuccess ||
                        string.IsNullOrWhiteSpace(imageUrlResult.Value))
                    {
                        return;
                    }

                    await GetDogImage(imageUrlResult.Value, primaryBreed, subBreed);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while retrieving dog image.");
                    ReceivedDogImage?.Invoke(this, Result.Fail<DogImage>("Error while retrieving dog image."));
                }
            });
        }

        private async Task GetDogImage(string imageUrl, string primaryBreed, string? subBreed)
        {
            using var httpClient = _httpClientFactory.CreateClient();

            if (_responseCache.TryGetValue(imageUrl, out var cachedItem) &&
                cachedItem is DogImage cachedImage)
            {
                ReceivedDogImage?.Invoke(this, Result.Ok(cachedImage));
                return;
            }

            var imageBytes = await httpClient.GetByteArrayAsync(imageUrl);

            var dogImage = new DogImage(primaryBreed, subBreed, imageBytes);
            _responseCache.Set(imageUrl, dogImage);
            ReceivedDogImage?.Invoke(this, Result.Ok(dogImage));
        }

        private async Task<Result<string>> GetImageUrl(string primaryBreed, string? subBreed)
        {
            var endpoint = GetRandomImageEndpoint(primaryBreed, subBreed);

            using var httpClient = _httpClientFactory.CreateClient();

            var response = await httpClient.GetFromJsonAsync<ApiResponseResult<string>>(endpoint);

            if (response?.Status != ApiResponseStatus.Success)
            {
                var msg = $"API call failed with code {response?.Code}.";
                _logger.LogError(msg);
                ReceivedDogImage?.Invoke(this, Result.Fail<DogImage>(msg));
                return Result.Fail<string>(msg);
            }

            var imageUrl = response.Message;

            if (!Uri.TryCreate(imageUrl, UriKind.Absolute, out _))
            {
                var msg = "API did not return a valid image URL.";
                ReceivedDogImage?.Invoke(this, Result.Fail<DogImage>(msg));
                _logger.LogError(msg);
                return Result.Fail<string>(msg);
            }

            return Result.Ok(imageUrl);
        }

        private string GetRandomImageEndpoint(string primaryBreed, string? subBreed = null)
        {
            if (string.IsNullOrWhiteSpace(subBreed))
            {
                return $"https://dog.ceo/api/breed/{primaryBreed}/images/random";
            }
            return $"https://dog.ceo/api/breed/{primaryBreed}/{subBreed}/images/random";
        }
    }
}
