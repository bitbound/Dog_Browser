using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Dog_Browser.Dtos
{
    public class ApiResponseResult<T>
    {
        [JsonPropertyName("message")]
        public T? Message { get; init; }


        [JsonPropertyName("code")]
        public int? Code { get; init; }

        [JsonPropertyName("status")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ApiResponseStatus Status { get; init; }
    }
}
