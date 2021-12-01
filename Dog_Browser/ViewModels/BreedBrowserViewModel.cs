using Dog_Browser.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dog_Browser.ViewModels
{
    public class BreedBrowserViewModel : ObservableObject
    {
        private ListSortDirection _sortDirection;
        private RelayCommand? _toggleSortDirection;
        private bool _isGroupingEnabled;

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
                    });
                }
                return _toggleSortDirection;
            }
        }

        public bool IsGroupingEnabled
        {
            get => _isGroupingEnabled;
            set => SetProperty(ref _isGroupingEnabled, value);
        }
    }
}
