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
        event EventHandler<ApiResponseEventArgs<DogBreed[]>> ReceivedAllBreeds;
        event EventHandler<ApiResponseEventArgs<DogImage>>? ReceivedDogImage;
        Task GetAllBreeds();
        Task GetRandomImage(string primaryBreed, string? subBreed = null);
    }

    public class DogBreedsApi : IDogBreedsApi
    {
        // This could go in a config file or something.
        private readonly string _allBreedsEndpoint = "https://dog.ceo/api/breeds/list/all";

        private readonly IHttpClientWrapper _httpClientWrapper;
        private readonly ILogger<DogBreedsApi> _logger;

        // Normally, I'd break this cache out into a separate service so it could be tested.
        private readonly MemoryCache _responseCache = new(new MemoryCacheOptions());
        private readonly ISystemTime _systemTime;
        public DogBreedsApi(IHttpClientWrapper httpClientWrapper, ISystemTime systemTime, ILogger<DogBreedsApi> logger)
        {
            _httpClientWrapper = httpClientWrapper;
            _systemTime = systemTime;
            _logger = logger;
        }

        public event EventHandler<ApiResponseEventArgs<DogBreed[]>>? ReceivedAllBreeds;
        public event EventHandler<ApiResponseEventArgs<DogImage>>? ReceivedDogImage;

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
                        var result = Result.Ok(cachedBreeds);
                        ReceivedAllBreeds?.Invoke(this, new(result, true));
                        return;
                    }

                    var response = await _httpClientWrapper.GetFromJsonAsync<ApiResponseResult<Dictionary<string, string[]>>>(_allBreedsEndpoint);

                    if (response?.Status == ApiResponseStatus.Success &&
                        response.Message?.Any() == true)
                    {
                        var breeds = ApiResultsHelper.ConvertAllBreedsResponse(response.Message);
                        _responseCache.Set(_allBreedsEndpoint, breeds, _systemTime.Now.AddHours(1));

                        var result = Result.Ok(breeds);
                        ReceivedAllBreeds?.Invoke(this, new(result, false));
                    }
                    else
                    {
                        var result = Result.Fail<DogBreed[]>($"API call failed with code {response?.Code}.");
                        ReceivedAllBreeds?.Invoke(this, new(result, false));
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while getting all dog breeds.");
                    var result = Result.Fail<DogBreed[]>(ex);
                    ReceivedAllBreeds?.Invoke(this, new(result, false));
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
                    var result = Result.Fail<DogImage>("Error while retrieving dog image.");
                    ReceivedDogImage?.Invoke(this, new(result, false));
                }
            });
        }

        private async Task GetDogImage(string imageUrl, string primaryBreed, string? subBreed)
        {
            if (_responseCache.TryGetValue(imageUrl, out var cachedItem) &&
                cachedItem is DogImage cachedImage)
            {
                ReceivedDogImage?.Invoke(this, new(Result.Ok(cachedImage), true));
                return;
            }

            var imageBytes = await _httpClientWrapper.GetByteArrayAsync(imageUrl);

            var dogImage = new DogImage(primaryBreed, subBreed, imageBytes);
            _responseCache.Set(imageUrl, dogImage);
            ReceivedDogImage?.Invoke(this, new(Result.Ok(dogImage), false));
        }

        private async Task<Result<string>> GetImageUrl(string primaryBreed, string? subBreed)
        {
            var endpoint = GetRandomImageEndpoint(primaryBreed, subBreed);

            var response = await _httpClientWrapper.GetFromJsonAsync<ApiResponseResult<string>>(endpoint);

            if (response?.Status != ApiResponseStatus.Success)
            {
                var msg = $"API call failed with code {response?.Code}.";
                _logger.LogError(msg);
                ReceivedDogImage?.Invoke(this, new(Result.Fail<DogImage>(msg), false));
                return Result.Fail<string>(msg);
            }

            var imageUrl = response.Message;

            if (!Uri.TryCreate(imageUrl, UriKind.Absolute, out _))
            {
                var msg = "API did not return a valid image URL.";
                ReceivedDogImage?.Invoke(this, new(Result.Fail<DogImage>(msg), false));
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
