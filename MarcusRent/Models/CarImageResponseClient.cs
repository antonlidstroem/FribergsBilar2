using System.Text.Json.Serialization;

namespace MarcusRent.Models
{
    public class CarImageResponseClient
    {
        [JsonPropertyName("$values")]
        public List<CarImageDtoClient> Values { get; set; } = new List<CarImageDtoClient>();
    }
}
