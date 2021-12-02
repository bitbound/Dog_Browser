using Dog_Browser.Helpers;
using Dog_Browser.Models;
using Dog_Browser.Mvvm;
using Dog_Browser.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Dog_Browser.ViewModels
{
    public class BreedDetailsViewModel : ObservableObject
    {
        private readonly IDialogService _dialogService;
        private readonly IDispatcherService _dispatcherService;
        private readonly IDogBreedsApi _dogBreedsApi;
        private DogBreed? _dogBreed;
        private BitmapImage? _imageSource;

        public BreedDetailsViewModel(IDogBreedsApi dogBreedsApi, IDialogService dialogService, IDispatcherService dispatcherService)
        {
            _dogBreedsApi = dogBreedsApi;
            _dialogService = dialogService;
            _dispatcherService = dispatcherService;

            _dogBreedsApi.ReceivedDogImage += DogBreedsApi_ReceivedDogImage;
        }

        public Guid Id { get; } = Guid.NewGuid();

        public DogBreed? DogBreed
        {
            get => _dogBreed;
            set => SetProperty(ref _dogBreed, value);
        }

        public BitmapImage? ImageSource
        {
            get => _imageSource;
            set => SetProperty(ref _imageSource, value);
        }

        public void RetrieveImage()
        {
            if (DogBreed is null)
            {
                return;
            }

            _dogBreedsApi.GetRandomImage(Id, DogBreed.PrimaryBreed, DogBreed.SubBreed);
        }
        private void DogBreedsApi_ReceivedDogImage(object? sender, BaseTypes.ApiResponseEventArgs<DogImage> e)
        {
            if (!e.Result.IsSuccess || e.Result.Value is null)
            {
                _dialogService.Show(
                    "An error occurred while contacting the Dog Breeds server.  Please check the logs or try again later.",
                    "Communication Failure",
                     MessageBoxButton.OK,
                     MessageBoxImage.Error);

                return;
            }

            var dogImage = e.Result.Value;

            // We might have multiple windows open of the same breed.
            // Only act on API calls that originated from this view model.
            if (dogImage.ViewModelId != Id)
            {
                return;
            }

            _dispatcherService.Invoke(() =>
            {
                ImageSource = ImageHelper.CreateImageSource(e.Result.Value.ImageBytes);

            });
        }
    }
}
