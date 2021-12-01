using Castle.Core.Logging;
using Dog_Browser.BaseTypes;
using Dog_Browser.Dtos;
using Dog_Browser.Helpers;
using Dog_Browser.Models;
using Dog_Browser.Services;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Dog_Browser.Tests
{
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8604 // Possible null reference argument.
    [TestClass]
    public class DogBreedsApiTests
    {
        private ApiResponseResult<Dictionary<string, string[]>>? _allBreedsResponse;

        [TestInitialize]
        public void Init()
        {
            _allBreedsResponse = JsonSerializer.Deserialize<ApiResponseResult<Dictionary<string, string[]>>>(SampleData.AllBreeds);
        }

        [TestMethod]
        public async Task GetAllBreeds_GivenValidHttpResponse_Succeeds()
        {
            var httpClientWrapper = new Mock<IHttpClientWrapper>();
            httpClientWrapper
                .Setup(x => x.GetFromJsonAsync<ApiResponseResult<Dictionary<string, string[]>>>(It.IsAny<string>()))
                .Returns(Task.FromResult(_allBreedsResponse));

            var systemTime = new Mock<ISystemTime>();
            systemTime.SetupGet(x => x.Now).Returns(DateTimeOffset.Now);

            var logger = new Mock<ILogger<DogBreedsApi>>();

            var api = new DogBreedsApi(httpClientWrapper.Object, systemTime.Object, logger.Object);

            ApiResponseEventArgs<DogBreed[]>? response = null;

            api.ReceivedAllBreeds += (s, a) =>
            {
                response = a;    
            };

            _ = api.GetAllBreeds();
            await Task.Delay(500);

            // Results shouldn't come from cache on the first request.
            Assert.IsFalse(response.ResolvedFromCache);
            Assert.IsTrue(response.Result.IsSuccess);
            Assert.IsTrue(response.Result.Value.Any());
            httpClientWrapper.Verify(x => 
                x.GetFromJsonAsync<ApiResponseResult<Dictionary<string, string[]>>>(It.IsAny<string>()),
                Times.Once);
            httpClientWrapper.VerifyNoOtherCalls();

            _ = api.GetAllBreeds();
            await Task.Delay(500);

            // Results should have been resolved from cache this time.
            Assert.IsTrue(response.ResolvedFromCache);
            Assert.IsTrue(response.Result.IsSuccess);
            Assert.IsTrue(response.Result.Value.Any());
            httpClientWrapper.Verify(x =>
                x.GetFromJsonAsync<ApiResponseResult<Dictionary<string, string[]>>>(It.IsAny<string>()),
                Times.Once);
            httpClientWrapper.VerifyNoOtherCalls();
        }

        // TODO: Test GetRandomImage method in similar fashion to above.
    }
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
}
