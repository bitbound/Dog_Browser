using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Dog_Browser.Services
{
    public interface IDialogService
    {
        MessageBoxResult Show(string message, string caption, MessageBoxButton buttons, MessageBoxImage image);
    }

    public class DialogService : IDialogService
    {

        public MessageBoxResult Show(string message, string caption, MessageBoxButton buttons, MessageBoxImage image)
        {
            if (App.Current?.Dispatcher is null)
            {
                return MessageBoxResult.None;
            }

            return App.Current.Dispatcher.Invoke(() =>
            {
                return MessageBox.Show(message, caption, buttons, image);
            });
        }
    }
}
