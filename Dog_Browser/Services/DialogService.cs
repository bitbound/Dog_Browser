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
        private readonly IDispatcherService _dispatcherService;

        public DialogService(IDispatcherService dispatcherService)
        {
            _dispatcherService = dispatcherService;
        }

        public MessageBoxResult Show(string message, string caption, MessageBoxButton buttons, MessageBoxImage image)
        {
            return _dispatcherService.Invoke(() =>
            {
                return MessageBox.Show(message, caption, buttons, image);
            });
        }
    }
}
