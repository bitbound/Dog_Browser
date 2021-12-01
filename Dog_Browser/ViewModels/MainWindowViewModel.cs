using Dog_Browser.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Dog_Browser.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        private NavigationMenuItem? _selectedNavItem;

        public MainWindowViewModel()
        {
            AddMenuItems();
            SelectedNavItem = NavMenuItems.First();
        }

        private void AddMenuItems()
        {
            NavMenuItems.Add(new("Dog Browser", "DogBrowserPage"));
            NavMenuItems.Add(new("Adopt a Pet", "AdoptionPage"));
            NavMenuItems.Add(new("Logs", "LogsPage"));
            NavMenuItems.Add(new("About", "AboutPage"));
        }

        public ObservableCollection<NavigationMenuItem> NavMenuItems { get; } = new();

        public NavigationMenuItem? SelectedNavItem
        {
            get => _selectedNavItem;
            set => SetProperty(ref _selectedNavItem, value);
        }
    }
}
