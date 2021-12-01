using Dog_Browser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dog_Browser.Helpers
{
    public static class ApiResultsTransformer
    {
        public static DogBreed[] ConvertAllBreedsResponse(Dictionary<string, string[]> response)
        {
            if (response is null)
            {
                throw new ArgumentNullException(nameof(response));
            }
            
            return response.SelectMany(kvp =>
            {
                var breeds = new List<DogBreed>();

                if (kvp.Value?.Any() != true)
                {
                    breeds.Add(new DogBreed(kvp.Key));
                }
                else
                {
                    var subbreeds = kvp.Value.Select(sub => new DogBreed(kvp.Key, sub));
                    breeds.AddRange(subbreeds);
                }

                return breeds;
            }).ToArray();
        }
    }
}
