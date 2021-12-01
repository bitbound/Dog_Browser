using Dog_Browser.Helpers;
using Dog_Browser.Mvvm;
using Dog_Browser.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Dog_Browser.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        private BitmapSource? _currentImage;

        public MainWindowViewModel(IDogBreedsApi dogBreedsApi)
        {
            dogBreedsApi.ReceivedAllBreeds += (s, e) =>
            {
                var test = e;
            };

            dogBreedsApi.ReceivedDogImage += (s, e) =>
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    CurrentImage = ImageHelper.CreateImageSource(e.Result.Value.ImageBytes);
                });
                
            };

            _ = dogBreedsApi.GetAllBreeds();
            _ = dogBreedsApi.GetRandomImage("pug");
        }

        public BitmapSource? CurrentImage
        {
            get => _currentImage;
            set => SetProperty(ref _currentImage, value);
        }
    }
}
