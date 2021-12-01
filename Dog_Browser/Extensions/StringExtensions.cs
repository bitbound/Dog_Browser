using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dog_Browser.Extensions
{
    public static class StringExtensions
    {
        public static string ToCapitalCase(this string self)
        {
            if (string.IsNullOrWhiteSpace(self))
            {
                return self;
            }

            var sections = self.Split(' ');

            var modifiedSections = new List<string>();

            foreach (var section in sections)
            {
                var firstLetter = section.First().ToString().ToUpper();

                var newSection = section.Length > 1 ?
                    string.Join("", firstLetter, section[1..]) :
                    firstLetter;

                modifiedSections.Add(newSection);
            }

            return string.Join(" ", modifiedSections);
        }
    }
}
