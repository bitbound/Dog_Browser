using Dog_Browser.BaseTypes;
using Dog_Browser.Extensions;
using Dog_Browser.Helpers;
using Dog_Browser.Models;
using Dog_Browser.Mvvm;
using Dog_Browser.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Dog_Browser.ViewModels
{
    public class BreedBrowserViewModel : ObservableObject
    {
        private readonly List<DogBreed> _allDogBreeds = new();
        private readonly IDialogService _dialogService;
        private readonly IDogBreedsApi _dogBreedsApi;
        private bool _isGroupingEnabled;
        private string _searchText = "";
        private ListSortDirection _sortDirection;
        private RelayCommand? _toggleSortDirection;

        public BreedBrowserViewModel(IDogBreedsApi dogBreedsApi, IDialogService dialogService)
        {
            _dogBreedsApi = dogBreedsApi;
            _dialogService = dialogService;

            _dogBreedsApi.ReceivedAllBreeds += DogBreedsApi_ReceivedAllBreeds;

            _dogBreedsApi.GetAllBreeds();
        }

        public ObservableCollection<DogBreed> DogBreeds { get; } = new();

        public bool IsGroupingEnabled
        {
            get => _isGroupingEnabled;
            set
            {
                SetProperty(ref _isGroupingEnabled, value);
                NotifyPropertyChanged(nameof(GroupStyleSelector));
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                SetProperty(ref _searchText, value);
                ApplySearch();
            }
        }

        public ListSortDirection SortDirection
        {
            get => _sortDirection;
            set
            {
                SetProperty(ref _sortDirection, value);
                // Update UI for sort icon when this changes.
                NotifyPropertyChanged(nameof(SortIcon));
            }
        }

        public string SortIcon => SortDirection == ListSortDirection.Ascending ?
            char.ConvertFromUtf32(0xF0AD) :
            char.ConvertFromUtf32(0xF0AE);

        public RelayCommand ToggleSortDirection
        {
            get
            {
                if (_toggleSortDirection is null)
                {
                    _toggleSortDirection = new RelayCommand(() =>
                    {
                        if (SortDirection == ListSortDirection.Ascending)
                        {
                            SortDirection = ListSortDirection.Descending;
                        }
                        else
                        {
                            SortDirection = ListSortDirection.Ascending;
                        }
                        DogBreeds.Sort(BreedComparer);
                    });
                }
                return _toggleSortDirection;
            }
        }

        private void ApplySearch()
        {
            // We don't want to update with every keystroke, but we also
            // don't want to wait for lost focus.  So we'll debounce the
            // change event.
            Debouncer.Debounce(TimeSpan.FromSeconds(1), () =>
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    DogBreeds.Filter(_allDogBreeds, _searchText);

                });
            });
        }
        private int BreedComparer(DogBreed a, DogBreed b)
        {
            if (IsGroupingEnabled)
            {
                // If grouping is enabled, we want to sort first by the group name (primary breed).
                // If they differ, return the order.  Else, sort by display name.
                var groupSort = SortDirection == ListSortDirection.Ascending ?
                    a.PrimaryBreed.CompareTo(b.PrimaryBreed) :
                    b.PrimaryBreed.CompareTo(a.PrimaryBreed);

                if (groupSort != 0)
                {
                    return groupSort;
                }
            }

            return SortDirection == ListSortDirection.Ascending ?
                  a.DisplayName.CompareTo(b.DisplayName) :
                  b.DisplayName.CompareTo(a.DisplayName);
        }

        private void DogBreedsApi_ReceivedAllBreeds(object? sender, ApiResponseEventArgs<DogBreed[]> e)
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

            _allDogBreeds.Clear();
            _allDogBreeds.AddRange(e.Result.Value);

            App.Current.Dispatcher.Invoke(() =>
            {
                DogBreeds.Clear();

                _allDogBreeds.Sort(BreedComparer);

                foreach (var dogBreed in e.Result.Value)
                {
                    DogBreeds.Add(dogBreed);
                }
            });
           
        }
    }
}
