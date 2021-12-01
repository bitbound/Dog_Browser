using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Dog_Browser.Models
{
    public class DogImage
    {
        public DogImage(string primaryBreed, string? subBreed, byte[] imageBytes)
        {
            PrimaryBreed = primaryBreed;
            SubBreed = subBreed;
            ImageBytes = imageBytes;
        }

        public string PrimaryBreed { get; init; }
        public string? SubBreed { get; init; }
        public byte[] ImageBytes { get; init; }
    }
}
