using System.Text.Json.Serialization;
using FribergsApi.Models;

namespace Fribergs.Core.Models
{
    public class CarImageResponse
    {
        [JsonPropertyName("$values")]
        public List<CarImageDto> Values { get; set; } = new List<CarImageDto>();
    }
}
