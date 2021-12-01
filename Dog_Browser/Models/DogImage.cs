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
        public DogImage(Guid viewModelId, string primaryBreed, string? subBreed, byte[] imageBytes)
        {
            ViewModelId = viewModelId;
            PrimaryBreed = primaryBreed;
            SubBreed = subBreed;
            ImageBytes = imageBytes;
        }

        public byte[] ImageBytes { get; init; }
        public string PrimaryBreed { get; init; }
        public string? SubBreed { get; init; }
        public Guid ViewModelId { get; init; }
    }
}
