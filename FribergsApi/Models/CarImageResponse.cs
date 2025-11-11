using System.Text.Json.Serialization;

namespace FribergsApi.Models
{
    public class CarImageResponse
    {
        [JsonPropertyName("$values")]
        public List<CarImageDto> Values { get; set; } = new List<CarImageDto>();
    }
}
