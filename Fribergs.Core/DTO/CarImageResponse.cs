using System.Text.Json.Serialization;
using Fribergs.Core.DTO;

namespace Fribergs.Core.DTO { 

    public class CarImageResponse
    {
        [JsonPropertyName("$values")]
        public List<CarImageDto> Values { get; set; } = new List<CarImageDto>();
    }
}
