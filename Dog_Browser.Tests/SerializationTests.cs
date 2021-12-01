using Dog_Browser.Dtos;
using Dog_Browser.SampleData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Dog_Browser.Tests
{
#pragma warning disable CS8602 // Dereference of a possibly null reference.
    [TestClass]
    public class SerializationTests
    {
        [TestMethod]
        public void Deserialize_GivenValidAllBreedsResponse_Succeeds()
        {
            var result = JsonSerializer.Deserialize<ApiResponseResult<Dictionary<string, string[]>>>(SampleResponses.AllBreeds);

            Assert.AreEqual(95, result.Message.Count);
            Assert.AreEqual("golden", result.Message["retriever"].ElementAt(3));
            Assert.AreEqual(ApiResponseStatus.Success, result.Status);
        }

        [TestMethod]
        public void Deserialize_GivenValid404Response_Succeeds()
        {
            var result = JsonSerializer.Deserialize<ApiResponseResult<string>>(SampleResponses.NotFoundResponse);

            Assert.AreEqual(404, result.Code);
            Assert.AreEqual(ApiResponseStatus.Error, result.Status);
            Assert.AreEqual("Not found.", result.Message);
        }
    }
#pragma warning restore CS8602 // Dereference of a possibly null reference.
}