using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dog_Browser.Extensions
{
    public static class ObservableCollectionExtensions
    {
        public static void Sort<T>(this ObservableCollection<T> self, Comparison<T> comparer)
        {
            var sorted = self.ToList();
            sorted.Sort(comparer);

            self.Clear();

            foreach (var item in sorted)
            {
                self.Add(item);
            }
        }
    }
}
