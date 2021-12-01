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

        public static void Filter<T>(this ObservableCollection<T> self, List<T> allValues, string searchText)
        {
            self.Clear();

            IEnumerable<T> itemsToAdd = allValues.ToArray();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                itemsToAdd = allValues.Where(x => x?.ToString()?.Contains(searchText, StringComparison.OrdinalIgnoreCase) ?? false);
            }

            foreach (var item in itemsToAdd)
            {
                self.Add(item);
            }
        }
    }
}
