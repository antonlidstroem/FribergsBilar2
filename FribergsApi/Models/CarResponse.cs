using System.Text.Json.Serialization;

namespace FribergsApi.Models
{
    public class CarResponse
    {
        [JsonPropertyName("$values")]
        public List<CarDto> Values { get; set; } = new List<CarDto>();
    }
}

