using Dog_Browser.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Dog_Browser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindowViewModel? ViewModel => DataContext as MainWindowViewModel;

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ViewModel?.SelectedNavItem?.PageName is not null)
            {
                var pageName = $"Dog_Browser.Pages.{ViewModel.SelectedNavItem.PageName}";
                var pageType = Type.GetType(pageName);
                if (pageType is not null)
                {
                    NavigationFrame?.Navigate(Activator.CreateInstance(pageType));
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            NavigationList.SelectedIndex = 0;
        }
    }
}
