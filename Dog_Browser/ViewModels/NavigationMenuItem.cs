using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dog_Browser.ViewModels
{
    public class NavigationMenuItem
    {
        public NavigationMenuItem(string label, string pageName)
        {
            Label = label;
            PageName = pageName;
        }

        public string Label { get; init; }
        public string PageName { get; init; }
    }
}
