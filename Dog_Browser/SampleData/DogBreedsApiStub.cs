using Dog_Browser.BaseTypes;
using Dog_Browser.Models;
using Dog_Browser.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dog_Browser.SampleData
{

    public class DogBreedsApiStub : IDogBreedsApi
    {
        public event EventHandler<ApiResponseEventArgs<DogBreed[]>> ReceivedAllBreeds;
        public event EventHandler<ApiResponseEventArgs<DogImage>>? ReceivedDogImage;

        public Task GetAllBreeds()
        {
            return Task.CompletedTask;
        }

        public Task GetRandomImage(string primaryBreed, string? subBreed = null)
        {
            return Task.CompletedTask;
        }
    }
}
