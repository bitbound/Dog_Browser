using Dog_Browser.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dog_Browser.Mvvm
{
    public class ViewModelLocator
    {
        public MainWindowViewModel MainWindow => IocContainer.GetRequiredService<MainWindowViewModel>();
    }
}
