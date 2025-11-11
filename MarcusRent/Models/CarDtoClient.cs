using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MarcusRent.Models
{
    public class CarDtoClient
    {
        public int CarId { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public decimal PricePerDay { get; set; }
        public bool Available { get; set; }
        public string CarDescription { get; set; }

        
        [JsonPropertyName("carImages")]
        public CarImageResponseClient CarImagesResponse { get; set; } = new CarImageResponseClient();

        [JsonIgnore]
        public List<CarImageDtoClient> CarImages => CarImagesResponse?.Values ?? new List<CarImageDtoClient>();
    }


}
