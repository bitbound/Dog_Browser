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
using System.Windows.Shapes;

namespace Dog_Browser.Windows
{
    /// <summary>
    /// Interaction logic for BreedDetailsWindow.xaml
    /// </summary>
    public partial class BreedDetailsWindow : Window
    {
        public BreedDetailsWindow()
        {
            InitializeComponent();
        }

        public BreedDetailsViewModel? ViewModel => DataContext as BreedDetailsViewModel;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel?.RetrieveImage();
        }
    }
}
