using Dog_Browser.Dtos;
using Dog_Browser.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Dog_Browser.Tests
{
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
    [TestClass]
    public class ApiResultsTransformerTests
    {
        [TestMethod]
        public void ConvertAllBreedsResponse_GivenValidDictionary_ReturnsBreeds()
        {
            var result = JsonSerializer.Deserialize<ApiResponseResult<Dictionary<string, string[]>>>(SampleData.AllBreeds);

            var conversion = ApiResultsTransformer.ConvertAllBreedsResponse(result.Message);

            
        }
    }
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8604 // Possible null reference argument.
}
