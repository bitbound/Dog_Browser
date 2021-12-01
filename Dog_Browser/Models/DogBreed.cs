using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dog_Browser.Models
{
    public class DogBreed
    {
        public DogBreed(string primaryBreed, string subBreed = "")
        {
            PrimaryBreed = primaryBreed;
            SubBreed = subBreed;
        }

        public string DisplayName => ToString();
        public bool IsSubBreed => !string.IsNullOrWhiteSpace(SubBreed);
        public string PrimaryBreed { get; init; }
        public string SubBreed { get; init; }

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(SubBreed))
            {
                return PrimaryBreed;
            }

            return $"{SubBreed} {PrimaryBreed}";
        }
    }
}
