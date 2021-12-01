using Dog_Browser.BaseTypes;
using Dog_Browser.Dtos;
using Dog_Browser.Helpers;
using Dog_Browser.Models;
using Dog_Browser.SampleData;
using Dog_Browser.Services;
using Dog_Browser.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Dog_Browser.Mvvm
{
    public class ViewModelLocator
    {
        public MainWindowViewModel MainWindow => IocContainer.GetRequiredService<MainWindowViewModel>();
        public BreedBrowserViewModel BreedBrowser => IocContainer.GetRequiredService<BreedBrowserViewModel>();

        // This view model provides sample data at design time, so it doesn't make
        // actual API calls.
        public BreedBrowserViewModel BreedBrowserDesignTime
        {
            get
            {

                var viewModel = new BreedBrowserViewModel(new DogBreedsApiStub(), new DialogServiceStub());
                var result = JsonSerializer.Deserialize<ApiResponseResult<Dictionary<string, string[]>>>(SampleResponses.AllBreeds);

                if (result?.Message is null)
                {
                    return viewModel;
                }

                var dogBreeds = ApiResultsHelper.ConvertAllBreedsResponse(result.Message);

                foreach (var breed in dogBreeds)
                {
                    viewModel.DogBreeds.Add(breed);
                }

                return viewModel;
            }
        }

        public LogsPageViewModel LogsPage => IocContainer.GetRequiredService<LogsPageViewModel>();

    }
}
