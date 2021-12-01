using Dog_Browser.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Dog_Browser.SampleData
{
    internal class DialogServiceStub : IDialogService
    {
        public MessageBoxResult Show(string message, string caption, MessageBoxButton buttons, MessageBoxImage image)
        {
            return MessageBoxResult.OK;
        }
    }
}
