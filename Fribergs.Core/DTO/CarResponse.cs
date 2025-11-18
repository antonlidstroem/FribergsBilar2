using System.Text.Json.Serialization;
using Fribergs.Core.DTO;

namespace Fribergs.Core.DTO
{
    public class CarResponse
    {
        [JsonPropertyName("$values")]
        public List<CarDto> Values { get; set; } = new List<CarDto>();
    }
}

